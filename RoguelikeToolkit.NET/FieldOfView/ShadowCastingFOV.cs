using System;
using System.Collections.Generic;
using RoguelikeToolkit.NET.FOV;
using RoguelikeToolkit.NET.Maps;

namespace RoguelikeToolkit.NET.FieldOfView
{
    /// <summary>
    /// Represents the Shadow Casting FOV algorithm for a roguelike game.
    /// </summary>
    public class ShadowCastingFOV : IFOV
    {
        private readonly IMap map;
        private readonly bool[,] fovMap;

        public ShadowCastingFOV(IMap map)
        {
            this.map = map;
            fovMap = new bool[map.Width, map.Height];
            ClearFOV(); // Add this line to initialize the FOV map.
        }

        // Add this method to initialize the FOV map.
        public void ClearFOV()
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    fovMap[x, y] = false;
                }
            }
        }

        /// <inheritdoc />
        public void ComputeFOV(int playerX, int playerY, int radius)
        {
            fovMap[playerX, playerY] = true; // The player's position is always visible.

            for (int octant = 0; octant < 8; octant++)
            {
                ComputeOctant(playerX, playerY, radius, octant * 0.125, (octant + 1) * 0.125);
            }
        }




        private void ComputeOctant(int playerX, int playerY, int radius, double startSlope, double endSlope)
        {
            int cx = playerX;
            int cy = playerY;

            for (int dx = 1, dy = 0; dx <= radius; dx++)
            {
                double slope = ((double)dx - 0.5) / ((double)dy + 0.5);

                if (slope < endSlope)
                {
                    dy++;
                }

                if (slope >= startSlope)
                {
                    // Compute visible cells (dx, dy) and (-dx, dy) symmetrically.
                    for (int i = -dy; i <= dy; i++)
                    {
                        int x = cx + dx;
                        int y = cy + i;
                        if (map.IsInBounds(x, y))
                        {
                            fovMap[x, y] = true;
                            if (map.IsBlocked(x, y))
                            {
                                break;
                            }
                        }

                        x = cx - dx;
                        if (map.IsInBounds(x, y))
                        {
                            fovMap[x, y] = true;
                            if (map.IsBlocked(x, y))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }



        /// <inheritdoc />
        public bool IsInFOV(int x, int y)
        {
            if (x < 0 || x >= map.Width || y < 0 || y >= map.Height)
            {
                return false;
            }

            return fovMap[x, y];
        }

        public void PrintFOV()
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    System.Console.Write(fovMap[x, y]);
                }
                System.Console.WriteLine();
            }
        }
    }
}
