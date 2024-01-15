namespace chessbot
{
    public partial class MainPage : ContentPage
    {
        static ImageSource[] WhitePieces = {"white_rook.png", "white_knight.png", "white_bishop.png", "white_queen.png", "white_king.png", "white_bishop.png", "white_knight.png", "white_rook.png"};
        static ImageSource[] BlackPieces = {"black_rook.png", "black_knight.png", "black_bishop.png", "black_queen.png", "black_king.png", "black_bishop.png", "black_knight.png", "black_rook.png" };
        public MainPage()
        {
            InitializeComponent();
            ImageButton[] AllSquares = { SquareA8, SquareB8, SquareC8, SquareD8, SquareE8, SquareF8, SquareG8, SquareH8,
                                         SquareA7, SquareB7, SquareC7, SquareD7, SquareE7, SquareF7, SquareG7, SquareH7,
                                         SquareA6, SquareB6, SquareC6, SquareD6, SquareE6, SquareF6, SquareG6, SquareH6,
                                         SquareA5, SquareB5, SquareC5, SquareD5, SquareE5, SquareF5, SquareG5, SquareH5,
                                         SquareA4, SquareB4, SquareC4, SquareD4, SquareE4, SquareF4, SquareG4, SquareH4,
                                         SquareA3, SquareB3, SquareC3, SquareD3, SquareE3, SquareF3, SquareG3, SquareH3,
                                         SquareA2, SquareB2, SquareC2, SquareD2, SquareE2, SquareF2, SquareG2, SquareH2,
                                         SquareA1, SquareB1, SquareC1, SquareD1, SquareE1, SquareF1, SquareG1, SquareH1,
                                       };

            for (int i = 0; i < 8; i++)
            {
                AllSquares[i].Source = BlackPieces[i];
                AllSquares[i + 8].Source = "black_pawn.png";
                AllSquares[i + 16].Source = "transparent.png";
                AllSquares[i + 24].Source = "transparent.png";
                AllSquares[i + 32].Source = "transparent.png";
                AllSquares[i + 40].Source = "transparent.png";
                AllSquares[i + 48].Source = "white_pawn.png";
                AllSquares[i + 56].Source = WhitePieces[i];
            }
        }

        private void ResetBoard(object sender, System.EventArgs e)
        {

        }
        private void SquareSelected(object sender, System.EventArgs e)
        {

        }
    }

}
