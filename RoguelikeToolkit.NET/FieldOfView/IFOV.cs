using RoguelikeToolkit.NET.Maps;

namespace RoguelikeToolkit.NET.FOV
{
    /// <summary>
    /// Represents a field of view (FOV) algorithm for a roguelike game.
    /// </summary>
    public interface IFOV
    {
        /// <summary>
        /// Computes the field of view for a given map and player's position.
        /// </summary>
        /// <param name="playerX">The X-coordinate of the player's position.</param>
        /// <param name="playerY">The Y-coordinate of the player's position.</param>
        /// <param name="radius">The radius of the FOV.</param>
        void ComputeFOV(int playerX, int playerY, int radius);

        /// <summary>
        /// Checks if a cell at the specified coordinates is within the computed FOV.
        /// </summary>
        /// <param name="x">The X-coordinate of the cell to check.</param>
        /// <param name="y">The Y-coordinate of the cell to check.</param>
        /// <returns>True if the cell is within FOV, false otherwise.</returns>
        bool IsInFOV(int x, int y);
    }
}
