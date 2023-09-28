using System;
using System.Drawing;
using SDL2;

namespace RoguelikeToolkit.NET.Drawing
{
    /// <summary>
    /// Represents a font loaded from an image sheet for text-based rendering.
    /// </summary>
    public class TerminalFont
    {
        /// <summary>
        /// Gets the input image sheet file path used for the font.
        /// </summary>
        public string FontImagepath { get; private set; }

        /// <summary>
        /// Gets the height of each glyph in the font.
        /// </summary>
        public int GlyphHeight { get; private set; }

        /// <summary>
        /// Gets the width of each glyph in the font.
        /// </summary>
        public int GlyphWidth { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the layout of the font sheet is row major.
        /// </summary>
        public bool IsRowMajor { get; private set; }

        /// <summary>
        /// Gets the number of glyphs per row in the font sheet.
        /// </summary>
        public int GlyphsPerRow { get; private set; }

        /// <summary>
        /// Gets the number of glyphs per column in the font sheet.
        /// </summary>
        public int GlyphsPerColumn { get; private set; }

        /// <summary>
        /// Gets the OpenGL texture ID for the font sheet.
        /// </summary>
        public int TextureID { get; private set; }

        /// <summary>
        /// Gets the width of the font texture.
        /// </summary>
        public int TextureWidth { get; internal set; }
        /// <summary>
        /// Gets the height of the font texture.
        /// </summary>
        public int TextureHeight { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalFont"/> class with the specified input image, glyph height, glyph width, and layout.
        /// </summary>
        /// <param name=" filepath">The input image sheet file path used for the font.</param>
        /// <param name="glyphHeight">The height of each glyph in the font.</param>
        /// <param name="glyphWidth">The width of each glyph in the font.</param>
        /// <param name="isRowMajor">A value indicating whether the layout of the font sheet is row major.</param>
        /// <exception cref="ArgumentNullException">Thrown if fontImage is null.</exception>
        /// <exception cref="FileNotFoundException">Thrown if Font image was not found.</exception>
        public TerminalFont(string filepath, int glyphHeight, int glyphWidth, bool isRowMajor)
        {
            FontImagepath = Environment.CurrentDirectory + "\\" + filepath;
            if (!System.IO.File.Exists(FontImagepath))
                throw new System.IO.FileNotFoundException("Font image was not found.", FontImagepath);

            if (glyphHeight <= 0 || glyphWidth <= 0)
                throw new ArgumentException("Glyph height and width must be positive integers.");


            GlyphHeight = glyphHeight;
            GlyphWidth = glyphWidth;
            IsRowMajor = isRowMajor;
            var fontimage = Image.FromFile(FontImagepath);

            TextureWidth = fontimage.Width;
            TextureHeight = fontimage.Height;

            // Calculate the number of glyphs per row and column based on the layout.
            if (IsRowMajor)
            {
                GlyphsPerRow = TextureWidth / glyphWidth;
                GlyphsPerColumn = TextureHeight / glyphHeight;
            }
            else
            {
                GlyphsPerRow = TextureWidth / glyphHeight;
                GlyphsPerColumn = TextureWidth / glyphWidth;
            }

            // Initialize the OpenGL texture ID (you'll need to set this in your rendering code).
            TextureID = 0; // Set this to the appropriate value.
        }
    }
}
