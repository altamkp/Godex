# Hosting

A host is an object that manages the game's background services. Its purpose is to provide an access point to any required background services, whether they are derived from `Node` or not. `Godex.Hosting` provides a `Host` node which can be added to your scenes to host background services.

There can only be **one** host in a scene tree. `Host.ServiceProvider` is a static provider and entering the tree with a second `Host` throws an `InvalidOperationException`. The easiest and recommended way to host application-scoped background services is to create an [autoload](https://docs.godotengine.org/en/stable/tutorials/scripting/singletons_autoload.html#autoload) host, which is a special kind of host existing outside the current scene. Host nodes can otherwise exist anywhere within the current scene.

## Setting up an Autoload Host

1. Create a new scene and name it `ApplicationHost`
2. Attach a C# script to it, also naming it `ApplicationHost`

   ![](~/images/ApplicationHostStructure.png)

3. Copy the following snippet to the script

   ```csharp
    using Godex.Hosting;
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
        services.AddSingleton(new Random());
        services.AddSingleton<SaveGame>();
    }
   ```

   If you have any children nodes under the `ApplicationHost` scene that you want to add as background services, you can include the following snippet in `ConfigureServices(IServiceCollection)`:

   ```csharp
    foreach (var node in GetChildren()) {
        services.AddSingleton(node.GetType(), node);
    }
   ```

5. Setup the scene as an autoload in `Project > Project Settings... > Autoload`

   ![](~/images/ApplicationHostAutoload.png)

6. You can now access any of the background services through the host's static `ServiceProvider`:

   ```csharp
   var logger = Host.ServiceProvider.GetRequiredService<ILogger>();
   var random = Host.ServiceProvider.GetRequiredService<Random>();
   ```

## Dependency Injection

Service classes that do not derive from `Node` can benefit directly from [.NET dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection#service-registration-methods), as long as this class and all its dependencies have been added to the service collection.

To inject dependencies, simply add the dependencies in the constructor as parameters. For example, say you have a `SaveGame` class for saving and loading game levels, and it depends on `ILogger` and `Random`:

```csharp
public class SaveGame {
    private readonly ILogger _logger;
    private readonly Random _random;

    public SaveGame(ILogger logger, Random random) {
        _logger = logger;
        _random = random;
    }

    public void Save(Level level) { /* Neglected */ }
    public Level Load(int id) { /* Neglected */ }
}
```

For classes that derive from `Node`, declare the dependencies as fields or properties and label them with the `[Inject]` attribute. `[Inject]` members of your own base classes are injected as well. Note that you have to set up the [autoload host](#setting-up-an-autoload-host) for this to work, and that a missing service throws an `InvalidOperationException`. As an example:

```csharp
public partial class Level : Node3D {
    [Inject] private SaveGame _saveGame;
}
```

## Service Initialization

Services added without a concrete instance are initialized lazily, which means that they are not instantiated until they are required. Passing an instance to `AddSingleton()`, as with `services.AddSingleton(new Random())` above, skips this and the given instance is used as is.

For services that need to do work when the host starts and stops, implement [IHostedService](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostedservice) and register it with the `AddSingletonHostedService<T>()` extension:

```csharp
public class SaveGame : IHostedService {
    public Task StartAsync(CancellationToken _) => Task.CompletedTask;
    public Task StopAsync(CancellationToken _) => Task.CompletedTask;
}

protected override void ConfigureServices(IServiceCollection services) {
    base.ConfigureServices(services);
    services.AddSingletonHostedService<SaveGame>();
}
```

Hosted services are started when the host enters the scene tree and stopped when it leaves it.

## Default Services

The `Host` node comes with a number of default services added by `base.ConfigureServices(services)`, which includes:

1. Current host registered by its concrete type
2. [SceneTree](https://docs.godotengine.org/en/stable/classes/class_scenetree.html)
3. `DependencyInjector` - responsible for injecting dependencies labeled by the [[Inject]] attribute to classes derived from `Node`
4. `NodeResolver` - responsible for resolving nodes that define the [[NodePath]](~/Godex/ResolvingNodeDependencies.md), [[BitFlags]](~/Godex/ResolvingBitFlags.md) or [[Group]](~/Godex/ResolvingGroups.md) attributes
5. `SingletonManager` - responsible for adding and removing nodes labeled with the [[Singleton]](~/Godex/SingletonNodes.md) attribute to the `SceneTree` as singleton nodes