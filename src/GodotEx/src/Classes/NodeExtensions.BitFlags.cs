using DotEx.Reflections;
using Godot;
using System.Reflection;

namespace GodotEx;

public partial class NodeExtensions {
    private static readonly Dictionary<Type, IEnumerable<FlagInfo>?> LAYER_INFOS = new();

    /// <summary>
    /// Set layer and mask properties by referencing the <see cref="BitFlagsAttribute"/>s
    /// on this <paramref name="node"/> and its fields and properties.
    /// </summary>
    /// <param name="node">Node to set.</param>
    public static void ResolveBitFlags(this Node node) {
        var type = node.GetType();
        if (!LAYER_INFOS.TryGetValue(type, out var infos)) {
            infos = GetLayerInfos(node, type);
            LAYER_INFOS.Add(type, infos);
        }

        foreach (var info in infos ?? Enumerable.Empty<FlagInfo>()) {
            info.Resolve(node);
        }
    }

    private static IEnumerable<FlagInfo>? GetLayerInfos(Node node, Type type) {
        var infos = new List<FlagInfo>();
        var attributes = type.GetCustomAttributes<BitFlagsAttribute>();
        foreach (var attribute in attributes) {
            var variantType = node.Get(attribute.PropertyName).VariantType;
            AssertVariantType(type.Name, attribute.PropertyName, variantType);
            infos.Add(new FlagInfo(attribute, null));
        }
        infos.AddRange(GetMemberLayerInfos(node, type));
        return infos.Any() ? infos : null;
    }

    private static IEnumerable<FlagInfo> GetMemberLayerInfos(Node node, Type type) {
        var infos = new List<FlagInfo>();
        var members = type.GetProperties(INSTANCE_FLAGS).Cast<MemberInfo>().Concat(type.GetFields(INSTANCE_FLAGS));
        foreach (var memberInfo in members) {
            var attributes = memberInfo.GetCustomAttributes<BitFlagsAttribute>();
            if (!attributes.Any()) {
                continue;
            }

            var member = memberInfo.GetValue(node) as Node
                ?? throw new InvalidCastException($"Member {memberInfo.Name} of {type.Name} must be derived from Node to use the Layer/Mask attribute.");
            foreach (var attribute in attributes) {
                var variantType = member.Get(attribute.PropertyName).VariantType;
                AssertVariantType(type.Name, attribute.PropertyName, variantType);
                infos.Add(new FlagInfo(attribute, memberInfo));
            }
        }
        return infos;
    }

    private static void AssertVariantType(string typeName, string propertyName, Variant.Type type) {
        if (type == Variant.Type.Nil) {
            throw new InvalidOperationException($"{typeName} does not have a property named {propertyName}.");
        }
        if (type != Variant.Type.Int) {
            throw new InvalidOperationException($"Property {propertyName} of {typeName} is not of type int.");
        }
    }

    private class FlagInfo {
        private readonly BitFlagsAttribute _attribute;
        private readonly MemberInfo? _member;

        public FlagInfo(BitFlagsAttribute attribute, MemberInfo? member) {
            _attribute = attribute;
            _member = member;
        }

        public void Resolve(Node node) {
            var target = _member?.GetValue(node) as Node ?? node;
            target.Set(_attribute.PropertyName, _attribute.Flags);
        }
    }
}
