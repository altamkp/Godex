using Godot;

namespace GodotEx.Tests;

public partial class SettingsTest : Node {
    private const string AUDIO = "audio";
    private const string MUSIC = "music";
    private const string SFX = "sfx";

    private const string GRAPHICS = "graphics";
    private const string SSRL = "ssrl";

    private const string GENERAL = "general";
    private const string LOCALE = "locale";

    [Setting(AUDIO, MUSIC)]
    private Setting<int> _music;

    [Setting(AUDIO, SFX)]
    private Setting<int> _sfx;

    [Setting(GRAPHICS, SSRL)]
    private Setting<bool> _ssrl;

    [Setting(GENERAL, LOCALE)]
    private Setting<string> _locale;

    public override void _Ready() {
        var server = new SettingsServerBuilder()
            .With(AUDIO, MUSIC, 100,
                vol => AudioServer.SetBusVolumeDb(1, vol),
                vol => vol >= 0 && vol <= 100)
            .With(AUDIO, SFX, 100,
                vol => AudioServer.SetBusVolumeDb(2, vol),
                vol => vol >= 0 && vol <= 100)
            .With(GRAPHICS, SSRL, false,
                value => ProjectSettings.SetSetting("rendering/anti_aliasing/screen_space_roughness_limiter/enabled", value))
            .With(GENERAL, LOCALE, "en", TranslationServer.SetLocale)
            .Build();

        server.Inject(this);

        GD.Print(_music.Value);
        GD.Print(_sfx.Value);
        GD.Print(_ssrl.Value);
        GD.Print(_locale.Value);

        _music.Value = 3;
    }
}
