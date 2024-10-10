namespace GodotEx.Hosting;

/// <summary>
/// Attribute that is recognized by <see cref="DependencyInjector"/> to inject field or 
/// property dependencies. Use with <see cref="GodotEx.Hosting.Host"/> is recommended.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class InjectAttribute : Attribute { 
    internal static readonly Type TYPE = typeof(InjectAttribute);
}
