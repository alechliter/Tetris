namespace Lechliter.Tetris_Console.lib.objects.Grid
{
    public interface IGrid<TDimensions, TPoint>
    {
        public TDimensions GridDim { get; }

        public TDimensions BoundsDim { get; }

        /// <summary>
        /// Rounds the x and y values of a block's position to the upper level and then converts the coordinate 
        /// to the corresponding position in the xy-matrix.
        /// </summary>
        /// <param name="point">The point to find the xy-matrix indices for. </param>
        /// <param name="X">The x index of the block.</param>
        /// <param name="Y">The y index of the block.</param>
        public void GridPosition(TPoint point, out int X, out int Y);
    }
}
