using DotEx.Reflections;
using Godot;
using System.Reflection;

namespace GodotEx;

public static partial class NodeExtensions {
    private static readonly Dictionary<Type, IEnumerable<NodePathInfo>> NODE_PATH_INFOS = new();

    /// <summary>
    /// Resolve field and property node dependencies labeled by <see cref="NodePathAttribute"/>.
    /// See <see cref="NodePathAttribute"/> for more.
    /// </summary>
    /// <param name="node">Node to resolve.</param>
    /// <exception cref="InvalidOperationException">Dependency not assignable to member.</exception>
    public static void ResolveNodePaths(this Node node) {
        var type = node.GetType();

        if (!NODE_PATH_INFOS.TryGetValue(type, out var dependencies)) {
            var fields = type.GetFieldsAndAttributes<NodePathAttribute>(INSTANCE_FLAGS)
                .Select(f => new NodePathInfo(f.FieldInfo, f.Attributes));
            var properties = type.GetPropertiesAndAttributes<NodePathAttribute>(INSTANCE_FLAGS)
                .Select(p => new NodePathInfo(p.PropertyInfo, p.Attributes));
            dependencies = fields.Concat(properties).ToArray();
            NODE_PATH_INFOS.Add(type, dependencies);
        }

        foreach (var dependency in dependencies) {
            dependency.Resolve(node);
        }
    }

    private class NodePathInfo {
        private readonly MemberInfo _member;
        private readonly IEnumerable<NodePathAttribute> _attributes;

        public NodePathInfo(MemberInfo member, IEnumerable<NodePathAttribute> attributes) {
            _member = member;
            _attributes = attributes;
        }

        public void Deconstruct(out MemberInfo member, out IEnumerable<NodePathAttribute> attributes) {
            member = _member;
            attributes = _attributes;
        }

        public void Resolve(Node node) {
            var path = _attributes.Single().Path;
            Node dependency;
            if (path != null) {
                dependency = node.GetNode(path);
            } else {
                var name = _member.Name.TrimStart('_').ToPascalCase();
                dependency = node.GetNodeOrNull(name) ?? node.GetNode($"%{name}");
            }

            var dependencyType = dependency.GetType();
            var memberType = _member.GetMemberType();
            if (!dependencyType.IsAssignableTo(memberType)) {
                throw new InvalidOperationException($"Dependency of type {dependencyType} is not assignable to {memberType}.");
            }
            _member.SetValue(node, dependency);
        }
    }
}
