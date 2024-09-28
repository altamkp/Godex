# Hosting

A host is an object that manages the game's background services. A game can have multiple hosts with different life cycles. For example, you can have an application-scoped host that hosts services for audio and scene transitions, you can also have a scene-scoped host that hosts services for making http requests and create game levels.

The purpose of using a host is to provide an access point to any required background services, whether they are derived from `Node` or not. `GodotEx.Hosting` provides a `Host` node which can be added to your scenes to host background services. The easiest and recommended way is to create an [autoload](https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html#autoload) host that manages application-scoped background services, which is a special kind of host existing outside the current scene and may only have one instance.

## Setting up an Autoload Host

1. Create a new scene and name it `ApplicationHost`
2. Attach a C# script to it, also naming it `ApplicationHost`

   ![](~/images/ApplicationHostStructure.png)

3. Copy the following snippet to the script

   ```csharp
    using GodotEx.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    public partial class ApplicationHost : Host {
        // This line is required due to a Godot bug that doesn't run _EnterTree() in an external library
        public override void _EnterTree() => base._EnterTree();

        protected override void ConfigureServices(IServiceCollection services) {
            base.ConfigureServices(services);
            // Add your background services here
        }
    }
   ```

4. Configure your background services by override the `ConfigureServices(IServiceCollection)` method. There are many ways to add services to the service collection, but `AddSingleton()` is the easiest as these services share the same lifecycle as the host itself. You can learn more about [registering services](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-registration-methods), but you would only require `AddSingleton()` with Godot most of the time.

   As an example:

   ```csharp
    protected override void ConfigureServices(IServiceCollection services) {
        base.ConfigureServices(services);

        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<Configuration>();
        services.AddSingleton<SaveGame>();
    }
   ```

   If you have any children nodes under the `ApplicationHost` scene that you want to add as background services, you can do include the following snippet in `ConfigureServices(IServiceCollection)`:

   ```csharp
    foreach (var node in GetChildren()) {
        services.AddSingleton(node.GetType(), node);
    }
   ```

5. Setup the scene as an autoload in `Project > Project Settings... > Autoload`

   ![](~/images/ApplicationHostAutoload.png)

6. You can now access any of the background services via the autoload host instance:

   ```csharp
   var logger = Host.Autoload.GetRequiredService<ILogger>();
   var config = Host.Autoload.GetRequiredService<Configuration>();
   ```

## Dependency Injection

Service classes that do not derive from `Node` can benefit directly from [.NET dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-registration-methods), as long as this class and all its dependencies have been added to the service collection.

To inject dependencies, simply add the dependencies in the constructor as parameters. For example, say you have a `SaveGame` class for saving and loading game levels, and it depends on `ILogger` and `Configuration`:

```csharp
public class SaveGame {
    private readonly ILogger _logger;
    private readonly Configuration _config;

    public SaveGame(ILogger logger, Configuration config) {
        _logger = logger;
        _config = config;
    }

    public void Save(Level level) { /* Neglected */ }
    public Level Load(int id) { /* Neglected */ }
}
```

For classes that derive from `Node`, declare the dependencies as fields or properties and label them with the `[Inject]` attribute. Note that you have to set up the [autoload host](#setting-up-an-autoload-host) for this to work. As an example:

```csharp
public partial class Level : Node3D {
    [Inject] private SaveGame _saveGame;
}
```

## Service Initialization

Unless added with a concrete instance, background services are initialized lazily, which means that they are not initialized until they are required. The opposite of lazy loading is eager loading, which instantiates services as soon as they are added. To achieve eager loading, you can label your service class with the `[EagerAttribute]`.

## Default Services

The `Host` node comes with a number of default services with `base.ConfigureServices(services)`, which includes:

1. Current host registered by its concrete type
2. [SceneTree](https://docs.godotengine.org/en/stable/classes/class_scenetree.html)
3. `DependencyInjector` - responsible for injecting dependencies labeled by the [[Inject]] attribute to classes derived from `Node`
4. `NodeResolver` - responsible for resolving nodes that define the [[NodePath]](~/docs/GodotEx/ResolvingNodeDependencies.md) attribute or [[Layer]/[Mask]](~/docs/GodotEx/ResolvingBitFlags.md) attributes
5. `SingleNodeManager` - responsible for adding and removing nodes labeled with the [[SingleNode]](~/docs/GodotEx/SingleNodes.md) attribute to the `SceneTree` as single nodes
