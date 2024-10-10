using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.RegularExpressions;

namespace GodotEx.Hosting.Tests;

public partial class ApplicationHost : Host {
    public override void _EnterTree() {
       base._EnterTree();
    }

    protected override void ConfigureServices(IServiceCollection services) {
        base.ConfigureServices(services);

        services.AddSingleton(new Random());
        services.AddSingleton(new Regex(string.Empty));

        foreach (var child in GetChildren()) {
            services.AddSingleton(child.GetType(), child);
        }
    }
}
