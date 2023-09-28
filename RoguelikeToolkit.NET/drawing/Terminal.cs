using System;
using System.Drawing;

namespace RoguelikeToolkit.NET.Drawing
{
    /// <summary>
    /// Represents a 2D grid of colored glyphs for text-based rendering.
    /// </summary>
    public class Terminal
    {
        /// <summary>
        /// Gets the width of the terminal.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets the height of the terminal.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets the font used for rendering in the terminal.
        /// </summary>
        public TerminalFont Font { get; private set; }

        private ColoredGlyph[,] grid;

        /// <summary>
        /// Initializes a new instance of the <see cref="Terminal"/> class with the specified width, height, and font.
        /// </summary>
        /// <param name="width">The width of the terminal grid.</param>
        /// <param name="height">The height of the terminal grid.</param>
        /// <param name="font">The font used for rendering in the terminal.</param>
        /// <exception cref="ArgumentException">Thrown if width or height is not a positive integer.</exception>
        public Terminal(int width, int height, TerminalFont font)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentException("Width and height must be positive integers.");

            Width = width;
            Height = height;
            Font = font;
            grid = new ColoredGlyph[width, height];
        }

        /// <summary>
        /// Sets the colored glyph at the specified position in the terminal grid.
        /// </summary>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        /// <param name="glyph">The colored glyph to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the bounds of the terminal grid.</exception>
        public void SetGlyph(int x, int y, ColoredGlyph glyph)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("Invalid position.");

            grid[x, y] = glyph;
        }

        /// <summary>
        /// Gets the colored glyph at the specified position in the terminal grid.
        /// </summary>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        /// <returns>The colored glyph at the specified position.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the bounds of the terminal grid.</exception>
        public ColoredGlyph GetGlyph(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                throw new ArgumentOutOfRangeException("Invalid position.");

            return grid[x, y];
        }

        /// <summary>
        /// Fills the terminal grid with junk (random <see cref="ColoredGlyph"/>s). 
        /// </summary>
        public void FillWithJunk()
        {
            var rng = new Random();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // Create a random colored glyph with random foreground and background colors.
                    ColoredGlyph randomGlyph = new ColoredGlyph(
                        (char)rng.Next(0, 255), // Random character within ASCII range
                        Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255)),
                        Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255))
                    );

                    // Set the random colored glyph at the current position in the grid.
                    SetGlyph(x, y, randomGlyph);
                }
            }
        }

        /// <summary>
        /// Clears the terminal by setting all glyphs to the specified default glyph.
        /// </summary>
        /// <param name="defaultGlyph">The default glyph to clear with.</param>
        public void Clear(ColoredGlyph defaultGlyph)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    SetGlyph(x, y, defaultGlyph);
                }
            }
        }

        /// <summary>
        /// Prints text on the terminal at the specified position with the given colors.
        /// </summary>
        /// <param name="text">The text to print.</param>
        /// <param name="x">The X-coordinate of the position.</param>
        /// <param name="y">The Y-coordinate of the position.</param>
        /// <param name="foregroundColor">The foreground color of the text.</param>
        /// <param name="backgroundColor">The background color behind the text.</param>
        public void PrintText(string text, int x, int y, Color foregroundColor, Color backgroundColor)
        {
            int textX = x;
            int textY = y;

            foreach (char character in text)
            {
                if (character == '\n')
                {
                    // Handle newline character
                    textX = x;
                    textY++;
                }
                else
                {
                    // Create a colored glyph for the character
                    ColoredGlyph glyph = new ColoredGlyph(character, foregroundColor, backgroundColor);

                    // Set the glyph at the current position
                    SetGlyph(textX, textY, glyph);

                    // Move to the next position
                    textX++;

                    // Check for text wrapping
                    if (textX >= Width)
                    {
                        textX = x;
                        textY++;
                    }
                }

                // Check for reaching the bottom of the terminal
                if (textY >= Height)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Draws a filled rectangle on the terminal with the specified border and fill glyphs.
        /// </summary>
        /// <param name="x">The X-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="y">The Y-coordinate of the top-left corner of the rectangle.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        /// <param name="borderGlyph">The colored glyph to use for the rectangle's border.</param>
        /// <param name="fillGlyph">The colored glyph to use for the rectangle's interior.</param>
        public void DrawRectangle(int x, int y, int width, int height, ColoredGlyph borderGlyph, ColoredGlyph fillGlyph)
        {
            for (int row = y; row < y + height; row++)
            {
                for (int col = x; col < x + width; col++)
                {
                    // Check if it's the border or inside the rectangle
                    if (col == x || col == x + width - 1 || row == y || row == y + height - 1)
                    {
                        SetGlyph(col, row, borderGlyph);
                    }
                    else
                    {
                        SetGlyph(col, row, fillGlyph);
                    }
                }
            }
        }

        public Terminal Clone()
        {
            // Create a new Terminal with the same dimensions and font
            Terminal clonedTerminal = new Terminal(Width, Height, Font);

            // Copy the glyphs from the original Terminal to the cloned one
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    ColoredGlyph originalGlyph = GetGlyph(x, y);
                    if (originalGlyph == null) continue;

                    clonedTerminal.SetGlyph(x, y, originalGlyph.Clone()); // Assuming ColoredGlyph has a Clone method
                }
            }

            return clonedTerminal;
        }
    }
}
