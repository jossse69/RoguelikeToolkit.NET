using System.Drawing;
using SDL2;
namespace RoguelikeToolkit.NET.Drawing
{
    /// <summary>
    /// Represents a colored glyph for text-based rendering.
    /// </summary>
    public class ColoredGlyph
    {
        /// <summary>
        /// Gets or sets the character (glyph) to be displayed.
        /// </summary>
        public char? Glyph { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the glyph.
        /// </summary>
        public Color? ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color behind the glyph.
        /// </summary>
        public Color? BackgroundColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColoredGlyph"/> class with the specified glyph, foreground color, and background color.
        /// </summary>
        /// <param name="glyph">The character (glyph) to be displayed.</param>
        /// <param name="foregroundColor">The foreground color of the glyph.</param>
        /// <param name="backgroundColor">The background color behind the glyph.</param>
        public ColoredGlyph(char? glyph, Color? foregroundColor, Color? backgroundColor)
        {
            Glyph = glyph;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        internal ColoredGlyph Clone()
        {
            var ClonedColoredGlyph = new ColoredGlyph(Glyph, ForegroundColor, BackgroundColor);

            return ClonedColoredGlyph;
        }
    }
}
