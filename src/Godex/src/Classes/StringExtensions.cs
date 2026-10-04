namespace Godex;

/// <summary>
/// Extensions for <see cref="string"/>.
/// </summary>
public static class StringExtensions {
    /// <summary>
    /// Converts the first character of <paramref name="str"/> to upper case, leaving the rest
    /// of the characters untouched.
    /// </summary>
    /// <param name="str">String to convert.</param>
    /// <returns><paramref name="str"/> with its first character in upper case.</returns>
    public static string ToPascalCase(this string str) {
        return str.Length == 0 ? str : char.ToUpperInvariant(str[0]) + str[1..];
    }
}
