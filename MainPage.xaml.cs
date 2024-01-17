namespace chessbot
{
    public partial class MainPage : ContentPage
    {
        ImageButton[] AllSquares = new ImageButton[64];
        ImageButton[] WhiteSquares = new ImageButton[32];
        ImageButton[] BlackSquares = new ImageButton[32];
        ImageSource[] WhitePieces = {"white_rook.png", "white_knight.png", "white_bishop.png", "white_queen.png", "white_king.png", "white_bishop.png", "white_knight.png", "white_rook.png", "white_pawn.png" };
        ImageSource[] BlackPieces = {"black_rook.png", "black_knight.png", "black_bishop.png", "black_queen.png", "black_king.png", "black_bishop.png", "black_knight.png", "black_rook.png", "black_pawn.png" };
        Boolean IsPlayerWhite = true;
        public MainPage()
        {
            InitializeComponent();

            ImageButton[] TempAllSquares = {SquareA8, SquareB8, SquareC8, SquareD8, SquareE8, SquareF8, SquareG8, SquareH8,
                                         SquareA7, SquareB7, SquareC7, SquareD7, SquareE7, SquareF7, SquareG7, SquareH7,
                                         SquareA6, SquareB6, SquareC6, SquareD6, SquareE6, SquareF6, SquareG6, SquareH6,
                                         SquareA5, SquareB5, SquareC5, SquareD5, SquareE5, SquareF5, SquareG5, SquareH5,
                                         SquareA4, SquareB4, SquareC4, SquareD4, SquareE4, SquareF4, SquareG4, SquareH4,
                                         SquareA3, SquareB3, SquareC3, SquareD3, SquareE3, SquareF3, SquareG3, SquareH3,
                                         SquareA2, SquareB2, SquareC2, SquareD2, SquareE2, SquareF2, SquareG2, SquareH2,
                                         SquareA1, SquareB1, SquareC1, SquareD1, SquareE1, SquareF1, SquareG1, SquareH1};
            ImageButton[] TempWhiteSquares = { SquareA8, SquareC8, SquareE8, SquareG8, SquareB7, SquareD7, SquareF7, SquareH7, SquareA6, SquareC6, SquareE6, SquareG6,SquareB5, SquareD5,SquareF5, SquareH5, SquareA4, SquareC4, SquareE4, SquareG4, SquareB3, SquareD3, SquareF3, SquareH3, SquareA2, SquareC2, SquareE2, SquareG2, SquareB1, SquareD1, SquareF1, SquareH1};
            ImageButton[] TempBlackSquares = {SquareB8, SquareD8, SquareF8, SquareH8, SquareA7, SquareC7, SquareE7, SquareG7, SquareB6, SquareD6, SquareF6, SquareH6, SquareA5, SquareC5, SquareE5, SquareG5, SquareB4, SquareD4, SquareF4, SquareH4, SquareA3, SquareC3, SquareE3, SquareG3, SquareB2, SquareD2, SquareF2, SquareH2, SquareA1, SquareC1, SquareE1, SquareG1};
                            

            for (int i = 0; i < 64; i++)
            {
                AllSquares[i] = TempAllSquares[i];
            }
            for (int i = 0; i < 32; i++)
            {
                WhiteSquares[i] = TempWhiteSquares[i];
                BlackSquares[i] = TempBlackSquares[i];
            }
            //sets up the pieces:
            ResetBoard();
        }

        private void ResetBoard()
        {
            if (IsPlayerWhite == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    AllSquares[i].Source = BlackPieces[i];
                    AllSquares[i + 8].Source = BlackPieces[8];
                    AllSquares[i + 16].Source = "transparent.png";
                    AllSquares[i + 24].Source = "transparent.png";
                    AllSquares[i + 32].Source = "transparent.png";
                    AllSquares[i + 40].Source = "transparent.png";
                    AllSquares[i + 48].Source = WhitePieces[8];
                    AllSquares[i + 56].Source = WhitePieces[i];
                }
            }
            else
            {
                for (int i = 7; i >= 0; i--)
                {
                    AllSquares[i].Source = WhitePieces[7-i];
                    AllSquares[i + 8].Source = WhitePieces[8];
                    AllSquares[i + 16].Source = "transparent.png";
                    AllSquares[i + 24].Source = "transparent.png";
                    AllSquares[i + 32].Source = "transparent.png";
                    AllSquares[i + 40].Source = "transparent.png";
                    AllSquares[i + 48].Source = BlackPieces[8];
                    AllSquares[i + 56].Source = BlackPieces[7-i];
                }
            }
            
        }
        private void ButtonResetClicked(object sender, EventArgs e)
        {
            ResetBoard();
        }
        private void FlipBoard(object sender, System.EventArgs e)
        {
            if (IsPlayerWhite == true)
            {
                IsPlayerWhite = false;
                ResetBoard();
            }
            else
            {
                IsPlayerWhite = true;
                ResetBoard();
            }
        }


        Boolean HasPlayerSelectedFromSquare = false;
        ImageSource SelectedPiece = null;
        ImageButton SelectedSquare = null;

        private void SquareSelected(object sender, System.EventArgs e)
        {
            ImageButton currentButton = (ImageButton)sender; 
            if (IsPlayerWhite == true)
            {
                if (WhitePieces.Contains(currentButton.Source))
                {

                    if (WhiteSquares.Contains(SelectedSquare))
                    {
                        SelectedSquare.Background = Color.FromArgb("EEEED2");
                    }
                    if (BlackSquares.Contains(SelectedSquare))
                    {
                        SelectedSquare.Background = Color.FromArgb("#769656");
                    }

                    SelectedSquare = currentButton;
                    SelectedPiece = currentButton.Source;
                    HasPlayerSelectedFromSquare = true;

                    if (WhiteSquares.Contains(SelectedSquare))
                    {
                        SelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
                    }
                    else
                    {
                        SelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
                    }
                    
                }
                else
                {
                    if (HasPlayerSelectedFromSquare == true)
                    {

                        if (WhiteSquares.Contains(SelectedSquare))
                        {
                            SelectedSquare.Background = Color.FromArgb("EEEED2");
                        }
                        if (BlackSquares.Contains(SelectedSquare))
                        {
                            SelectedSquare.Background = Color.FromArgb("#769656");
                        }
                    }
                    else
                    {
                        
                        SelectedPiece = null;
                        SelectedSquare = null;
                    }
                }
            }
            else
            {
                if (BlackPieces.Contains(currentButton.Source))
                {
                    //need to copy paste it for the black moves too
                }
                else
                {
                    SelectedPiece = null;
                    SelectedSquare = null;
                }
            }
        }
    }

}
