namespace Godex.Tests;

using GdUnit4;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static GdUnit4.Assertions;

[TestSuite]
[RequireGodotRuntime]
public partial class SettingsTest {
    private const string PREFIX = "godex_settings_test_";

    // Every test gets its own config file. Setting's constructor schedules its
    // initial setter with CallDeferred, so a setting built by one test can still be
    // saving its file while the next test runs - sharing a path would leak values
    // between tests.
    private static readonly string RUN = Guid.NewGuid().ToString("N")[..8];

    private static int counter;

    private string path = string.Empty;

    private partial class Subject : Node {
        [Setting("audio", "music")] private Setting<int> _music;
        [Setting("audio", "sfx")] public Setting<int> Sfx { get; set; }

        public Setting<int> Music => _music;
    }

    private partial class NotASetting : Node {
        [Setting("audio", "music")] private int _music;
    }

    [BeforeTest]
    public void UseScratchConfig() {
        // Clear scratch files left by earlier runs. A deferred setter can re-create
        // its file after its own [AfterTest] ran, so this runs before every test.
        using DirAccess directory = DirAccess.Open("user://");
        if (directory != null) {
            foreach (string file in directory.GetFiles()) {
                if (file.StartsWith(PREFIX, StringComparison.Ordinal)) {
                    directory.Remove(file);
                }
            }
        }
        // The run token keeps consecutive runs from picking up each other's files.
        path = $"user://{PREFIX}{RUN}_{counter++}.cfg";
    }

    [AfterTest]
    public void RemoveScratchConfig() {
        if (FileAccess.FileExists(path)) {
            DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(path));
        }
    }

    private SettingsServer BuildServer(List<int> applied) => new SettingsServerBuilder(path)
        .WithSection("audio")
            .WithSetting("music", 100, applied.Add)
            .WithSetting("sfx", 80, applied.Add)
        .WithSection("graphics")
            .WithSetting("ssrl", false, _ => { })
        .Build();

    [TestCase]
    public void BuildCreatesTheConfigFileWhenItIsMissing() {
        AssertBool(FileAccess.FileExists(path)).IsFalse();

        BuildServer(new List<int>());

        AssertBool(FileAccess.FileExists(path)).IsTrue();
    }

    [TestCase]
    public void BuilderMethodsReturnTheSameInstance() {
        SettingsServerBuilder builder = new SettingsServerBuilder(path);

        AssertObject(builder.WithSection("audio")).IsSame(builder);
        AssertObject(builder.WithSetting("music", 1, _ => { })).IsSame(builder);
    }

    [TestCase]
    public void DuplicateKeyInTheSameSectionIsRejected() {
        SettingsServerBuilder builder = new SettingsServerBuilder(path)
            .WithSection("audio")
                .WithSetting("music", 1, _ => { });

        AssertThrown(() => builder.WithSetting("music", 2, _ => { }))
            .HasMessage("Duplicate key audio-music.");
    }

    [TestCase]
    public void GetSettingExposesSectionKeyAndDefault() {
        SettingsServer server = BuildServer(new List<int>());

        Setting<int> music = server.GetSetting<int>("audio", "music");

        AssertString(music.Section).IsEqual("audio");
        AssertString(music.Key).IsEqual("music");
        AssertInt(music.Default).IsEqual(100);
        AssertInt(music.Value).IsEqual(100);
    }

    [TestCase]
    public void GetSettingThrowsForAnUnknownSection() {
        SettingsServer server = BuildServer(new List<int>());

        AssertThrown(() => server.GetSetting<int>("nope", "music")).HasMessage("Section nope not found.");
    }

    [TestCase]
    public void GetSettingThrowsForAnUnknownKey() {
        SettingsServer server = BuildServer(new List<int>());

        AssertThrown(() => server.GetSetting<int>("audio", "nope")).HasMessage("Setting audio-nope not found.");
    }

    [TestCase]
    public void GetSettingThrowsOnATypeMismatch() {
        SettingsServer server = BuildServer(new List<int>());

        AssertThrown(() => server.GetSetting<string>("audio", "music"))
            .HasMessage("Setting audio-music is not of type System.String.");
    }

    [TestCase]
    public void AssigningAValueRunsTheSetter() {
        List<int> applied = new List<int>();
        SettingsServer server = BuildServer(applied);

        server.GetSetting<int>("audio", "music").Value = 42;

        AssertArray(applied).Contains(42);
    }

    [TestCase]
    public void AssigningAValueRaisesUpdated() {
        List<int> updated = new List<int>();
        SettingsServer server = BuildServer(new List<int>());
        Setting<int> music = server.GetSetting<int>("audio", "music");
        music.Updated += value => updated.Add(value);

        music.Value = 42;

        AssertArray(updated).ContainsExactly(42);
    }

    [TestCase]
    public void AssigningTheSameValueIsIgnored() {
        List<int> applied = new List<int>();
        SettingsServer server = BuildServer(applied);
        Setting<int> music = server.GetSetting<int>("audio", "music");
        applied.Clear();

        music.Value = music.Value;

        AssertArray(applied).IsEmpty();
    }

    [TestCase]
    public void PredicateRejectsValues() {
        List<int> applied = new List<int>();
        SettingsServer server = new SettingsServerBuilder(path)
            .WithSection("audio")
                .WithSetting("music", 100, applied.Add, value => value is >= 0 and <= 100)
            .Build();
        Setting<int> music = server.GetSetting<int>("audio", "music");
        applied.Clear();

        music.Value = 500;

        AssertInt(music.Value).IsEqual(100);
        AssertArray(applied).IsEmpty();
    }

    [TestCase]
    public void PredicateAcceptsValidValues() {
        List<int> applied = new List<int>();
        SettingsServer server = new SettingsServerBuilder(path)
            .WithSection("audio")
                .WithSetting("music", 100, applied.Add, value => value is >= 0 and <= 100)
            .Build();

        server.GetSetting<int>("audio", "music").Value = 50;

        AssertArray(applied).Contains(50);
    }

    [TestCase]
    public void ValuesArePersistedAcrossServers() {
        SettingsServer first = BuildServer(new List<int>());
        first.GetSetting<int>("audio", "music").Value = 42;

        // A fresh builder reads the saved file back.
        SettingsServer second = BuildServer(new List<int>());

        AssertInt(second.GetSetting<int>("audio", "music").Value).IsEqual(42);
        // The default passed to the builder is still the declared default.
        AssertInt(second.GetSetting<int>("audio", "music").Default).IsEqual(100);
    }

    [TestCase]
    public async Task TheInitialSetterCallIsDeferred() {
        List<int> applied = new List<int>();
        SettingsServer server = BuildServer(applied);

        // The value is available straight away, the setter is not: Setting's
        // constructor schedules it with CallDeferred.
        AssertInt(server.GetSetting<int>("audio", "music").Value).IsEqual(100);
        AssertArray(applied).IsEmpty();

        SceneTree tree = (SceneTree)Engine.GetMainLoop();
        await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);
        await tree.ToSignal(tree, SceneTree.SignalName.ProcessFrame);

        AssertArray(applied).Contains(100);
    }

    [TestCase]
    public void InjectAssignsSettingsToAnnotatedMembers() {
        SettingsServer server = BuildServer(new List<int>());
        Subject subject = AddNode(new Subject(), true);

        server.Inject(subject);

        AssertObject(subject.Music).IsNotNull();
        AssertObject(subject.Sfx).IsNotNull();
        AssertString(subject.Sfx.Section).IsEqual("audio");
        AssertString(subject.Sfx.Key).IsEqual("sfx");
    }

    [TestCase]
    public void InjectThrowsWhenTheMemberIsNotASetting() {
        SettingsServer server = BuildServer(new List<int>());
        NotASetting subject = AddNode(new NotASetting(), true);

        AssertThrown(() => server.Inject(subject)).HasMessage("Member _music is not of type Setting.");
    }
}