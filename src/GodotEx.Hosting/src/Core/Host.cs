using Godot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GodotEx.Hosting;

/// <summary>
/// A node that provides hosting service. Override <see cref="ConfigureServices(IServiceCollection)"/>
/// to configure services added to the internal <see cref="Microsoft.Extensions.DependencyInjection.ServiceProvider"/>. 
/// <br/><br/>
/// Includes default services, namely:
/// <br/>Current <see cref="Host"/>
/// <br/><see cref="SceneTree"/>
/// <br/><see cref="DependencyInjector"/>
/// <br/><see cref="NodeResolver"/>
/// <br/><see cref="SingletonManager"/>
/// </summary>
/// <remarks>
/// <b>Note</b>: host nodes can exist any where within the current scene, 
/// but there may only be at most one autoload host.
/// </remarks>
public abstract partial class Host : Node {
    /// <summary>
    /// Static <see cref="Microsoft.Extensions.DependencyInjection.ServiceProvider"/> reference.
    /// </summary>
    public static ServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    /// Called when the node enters the Godot.SceneTree (e.g. upon instancing, scene
    /// changing, or after calling <see cref="Node.AddChild(Node, bool, InternalMode)"/>
    /// in a script). If the node has children, its Godot.Node._EnterTree callback will
    /// be called first, and then that of the children.
    /// </summary>
    public override void _EnterTree() {
        if (ServiceProvider != null) {
            throw new InvalidOperationException("Host exists already.");
        }

        var services = new ServiceCollection();
        services.AddSingleton(GetType(), this);
        services.AddSingleton(GetTree());
        services.AddSingletonHostedService<DependencyInjector>();
        services.AddSingletonHostedService<NodeResolver>();
        services.AddSingletonHostedService<SingletonManager>();
        ConfigureServices(services);

        AssertServiceTypes(services);
        ServiceProvider = services.BuildServiceProvider();

        foreach (IHostedService hostedService in ServiceProvider.GetServices<IHostedService>()) {
            hostedService.StartAsync(CancellationToken.None).GetAwaiter().GetResult();
        }

        static void AssertServiceTypes(IServiceCollection services) {
            var hostType = typeof(Host);
            var nodeType = typeof(Node);

            foreach (ServiceDescriptor desc in services) {
                var serviceType = desc.ServiceType;
                if (!serviceType.IsSubclassOf(hostType) && serviceType.IsSubclassOf(nodeType)) {
                    throw new InvalidOperationException("Godot nodes cannot be added to hosts, use SceneTree.AddSingleton() or [Singleton] instead.");
                }
            }
        }
    }

    /// <summary>
    /// Configures service collection.
    /// </summary>
    /// <param name="services">Service collection to configure.</param>
    protected virtual void ConfigureServices(IServiceCollection services) { }
}
