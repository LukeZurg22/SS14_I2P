using System.Drawing;

namespace SS14_I2P.Models;

#pragma warning disable CS8618
public static class ColorExtensions
{
    /// Converts System.Drawing.Color to RGB HEX.
    public static string ToHex(this Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";
}