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
        private Dictionary<ImageSource, int> pieceValuesMap;
        bool IsPlayerWhite = true;
        bool PlayerToMoveWhite = true;

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
            ImageButton[] TempWhiteSquares = { SquareA8, SquareC8, SquareE8, SquareG8, SquareB7, SquareD7, SquareF7, SquareH7, SquareA6, SquareC6, SquareE6, SquareG6, SquareB5, SquareD5, SquareF5, SquareH5, SquareA4, SquareC4, SquareE4, SquareG4, SquareB3, SquareD3, SquareF3, SquareH3, SquareA2, SquareC2, SquareE2, SquareG2, SquareB1, SquareD1, SquareF1, SquareH1 };
            ImageButton[] TempBlackSquares = { SquareB8, SquareD8, SquareF8, SquareH8, SquareA7, SquareC7, SquareE7, SquareG7, SquareB6, SquareD6, SquareF6, SquareH6, SquareA5, SquareC5, SquareE5, SquareG5, SquareB4, SquareD4, SquareF4, SquareH4, SquareA3, SquareC3, SquareE3, SquareG3, SquareB2, SquareD2, SquareF2, SquareH2, SquareA1, SquareC1, SquareE1, SquareG1 };
            pieceValuesMap = new Dictionary<ImageSource, int>
            {
                { BlackPieces[8], PieceValues[8] },
                { WhitePieces[8], PieceValues[8] },
                { BlackPieces[1], PieceValues[1] },
                { BlackPieces[6], PieceValues[1] },
                { WhitePieces[1], PieceValues[1] },
                { WhitePieces[6], PieceValues[1] },
                { BlackPieces[2], PieceValues[2] },
                { BlackPieces[5], PieceValues[2] },
                { WhitePieces[2], PieceValues[2] },
                { WhitePieces[5], PieceValues[2] },
                { BlackPieces[0], PieceValues[0] },
                { BlackPieces[7], PieceValues[0] },
                { WhitePieces[0], PieceValues[0] },
                { WhitePieces[7], PieceValues[0] },
                { BlackPieces[3], PieceValues[3] },
                { BlackPieces[4], 0 },
                { WhitePieces[3], PieceValues[3] },
                { WhitePieces[4], 0 }
            };
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
                GenerateMoves(Moves, Position);
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

                if (Position[selectedIndexSquare] == NoPiece)
                {
                    MoveSound.Play();
                }
                else
                {
                    CaptureSound.Play();
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

        bool[] castling = new bool[6];
        public struct Move
        {
            public int StartingSquare;
            public int TargetSquare;
            public int Value;
            //Extra: used for special moves like castling, promotion and en passant
            public int Extra;
            public Move(int startingSquare, int targetSquare, int value, int extra)
            {
                StartingSquare = startingSquare;
                TargetSquare = targetSquare;
                Value = value;
                Extra = extra;
            }
        }
        List<Move> TempMoves = new List<Move>();
        List<Move> CurrentColorMoves = new List<Move>();
        List<Move> OpponentMoves = new List<Move>();
        List<Move> Moves = new List<Move>();
        List<Move> Temp1 = new List<Move>();
        List<Move> Temp2 = new List<Move>();

        List<Move> CastlingCheck = new List<Move>();

        int PiecesOnBoard = 32;
        private List<Move> PossibleMoves(ImageSource[] inposition)
        {
            TempMoves.Clear();
            TempMoves.AddRange(PossibleRegularMoves(inposition));
            TempMoves.AddRange(PossibleCastlingMoves(inposition));
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
        private List<Move> PossibleRegularMoves(ImageSource[] inposition)
        {
            Temp1.Clear();
            if (PlayerToMoveWhite == true)
            {
                for (int i = 0; i < 64; i++)
                {
                    if (WhitePieces.Contains(inposition[i]))
                    {
                        //WHITE PAWN
                        if (inposition[i] == WhitePieces[8])
                        {
                            if (IsPlayerWhite == true)
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if ((inposition[i - 8] == NoPiece) && (inposition[i - 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i - 16, 0, 0, inposition);
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i, 0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, 0, 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, 0, 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, 0, 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, 0, 1, inposition);
                                        ValueTempMoves(i, i - 8, 0, 2, inposition);
                                        ValueTempMoves(i, i - 8, 0, 3, inposition);
                                        ValueTempMoves(i, i - 8, 0, 4, inposition);
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, 0, 1, inposition);
                                            ValueTempMoves(i, i - 7, 0, 2, inposition);
                                            ValueTempMoves(i, i - 7, 0, 3, inposition);
                                            ValueTempMoves(i, i - 7, 0, 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, 0, 1, inposition);
                                            ValueTempMoves(i, i - 9, 0, 2, inposition);
                                            ValueTempMoves(i, i - 9, 0, 3, inposition);
                                            ValueTempMoves(i, i - 9, 0, 4, inposition);
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 0] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (inposition[i + 1] == BlackPieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            ValueTempMoves(i, i - 7, 0, 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i - 9, 0, 6, inposition);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if ((inposition[i + 8] == NoPiece) && (inposition[i + 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i + 16, 0, 0, inposition);
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i, 2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, 0, 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, 0, 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, 0, 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, 0, 1, inposition);
                                        ValueTempMoves(i, i + 8, 0, 2, inposition);
                                        ValueTempMoves(i, i + 8, 0, 3, inposition);
                                        ValueTempMoves(i, i + 8, 0, 4, inposition);
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, 0, 1, inposition);
                                            ValueTempMoves(i, i + 7, 0, 2, inposition);
                                            ValueTempMoves(i, i + 7, 0, 3, inposition);
                                            ValueTempMoves(i, i + 7, 0, 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, 0, 1, inposition);
                                            ValueTempMoves(i, i + 9, 0, 2, inposition);
                                            ValueTempMoves(i, i + 9, 0, 3, inposition);
                                            ValueTempMoves(i, i + 9, 0, 4, inposition);
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 2] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (inposition[i + 1] == BlackPieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            ValueTempMoves(i, i + 9, 0, 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i + 7, 0, 6, inposition);
                                        }
                                    }
                                }
                            }
                        }
                        //WHITE ROOK
                        else if (inposition[i] == WhitePieces[0] || inposition[i] == WhitePieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    if (IsPlayerWhite == true)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);
                                    }

                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE BISHOP
                        else if (inposition[i] == WhitePieces[2] || inposition[i] == WhitePieces[5])
                        {
                            for (int j = 4; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);

                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE QUEEN
                        else if (inposition[i] == WhitePieces[3])
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);

                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //WHITE KNIGHT
                        else if (inposition[i] == WhitePieces[1] || inposition[i] == WhitePieces[6])
                        {
                            //8 if, 8 possible move
                            if (SquaresToEdge[i, 0] >= 2 && SquaresToEdge[i, 1] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 15])))
                                {
                                    ValueTempMoves(i, i - 15, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 0] >= 1 && SquaresToEdge[i, 1] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 0] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 0] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, 0, 0, inposition);
                                }
                            }
                        }
                        //WHITE KING
                        else if (inposition[i] == WhitePieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i, j] > 0))
                                {
                                    if (!(WhitePieces.Contains(inposition[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j], k] > 0))
                                            {
                                                //check if the kings are next to each other:
                                                if (inposition[i + DirectionOffsets[j] + DirectionOffsets[k]] == BlackPieces[4])
                                                {
                                                    IsKingNextTo = true; break;
                                                }
                                            }
                                        }
                                        if (IsKingNextTo == false)
                                        {
                                            PiecesOnBoard = inposition.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == true)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
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
                    if (BlackPieces.Contains(inposition[i]))
                    {
                        //BLACK PAWN
                        if (inposition[i] == BlackPieces[8])
                        {
                            if (IsPlayerWhite == false)
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if ((inposition[i - 8] == NoPiece) && (inposition[i - 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i - 16, 0, 0, inposition);
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i, 0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, 0, 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, 0, 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, 0, 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, 0, 1, inposition);
                                        ValueTempMoves(i, i - 8, 0, 2, inposition);
                                        ValueTempMoves(i, i - 8, 0, 3, inposition);
                                        ValueTempMoves(i, i - 8, 0, 4, inposition);
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, 0, 1, inposition);
                                            ValueTempMoves(i, i - 7, 0, 2, inposition);
                                            ValueTempMoves(i, i - 7, 0, 3, inposition);
                                            ValueTempMoves(i, i - 7, 0, 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, 0, 1, inposition);
                                            ValueTempMoves(i, i - 9, 0, 2, inposition);
                                            ValueTempMoves(i, i - 9, 0, 3, inposition);
                                            ValueTempMoves(i, i - 9, 0, 4, inposition);
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 0] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (inposition[i + 1] == WhitePieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            ValueTempMoves(i, i - 7, 0, 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i - 9, 0, 6, inposition);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i, 0] == 1)
                                {
                                    if ((inposition[i + 8] == NoPiece) && (inposition[i + 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i + 16, 0, 0, inposition);
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i, 2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, 0, 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i, 3] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, 0, 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i, 1] > 0 && SquaresToEdge[i, 2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, 0, 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i, 2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, 0, 1, inposition);
                                        ValueTempMoves(i, i + 8, 0, 2, inposition);
                                        ValueTempMoves(i, i + 8, 0, 3, inposition);
                                        ValueTempMoves(i, i + 8, 0, 4, inposition);
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, 0, 1, inposition);
                                            ValueTempMoves(i, i + 7, 0, 2, inposition);
                                            ValueTempMoves(i, i + 7, 0, 3, inposition);
                                            ValueTempMoves(i, i + 7, 0, 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, 0, 1, inposition);
                                            ValueTempMoves(i, i + 9, 0, 2, inposition);
                                            ValueTempMoves(i, i + 9, 0, 3, inposition);
                                            ValueTempMoves(i, i + 9, 0, 4, inposition);
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i, 2] == 3)
                                {
                                    if (SquaresToEdge[i, 1] > 0)
                                    {
                                        if (inposition[i + 1] == WhitePieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            ValueTempMoves(i, i + 9, 0, 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i, 3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i + 7, 0, 6, inposition);
                                        }
                                    }
                                }
                            }
                        }
                        //BLACK ROOK
                        else if (inposition[i] == BlackPieces[0] || inposition[i] == BlackPieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    if (IsPlayerWhite == false)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);
                                    }

                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK BISHOP
                        else if (inposition[i] == BlackPieces[2] || inposition[i] == BlackPieces[5])
                        {
                            for (int j = 4; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);

                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK QUEEN
                        else if (inposition[i] == BlackPieces[3])
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i, j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), 0, 0, inposition);

                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        //BLACK KNIGHT
                        else if (inposition[i] == BlackPieces[1] || inposition[i] == BlackPieces[6])
                        {
                            //8 if, 8 possible move
                            if (SquaresToEdge[i, 0] >= 2 && SquaresToEdge[i, 1] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 15])))
                                {
                                    ValueTempMoves(i, i - 15, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 0] >= 1 && SquaresToEdge[i, 1] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 1] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 2 && SquaresToEdge[i, 0] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, 0, 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i, 3] >= 1 && SquaresToEdge[i, 0] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, 0, 0, inposition);
                                }
                            }
                        }
                        //BLACK KING
                        else if (inposition[i] == BlackPieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i, j] > 0))
                                {
                                    if (!(BlackPieces.Contains(inposition[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j], k] > 0))
                                            {
                                                //check if the kings are next to each other:
                                                if (inposition[i + DirectionOffsets[j] + DirectionOffsets[k]] == WhitePieces[4])
                                                {
                                                    IsKingNextTo = true; break;
                                                }
                                            }
                                        }
                                        if (IsKingNextTo == false)
                                        {
                                            PiecesOnBoard = inposition.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == false)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], 0, 0, inposition);
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
        private List<Move> PossibleCastlingMoves(ImageSource[] inposition)
        {
            Temp2.Clear();
            if (PlayerToMoveWhite == true)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (inposition[j] == WhitePieces[4])
                    {
                        //castling
                        if (IsPlayerWhite == true)
                        {
                            //O-O-O
                            if (castling[0] && castling[2] && SquareB1.Source == NoPiece && SquareC1.Source == NoPiece && SquareD1.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 60) && !CastlingCheck.Any(move => move.TargetSquare == 59))
                                {
                                    if (inposition[49] != BlackPieces[4] && inposition[50] != BlackPieces[4])
                                    {
                                        Temp2.Add(new Move(60, 58, 20, 7));
                                    }
                                }
                            }
                            //O-O
                            if (castling[0] && castling[3] && SquareF1.Source == NoPiece && SquareG1.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 60) && !CastlingCheck.Any(move => move.TargetSquare == 61))
                                {
                                    if (inposition[54] != BlackPieces[4] && inposition[55] != BlackPieces[4])
                                    {
                                        Temp2.Add(new Move(60, 62, 20, 8));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //O-O-O
                            if (castling[0] && castling[5] && SquareE8.Source == NoPiece && SquareF8.Source == NoPiece && SquareG8.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 3) && !CastlingCheck.Any(move => move.TargetSquare == 4))
                                {
                                    if (inposition[13] != BlackPieces[4] && inposition[14] != BlackPieces[4])
                                    {
                                        Temp2.Add(new Move(3, 5, 20, 10));
                                    }
                                }
                            }
                            //O-O
                            if (castling[0] && castling[4] && SquareB8.Source == NoPiece && SquareC8.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 3) && !CastlingCheck.Any(move => move.TargetSquare == 2))
                                {
                                    if (inposition[8] != BlackPieces[4] && inposition[9] != BlackPieces[4])
                                    {
                                        Temp2.Add(new Move(3, 1, 20, 9));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 64; j++)
                {
                    if (inposition[j] == BlackPieces[4])
                    {
                        //castling
                        if (IsPlayerWhite == false)
                        {
                            //O-O-O
                            if (castling[1] && castling[3] && SquareE1.Source == NoPiece && SquareF1.Source == NoPiece && SquareG1.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 59) && !CastlingCheck.Any(move => move.TargetSquare == 60))
                                {
                                    if (inposition[53] != WhitePieces[4] && inposition[54] != WhitePieces[4])
                                    {
                                        Temp2.Add(new Move(59, 61, 20, 8));
                                    }
                                }
                            }
                            //O-O
                            if (castling[1] && castling[2] && SquareB1.Source == NoPiece && SquareC1.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 59) && !CastlingCheck.Any(move => move.TargetSquare == 58))
                                {
                                    if (inposition[48] != WhitePieces[4] && inposition[49] != WhitePieces[4])
                                    {
                                        Temp2.Add(new Move(59, 57, 20, 7));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //O-O-O
                            if (castling[1] && castling[4] && SquareB8.Source == NoPiece && SquareC8.Source == NoPiece && SquareD8.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 4) && !CastlingCheck.Any(move => move.TargetSquare == 3))
                                {
                                    if (inposition[9] != WhitePieces[4] && inposition[10] != WhitePieces[4])
                                    {
                                        Temp2.Add(new Move(4, 2, 20, 9));
                                    }
                                }
                            }
                            //O-O
                            if (castling[1] && castling[5] && SquareF8.Source == NoPiece && SquareG8.Source == NoPiece)
                            {
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                CastlingCheck = new List<Move>(PossibleRegularMoves(inposition));
                                PlayerToMoveWhite = !PlayerToMoveWhite;
                                if (!CastlingCheck.Any(move => move.TargetSquare == 4) && !CastlingCheck.Any(move => move.TargetSquare == 5))
                                {
                                    if (inposition[14] != WhitePieces[4] && inposition[15] != WhitePieces[4])
                                    {
                                        Temp2.Add(new Move(4, 6, 20, 10));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Temp2;
        }
        private void ValueTempMoves(int _StartingSquare, int _TargetSquare, int _Value, int _Extra, ImageSource[] inposition)
        {
            pieceValuesMap.TryGetValue(inposition[_TargetSquare], out int pieceValue);
            Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + pieceValue, _Extra));
        }
        private bool GenerateMoves(List<Move> ListToAddMoves, ImageSource[] inposition)
        {
            ListToAddMoves.Clear();
            CurrentColorMoves = new List<Move>(PossibleMoves(inposition));
            //checking for checks:
            foreach (Move move in CurrentColorMoves)
            {
                MakeCurrentMove(move, inposition);
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves(inposition));
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (!OpponentMoves.Any(move => inposition[move.TargetSquare] == (PlayerToMoveWhite ? WhitePieces[4] : BlackPieces[4])))
                {
                    ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, move.Value, move.Extra));
                }
                UnmakeCurrentMove(move, inposition, TempStartingSquareSource, TempTargetSquareSource);
            }
            ListToAddMoves = ListToAddMoves.OrderByDescending(move => move.Value).ToList();
            //end of match:
            return ListToAddMoves.Count > 0;
        }
        ImageSource TempTargetSquareSource = null;
        ImageSource TempStartingSquareSource = null;
        private void MakeCurrentMove(Move move, ImageSource[] onPosition)
        {
            TempTargetSquareSource = onPosition[move.TargetSquare];
            TempStartingSquareSource = onPosition[move.StartingSquare];
            switch (move.Extra)
            {
                case 0: // regular move
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    break;
                case 1: //promotion
                    onPosition[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[3] : BlackPieces[3];
                    break;
                case 2: //promotion
                    onPosition[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 3: //promotion
                    onPosition[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[2] : BlackPieces[2];
                    break;
                case 4: //promotion
                    onPosition[move.TargetSquare] = PlayerToMoveWhite ? WhitePieces[1] : BlackPieces[1];
                    break;
                case 5: // en passant
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[move.StartingSquare + 1] = NoPiece;
                    break;
                case 6: // en passant
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[move.StartingSquare - 1] = NoPiece;
                    break;
                case 7: // castling
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[56] = NoPiece;
                    onPosition[PlayerToMoveWhite ? 59 : 58] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 8: // castling
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[63] = NoPiece;
                    onPosition[PlayerToMoveWhite ? 61 : 60] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 9: // castling
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[0] = NoPiece;
                    onPosition[PlayerToMoveWhite ? 2 : 3] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
                case 10: // castling
                    onPosition[move.TargetSquare] = onPosition[move.StartingSquare];
                    onPosition[7] = NoPiece;
                    onPosition[PlayerToMoveWhite ? 4 : 5] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    break;
            }
            onPosition[move.StartingSquare] = NoPiece;
            LastMoveStarting = move.StartingSquare;
            LastMoveTarget = move.TargetSquare;
        }
        private void UnmakeCurrentMove(Move move, ImageSource[] onPosition, ImageSource TempStarting, ImageSource TempTarget)
        {
            onPosition[move.StartingSquare] = TempStarting;
            onPosition[move.TargetSquare] = TempTarget;
            switch (move.Extra)
            {
                case 5: // en passant
                    if (PlayerToMoveWhite)
                    {
                        onPosition[move.StartingSquare + 1] = BlackPieces[8];
                    }
                    else
                    {
                        onPosition[move.StartingSquare + 1] = WhitePieces[8];
                    }
                    break;
                case 6: // en passant
                    if (PlayerToMoveWhite)
                    {
                        onPosition[move.StartingSquare - 1] = BlackPieces[8];
                    }
                    else
                    {
                        onPosition[move.StartingSquare - 1] = WhitePieces[8];
                    }
                    break;
                case 7: // castling
                    onPosition[56] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    onPosition[PlayerToMoveWhite ? 59 : 58] = NoPiece;
                    break;
                case 8: // castling
                    onPosition[63] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    onPosition[PlayerToMoveWhite ? 61 : 60] = NoPiece;
                    break;
                case 9: // castling
                    onPosition[0] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    onPosition[PlayerToMoveWhite ? 2 : 3] = NoPiece;
                    break;
                case 10: // castling
                    onPosition[7] = PlayerToMoveWhite ? WhitePieces[0] : BlackPieces[0];
                    onPosition[PlayerToMoveWhite ? 4 : 5] = NoPiece;
                    break;
            }
        }
        ImageButton AISelectedBefore = null;
        ImageButton AISelectedSquare = null;
        ImageSource AISelectedPiece = null;
        int AIselectedIndexBefore = -1;
        int AIselectedIndexSquare = -1;
        Move GeneratedMove = new Move();
        private void AIToMove()
        {
            if (GenerateMoves(Moves, Position) == false)
            {
                //checkmate or stalemate
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves(Position));
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (PlayerToMoveWhite == true)
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
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
                }
                else
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
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
                }
                return;
            }
            GeneratedMove = GetBestMove(5);

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
            //todo: en passant move sound
            if (Position[AIselectedIndexSquare] == NoPiece)
            {
                MoveSound.Play();
            }
            else
            {
                CaptureSound.Play();
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
            if (GenerateMoves(Moves, Position) == false)
            {
                //checkmate or stalemate
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves(Position));
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (PlayerToMoveWhite == true)
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
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
                }
                else
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
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
                }
                return;
            }
        }
        //---------------------
        //minimax eval:
        List<Move> possibleMoves = new List<Move>();
        List<Move> possibleOpponentMoves = new List<Move>();

        private int Minimax(ImageSource[] position, int depth, bool maximizingPlayer, int alpha, int beta)
        {
            PlayerToMoveWhite = (maximizingPlayer && IsPlayerWhite) || (!maximizingPlayer && !IsPlayerWhite);
            GenerateMoves(possibleMoves, position);
            //OR CHECKMATE
            if (depth == 0 || possibleMoves.Count == 0)
            {
                int BoardValue = 0;
                PiecesOnBoard = GenPosition.Count(source => source != NoPiece);
                //calculate board value
                if (IsPlayerWhite)
                {
                    for (int i = 0; i < 64; i++)
                    {
                        var piece = GenPosition[i];
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
                        possibleOpponentMoves = new List<Move>(PossibleMoves(GenPosition));
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        if (PlayerToMoveWhite)
                        {
                            if (possibleOpponentMoves.Any(move => GenPosition[move.TargetSquare] == WhitePieces[4]))
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
                            if (possibleOpponentMoves.Any(move => GenPosition[move.TargetSquare] == BlackPieces[4]))
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
                        var piece = GenPosition[i];
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
                        possibleOpponentMoves = new List<Move>(PossibleMoves(GenPosition));
                        PlayerToMoveWhite = !PlayerToMoveWhite;
                        if (PlayerToMoveWhite)
                        {
                            if (possibleOpponentMoves.Any(move => GenPosition[move.TargetSquare] == WhitePieces[4]))
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
                            if (possibleOpponentMoves.Any(move => GenPosition[move.TargetSquare] == BlackPieces[4]))
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
                    MakeCurrentMove(move, position);
                    StartStack.Push(TempStartingSquareSource);
                    TargetStack.Push(TempTargetSquareSource);
                    int eval = Minimax(position, depth - 1, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    UnmakeCurrentMove(move, position, StartStack.Pop(), TargetStack.Pop());
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
                    MakeCurrentMove(move, position);
                    StartStack.Push(TempStartingSquareSource);
                    TargetStack.Push(TempTargetSquareSource);
                    int eval = Minimax(position, depth - 1, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    UnmakeCurrentMove(move, position, StartStack.Pop(), TargetStack.Pop());
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
        }
        ImageSource[] GenPosition = new ImageSource[64];
        Stack<ImageSource> StartStack = new Stack<ImageSource>();
        Stack<ImageSource> TargetStack = new Stack<ImageSource>();
        private Move GetBestMove(int depth)
        {
            GenerateMoves(possibleMoves, Position);
            int bestEval = int.MaxValue;
            Move bestMove = default;
            foreach (Move move in possibleMoves.ToList())
            {
                Position.CopyTo(GenPosition, 0);
                MakeCurrentMove(move, GenPosition);
                StartStack.Push(TempStartingSquareSource);
                TargetStack.Push(TempTargetSquareSource);
                int eval = Minimax(GenPosition, depth - 1, true, int.MinValue, int.MaxValue);
                if (eval < bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                UnmakeCurrentMove(move, GenPosition, StartStack.Pop(), TargetStack.Pop());
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
    }
}