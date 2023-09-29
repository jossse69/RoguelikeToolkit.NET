namespace RoguelikeToolkit.NET.Maps
{
    /// <summary>
    /// Represents a map for a roguelike game.
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets the width of the map.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Gets the height of the map.
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Checks if a cell at the specified coordinates is blocked or not.
        /// </summary>
        /// <param name="x">The X-coordinate of the cell to check.</param>
        /// <param name="y">The Y-coordinate of the cell to check.</param>
        /// <returns>True if the cell is blocked, false otherwise.</returns>
        bool IsBlocked(int x, int y);

        /// <summary>
        /// Checks if a cell at the specified coordinates is in bounds or not.
        /// </summary>
        /// <param name="x">The X-coordinate of the cell to check.</param>
        /// <param name="y">The Y-coordinate of the cell to check.</param>
        /// <returns>True if the cell is in bounds, false otherwise.</returns>
        bool IsInBounds(int x, int y);

        /// <summary>
        /// Sets the blocking status of a cell at the specified coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate of the cell to set.</param>
        /// <param name="y">The Y-coordinate of the cell to set.</param>
        /// <param name="blocked">True to block the cell, false to unblock it.</param>
        void SetBlocked(int x, int y, bool blocked);
    }
}
