using Godot;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Godex.Hosting;

internal class DependencyInjector : IHostedService, IDisposable {
    private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private readonly SceneTree _sceneTree;

    private readonly Dictionary<Type, MemberInfo[]> _members = new();

    public DependencyInjector(SceneTree sceneTree) {
        _sceneTree = sceneTree;
        _sceneTree.NodeAdded += Inject;
    }

    public Task StartAsync(CancellationToken _) => Task.CompletedTask;
    public Task StopAsync(CancellationToken _) => Task.CompletedTask;

    public void Dispose() {
        _sceneTree.NodeAdded -= Inject;
    }

    private void Inject(Node node) {
        var type = node.GetType();

        if (!_members.TryGetValue(type, out var members)) {
            members = GetMembersRecursive(type, Enumerable.Empty<MemberInfo>()).ToArray();
            _members[type] = members;
        }

        foreach (var member in members) {
            var memberType = member.GetMemberType();
            var service = Host.ServiceProvider.GetService(memberType)
                ?? throw new InvalidOperationException($"Service of type {memberType} not found.");
            member.SetValue(node, service);
        }
        return;

        static IEnumerable<MemberInfo> GetMembersRecursive(Type type, IEnumerable<MemberInfo> previous) {
            var members = type
                .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(mi => mi.IsDefined(typeof(InjectAttribute)));
            
            var results = previous.Union(members);
            if (!type.BaseType?.Namespace?.Contains(nameof(Godot)) ?? false) {
                results = GetMembersRecursive(type.BaseType!, results);
            }
            return results;
        }
    }
}
