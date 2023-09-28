using System;
using System.Drawing;
using RoguelikeToolkit.NET.Drawing;
using SDL2;

namespace RoguelikeDemo
{
    class Program
    {
        public static TerminalFont font = new TerminalFont("terminal8x8_gs_ro.bmp", 8, 8, true);
        public static Terminal terminal = new Terminal(80, 45, font);
        static void Main(string[] args)
        {

            // Initialize player position
            int playerX = 5;
            int playerY = 5;

           

            Renderer renderer = new CustomRenderer(terminal.Width * font.GlyphWidth, terminal.Height * font.GlyphHeight, terminal, "RoguelikeToolkit.NET - demo 1", playerX, playerY);

            // Game loop:
            renderer.RunGameLoop();
        }
    }

    // Create a custom renderer class that overrides ProcessKeys to handle player movement
    class CustomRenderer : Renderer
    {
        private int playerX;
        private int playerY;

        public CustomRenderer(int width, int height, Terminal initialTerminal, string title, int initialPlayerX, int initialPlayerY)
            : base(width, height, initialTerminal, title)
        {
            playerX = initialPlayerX;
            playerY = initialPlayerY;
        }

        // Override ProcessKeys to handle player movement
        public override void ProcessKeys(SDL.SDL_KeyboardEvent keyboardEvent)
        {
            // Handle keyboard input for player movement
            switch (keyboardEvent.keysym.sym)
            {
                case SDL.SDL_Keycode.SDLK_UP:
                    MovePlayer(0, -1);
                    break;
                case SDL.SDL_Keycode.SDLK_DOWN:
                    MovePlayer(0, 1);
                    break;
                case SDL.SDL_Keycode.SDLK_LEFT:
                    MovePlayer(-1, 0);
                    break;
                case SDL.SDL_Keycode.SDLK_RIGHT:
                    MovePlayer(1, 0);
                    break;
                default:
                    break;
            }
        }

        // Helper method to move the player and update the terminal
        private void MovePlayer(int deltaX, int deltaY)
        {
 

            // Calculate the new player position
            playerX += deltaX;
            playerY += deltaY;

            // Render the updated terminal
            RenderTerminal(selectedTerminal);
        }

        public override void Tick()
        {
            // clear the terminal
            Program.terminal.Clear(new ColoredGlyph(char.ConvertFromUtf32(0).ElementAt(0), Color.DarkBlue, Color.DarkBlue));

            // draw a glyph, like a lil' player Glyph!
            Program.terminal.SetGlyph(playerX, playerY, new ColoredGlyph('@', Color.Aquamarine, Color.Black));

            // draw an extra glyph
            Program.terminal.SetGlyph(10, 5, new ColoredGlyph('!', Color.White, Color.Red));

            // draw a rectangle of spades
            Program.terminal.DrawRectangle(9, 1, 14, 3, new ColoredGlyph(char.ConvertFromUtf32(6).ElementAt(0), Color.Blue, Color.Black), new ColoredGlyph(' ', Color.Black, Color.Black));

            // draw some text
            Program.terminal.PrintText("Hello world!", 10, 2, Color.Pink, Color.Black);
        }
    }
}
