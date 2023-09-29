using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using SDL2;

namespace RoguelikeToolkit.NET.Drawing
{
    /// <summary>
    /// Manages rendering using SDL2-CS and can render Terminal instances.
    /// </summary>
    public class Renderer
    {
        private IntPtr window;
        public Terminal selectedTerminal; // Store the selected terminal here
        private Terminal previousTerminal;
        private IntPtr SDLRenderer;
        private IntPtr fontTexture; // SDL texture to hold the font


        public static Action tick;

        /// <summary>
        /// Initializes a new instance of the <see cref="Renderer"/> class with the specified width and height.
        /// </summary>
        /// <param name="width">The width of the rendering window.</param>
        /// <param name="height">The height of the rendering window.</param>
        public Renderer(int width, int height, Terminal initialTerminal, string title)
        {
            if (SDL.SDL_Init(SDL.SDL_INIT_VIDEO) != 0)
            {
                throw new Exception("SDL_Init failed: " + SDL.SDL_GetError());
            }
            else
            {
                System.Console.WriteLine("Simple DirectMedia Layer 2 initiated. (v" + SDL.SDL_COMPILEDVERSION + ")");
            }

            window = SDL.SDL_CreateWindow(title, SDL.SDL_WINDOWPOS_UNDEFINED, SDL.SDL_WINDOWPOS_UNDEFINED, width, height, SDL.SDL_WindowFlags.SDL_WINDOW_OPENGL);

            selectedTerminal = initialTerminal; // Set the initial selected terminal
            SDLRenderer = SDL.SDL_CreateRenderer(window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

            // Load the font file as an SDL surface
            IntPtr fontSurface = SDL.SDL_LoadBMP(initialTerminal.Font.FontImagepath);

            // Check if the surface was loaded successfully
            if (fontSurface == IntPtr.Zero)
            {
                Console.WriteLine("Failed to load font surface: " + SDL.SDL_GetError());
                // Handle the error appropriately
            }
            else
            {
                Console.WriteLine("Font surface loaded successfully.");
                // Proceed with creating a texture from the surface
            }


            // Load the font texture here (replace 'YourFontTexturePath' with the actual path to your font texture)
            fontTexture = SDL.SDL_CreateTextureFromSurface(SDLRenderer, fontSurface);

            if (fontTexture == IntPtr.Zero)
            {
                // Print an error message if the texture failed to load.
                Console.WriteLine("Failed to load font texture: " + SDL.SDL_GetError());
            }
            else
            {
                Console.WriteLine("Font texture loaded successfully.");
            }

            tick = new Action(Tick);
        }

        /// <summary>
        /// Handles SDL keyborad related input.
        /// </summary>
        public virtual void ProcessKeys(SDL.SDL_KeyboardEvent keyboardEvent)
        {
            // Override this method in a derived class to handle keyboard input.
        }

        /// <summary>
        /// Handles SDL mouse related input.
        /// </summary>
        public virtual void ProcessMouse(SDL.SDL_MouseButtonEvent buttonEvent, SDL.SDL_MouseMotionEvent motionEvent, SDL.SDL_MouseWheelEvent wheelEvent)
        {
            // Override this method in a derived class to handle mouse input.
        }

        /// <summary>
        /// Handles cleanup when the window is closing.
        /// </summary>
        private void Close(CancelEventArgs args)
        {
            SDL.SDL_DestroyRenderer(SDLRenderer);
            SDL.SDL_DestroyWindow(window);
            SDL.SDL_DestroyTexture(fontTexture); // Destroy the font texture
            SDL.SDL_Quit();
        }

        /// <summary>
        /// Renders a character from a Terminal to the window.
        /// </summary>
        /// <param name="terminal">The Terminal containing the character to render.</param>
        /// <param name="characterX">The X-coordinate of the character within the Terminal.</param>
        /// <param name="characterY">The Y-coordinate of the character within the Terminal.</param>
        /// <param name="font">The TerminalFont to use for rendering.</param>
        /// <param name="x">The X-coordinate where the character should be rendered.</param>
        /// <param name="y">The Y-coordinate where the character should be rendered.</param>
        public void RenderCharacter(Terminal terminal, int characterX, int characterY, TerminalFont font, int x, int y)
        {
            ColoredGlyph glyph = terminal.GetGlyph(characterX, characterY);

            // Calculate the size of each glyph in texture coordinates.
            float glyphWidth = 1.0f / font.GlyphsPerRow;
            float glyphHeight = 1.0f / font.GlyphsPerColumn;

            // Calculate the texture coordinates for the glyph in the font texture.
            float texX, texY;

            if (font.IsRowMajor)
            {
                // Calculate texture coordinates for row-major layout.
                texX = (float)glyph.Glyph % font.GlyphsPerColumn;
                texY = (float)glyph.Glyph / font.GlyphsPerRow;
            }
            else
            {
                // Calculate texture coordinates for column-major layout.
                texX = (float)glyph.Glyph / font.GlyphsPerColumn;
                texY = (float)glyph.Glyph % font.GlyphsPerRow;
            }

            // Calculate the position and size of the glyph in the texture.
            int texWidth = font.TextureWidth / font.GlyphsPerRow;
            int texHeight = font.TextureHeight / font.GlyphsPerColumn;
            int texPosX = (int)(texX * texWidth);
            int texPosY = (int)(texY * texHeight);

            // Create a rectangle that represents the source area of the texture for the glyph.
            SDL.SDL_Rect srcRect = new SDL.SDL_Rect
            {
                x = texPosX,
                y = texPosY,
                w = texWidth,
                h = texHeight
            };

            // Create a rectangle that represents the destination area on the screen for the background.
            SDL.SDL_Rect bgRect = new SDL.SDL_Rect
            {
                x = x,
                y = y,
                w = font.GlyphWidth,
                h = font.GlyphHeight
            };

            // Set the color modulation for the font texture (glyph color).
            SDL.SDL_SetTextureColorMod(fontTexture, (byte)glyph.ForegroundColor.Value.R, (byte)glyph.ForegroundColor.Value.G, (byte)glyph.ForegroundColor.Value.B);

            // Set the color for rendering the background (bg color).
            SDL.SDL_SetRenderDrawColor(SDLRenderer, (byte)glyph.BackgroundColor.Value.R, (byte)glyph.BackgroundColor.Value.G, (byte)glyph.BackgroundColor.Value.B, 255);

            // Fill the background rectangle with the bg color.
            SDL.SDL_RenderFillRect(SDLRenderer, ref bgRect);

            // Bind the font texture.
            SDL.SDL_SetRenderTarget(SDLRenderer, fontTexture);

            // Render the portion of the texture onto the screen.
            SDL.SDL_RenderCopy(SDLRenderer, fontTexture, ref srcRect, ref bgRect);

            // Restore the render target to the window.
            SDL.SDL_SetRenderTarget(SDLRenderer, IntPtr.Zero);
        }








        /// <summary>
        /// Renders a Terminal to the window.
        /// </summary>
        /// <param name="terminal">The Terminal to render.</param>
        public void RenderTerminal(Terminal terminal)
        {
            int terminalWidth = terminal.Width;
            int terminalHeight = terminal.Height;

            TerminalFont font = terminal.Font; // Get the font from the Terminal instance.

            // Calculate the size of each glyph in pixels.
            int glyphWidth = font.GlyphWidth;
            int glyphHeight = font.GlyphHeight;

            bool terminalChanged = !TerminalStatesEqual(terminal, previousTerminal); // Check if the terminal has changed

            if (terminalChanged)
            {
                // Clear the renderer
                SDL.SDL_RenderClear(SDLRenderer);

                // Loop through the terminal grid and render each character.
                for (int y = 0; y < terminalHeight; y++)
                {
                    for (int x = 0; x < terminalWidth; x++)
                    {
                        ColoredGlyph Glyph = terminal.GetGlyph(x, y);

                        // if Glyph is null
                        if (Glyph == null)
                        {
                            continue;
                        }

                        // Calculate the position where the character should be rendered.
                        int renderX = x * glyphWidth;
                        int renderY = y * glyphHeight;

                        // Render the character using the RenderCharacter method.
                        RenderCharacter(terminal, x, y, font, renderX, renderY);
                    }
                }

                SDL.SDL_RenderPresent(SDLRenderer);

                // Update the previous terminal state
                previousTerminal = terminal.Clone(); // Clone the current terminal to create a copy
            }
        }


        private bool TerminalStatesEqual(Terminal terminal1, Terminal terminal2)
        {
            // Check if either of the terminals is null
            if (terminal1 == null || terminal2 == null)
            {
                return false;
            }

            // Compare terminal properties (width, height, font)
            if (terminal1.Width != terminal2.Width || terminal1.Height != terminal2.Height || !terminal1.Font.Equals(terminal2.Font))
            {
                return false;
            }

            // Compare each glyph in the terminal
            for (int y = 0; y < terminal1.Height; y++)
            {
                for (int x = 0; x < terminal1.Width; x++)
                {
                    ColoredGlyph glyph1 = terminal1.GetGlyph(x, y);
                    ColoredGlyph glyph2 = terminal2.GetGlyph(x, y);

                    // Compare individual glyphs
                    if (!ColoredGlyphsEqual(glyph1, glyph2))
                    {
                        return false;
                    }
                }
            }

            // All checks passed; the terminals are equal
            return true;
        }

        private bool ColoredGlyphsEqual(ColoredGlyph glyph1, ColoredGlyph glyph2)
        {
            // Check if both glyphs are null
            if (glyph1 == null && glyph2 == null)
            {
                return true;
            }

            // Check if one of the glyphs is null while the other is not
            if (glyph1 == null || glyph2 == null)
            {
                return false;
            }

            // Compare glyph properties (character, foreground color, background color)
            return glyph1.Glyph == glyph2.Glyph &&
                   glyph1.ForegroundColor == glyph2.ForegroundColor &&
                   glyph1.BackgroundColor == glyph2.BackgroundColor;
        }


        /// <summary>
        /// Handles SDL events and runs the game loop.
        /// </summary>
        public void RunGameLoop()
        {
            bool quit = false;
            SDL.SDL_Event e;

            while (!quit)
            {
                while (SDL.SDL_PollEvent(out e) != 0)
                {
                    if (e.type == SDL.SDL_EventType.SDL_QUIT)
                    {
                        quit = true;
                    }
                    else if (e.type == SDL.SDL_EventType.SDL_KEYDOWN || e.type == SDL.SDL_EventType.SDL_KEYUP)
                    {
                        // Handle keyboard events
                        ProcessKeys(e.key);
                    }
                    else if (e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN || e.type == SDL.SDL_EventType.SDL_MOUSEBUTTONUP ||
                             e.type == SDL.SDL_EventType.SDL_MOUSEMOTION || e.type == SDL.SDL_EventType.SDL_MOUSEWHEEL)
                    {
                        // Handle mouse events
                        ProcessMouse(e.button, e.motion, e.wheel);
                    }
                }

                RenderTerminal(selectedTerminal); // Render the selected terminal
                tick.Invoke();
            }
        }

        public virtual void Tick()
        {
            
        }
    }
}

