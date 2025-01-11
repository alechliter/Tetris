using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Enumerations;
using Lechliter.Tetris.TetrisConsole.Models;

namespace Lechliter.Tetris.TetrisConsole
{
    class PreviewPieceView
    {
        public DynamicComponent Component { get; protected set; }

        public IPreview<ePieceType> Preview { get; protected set; }

        public IntDimensions Dim
        {
            get
            {
                return Component.Dimensions;
            }
        }

        public PreviewPieceView(int layer, IntPoint position, IPreview<ePieceType> preview)
        {
            Preview = preview;
            preview.PieceUpdated += OnUpdate;

            Component = new DynamicComponent(layer, position, 2);
            Component.Grid = CreateComponentContents();
        }

        public void OnUpdate()
        {
            Component.OnUpdate(CreateComponentContents());
        }

        private ComponentContent[,] CreateComponentContents()
        {
            ComponentContent[,] pieceGrid = ConsoleView.ConvertPieceGridToContentGrid(Preview.Grid);
            ComponentContent[,] content = new ComponentContent[pieceGrid.GetLength(0) + 2, pieceGrid.GetLength(1) + 2];

            for (int row = 0; row < pieceGrid.GetLength(1); row++)
            {
                for (int col = 0; col < pieceGrid.GetLength(0); col++)
                {
                    content[col + 1, row + 1] = pieceGrid[col, row];
                }
            }

            for (int col = 0; col < content.GetLength(0); col++)
            {
                content[col, 0] = new ComponentContent('·', eTextColor.Default);
                content[col, content.GetLength(1) - 1] = new ComponentContent('·', eTextColor.Default);
            }

            for (int row = 1; row < content.GetLength(1) - 1; row++)
            {
                content[0, row] = new ComponentContent('·', eTextColor.Default);
                content[content.GetLength(0) - 1, row] = new ComponentContent('·', eTextColor.Default);
            }

            return content;
        }
    }
}
