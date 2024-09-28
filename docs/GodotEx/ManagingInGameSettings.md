# Managing In-Game Settings

## Problems

Almost every game allows users to tailor their experience by configuring in-game settings. For a small puzzle game, settings may include minimal options such as sound volumes, while AAA and sandbox games may have hundreds of thousands of settings.

The values of specific settings may need to be read from or written to all over the game. Without a centralized approach, low-level code that accesses the file system directly has to be written repeatedly.

## Solution

`SettingsServer` provides a high-level, centralized interface for loading, updating and saving settings from a `.cfg` file.

### Configuration

To start with, use a `SettingsServerBuilder` to configure your settings:

```csharp
SettingServer server = new SettingsServerBuilder()
    .With("audio", "music", 100,
        vol => AudioServer.SetBusVolumeDb(1, vol),
        vol => vol >= 0 && vol <= 100)
    .With("audio", "sfx", 100,
        vol => AudioServer.SetBusVolumeDb(2, vol),
        vol => vol >= 0 && vol <= 100)
    .With("graphics", "ssrl", false,
        value => ProjectSettings.SetSetting("rendering/anti_aliasing/screen_space_roughness_limiter/enabled", value))
    .With("general", "locale", "en", TranslationServer.SetLocale)
    .Build();
```

Each `.With()` chain configures a setting by using 4 parameters:

1. `section` - section where the setting is stored
2. `key` - key where the setting is mapped from
3. `setter` - action to perform when the setting is updated, given that the predicate is satisfied
4. `predicate` - conditions to check before updating the setting

> [!Note]
> The following shows the cfg file created by this builder:
> ```cfg
> [audio]
> 
> music=100
> sfx=100
> 
> [graphics]
> 
> ssrl=false
> 
> [general]
> 
> locale="en"
> ```

### Reading a setting

There are 2 APIs in `SettingsServer` for obtaining settings:

1. `GetSetting(string section, string key)`

    Used for obtaining a single setting by specifying its section and key.

    ```csharp
    var music = server.GetSetting<int>("audio", "music");
    GD.Print(music.Value);
    ```

2. `Inject(Node node)`

    Used for injecting multiple setting instances to properties and fields within the input node.

    ```csharp
    [Setting("audio", "music")]
    private Setting<int> _music;

    [Setting("audio", "sfx")]
    private Setting<int> _sfx;

    [Setting("graphics", "ssrl")]
    private Setting<bool> _ssrl;

    [Setting("general", "locale")]
    private Setting<string> _locale;

    private override void _Ready() {
        var server = new SettingsServerBuilder()
            .With(...)
            .Build();
        server.Inject(this);

        GD.Print(_music.Value);
        GD.Print(_locale.Value);
    }
    ```

> [!Tip]
> The examples above assume that settings is only needed in one class. In real life, the server should be accessed locally. For this, check out [GodotEx.Hosting](~/docs/GodotEx.Hosting/Hosting.md) where you can create an [autoload host](~/docs/GodotEx.Hosting/Hosting.md#setting-up-an-autoload-host) for hosting the `SettingsServer`.

### Updating a setting

To update a setting, simply assign a new value to it:

```csharp
var music = server.GetSetting<int>("audio", "music");
music.Value = 88;
```

The update executes in the following steps:

1. Checks if the value to assign satisfies the predicate
2. If the predicate is satisfied, the value is updated
3. Performs `setter` action
4. Fires the `Updated` event is fired, notifying any subscribers
5. Updates the config file
   
## Usage with `GodotEx.Hosting.Host`

It is **highly recommended** to use the `SettingsServer` with `GodotEx.Hosting.Host` which makes use of dependency injection for acquire the `SettingsServer` instance. Read more about hosting in [GodotEx.Hosting](~/docs/GodotEx.Hosting/Hosting.md).

```csharp
public partial class AppHost : Host {
    protected override void ConfigureServices(IServiceCollection services) {
        base.ConfigureServices(services);

        var settingsServer = new SettingsServerBuilder()
            // .With()
            .Build();
        services.AddSingleton(settingsServer);
    }
}
```
