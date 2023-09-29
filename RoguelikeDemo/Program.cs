using System;
using System.Drawing;
using RoguelikeToolkit.NET.Drawing;
using RoguelikeToolkit.NET.FieldOfView;
using RoguelikeToolkit.NET.Maps;
using SDL2;

namespace RoguelikeDemo
{
    class Program
    {
        public static TerminalFont font = new TerminalFont("terminal8x8_gs_ro.bmp", 8, 8, true);
        public static Terminal terminal = new Terminal(80, 45, font);
        public static Gamemap map = new Gamemap();
        public static ShadowCastingFOV FOV = new ShadowCastingFOV(map);
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
            Program.FOV.ClearFOV();

            // Calculate the new player position
            playerX += deltaX;
            playerY += deltaY;

            Program.FOV.ComputeFOV(playerX, playerY, 8);
            Program.FOV.PrintFOV();
        }

        public override void Tick()
        {
            // clear the terminal
            Program.terminal.Clear(new ColoredGlyph(char.ConvertFromUtf32(0).ElementAt(0), Color.DarkBlue, Color.DarkBlue));


            // render map tiles 48
            for (var x = 0; x < Program.map.Width; x++)
            {
                for (var y = 0; y < Program.map.Height; y++)
                {
                    if (Program.FOV.IsInFOV(x, y))
                    {
                        if (Program.map.IsBlocked(x, y))
                        {
                            Program.terminal.SetGlyph(x, y, new ColoredGlyph(char.ConvertFromUtf32(48).ElementAt(0), Color.BlueViolet, null));
                        }
                        else
                        {
                            Program.terminal.SetGlyph(x, y, new ColoredGlyph(char.ConvertFromUtf32(59).ElementAt(0), Color.Orange, null));
                        }
                    }
                    else
                    {
                        Program.terminal.SetGlyph(x, y, new ColoredGlyph(char.ConvertFromUtf32(0).ElementAt(0), Color.Black, Color.Black));
                    }
                }
            }

            // draw a glyph, like a lil' player Glyph!
            Program.terminal.SetGlyph(playerX, playerY, new ColoredGlyph('@', Color.Aquamarine, null));

            // Render the updated terminal
            RenderTerminal(selectedTerminal);
        }
    }
    class Gamemap : IMap
    {
        private bool[,] tiles; // Represents the map tiles as a 2D boolean array where true means blocked.

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Gamemap()
        {
            Width = 80; // Set your desired map width
            Height = 45; // Set your desired map height
            tiles = new bool[Width, Height];

            // Initialize map with some example blocked tiles
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    tiles[x, y] = (x % 2 == 0) && (y % 2 == 0);
                }
            }
        }

        public bool IsBlocked(int x, int y)
        {
            // Check if the specified tile is blocked (true) or not (false)
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return tiles[x, y];
            }
            return true; // Consider out-of-bounds as blocked
        }

        public bool IsInBounds(int x, int y)
        {
            // Check if the specified coordinates are within the map bounds
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void SetBlocked(int x, int y, bool blocked)
        {
            // Set the blocked status of the specified tile
            if (IsInBounds(x, y))
            {
                tiles[x, y] = blocked;
            }
        }
    }
}
