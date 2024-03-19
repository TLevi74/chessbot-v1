using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace chessbot
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        //R-N-B-Q-K-B-N-R-P
        readonly int[] PieceValues = {500, 320, 330, 900, 0, 330, 320, 500, 100};
        readonly int[] P2M_PawnExtraValues = { 0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 50, 50, 10, 10, 20, 30, 30, 20, 10, 10, 5, 5, 10, 25, 25, 10, 5, 5, 0, 0, 0, 20, 20, 0, 0, 0, 5, -5, -10, 0, 0, -10, -5, 5, 5, 10, 10, -20, -20, 10, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] AI2M_PawnExtraValues = { 0, 0, 0, 0, 0, 0, 0, 0, 5, 10, 10, -20, -20, 10, 10, 5, 5, -5, -10, 0, 0, -10, -5, 5, 0, 0, 0, 20, 20, 0, 0, 0, 5, 5, 10, 25, 25, 10, 5, 5, 10, 10, 20, 30, 30, 20, 10, 10, 50, 50, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] KnightExtraValues = { -50, -20, -30, -30, -30, -30, -20, -50, -40, -20, 0, 0, 0, 0, -20, -40, -30, 0, 10, 15, 15, 10, 0, -30, -30, 5, 15, 20, 20, 15, 5, -30, -30, 0, 15, 20, 20, 15, 0, -30, -30, 5, 10, 15, 15, 10, 5, -30, -40, -20, 0, 5, 5, 0, -20, -40, -50, -20, -30, -30, -30, -30, -20, -50 };
        readonly int[] BishopExtraValues = { -20, -10, -10, -10, -10, -10, -10, -20, -10, 5, 0, 0, 0, 0, 5, -10, -10, 10, 10, 10, 10, 10, 10, -10, -10, 0, 10, 10, 10, 10, 0, -10, -10, 0, 10, 10, 10, 10, 0, -10, -10, 10, 10, 10, 10, 10, 10, -10, -10, 5, 0, 0, 0, 0, 5, -10, -20, -10, -10, -10, -10, -10, -10, -20 };
        readonly int[] P2M_RookExtraValues = { 0, 0, 0, 0, 0, 0, 0, 0, 5, 10, 10, 10, 10, 10, 10, 5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, 0, 0, 0, 5, 5, 0, 0, 0 };
        readonly int[] AI2M_RookExtraValues = { 0, 0, 0, 5, 5, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, 5, 10, 10, 10, 10, 10, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0 };
        readonly int[] QueenExtraValues = { -20, -10, -10, -5, -5, -10, -10, -20, -10, 0, 0, 0, 0, 0, 0, -10, -10, 0, 5, 5, 5, 5, 0, -10, -5, 0, 5, 5, 5, 5, 0, -5, -5, 0, 5, 5, 5, 5, 0, -5, -10, 5, 5, 5, 5, 5, 0, -10, -10, 0, 5, 0, 0, 0, 0, -10, -20, -10, -10, -5, -5, -10, -10, -20 };
        readonly int[] P2M_KingExtraValues = { -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -20, -30, -30, -40, -40, -30, -30, -20, -10, -20, -20, -20, -20, -20, -20, -10, 20, 20, 0, 0, 0, 0, 20, 20, 20, 30, 10, 0, 0, 10, 30, 20 };
        readonly int[] AI2M_KingExtraValues = { 20, 30, 10, 0, 0, 10, 30, 20, 20, 20, 0, 0, 0, 0, 20, 20, -10, -20, -20, -20, -20, -20, -20, -10, -20, -30, -30, -40, -40, -30, -30, -20, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30 };
        readonly int[] Endgame_KingExtraValues = { -50, -40, -30, -20, -20, -30, -40, -50, -30, -20, -10, 0, 0, -10, -20, -30, -30, -10, 20, 30, 30, 20, -10, -30, -30, -10, 30, 40, 40, 30, -10, -30, -30, -10, 30, 40, 40, 30, -10, -30, -30, -10, 20, 30, 30, 20, -10, -30, -30, -30, 0, 0, 0, 0, -30, -30, -50, -30, -30, -30, -30, -30, -30, -50 };

        ImageButton[] AllSquares = new ImageButton[64];
        ImageSource[] Position = new ImageSource[64];
        ImageButton[] WhiteSquares = new ImageButton[32];
        ImageButton[] BlackSquares = new ImageButton[32];
        readonly ImageSource[] WhitePieces = { "white_rook.png", "white_knight.png", "white_bishop.png", "white_queen.png", "white_king.png", "white_bishop.png", "white_knight.png", "white_rook.png", "white_pawn.png" };
        readonly ImageSource[] BlackPieces = { "black_rook.png", "black_knight.png", "black_bishop.png", "black_queen.png", "black_king.png", "black_bishop.png", "black_knight.png", "black_rook.png", "black_pawn.png" };
        readonly ImageSource NoPiece = "transparent.png";
        bool IsPlayerWhite = true;
        bool PlayerToMoveWhite = true;
        void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double newValue = Math.Round(e.NewValue);
            DepthSlider.Value = newValue;
            if (newValue == 3)
            {
                DepthLabel.Text = "Very Fast";
            }
            else if (newValue == 4)
            {
                DepthLabel.Text = "Fast";
            }
            else
            {
                DepthLabel.Text = "Very Slow";
            }
        }
        public MainPage()
        {
            InitializeComponent();
            DepthSlider.Value = 3;
            ImageButton[] TempAllSquares = {SquareA8, SquareB8, SquareC8, SquareD8, SquareE8, SquareF8, SquareG8, SquareH8,
                                            SquareA7, SquareB7, SquareC7, SquareD7, SquareE7, SquareF7, SquareG7, SquareH7,
                                            SquareA6, SquareB6, SquareC6, SquareD6, SquareE6, SquareF6, SquareG6, SquareH6,
                                            SquareA5, SquareB5, SquareC5, SquareD5, SquareE5, SquareF5, SquareG5, SquareH5,
                                            SquareA4, SquareB4, SquareC4, SquareD4, SquareE4, SquareF4, SquareG4, SquareH4,
                                            SquareA3, SquareB3, SquareC3, SquareD3, SquareE3, SquareF3, SquareG3, SquareH3,
                                            SquareA2, SquareB2, SquareC2, SquareD2, SquareE2, SquareF2, SquareG2, SquareH2,
                                            SquareA1, SquareB1, SquareC1, SquareD1, SquareE1, SquareF1, SquareG1, SquareH1};
            ImageButton[] TempWhiteSquares = { SquareA8, SquareC8, SquareE8, SquareG8, SquareB7, SquareD7, SquareF7, SquareH7, SquareA6, SquareC6, SquareE6, SquareG6, SquareB5, SquareD5, SquareF5, SquareH5, SquareA4, SquareC4, SquareE4, SquareG4, SquareB3, SquareD3, SquareF3, SquareH3, SquareA2, SquareC2, SquareE2, SquareG2, SquareB1, SquareD1, SquareF1, SquareH1 };
            ImageButton[] TempBlackSquares = { SquareB8, SquareD8, SquareF8, SquareH8, SquareA7, SquareC7, SquareE7, SquareG7, SquareB6, SquareD6, SquareF6, SquareH6, SquareA5, SquareC5, SquareE5, SquareG5, SquareB4, SquareD4, SquareF4, SquareH4, SquareA3, SquareC3, SquareE3, SquareG3, SquareB2, SquareD2, SquareF2, SquareH2, SquareA1, SquareC1, SquareE1, SquareG1 };
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
            CalculateSquaresToEdge();
            ResetBoard();
        }
        int LastMoveStarting; int LastMoveTarget;
        private void ResetBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                castling[i] = true;
            }

            btnReset.Text = "Reset";
            HasPlayerSelectedFromSquare = false;
            SelectedPiece = null;
            SelectedSquare = null;
            SelectedBefore = null;
            AISelectedPiece = null;
            AISelectedBefore = null;
            AISelectedSquare = null;
            PlayerToMoveWhite = true;
            LastMoveStarting = 0;
            LastMoveTarget = 0;
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
            PromotionQueen.Source = NoPiece;
            PromotionRook.Source = NoPiece;
            PromotionBishop.Source = NoPiece;
            PromotionKnight.Source = NoPiece;

            if (IsPlayerWhite == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    AllSquares[i].Source = BlackPieces[i];
                    Position[i] = BlackPieces[i];
                    AllSquares[i + 8].Source = BlackPieces[8];
                    Position[i + 8] = BlackPieces[8];
                    AllSquares[i + 16].Source = NoPiece;
                    Position[i + 16] = NoPiece;
                    AllSquares[i + 24].Source = NoPiece;
                    Position[i + 24] = NoPiece;
                    AllSquares[i + 32].Source = NoPiece;
                    Position[i + 32] = NoPiece;
                    AllSquares[i + 40].Source = NoPiece;
                    Position[i + 40] = NoPiece;
                    AllSquares[i + 48].Source = WhitePieces[8];
                    Position[i + 48] = WhitePieces[8];
                    AllSquares[i + 56].Source = WhitePieces[i];
                    Position[i + 56] = WhitePieces[i];
                }
                GenerateMoves(Moves);
            }
            else
            {
                for (int i = 7; i >= 0; i--)
                {
                    AllSquares[i].Source = WhitePieces[7 - i];
                    Position[i] = WhitePieces[7 - i];
                    AllSquares[i + 8].Source = WhitePieces[8];
                    Position[i + 8] = WhitePieces[8];
                    AllSquares[i + 16].Source = NoPiece;
                    Position[i + 16] = NoPiece;
                    AllSquares[i + 24].Source = NoPiece;
                    Position[i + 24] = NoPiece;
                    AllSquares[i + 32].Source = NoPiece;
                    Position[i + 32] = NoPiece;
                    AllSquares[i + 40].Source = NoPiece;
                    Position[i + 40] = NoPiece;
                    AllSquares[i + 48].Source = BlackPieces[8];
                    Position[i + 48] = BlackPieces[8];
                    AllSquares[i + 56].Source = BlackPieces[7 - i];
                    Position[i + 56] = BlackPieces[7 - i];
                }
                AIToMove();
            }
        }
        private void ButtonResetClicked(object sender, EventArgs e)
        {
            ResetBoard();
        }
        private void FlipBoard(object sender, EventArgs e)
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
        int[,] SquaresToEdge = new int[64, 8];
        private void CalculateSquaresToEdge()
        {
            int index = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    int E = i;
                    int K = 7 - j;
                    int D = 7 - i;
                    int N = j;
                    SquaresToEdge[index, 0] = E;
                    SquaresToEdge[index, 1] = K;
                    SquaresToEdge[index, 2] = D;
                    SquaresToEdge[index, 3] = N;
                    SquaresToEdge[index, 4] = Math.Min(E, K);
                    SquaresToEdge[index, 5] = Math.Min(K, D);
                    SquaresToEdge[index, 6] = Math.Min(D, N);
                    SquaresToEdge[index, 7] = Math.Min(N, E);
                    index++;
                }
            }
        }

        bool HasPlayerSelectedFromSquare = false;
        ImageSource SelectedPiece = null;
        ImageButton SelectedSquare = null;
        ImageButton SelectedBefore = null;
        public ImageButton currentButton = new ImageButton();

        private void PromotionPieceSelected(object sender, EventArgs e)
        {
            currentButton = (ImageButton)sender;
            if (currentButton.Source != NoPiece)
            {
                Position[selectedIndexSquare] = currentButton.Source;
                AllSquares[selectedIndexSquare].Source = currentButton.Source;
                PromotionQueen.Source = NoPiece;
                PromotionRook.Source = NoPiece;
                PromotionBishop.Source = NoPiece;
                PromotionKnight.Source = NoPiece;
                AIToMove();
            }
        }
        private void SquareSelected(object sender, EventArgs e)
        {
            currentButton = (ImageButton)sender;
            //------PLAYER MOVES AS WHITE--------
            if (IsPlayerWhite == true && PlayerToMoveWhite == true)
            {
                if (WhitePieces.Contains(currentButton.Source))
                {
                    PlayerMovesStartingSquare();
                }
                else if (HasPlayerSelectedFromSquare == true)
                {
                    PlayerMovesTargetSquare(true);
                }
            }
            //------PLAYER MOVES AS BLACK--------
            else if (IsPlayerWhite == false && PlayerToMoveWhite == false)
            {
                if (BlackPieces.Contains(currentButton.Source))
                {
                    PlayerMovesStartingSquare();
                }
                else if (HasPlayerSelectedFromSquare == true)
                {
                    PlayerMovesTargetSquare(false);
                }
            }
        }

        //visual:  ---DONE---
        private void PlayerMovesStartingSquare()
        {
            if (WhiteSquares.Contains(SelectedBefore))
            {
                SelectedBefore.BackgroundColor = Color.FromArgb("#EEEED2");
            }
            else if (BlackSquares.Contains(SelectedBefore))
            {
                SelectedBefore.BackgroundColor = Color.FromArgb("#769656");
            }
            if (WhiteSquares.Contains(SelectedSquare))
            {
                SelectedSquare.BackgroundColor = Color.FromArgb("#EEEED2");
            }
            else if (BlackSquares.Contains(SelectedSquare))
            {
                SelectedSquare.BackgroundColor = Color.FromArgb("#769656");
            }
            for (int i = 0; i < Moves.Count; i++)
            {
                if (AllSquares[Moves[i].StartingSquare] == SelectedSquare)
                {
                    if (WhiteSquares.Contains(AllSquares[Moves[i].TargetSquare]))
                    {
                        AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#EEEED2");
                    }
                    else
                    {
                        AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#769656");
                    }
                }
            }
            if (AISelectedBefore != null)
            {
                if (WhiteSquares.Contains(AISelectedBefore))
                {
                    AISelectedBefore.BackgroundColor = Color.FromArgb("#F4F680");
                }
                else
                {
                    AISelectedBefore.BackgroundColor = Color.FromArgb("#BBCC44");
                }
                if (WhiteSquares.Contains(AISelectedSquare))
                {
                    AISelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
                }
                else
                {
                    AISelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
                }
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
            for (int i = 0; i < Moves.Count; i++)
            {
                if (AllSquares[Moves[i].StartingSquare] == SelectedSquare)
                {
                    if (WhiteSquares.Contains(AllSquares[Moves[i].TargetSquare]))
                    {
                        AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#EB7D69");
                    }
                    else
                    {
                        AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#D46D51");
                    }
                }
            }
        }
        public int selectedIndexBefore = -1;
        public int selectedIndexSquare = -1;
        private void PlayerMovesTargetSquare(bool TempPlayerWhiteMoves)
        {
            SelectedBefore = SelectedSquare;
            SelectedSquare = currentButton;
            //checks if the selected move is in the Moves list:
            // Find the index of SelectedBefore
            for (int i = 0; i < AllSquares.Count(); i++)
            {
                if (AllSquares[i] == SelectedBefore)
                {
                    selectedIndexBefore = i;
                    break;
                }
            }
            // Find the index of SelectedSquare
            for (int i = 0; i < AllSquares.Count(); i++)
            {
                if (AllSquares[i] == SelectedSquare)
                {
                    selectedIndexSquare = i;
                    break;
                }
            }
            bool pairExists = Moves.Any(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare);

            if (pairExists)
            {
                var selectedMove = Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare);

                //resets AI move color
                if (WhiteSquares.Contains(AISelectedBefore))
                {
                    AISelectedBefore.BackgroundColor = Color.FromArgb("EEEED2");
                }
                else if (BlackSquares.Contains(AISelectedBefore))
                {
                    AISelectedBefore.BackgroundColor = Color.FromArgb("#769656");
                }
                if (WhiteSquares.Contains(AISelectedSquare))
                {
                    AISelectedSquare.BackgroundColor = Color.FromArgb("EEEED2");
                }
                else if (BlackSquares.Contains(AISelectedSquare))
                {
                    AISelectedSquare.BackgroundColor = Color.FromArgb("#769656");
                }
                //make move
                for (int i = 0; i < AllSquares.Count(); i++)
                {
                    if (AllSquares[i] == SelectedBefore)
                    {
                        selectedIndexBefore = i;
                        break;
                    }
                }
                for (int i = 0; i < AllSquares.Count(); i++)
                {
                    if (AllSquares[i] == SelectedSquare)
                    {
                        selectedIndexSquare = i;
                        break;
                    }
                }
                if ((Position[selectedIndexSquare] != NoPiece) || (selectedMove.Extra == 5) || (selectedMove.Extra == 6))
                {
                    CaptureSound.Play();
                }
                else
                {
                    MoveSound.Play();
                }
                //for castling:
                if (SelectedBefore == SquareA1)
                {
                    castling[2] = false;
                }
                if (SelectedBefore == SquareH1)
                {
                    castling[3] = false;
                }
                if (TempPlayerWhiteMoves == true)
                {
                    if (SelectedBefore == SquareE1)
                    {
                        castling[0] = false;
                    }
                }
                else
                {
                    if (SelectedBefore == SquareD1)
                    {
                        castling[1] = false;
                    }
                }

                Position[selectedIndexBefore] = NoPiece;
                Position[selectedIndexSquare] = SelectedPiece;
                SelectedBefore.Source = NoPiece;
                SelectedSquare.Source = SelectedPiece;
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (AllSquares[Moves[i].StartingSquare] == SelectedBefore)
                    {
                        if (WhiteSquares.Contains(AllSquares[Moves[i].TargetSquare]))
                        {
                            AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#EEEED2");
                        }
                        else
                        {
                            AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#769656");
                        }
                    }
                }
                if (WhiteSquares.Contains(SelectedSquare))
                {
                    SelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
                }
                else
                {
                    SelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
                }
                HasPlayerSelectedFromSquare = false;

                if (selectedMove.Extra > 0)
                {
                    //if promotion:
                    if (selectedMove.Extra == 1 || selectedMove.Extra == 2 || selectedMove.Extra == 3 || selectedMove.Extra == 4)
                    {
                        if (TempPlayerWhiteMoves == true)
                        {
                            PromotionQueen.Source = WhitePieces[3];
                            PromotionRook.Source = WhitePieces[0];
                            PromotionBishop.Source = WhitePieces[2];
                            PromotionKnight.Source = WhitePieces[1];
                            PlayerToMoveWhite = !PlayerToMoveWhite;
                            return;
                        }
                        else
                        {
                            PromotionQueen.Source = BlackPieces[3];
                            PromotionRook.Source = BlackPieces[0];
                            PromotionBishop.Source = BlackPieces[2];
                            PromotionKnight.Source = BlackPieces[1];
                            PlayerToMoveWhite = !PlayerToMoveWhite;
                            return;
                        }
                    }
                    //if en passant:
                    else if (selectedMove.Extra == 5)
                    {
                        Position[selectedIndexBefore + 1] = NoPiece;
                        AllSquares[selectedIndexBefore + 1].Source = NoPiece;
                    }
                    else if (selectedMove.Extra == 6)
                    {
                        Position[selectedIndexBefore - 1] = NoPiece;
                        AllSquares[selectedIndexBefore - 1].Source = NoPiece;
                    }
                    //castling:
                    else if (selectedMove.Extra == 7)
                    {
                        Position[56] = NoPiece;
                        AllSquares[56].Source = NoPiece;
                        if (IsPlayerWhite)
                        {
                            Position[59] = WhitePieces[0];
                            AllSquares[59].Source = WhitePieces[0];
                        }
                        else
                        {
                            Position[58] = BlackPieces[0];
                            AllSquares[58].Source = BlackPieces[0];
                        }
                    }
                    else if (selectedMove.Extra == 8)
                    {
                        Position[63] = NoPiece;
                        AllSquares[63].Source = NoPiece;
                        if (IsPlayerWhite)
                        {
                            Position[61] = WhitePieces[0];
                            AllSquares[61].Source = WhitePieces[0];
                        }
                        else
                        {
                            Position[60] = BlackPieces[0];
                            AllSquares[60].Source = BlackPieces[0];
                        }
                    }
                }
                LastMoveStarting = selectedIndexBefore;
                LastMoveTarget = selectedIndexSquare;
                if (TempPlayerWhiteMoves == true)
                {
                    PlayerToMoveWhite = false;
                }
                else
                {
                    PlayerToMoveWhite = true;
                }
                AIToMove();
            }
            else
            {
                if (WhiteSquares.Contains(SelectedBefore))
                {
                    SelectedBefore.BackgroundColor = Color.FromArgb("EEEED2");
                }
                else if (BlackSquares.Contains(SelectedBefore))
                {
                    SelectedBefore.BackgroundColor = Color.FromArgb("#769656");
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (AllSquares[Moves[i].StartingSquare] == SelectedBefore)
                    {
                        if (WhiteSquares.Contains(AllSquares[Moves[i].TargetSquare]))
                        {
                            AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#EEEED2");
                        }
                        else
                        {
                            AllSquares[Moves[i].TargetSquare].BackgroundColor = Color.FromArgb("#769656");
                        }
                    }
                }
                if (AISelectedBefore != null)
                {
                    if (WhiteSquares.Contains(AISelectedBefore))
                    {
                        AISelectedBefore.BackgroundColor = Color.FromArgb("#F4F680");
                    }
                    else
                    {
                        AISelectedBefore.BackgroundColor = Color.FromArgb("#BBCC44");
                    }
                    if (WhiteSquares.Contains(AISelectedSquare))
                    {
                        AISelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
                    }
                    else
                    {
                        AISelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
                    }
                }
            }
        }




        //-----CALCULATE POSSIBLE MOVES-----

        readonly int[] DirectionOffsets = { -8, 1, 8, -1, -7, 9, 7, -9 };
        bool IsKingNextTo = false;
        //for castling:
        //white king - black king - a1 rook - h1 rook - a8 rook - h8 rook
        //TODO: stalemate if there're only 2 kings!!!
        bool[] castling = new bool[6];
        public struct Move
        {
            public int StartingSquare;
            public int TargetSquare;
            public int Value;
            //Extra: used for special moves like castling, promotion and en passant
            public int Extra;
            public Move(int startingSquare, int targetSquare, int extra)
            {
                StartingSquare = startingSquare;
                TargetSquare = targetSquare;
                Extra = extra;
            }
        }
        List<Move> TempMoves = new List<Move>();
        List<Move> CurrentColorMoves = new List<Move>();
        List<Move> Moves = new List<Move>();
        List<Move> Temp1 = new List<Move>();
        List<Move> Temp2 = new List<Move>();

        int PiecesOnBoard = 32;
        private List<Move> PossibleMoves()
        {
            TempMoves.Clear();
            TempMoves.AddRange(PossibleRegularMoves());
            TempMoves.AddRange(PossibleCastlingMoves());
            for (int i = 0; i < TempMoves.Count; i++)
            {
                if (TempMoves[i].Extra != 0)
                {
                    Move move = TempMoves[i];
                    switch (TempMoves[i].Extra)
                    {
                        case 1:
                            move.Value += PieceValues[3] - PieceValues[8];
                            break;
                        case 2:
                            move.Value += PieceValues[0] - PieceValues[8];
                            break;
                        case 3:
                            move.Value += PieceValues[2] - PieceValues[8];
                            break;
                        case 4:
                            move.Value += PieceValues[1] - PieceValues[8];
                            break;
                        case 5:
                        case 6:
                            move.Value += PieceValues[8];
                            break;
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                            break;
                    }
                    TempMoves[i] = move;
                }
            }
            return TempMoves;
        }
        private List<Move> PossibleRegularMoves()
        {
            Temp1.Clear();
            if (PlayerToMoveWhite == true)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (WhitePieces.Contains(Position[i]))
                    {
                        //WHITE PAWN
                        if (Position[i] == WhitePieces[8])
                        {
                            if (IsPlayerWhite == true)
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i - 16, 0));
                                    }
                                }
                                if (Position[i - 8] == NoPiece && SquaresToEdge[i, 0] > 1)
                                {
                                    Temp1.Add(new Move(i, i - 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i - 9]))
                                    {
                                        Temp1.Add(new Move(i, i - 9, 0));
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i - 7]))
                                    {
                                        Temp1.Add(new Move(i, i - 7, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if (Position[i - 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i - 8, 1));
                                        Temp1.Add(new Move(i, i - 8, 2));
                                        Temp1.Add(new Move(i, i - 8, 3));
                                        Temp1.Add(new Move(i, i - 8, 4));
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i - 7]))
                                        {
                                            Temp1.Add(new Move(i, i - 7, 1));
                                            Temp1.Add(new Move(i, i - 7, 2));
                                            Temp1.Add(new Move(i, i - 7, 3));
                                            Temp1.Add(new Move(i, i - 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i - 9]))
                                        {
                                            Temp1.Add(new Move(i, i - 9, 1));
                                            Temp1.Add(new Move(i, i - 9, 2));
                                            Temp1.Add(new Move(i, i - 9, 3));
                                            Temp1.Add(new Move(i, i - 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 0] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (Position[i + 1] == BlackPieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i - 7, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (Position[i - 1] == BlackPieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i - 9, 6));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i + 16, 0));
                                    }
                                }
                                if (Position[i + 8] == NoPiece && SquaresToEdge[i, 2] > 1)
                                {
                                    Temp1.Add(new Move(i, i + 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i + 7]))
                                    {
                                        Temp1.Add(new Move(i, i + 7, 0));
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i + 9]))
                                    {
                                        Temp1.Add(new Move(i, i + 9, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if (Position[i + 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i + 8, 1));
                                        Temp1.Add(new Move(i, i + 8, 2));
                                        Temp1.Add(new Move(i, i + 8, 3));
                                        Temp1.Add(new Move(i, i + 8, 4));
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i + 7]))
                                        {
                                            Temp1.Add(new Move(i, i + 7, 1));
                                            Temp1.Add(new Move(i, i + 7, 2));
                                            Temp1.Add(new Move(i, i + 7, 3));
                                            Temp1.Add(new Move(i, i + 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i + 9]))
                                        {
                                            Temp1.Add(new Move(i, i + 9, 1));
                                            Temp1.Add(new Move(i, i + 9, 2));
                                            Temp1.Add(new Move(i, i + 9, 3));
                                            Temp1.Add(new Move(i, i + 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 2] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (Position[i + 1] == BlackPieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i + 9, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (Position[i - 1] == BlackPieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i + 7, 6));
                                        }
                                    }
                                }
                            }
                        }
                        //WHITE ROOK
                        else if (Position[i] == WhitePieces[0] || Position[i] == WhitePieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE BISHOP
                        else if (Position[i] == WhitePieces[2] || Position[i] == WhitePieces[5])
                        {
                            for (int j = 4; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE QUEEN
                        else if (Position[i] == WhitePieces[3])
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE KNIGHT
                        else if (Position[i] == WhitePieces[1] || Position[i] == WhitePieces[6])
                        {
                            //8 if, 8 possible move
                            if (SquaresToEdge[i, 0] >= 2 && SquaresToEdge[i, 1] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i - 15])))
                                {
                                    Temp1.Add(new Move(i, i - 15, 0));
                                }
                            }
                            if (SquaresToEdge[i, 0] >= 1 && SquaresToEdge[i, 1] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 6])))
                                {
                                    Temp1.Add(new Move(i, i - 6, 0));
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 10])))
                                {
                                    Temp1.Add(new Move(i, i + 10, 0));
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 17])))
                                {
                                    Temp1.Add(new Move(i, i + 17, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 15])))
                                {
                                    Temp1.Add(new Move(i, i + 15, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 6])))
                                {
                                    Temp1.Add(new Move(i, i + 6, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 0] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i - 10])))
                                {
                                    Temp1.Add(new Move(i, i - 10, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 0] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 17])))
                                {
                                    Temp1.Add(new Move(i, i - 17, 0));
                                }
                            }
                        }
                        //WHITE KING
                        else if (Position[i] == WhitePieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i, j] > 0))
                                {
                                    if (!(WhitePieces.Contains(Position[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j], k] > 0))
                                            {
                                                //check if the kings are next to each other:
                                                if (Position[i + DirectionOffsets[j] + DirectionOffsets[k]] == BlackPieces[4])
                                                {
                                                    IsKingNextTo = true; break;
                                                }
                                            }
                                        }
                                        if (IsKingNextTo == false)
                                        {
                                            PiecesOnBoard = Position.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == true)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                                else
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                                else
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                            }
                                        }
                                        IsKingNextTo = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 64; i++)
                {
                    if (BlackPieces.Contains(Position[i]))
                    {
                        //BLACK PAWN
                        if (Position[i] == BlackPieces[8])
                        {
                            if (IsPlayerWhite == false)
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i - 16, 0));
                                    }
                                }
                                if (Position[i - 8] == NoPiece && SquaresToEdge[i, 0] > 1)
                                {
                                    Temp1.Add(new Move(i, i - 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i - 9]))
                                    {
                                        Temp1.Add(new Move(i, i - 9, 0));
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i - 7]))
                                    {
                                        Temp1.Add(new Move(i, i - 7, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if (Position[i - 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i - 8, 1));
                                        Temp1.Add(new Move(i, i - 8, 2));
                                        Temp1.Add(new Move(i, i - 8, 3));
                                        Temp1.Add(new Move(i, i - 8, 4));
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i - 7]))
                                        {
                                            Temp1.Add(new Move(i, i - 7, 1));
                                            Temp1.Add(new Move(i, i - 7, 2));
                                            Temp1.Add(new Move(i, i - 7, 3));
                                            Temp1.Add(new Move(i, i - 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i - 9]))
                                        {
                                            Temp1.Add(new Move(i, i - 9, 1));
                                            Temp1.Add(new Move(i, i - 9, 2));
                                            Temp1.Add(new Move(i, i - 9, 3));
                                            Temp1.Add(new Move(i, i - 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 0] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (Position[i + 1] == WhitePieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i - 7, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (Position[i - 1] == WhitePieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i - 9, 6));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i + 16, 0));
                                    }
                                }
                                if (Position[i + 8] == NoPiece && SquaresToEdge[i, 2] > 1)
                                {
                                    Temp1.Add(new Move(i, i + 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i + 7]))
                                    {
                                        Temp1.Add(new Move(i, i + 7, 0));
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i + 9]))
                                    {
                                        Temp1.Add(new Move(i, i + 9, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if (Position[i + 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i + 8, 1));
                                        Temp1.Add(new Move(i, i + 8, 2));
                                        Temp1.Add(new Move(i, i + 8, 3));
                                        Temp1.Add(new Move(i, i + 8, 4));
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i + 7]))
                                        {
                                            Temp1.Add(new Move(i, i + 7, 1));
                                            Temp1.Add(new Move(i, i + 7, 2));
                                            Temp1.Add(new Move(i, i + 7, 3));
                                            Temp1.Add(new Move(i, i + 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i + 9]))
                                        {
                                            Temp1.Add(new Move(i, i + 9, 1));
                                            Temp1.Add(new Move(i, i + 9, 2));
                                            Temp1.Add(new Move(i, i + 9, 3));
                                            Temp1.Add(new Move(i, i + 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 2] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (Position[i + 1] == WhitePieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i + 9, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (Position[i - 1] == WhitePieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i + 7, 6));
                                        }
                                    }
                                }
                            }
                        }
                        //BLACK ROOK
                        else if (Position[i] == BlackPieces[0] || Position[i] == BlackPieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK BISHOP
                        else if (Position[i] == BlackPieces[2] || Position[i] == BlackPieces[5])
                        {
                            for (int j = 4; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK QUEEN
                        else if (Position[i] == BlackPieces[3])
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK KNIGHT
                        else if (Position[i] == BlackPieces[1] || Position[i] == BlackPieces[6])
                        {
                            //8 if, 8 possible move
                            if (SquaresToEdge[i, 0] >= 2 && SquaresToEdge[i, 1] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i - 15])))
                                {
                                    Temp1.Add(new Move(i, i - 15, 0));
                                }
                            }
                            if (SquaresToEdge[i, 0] >= 1 && SquaresToEdge[i, 1] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 6])))
                                {
                                    Temp1.Add(new Move(i, i - 6, 0));
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 10])))
                                {
                                    Temp1.Add(new Move(i, i + 10, 0));
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 17])))
                                {
                                    Temp1.Add(new Move(i, i + 17, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 15])))
                                {
                                    Temp1.Add(new Move(i, i + 15, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 6])))
                                {
                                    Temp1.Add(new Move(i, i + 6, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 0] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i - 10])))
                                {
                                    Temp1.Add(new Move(i, i - 10, 0));
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 0] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 17])))
                                {
                                    Temp1.Add(new Move(i, i - 17, 0));
                                }
                            }
                        }
                        //BLACK KING
                        else if (Position[i] == BlackPieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i, j] > 0))
                                {
                                    if (!(BlackPieces.Contains(Position[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j], k] > 0))
                                            {
                                                //check if the kings are next to each other:
                                                if (Position[i + DirectionOffsets[j] + DirectionOffsets[k]] == WhitePieces[4])
                                                {
                                                    IsKingNextTo = true; break;
                                                }
                                            }
                                        }
                                        if (IsKingNextTo == false)
                                        {
                                            PiecesOnBoard = Position.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == false)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                                else
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                                else
                                                {
                                                    Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
                                                }
                                            }
                                        }
                                        IsKingNextTo = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Temp1;
        }
        private List<Move> PossibleCastlingMoves()
        {
            Temp2.Clear();
            if (PlayerToMoveWhite)
            {
                if (IsPlayerWhite)
                {
                    if (Position[60] == WhitePieces[4])
                    {
                        //O-O-O
                        if (castling[0] && castling[2] && Position[57] == NoPiece && Position[58] == NoPiece && Position[59] == NoPiece && !IsInCheck(60) && !IsInCheck(59) && Position[49] != BlackPieces[4] && Position[50] != BlackPieces[4])
                        {
                            Temp2.Add(new Move(60, 58, 7));
                        }
                        //O-O
                        if (castling[0] && castling[3] && Position[61] == NoPiece && Position[62] == NoPiece && !IsInCheck(60) && !IsInCheck(61) && Position[54] != BlackPieces[4] && Position[55] != BlackPieces[4])
                        {
                            Temp2.Add(new Move(60, 62, 8));
                        }
                    }
                }
                else
                {
                    if (Position[3] == WhitePieces[4])
                    {
                        //O-O-O
                        if (castling[0] && castling[5] && Position[4] == NoPiece && Position[5] == NoPiece && Position[6] == NoPiece && !IsInCheck(3) && !IsInCheck(4) && Position[13] != BlackPieces[4] && Position[14] != BlackPieces[4])
                        {
                            Temp2.Add(new Move(3, 5, 10));
                        }
                        //O-O
                        if (castling[0] && castling[4] && Position[1] == NoPiece && Position[2] == NoPiece && !IsInCheck(2) && !IsInCheck(3) && Position[8] != BlackPieces[4] && Position[9] != BlackPieces[4])
                        {
                            Temp2.Add(new Move(3, 1, 9));
                        }
                    }
                }
            }
            else
            {
                if (IsPlayerWhite)
                {
                    if (Position[4] == BlackPieces[4])
                    {
                        //O-O-O
                        if (castling[1] && castling[4] && Position[1] == NoPiece && Position[2] == NoPiece && Position[3] == NoPiece && !IsInCheck(4) && !IsInCheck(3) && Position[9] != WhitePieces[4] && Position[10] != WhitePieces[4])
                        {
                            Temp2.Add(new Move(4, 2, 9));
                        }
                        //O-O
                        if (castling[1] && castling[5] && Position[5] == NoPiece && Position[6] == NoPiece && !IsInCheck(4) && !IsInCheck(5) && Position[14] != WhitePieces[4] && Position[15] != WhitePieces[4])
                        {
                            Temp2.Add(new Move(4, 6, 10));
                        }
                    }
                }
                else
                {
                    if (Position[59] == BlackPieces[4])
                    {
                        //O-O-O
                        if (castling[1] && castling[3] && Position[60] == NoPiece && Position[61] == NoPiece && Position[62] == NoPiece && !IsInCheck(59) && !IsInCheck(60) && Position[53] != WhitePieces[4] && Position[54] != WhitePieces[4])
                        {
                            Temp2.Add(new Move(59, 61, 8));
                        }
                        //O-O
                        if (castling[1] && castling[2] && Position[57] == NoPiece && Position[58] == NoPiece && !IsInCheck(58) && !IsInCheck(59) && Position[48] != WhitePieces[4] && Position[49] != WhitePieces[4])
                        {
                            Temp2.Add(new Move(59, 57, 7));
                        }
                    }
                }
            }
            
            return Temp2;
        }
        private bool GenerateMoves(List<Move> ListToAddMoves)
        {
            ListToAddMoves.Clear();
            CurrentColorMoves = new List<Move>(PossibleMoves());
            //checking for checks:
            foreach (Move move in CurrentColorMoves)
            {
                MakeCurrentMove(move);
                if (!IsInCheck(Array.IndexOf(Position, PlayerToMoveWhite ? WhitePieces[4] : BlackPieces[4])))
                {
                    ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, move.Extra));
                }                
                UnmakeCurrentMove(move, TempStartingSquareSource, TempTargetSquareSource);
            }
            //end of match:
            if (ListToAddMoves.Count == 0)
            {
                return false;
            }
            else if (Position.Count(piece => piece != NoPiece) == 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        ImageSource TempTargetSquareSource = null;
        ImageSource TempStartingSquareSource = null;
        Stack<Tuple<int, int>> previousMoves = new Stack<Tuple<int, int>>();
        private void MakeCurrentMove(Move move)
        {
            bool[] currentCastlingRights = (bool[])castling.Clone();
            castlingHistory.Push(currentCastlingRights);
            TempTargetSquareSource = Position[move.TargetSquare];
            TempStartingSquareSource = Position[move.StartingSquare];
            switch (move.Extra)
            {
                case 0: // regular move
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    break;
                case 1: //promotion
                    Position[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[3] : BlackPieces[3];
                    break;
                case 2: //promotion
                    Position[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 3: //promotion
                    Position[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[2] : BlackPieces[2];
                    break;
                case 4: //promotion
                    Position[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[1] : BlackPieces[1];
                    break;
                case 5: // en passant
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[move.StartingSquare + 1] = NoPiece;
                    break;
                case 6: // en passant
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[move.StartingSquare - 1] = NoPiece;
                    break;
                case 7: // castling
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[56] = NoPiece;
                    Position[PlayerToMoveWhite ? 59 : 58] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 8: // castling
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[63] = NoPiece;
                    Position[PlayerToMoveWhite ? 61 : 60] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 9: // castling
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[0] = NoPiece;
                    Position[PlayerToMoveWhite ? 2 : 3] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 10: // castling
                    Position[move.TargetSquare] = Position[move.StartingSquare];
                    Position[7] = NoPiece;
                    Position[PlayerToMoveWhite ? 4 : 5] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
            }
            Position[move.StartingSquare] = NoPiece;
            switch (move.StartingSquare)
            {
                case 56:
                    castling[2] = false;
                    break;
                case 63:
                    castling[3] = false;
                    break;
                case 0:
                    castling[4] = false;
                    break;
                case 7:
                    castling[5] = false;
                    break;
                case 60:
                    if (IsPlayerWhite)
                    {
                        castling[0] = false;
                    }
                    break;
                case 4:
                    if (IsPlayerWhite)
                    {
                        castling[1] = false;
                    }
                    break;
                case 3:
                    if (!IsPlayerWhite)
                    {
                        castling[0] = false;
                    }
                    break;
                case 59:
                    if (!IsPlayerWhite)
                    {
                        castling[1] = false;
                    }
                    break;
            }
            switch (move.TargetSquare)
            {
                case 56:
                    castling[2] = false;
                    break;
                case 63:
                    castling[3] = false;
                    break;
                case 0:
                    castling[4] = false;
                    break;
                case 7:
                    castling[5] = false;
                    break;
            }
            previousMoves.Push(new Tuple<int, int>(LastMoveStarting, LastMoveTarget));
            LastMoveStarting = move.StartingSquare;
            LastMoveTarget = move.TargetSquare;
        }
        private void UnmakeCurrentMove(Move move, ImageSource TempStarting, ImageSource TempTarget)
        {
            Position[move.StartingSquare] = TempStarting;
            Position[move.TargetSquare] = TempTarget;
            switch (move.Extra)
            {
                case 5: // en passant
                    if (PlayerToMoveWhite)
                    {
                        Position[move.StartingSquare + 1] = BlackPieces[8];
                    }
                    else
                    {
                        Position[move.StartingSquare + 1] = WhitePieces[8];
                    }
                    break;
                case 6: // en passant
                    if (PlayerToMoveWhite)
                    {
                        Position[move.StartingSquare - 1] = BlackPieces[8];
                    }
                    else
                    {
                        Position[move.StartingSquare - 1] = WhitePieces[8];
                    }
                    break;
                case 7: // castling
                    Position[56] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    Position[PlayerToMoveWhite ? 59 : 58] = NoPiece;
                    break;
                case 8: // castling
                    Position[63] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    Position[PlayerToMoveWhite ? 61 : 60] = NoPiece;
                    break;
                case 9: // castling
                    Position[0] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    Position[PlayerToMoveWhite ? 2 : 3] = NoPiece;
                    break;
                case 10: // castling
                    Position[7] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    Position[PlayerToMoveWhite ? 4 : 5] = NoPiece;
                    break;
            }
            var lastMove = previousMoves.Pop();
            LastMoveStarting = lastMove.Item1;
            LastMoveTarget = lastMove.Item2;
            castling = castlingHistory.Pop();
        }
        ImageButton AISelectedBefore = null;
        ImageButton AISelectedSquare = null;
        ImageSource AISelectedPiece = null;
        int AIselectedIndexBefore = -1;
        int AIselectedIndexSquare = -1;
        Move GeneratedMove = new Move();
        private void AIToMove()
        {
            if (GenerateMoves(Moves) == false)
            {
                //checkmate or stalemate
                if (IsInCheck(Array.IndexOf(Position, PlayerToMoveWhite ? WhitePieces[4] : BlackPieces[4])))
                {
                    //checkmate:
                    MateSound.Play();
                    btnReset.Text = "CHECKMATE!";
                }
                else
                {
                    //stalemate:
                    MateSound.Play();
                    btnReset.Text = "STALEMATE!";
                }
                return;
            }
            //calculating depth:
            PiecesOnBoard = Position.Count(source => source != NoPiece);
            if (PiecesOnBoard > 10)
            {
                Trace.WriteLine(Convert.ToInt16(DepthSlider.Value));
                GeneratedMove = GetBestMove(Convert.ToInt16(DepthSlider.Value));
            }
            else if (PiecesOnBoard > 6)
            {
                Trace.WriteLine(Convert.ToInt16(DepthSlider.Value) *2);
                GeneratedMove = GetBestMove(Convert.ToInt16(DepthSlider.Value) *2);
            }
            else
            {
                Trace.WriteLine(10);
                GeneratedMove = GetBestMove(10);
            }
            AISelectedBefore = AllSquares[GeneratedMove.StartingSquare];
            AISelectedPiece = AllSquares[GeneratedMove.StartingSquare].Source;
            AISelectedSquare = AllSquares[GeneratedMove.TargetSquare];

            for (int i = 0; i < AllSquares.Count(); i++)
            {
                if (AllSquares[i] == AISelectedBefore)
                {
                    AIselectedIndexBefore = i;
                    break;
                }
            }
            for (int i = 0; i < AllSquares.Count(); i++)
            {
                if (AllSquares[i] == AISelectedSquare)
                {
                    AIselectedIndexSquare = i;
                    break;
                }
            }
            if ((Position[AIselectedIndexSquare] != NoPiece) || (GeneratedMove.Extra == 5) || (GeneratedMove.Extra == 6))
            {
                CaptureSound.Play();
            }
            else
            {
                MoveSound.Play();
            }
            //for castling:
            if (AISelectedBefore == SquareA8)
            {
                castling[4] = false;
            }
            if (AISelectedBefore == SquareH8)
            {
                castling[5] = false;
            }
            if (PlayerToMoveWhite == true)
            {
                if (AISelectedBefore == SquareD8)
                {
                    castling[0] = false;
                }
            }
            else
            {
                if (AISelectedBefore == SquareE8)
                {
                    castling[1] = false;
                }
            }

            if (GeneratedMove.Extra > 0 && GeneratedMove.Extra < 5)
            {
                if (PlayerToMoveWhite == true)
                {
                    //promotion:
                    Position[AIselectedIndexBefore] = NoPiece;
                    AISelectedBefore.Source = NoPiece;
                    if (GeneratedMove.Extra == 1)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[3];
                        AISelectedSquare.Source = WhitePieces[3];
                    }
                    else if (GeneratedMove.Extra == 2)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[0];
                        AISelectedSquare.Source = WhitePieces[0];
                    }
                    else if (GeneratedMove.Extra == 3)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[2];
                        AISelectedSquare.Source = WhitePieces[2];
                    }
                    else if (GeneratedMove.Extra == 4)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[1];
                        AISelectedSquare.Source = WhitePieces[1];
                    }
                }
                else
                {
                    //promotion:
                    Position[AIselectedIndexBefore] = NoPiece;
                    AISelectedBefore.Source = NoPiece;
                    if (GeneratedMove.Extra == 1)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[3];
                        AISelectedSquare.Source = BlackPieces[3];
                    }
                    else if (GeneratedMove.Extra == 2)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[0];
                        AISelectedSquare.Source = BlackPieces[0];
                    }
                    else if (GeneratedMove.Extra == 3)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[2];
                        AISelectedSquare.Source = BlackPieces[2];
                    }
                    else if (GeneratedMove.Extra == 4)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[1];
                        AISelectedSquare.Source = BlackPieces[1];
                    }
                }
            }
            else
            {
                Position[AIselectedIndexBefore] = NoPiece;
                Position[AIselectedIndexSquare] = AISelectedPiece;
                AISelectedBefore.Source = NoPiece;
                AISelectedSquare.Source = AISelectedPiece;
            }
            //reset player move:
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
            //make AI move:
            if (WhiteSquares.Contains(AISelectedBefore))
            {
                AISelectedBefore.BackgroundColor = Color.FromArgb("#F4F680");
            }
            else
            {
                AISelectedBefore.BackgroundColor = Color.FromArgb("#BBCC44");
            }
            if (WhiteSquares.Contains(AISelectedSquare))
            {
                AISelectedSquare.BackgroundColor = Color.FromArgb("#F4F680");
            }
            else
            {
                AISelectedSquare.BackgroundColor = Color.FromArgb("#BBCC44");
            }
            LastMoveStarting = AIselectedIndexBefore;
            LastMoveTarget = AIselectedIndexSquare;
            //if en passant:
            if (GeneratedMove.Extra == 5)
            {
                Position[AIselectedIndexBefore + 1] = NoPiece;
                AllSquares[AIselectedIndexBefore + 1].Source = NoPiece;
            }
            else if (GeneratedMove.Extra == 6)
            {
                Position[AIselectedIndexBefore - 1] = NoPiece;
                AllSquares[AIselectedIndexBefore - 1].Source = NoPiece;
            }
            //castling:
            else if (GeneratedMove.Extra == 9)
            {
                Position[0] = NoPiece;
                AllSquares[0].Source = NoPiece;
                if (IsPlayerWhite == false)
                {
                    Position[2] = WhitePieces[0];
                    AllSquares[2].Source = WhitePieces[0];
                }
                else
                {
                    Position[3] = BlackPieces[0];
                    AllSquares[3].Source = BlackPieces[0];
                }
            }
            else if (GeneratedMove.Extra == 10)
            {
                Position[7] = NoPiece;
                AllSquares[7].Source = NoPiece;
                if (IsPlayerWhite == false)
                {
                    Position[4] = WhitePieces[0];
                    AllSquares[4].Source = WhitePieces[0];
                }
                else
                {
                    Position[5] = BlackPieces[0];
                    AllSquares[5].Source = BlackPieces[0];
                }
            }
            if (IsPlayerWhite == true)
            {
                PlayerToMoveWhite = true;
            }
            else
            {
                PlayerToMoveWhite = false;
            }

            //At the end after everything is set so that it's the player's turn, we need to run possiblemoves again for the other color:
            if (GenerateMoves(Moves) == false)
            {
                //checkmate or stalemate
                if (IsInCheck(Array.IndexOf(Position, PlayerToMoveWhite ? WhitePieces[4] : BlackPieces[4])))
                {
                    //checkmate:
                    MateSound.Play();
                    btnReset.Text = "CHECKMATE!";
                }
                else
                {
                    //stalemate:
                    MateSound.Play();
                    btnReset.Text = "STALEMATE!";
                }
                return;
            }
        }
        //---------------------
        //minimax eval:
        List<Move> possibleMoves = new List<Move>();
        List<Move> possibleOpponentMoves = new List<Move>();

        private int Minimax(int depth, bool maximizingPlayer, int alpha, int beta)
        {
            PlayerToMoveWhite = (maximizingPlayer && IsPlayerWhite) || (!maximizingPlayer && !IsPlayerWhite);
            GenerateMoves(possibleMoves);
            //OR CHECKMATE
            if (depth == 0 || possibleMoves.Count == 0)
            {
                int BoardValue = 0;
                PiecesOnBoard = Position.Count(source => source != NoPiece);
                //calculate board value
                if (IsPlayerWhite)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        var piece = Position[i];
                        if (piece != NoPiece)
                        {
                            int pieceIndex = Array.IndexOf(WhitePieces, piece);
                            if (pieceIndex == -1)
                            {
                                pieceIndex = Array.IndexOf(BlackPieces, piece);
                                BoardValue -= PieceValues[pieceIndex] + GetExtraValuesWhite(pieceIndex, i, false);
                            }
                            else
                            {
                                BoardValue += PieceValues[pieceIndex] + GetExtraValuesWhite(pieceIndex, i, true);
                            }
                        }
                    }
                    if (possibleMoves.Count == 0)
                    {
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        possibleOpponentMoves = new List<Move>(PossibleMoves());
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        if (PlayerToMoveWhite)
                        {
                            if (possibleOpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                            {
                                //making this move will result in checkmate:
                                BoardValue = -20000;
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                BoardValue = BoardValue < -100 ? 18000 : -18000;
                            }
                        }
                        else
                        {
                            if (possibleOpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                            {
                                //making this move will result in checkmate:
                                BoardValue = 20000;
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                BoardValue = BoardValue > 100 ? -18000 : 18000;
                            }
                        }
                    } 
                }
                else
                {
                    for (int i = 0; i < 64; i++)
                    {
                        var piece = Position[i];
                        if (piece != NoPiece)
                        {
                            int pieceIndex = Array.IndexOf(BlackPieces, piece);
                            if (pieceIndex == -1)
                            {
                                pieceIndex = Array.IndexOf(WhitePieces, piece);
                                BoardValue -= PieceValues[pieceIndex] + GetExtraValuesBlack(pieceIndex, i, true);
                            }
                            else
                            {
                                BoardValue += PieceValues[pieceIndex] + GetExtraValuesBlack(pieceIndex, i, false);
                            }
                        }
                    }
                    if (possibleMoves.Count == 0)
                    {
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        possibleOpponentMoves = new List<Move>(PossibleMoves());
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        if (PlayerToMoveWhite)
                        {
                            if (possibleOpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                            {
                                //making this move will result in checkmate:
                                BoardValue = 20000;
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                BoardValue = BoardValue > 100 ? -18000 : 18000;
                            }
                        }
                        else
                        {
                            if (possibleOpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                            {
                                //making this move will result in checkmate:
                                BoardValue = -20000;
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                BoardValue = BoardValue < -100 ? 18000 : -18000;
                            }
                        }
                    }
                }
                return BoardValue;
            }
            if (maximizingPlayer)
            {
                int maxEval = int.MinValue;
                foreach (Move move in possibleMoves.ToList())
                {
                    MakeCurrentMove(move);
                    StartStack.Push(TempStartingSquareSource);
                    TargetStack.Push(TempTargetSquareSource);
                    PlayerToMoveWhiteStack.Push(PlayerToMoveWhite);

                    int eval = Minimax(depth - 1, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    PlayerToMoveWhite = PlayerToMoveWhiteStack.Pop();
                    UnmakeCurrentMove(move, StartStack.Pop(), TargetStack.Pop());
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return maxEval;
            }
            else
            {
                int minEval = int.MaxValue;
                foreach (Move move in possibleMoves.ToList())
                {
                    MakeCurrentMove(move);
                    StartStack.Push(TempStartingSquareSource);
                    TargetStack.Push(TempTargetSquareSource);
                    PlayerToMoveWhiteStack.Push(PlayerToMoveWhite);

                    int eval = Minimax(depth - 1, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    PlayerToMoveWhite = PlayerToMoveWhiteStack.Pop();
                    UnmakeCurrentMove(move, StartStack.Pop(), TargetStack.Pop());
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
        }
        Stack<ImageSource> StartStack = new Stack<ImageSource>();
        Stack<ImageSource> TargetStack = new Stack<ImageSource>();
        Stack<bool> PlayerToMoveWhiteStack = new Stack<bool>();
        Stack<bool[]> castlingHistory = new Stack<bool[]>();
        private Move GetBestMove(int depth)
        {
            GenerateMoves(possibleMoves);
            int bestEval = int.MaxValue;
            Move bestMove = default;
            foreach (Move move in possibleMoves.ToList())
            {
                Position.CopyTo(Position, 0);
                MakeCurrentMove(move);
                StartStack.Push(TempStartingSquareSource);
                TargetStack.Push(TempTargetSquareSource);
                PlayerToMoveWhiteStack.Push(PlayerToMoveWhite);

                int eval = Minimax(depth - 1, true, int.MinValue, int.MaxValue);
                if (eval < bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                PlayerToMoveWhite = PlayerToMoveWhiteStack.Pop();
                UnmakeCurrentMove(move, StartStack.Pop(), TargetStack.Pop());
            }

            return bestMove;
        }

        private int GetExtraValuesWhite(int pieceIndex, int position, bool isWhite)
        {
            switch (pieceIndex)
            {
                case 8:
                    return isWhite ? P2M_PawnExtraValues[position] : AI2M_PawnExtraValues[position];
                case 0:
                case 7:
                    return isWhite ? P2M_RookExtraValues[position] : AI2M_RookExtraValues[position];
                case 2:
                case 5:
                    return BishopExtraValues[position];
                case 3:
                    return QueenExtraValues[position];
                case 1:
                case 6:
                    return KnightExtraValues[position];
                case 4:
                    return (PiecesOnBoard > 12 ? (isWhite ? P2M_KingExtraValues[position] : AI2M_KingExtraValues[position]) : Endgame_KingExtraValues[position]);
                default:
                    return 0;
            }
        }
        private int GetExtraValuesBlack(int pieceIndex, int position, bool isWhite)
        {
            switch (pieceIndex)
            {
                case 8:
                    return isWhite ? AI2M_PawnExtraValues[position] : P2M_PawnExtraValues[position];
                case 0:
                case 7:
                    return isWhite ? AI2M_RookExtraValues[position] : P2M_RookExtraValues[position];
                case 2:
                case 5:
                    return BishopExtraValues[position];
                case 3:
                    return QueenExtraValues[position];
                case 1:
                case 6:
                    return KnightExtraValues[position];
                case 4:
                    return (PiecesOnBoard > 12 ? (isWhite ? AI2M_KingExtraValues[position] : P2M_KingExtraValues[position]) : Endgame_KingExtraValues[position]);
                default:
                    return 0;
            }
        }
        private bool IsInCheck(int square)
        {
            if (PlayerToMoveWhite)
            {
                for (int j = 0; j < 8; j++)
                {
                    int Offset = DirectionOffsets[j];
                    for (int k = 0; k < SquaresToEdge[square, j]; k++)
                    {
                        if (WhitePieces.Contains(Position[square + ((k + 1) * Offset)]))
                        {
                            break;
                        }
                        //pawn
                        if (BlackPieces[8] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //knight
                        if (BlackPieces[1] == Position[square + ((k + 1) * Offset)] || BlackPieces[6] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //king
                        if (BlackPieces[4] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //queen
                        if (BlackPieces[3] == Position[square + ((k + 1) * Offset)])
                        {
                            return true;
                        }
                        //rook
                        if (j < 4 && ((BlackPieces[0] == Position[square + ((k + 1) * Offset)]) || (BlackPieces[7] == Position[square + ((k + 1) * Offset)])))
                        {
                            return true;
                        }
                        //bishop
                        if (j >= 4 && ((BlackPieces[2] == Position[square + ((k + 1) * Offset)]) || (BlackPieces[5] == Position[square + ((k + 1) * Offset)])))
                        {
                            return true;
                        }
                    }   
                }
                //knight
                if (SquaresToEdge[square, 0] >= 2 && SquaresToEdge[square, 1] >= 1)
                {
                    if ((BlackPieces[1] == Position[square - 15]) || (BlackPieces[6] == Position[square - 15]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 0] >= 1 && SquaresToEdge[square, 1] >= 2)
                {
                    if ((BlackPieces[1] == Position[square - 6]) || (BlackPieces[6] == Position[square - 6]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 1] >= 2 && SquaresToEdge[square, 2] >= 1)
                {
                    if ((BlackPieces[1] == Position[square + 10]) || (BlackPieces[6] == Position[square + 10]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 1] >= 1 && SquaresToEdge[square, 2] >= 2)
                {
                    if ((BlackPieces[1] == Position[square + 17]) || (BlackPieces[6] == Position[square + 17]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 1 && SquaresToEdge[square, 2] >= 2)
                {
                    if ((BlackPieces[1] == Position[square + 15]) || (BlackPieces[6] == Position[square + 15]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 2 && SquaresToEdge[square, 2] >= 1)
                {
                    if ((BlackPieces[1] == Position[square + 6]) || (BlackPieces[6] == Position[square + 6]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 2 && SquaresToEdge[square, 0] >= 1)
                {
                    if ((BlackPieces[1] == Position[square - 10]) || (BlackPieces[6] == Position[square - 10]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 1 && SquaresToEdge[square, 0] >= 2)
                {
                    if ((BlackPieces[1] == Position[square - 17]) || (BlackPieces[6] == Position[square - 17]))
                    {
                        return true;
                    }
                }
                //pawn
                if (IsPlayerWhite)
                {
                    if (SquaresToEdge[square, 0] > 0 && SquaresToEdge[square, 1] > 0)
                    {
                        if (BlackPieces[8] == Position[square - 7])
                        {
                            return true;
                        }
                    }
                    if (SquaresToEdge[square, 0] > 0 && SquaresToEdge[square, 3] > 0)
                    {
                        if (BlackPieces[8] == Position[square - 9])
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (SquaresToEdge[square, 2] > 0 && SquaresToEdge[square, 1] > 0)
                    {
                        if (BlackPieces[8] == Position[square + 9])
                        {
                            return true;
                        }
                    }
                    if (SquaresToEdge[square, 2] > 0 && SquaresToEdge[square, 3] > 0)
                    {
                        if (BlackPieces[8] == Position[square + 7])
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 8; j++)
                {
                    int Offset = DirectionOffsets[j];
                    for (int k = 0; k < SquaresToEdge[square, j]; k++)
                    {
                        if (BlackPieces.Contains(Position[square + ((k + 1) * Offset)]))
                        {
                            break;
                        }
                        //pawn
                        if (WhitePieces[8] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //knight
                        if (WhitePieces[1] == Position[square + ((k + 1) * Offset)] || WhitePieces[6] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //king
                        if (WhitePieces[4] == Position[square + ((k + 1) * Offset)])
                        {
                            break;
                        }
                        //queen
                        if (WhitePieces[3] == Position[square + ((k + 1) * Offset)])
                        {
                            return true;
                        }
                        //rook
                        if (j < 4 && ((WhitePieces[0] == Position[square + ((k + 1) * Offset)]) || (WhitePieces[7] == Position[square + ((k + 1) * Offset)])))
                        {
                            return true;
                        }
                        //bishop
                        if (j >= 4 && ((WhitePieces[2] == Position[square + ((k + 1) * Offset)]) || (WhitePieces[5] == Position[square + ((k + 1) * Offset)])))
                        {
                            return true;
                        }
                    }
                }
                //knight
                if (SquaresToEdge[square, 0] >= 2 && SquaresToEdge[square, 1] >= 1)
                {
                    if ((WhitePieces[1] == Position[square - 15]) || (WhitePieces[6] == Position[square - 15]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 0] >= 1 && SquaresToEdge[square, 1] >= 2)
                {
                    if ((WhitePieces[1] == Position[square - 6]) || (WhitePieces[6] == Position[square - 6]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 1] >= 2 && SquaresToEdge[square, 2] >= 1)
                {
                    if ((WhitePieces[1] == Position[square + 10]) || (WhitePieces[6] == Position[square + 10]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 1] >= 1 && SquaresToEdge[square, 2] >= 2)
                {
                    if ((WhitePieces[1] == Position[square + 17]) || (WhitePieces[6] == Position[square + 17]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 1 && SquaresToEdge[square, 2] >= 2)
                {
                    if ((WhitePieces[1] == Position[square + 15]) || (WhitePieces[6] == Position[square + 15]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 2 && SquaresToEdge[square, 2] >= 1)
                {
                    if ((WhitePieces[1] == Position[square + 6]) || (WhitePieces[6] == Position[square + 6]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 2 && SquaresToEdge[square, 0] >= 1)
                {
                    if ((WhitePieces[1] == Position[square - 10]) || (WhitePieces[6] == Position[square - 10]))
                    {
                        return true;
                    }
                }
                if (SquaresToEdge[square, 3] >= 1 && SquaresToEdge[square, 0] >= 2)
                {
                    if ((WhitePieces[1] == Position[square - 17]) || (WhitePieces[6] == Position[square - 17]))
                    {
                        return true;
                    }
                }
                //pawn
                if (IsPlayerWhite)
                {
                    if (SquaresToEdge[square, 2] > 0 && SquaresToEdge[square, 1] > 0)
                    {
                        if (WhitePieces[8] == Position[square + 9])
                        {
                            return true;
                        }
                    }
                    if (SquaresToEdge[square, 2] > 0 && SquaresToEdge[square, 3] > 0)
                    {
                        if (WhitePieces[8] == Position[square + 7])
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (SquaresToEdge[square, 0] > 0 && SquaresToEdge[square, 1] > 0)
                    {
                        if (WhitePieces[8] == Position[square - 7])
                        {
                            return true;
                        }
                    }
                    if (SquaresToEdge[square, 0] > 0 && SquaresToEdge[square, 3] > 0)
                    {
                        if (WhitePieces[8] == Position[square - 9])
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}