using Java.Nio.Channels;
using System.Diagnostics;

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
        Boolean PlayerToMoveWhite = true;
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
            CalculateSquaresToEdge();
        }

        private void ResetBoard()
        {
            HasPlayerSelectedFromSquare = false;
            SelectedPiece = null;
            SelectedSquare = null;
            SelectedBefore = null;

            for (int i = 0; i < 64; i++)
            {
                if (WhiteSquares.Contains(AllSquares[i]))
                {
                    AllSquares[i].BackgroundColor = Color.FromArgb("EEEED2");
                }
                else
                {
                    AllSquares[i].BackgroundColor = Color.FromArgb("#769656");
                }
            }
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
            PlayerToMoveWhite = true;
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

        //calculate for every square
        //required for rook, bishop, queen
        List<List<int>> SquaresToEdge = new List<List<int>>();
        private void CalculateSquaresToEdge()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int E = i;
                    int K = 7 - j;
                    int D = 7 - i;
                    int N = j;
                    List<int> rowList = new List<int>();
                    rowList.AddRange(new List<int>
                    {
                        E,
                        K,
                        D,
                        N,
                        Math.Min(E, K),
                        Math.Min(K, D),
                        Math.Min(D, N),
                        Math.Min(N, E)
                    });
                    SquaresToEdge.Add(rowList);
                }
            }
            //---FOR TESTING---
            //for (int i = 0; i < SquaresToEdge.Count; i++)
            //{
            //    Trace.WriteLine($"{i}: |" + string.Join(", ", SquaresToEdge[i]) + "|");
            //}
        }

        Boolean HasPlayerSelectedFromSquare = false;
        ImageSource SelectedPiece = null;
        ImageButton SelectedSquare = null;
        ImageButton SelectedBefore = null;

        private void SquareSelected(object sender, System.EventArgs e)
        {
            if (IsPlayerWhite == PlayerToMoveWhite)
            {
                ImageButton currentButton = (ImageButton)sender;
                if (IsPlayerWhite == true)
                {
                    if (WhitePieces.Contains(currentButton.Source))
                    {
                        if (WhiteSquares.Contains(SelectedBefore))
                        {
                            SelectedBefore.BackgroundColor = Color.FromArgb("EEEED2");
                        }
                        else if (BlackSquares.Contains(SelectedBefore))
                        {
                            SelectedBefore.BackgroundColor = Color.FromArgb("#769656");
                        }
                        if (WhiteSquares.Contains(SelectedSquare))
                        {
                            SelectedSquare.BackgroundColor = Color.FromArgb("EEEED2");
                        }
                        else if (BlackSquares.Contains(SelectedSquare))
                        {
                            SelectedSquare.BackgroundColor = Color.FromArgb("#769656");
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
                    else if (HasPlayerSelectedFromSquare == true)
                    {
                        SelectedBefore = SelectedSquare;
                        SelectedSquare = currentButton;
                        SelectedBefore.Source = "transparent.png";
                        SelectedSquare.Source = SelectedPiece;
                        if (WhiteSquares.Contains(SelectedSquare))
                        {
                            SelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
                        }
                        else
                        {
                            SelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
                        }
                        HasPlayerSelectedFromSquare = false;
                        PlayerToMoveWhite = false;
                        //only has to happen if move is not possible
                        //if (WhiteSquares.Contains(SelectedSquare))
                        //{
                        //    SelectedSquare.BackgroundColor = Color.FromArgb("EEEED2");
                        //}
                        //else if (BlackSquares.Contains(SelectedSquare))
                        //{
                        //    SelectedSquare.BackgroundColor = Color.FromArgb("#769656");
                        //}
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
                        //need to copy paste it for the black moves too
                    }
                }
            }
        }



        //-----CALCULATE POSSIBLE MOVES-----
        private void PossibleMoves()
        {
            if (PlayerToMoveWhite == true)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (!AllSquares[i].Source.Equals("transparent.png"))
                    {
                        if (AllSquares[i].Source.Equals("white_pawn.png"))
                        {

                        }
                        else if (AllSquares[i].Source.Equals("white_rook.png"))
                        {

                        }
                        else if (AllSquares[i].Source.Equals("white_bishop.png"))
                        {

                        }
                        else if (AllSquares[i].Source.Equals("white_queen.png"))
                        {

                        }
                        else if (AllSquares[i].Source.Equals("white_knight.png"))
                        {

                        }
                        else if (AllSquares[i].Source.Equals("white_king.png"))
                        {

                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 64; i++)
                {

                }
                //rook moves goes here...
            }   
        }
    }

}
