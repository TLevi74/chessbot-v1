﻿using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace chessbot
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        //P-N-B-R-Q
        int[] PieceValues = {100, 320, 330, 500, 900};
        int[] P2M_PawnExtraValues = {0, 0, 0, 0, 0, 0, 0, 0, 50, 50, 50, 50, 50, 50, 50, 50, 10, 10, 20, 30, 30, 20, 10, 10, 5, 5, 10, 25, 25, 10, 5, 5, 0, 0, 0, 20, 20, 0, 0, 0, 5, -5, -10, 0, 0, -10, -5, 5, 5, 10, 10, -20, -20, 10, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0};
        int[] AI2M_PawnExtraValues = {0, 0, 0, 0, 0, 0, 0, 0, 5, 10, 10, -20, -20, 10, 10, 5, 5, -5, -10, 0, 0, -10, -5, 5, 0, 0, 0, 20, 20, 0, 0, 0, 5, 5, 10, 25, 25, 10, 5, 5, 10, 10, 20, 30, 30, 20, 10, 10, 50, 50, 50, 50, 50, 50, 50, 50, 0, 0, 0, 0, 0, 0, 0, 0};
        int[] KnightExtraValues = {-50,-20,-30,-30,-30,-30,-20,-50, -40,-20,  0,  0,  0,  0,-20,-40, -30,  0, 10, 15, 15, 10,  0,-30, -30,  5, 15, 20, 20, 15,  5,-30, -30,  0, 15, 20, 20, 15,  0,-30, -30,  5, 10, 15, 15, 10,  5,-30, -40,-20,  0,  5,  5,  0,-20,-40, -50,-20,-30,-30,-30,-30,-20,-50};
        int[] BishopExtraValues = { -20, -10, -10, -10, -10, -10, -10, -20, -10, 5, 0, 0, 0, 0, 5, -10, -10, 10, 10, 10, 10, 10, 10, -10, -10, 0, 10, 10, 10, 10, 0, -10, -10,  0, 10, 10, 10, 10,  0,-10,-10, 10, 10, 10, 10, 10, 10,-10,-10,  5,  0,  0,  0,  0,  5,-10,-20,-10,-10,-10,-10,-10,-10,-20};
        int[] P2M_RookExtraValues = {0,  0,  0,  0,  0,  0,  0,  0, 5, 10, 10, 10, 10, 10, 10,  5, -5,  0,  0,  0,  0,  0,  0, -5, -5,  0,  0,  0,  0,  0,  0, -5, -5,  0,  0,  0,  0,  0,  0, -5, -5,  0,  0,  0,  0,  0,  0, -5, -5,  0,  0,  0,  0,  0,  0, -5, 0,  0,  0,  5,  5,  0,  0,  0};
        int[] AI2M_RookExtraValues = { 0, 0, 0, 5, 5, 0, 0, 0, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, -5, 0, 0, 0, 0, 0, 0, -5, 5, 10, 10, 10, 10, 10, 10, 5, 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] QueenExtraValues = {-20,-10,-10, -5, -5,-10,-10,-20,-10,  0,  0,  0,  0,  0,  0,-10,-10,  0,  5,  5,  5,  5,  0,-10,-5,  0,  5,  5,  5,  5,  0, -5,-5,  0,  5,  5,  5,  5,  0, -5,-10,  5,  5,  5,  5,  5,  0,-10,-10,  0,  5,  0,  0,  0,  0,-10,-20,-10,-10, -5, -5,-10,-10,-20};
        int[] P2M_KingExtraValues = {-30,-40,-40,-50,-50,-40,-40,-30,-30,-40,-40,-50,-50,-40,-40,-30,-30,-40,-40,-50,-50,-40,-40,-30,-30,-40,-40,-50,-50,-40,-40,-30,-20,-30,-30,-40,-40,-30,-30,-20,-10,-20,-20,-20,-20,-20,-20,-10,20, 20,  0,  0,  0,  0, 20, 20,20, 30, 10,  0,  0, 10, 30, 20};
        int[] AI2M_KingExtraValues = {20, 30, 10, 0, 0, 10, 30, 20, 20, 20, 0, 0, 0, 0, 20, 20, -10, -20, -20, -20, -20, -20, -20, -10, -20, -30, -30, -40, -40, -30, -30, -20, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30, -30, -40, -40, -50, -50, -40, -40, -30};
        int[] Endgame_KingExtraValues = {-50,-40,-30,-20,-20,-30,-40,-50,-30,-20,-10,  0,  0,-10,-20,-30,-30,-10, 20, 30, 30, 20,-10,-30,-30,-10, 30, 40, 40, 30,-10,-30,-30,-10, 30, 40, 40, 30,-10,-30,-30,-10, 20, 30, 30, 20,-10,-30,-30,-30,  0,  0,  0,  0,-30,-30,-50,-30,-30,-30,-30,-30,-30,-50};


        ImageButton[] AllSquares = new ImageButton[64];
        ImageSource[] Position = new ImageSource[64];
        ImageButton[] WhiteSquares = new ImageButton[32];
        ImageButton[] BlackSquares = new ImageButton[32];
        ImageSource[] WhitePieces = {"white_rook.png", "white_knight.png", "white_bishop.png", "white_queen.png", "white_king.png", "white_bishop.png", "white_knight.png", "white_rook.png", "white_pawn.png" };
        ImageSource[] BlackPieces = {"black_rook.png", "black_knight.png", "black_bishop.png", "black_queen.png", "black_king.png", "black_bishop.png", "black_knight.png", "black_rook.png", "black_pawn.png" };
        ImageSource NoPiece = "transparent.png";
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
                    AllSquares[i].Source = WhitePieces[7-i];
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
                    AllSquares[i + 56].Source = BlackPieces[7-i];
                    Position[i + 56] = BlackPieces[7 - i];
                }
                AIToMove();
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
        public ImageButton currentButton = new ImageButton();

        private void PromotionPieceSelected(object sender, System.EventArgs e)
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
        private void SquareSelected(object sender, System.EventArgs e)
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
            else if(IsPlayerWhite == false && PlayerToMoveWhite == false)
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
        private void PlayerMovesTargetSquare(Boolean TempPlayerWhiteMoves)
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

        int[] DirectionOffsets = {-8, 1, 8, -1, -7, 9, 7, -9};
        Boolean IsKingNextTo = false;
        //for castling:
        //white king - black king - a1 rook - h1 rook - a8 rook - h8 rook

        Boolean[] castling = new Boolean[6];
        public struct Move
        {
            public int StartingSquare;
            public int TargetSquare;
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
            //---FOR TESTING---
            //for (int i = 0; i < TempMoves.Count; i++)
            //{
            //    Trace.WriteLine($"{i+1}: |{TempMoves[i].StartingSquare}, {TempMoves[i].TargetSquare}|");
            //}
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
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if ((inposition[i - 8] == NoPiece) && (inposition[i - 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i - 16, 0));
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    Temp1.Add(new Move(i, i - 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 9]))
                                    {
                                        Temp1.Add(new Move(i, i - 9, 0));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 7]))
                                    {
                                        Temp1.Add(new Move(i, i - 7, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i - 8, 1));
                                        Temp1.Add(new Move(i, i - 8, 2));
                                        Temp1.Add(new Move(i, i - 8, 3));
                                        Temp1.Add(new Move(i, i - 8, 4));
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 7]))
                                        {
                                            Temp1.Add(new Move(i, i - 7, 1));
                                            Temp1.Add(new Move(i, i - 7, 2));
                                            Temp1.Add(new Move(i, i - 7, 3));
                                            Temp1.Add(new Move(i, i - 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 9]))
                                        {
                                            Temp1.Add(new Move(i, i - 9, 1));
                                            Temp1.Add(new Move(i, i - 9, 2));
                                            Temp1.Add(new Move(i, i - 9, 3));
                                            Temp1.Add(new Move(i, i - 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i][0] == 3)
                                {
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (inposition[i + 1] == BlackPieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i - 7, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i - 9, 6));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if ((inposition[i + 8] == NoPiece) && (inposition[i + 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i + 16, 0));
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    Temp1.Add(new Move(i, i + 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 7]))
                                    {
                                        Temp1.Add(new Move(i, i + 7, 0));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 9]))
                                    {
                                        Temp1.Add(new Move(i, i + 9, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i + 8, 1));
                                        Temp1.Add(new Move(i, i + 8, 2));
                                        Temp1.Add(new Move(i, i + 8, 3));
                                        Temp1.Add(new Move(i, i + 8, 4));
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 7]))
                                        {
                                            Temp1.Add(new Move(i, i + 7, 1));
                                            Temp1.Add(new Move(i, i + 7, 2));
                                            Temp1.Add(new Move(i, i + 7, 3));
                                            Temp1.Add(new Move(i, i + 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 9]))
                                        {
                                            Temp1.Add(new Move(i, i + 9, 1));
                                            Temp1.Add(new Move(i, i + 9, 2));
                                            Temp1.Add(new Move(i, i + 9, 3));
                                            Temp1.Add(new Move(i, i + 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i][2] == 3)
                                {
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (inposition[i + 1] == BlackPieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i + 9, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i + 7, 6));
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                            if (SquaresToEdge[i][0] >= 2 && SquaresToEdge[i][1] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 15])))
                                {
                                    Temp1.Add(new Move(i, i - 15, 0));
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 6])))
                                {
                                    Temp1.Add(new Move(i, i - 6, 0));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 10])))
                                {
                                    Temp1.Add(new Move(i, i + 10, 0));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 17])))
                                {
                                    Temp1.Add(new Move(i, i + 17, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 15])))
                                {
                                    Temp1.Add(new Move(i, i + 15, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 6])))
                                {
                                    Temp1.Add(new Move(i, i + 6, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 10])))
                                {
                                    Temp1.Add(new Move(i, i - 10, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 17])))
                                {
                                    Temp1.Add(new Move(i, i - 17, 0));
                                }
                            }
                        }
                        //WHITE KING
                        else if (inposition[i] == WhitePieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i][j] > 0))
                                {
                                    if (!(WhitePieces.Contains(inposition[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j]][k] > 0))
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
                                            Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
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
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if ((inposition[i - 8] == NoPiece) && (inposition[i - 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i - 16, 0));
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    Temp1.Add(new Move(i, i - 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 9]))
                                    {
                                        Temp1.Add(new Move(i, i - 9, 0));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 7]))
                                    {
                                        Temp1.Add(new Move(i, i - 7, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i - 8, 1));
                                        Temp1.Add(new Move(i, i - 8, 2));
                                        Temp1.Add(new Move(i, i - 8, 3));
                                        Temp1.Add(new Move(i, i - 8, 4));
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 7]))
                                        {
                                            Temp1.Add(new Move(i, i - 7, 1));
                                            Temp1.Add(new Move(i, i - 7, 2));
                                            Temp1.Add(new Move(i, i - 7, 3));
                                            Temp1.Add(new Move(i, i - 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 9]))
                                        {
                                            Temp1.Add(new Move(i, i - 9, 1));
                                            Temp1.Add(new Move(i, i - 9, 2));
                                            Temp1.Add(new Move(i, i - 9, 3));
                                            Temp1.Add(new Move(i, i - 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i][0] == 3)
                                {
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (inposition[i + 1] == WhitePieces[8] && LastMoveStarting == i - 15 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i - 7, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i - 9, 6));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if ((inposition[i + 8] == NoPiece) && (inposition[i + 16] == NoPiece))
                                    {
                                        Temp1.Add(new Move(i, i + 16, 0));
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    Temp1.Add(new Move(i, i + 8, 0));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 7]))
                                    {
                                        Temp1.Add(new Move(i, i + 7, 0));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 9]))
                                    {
                                        Temp1.Add(new Move(i, i + 9, 0));
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        Temp1.Add(new Move(i, i + 8, 1));
                                        Temp1.Add(new Move(i, i + 8, 2));
                                        Temp1.Add(new Move(i, i + 8, 3));
                                        Temp1.Add(new Move(i, i + 8, 4));
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 7]))
                                        {
                                            Temp1.Add(new Move(i, i + 7, 1));
                                            Temp1.Add(new Move(i, i + 7, 2));
                                            Temp1.Add(new Move(i, i + 7, 3));
                                            Temp1.Add(new Move(i, i + 7, 4));
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 9]))
                                        {
                                            Temp1.Add(new Move(i, i + 9, 1));
                                            Temp1.Add(new Move(i, i + 9, 2));
                                            Temp1.Add(new Move(i, i + 9, 3));
                                            Temp1.Add(new Move(i, i + 9, 4));
                                        }
                                    }
                                }
                                //en passant:
                                if (SquaresToEdge[i][2] == 3)
                                {
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (inposition[i + 1] == WhitePieces[8] && LastMoveStarting == i + 17 && LastMoveTarget == i + 1)
                                        {
                                            Temp1.Add(new Move(i, i + 9, 5));
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            Temp1.Add(new Move(i, i + 7, 6));
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }
                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    Temp1.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j]), 0));

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
                            if (SquaresToEdge[i][0] >= 2 && SquaresToEdge[i][1] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 15])))
                                {
                                    Temp1.Add(new Move(i, i - 15, 0));
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 6])))
                                {
                                    Temp1.Add(new Move(i, i - 6, 0));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 10])))
                                {
                                    Temp1.Add(new Move(i, i + 10, 0));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 17])))
                                {
                                    Temp1.Add(new Move(i, i + 17, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 15])))
                                {
                                    Temp1.Add(new Move(i, i + 15, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 6])))
                                {
                                    Temp1.Add(new Move(i, i + 6, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 10])))
                                {
                                    Temp1.Add(new Move(i, i - 10, 0));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 17])))
                                {
                                    Temp1.Add(new Move(i, i - 17, 0));
                                }
                            }
                        }
                        //BLACK KING
                        else if (inposition[i] == BlackPieces[4])
                        {
                            //regular
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i][j] > 0))
                                {
                                    if (!(BlackPieces.Contains(inposition[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j]][k] > 0))
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
                                            Temp1.Add(new Move(i, i + DirectionOffsets[j], 0));
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
                                        Temp2.Add(new Move(60, 58, 7));
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
                                        Temp2.Add(new Move(60, 62, 8));
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
                                        Temp2.Add(new Move(3, 5, 10));
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
                                        Temp2.Add(new Move(3, 1, 9));
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
                                        Temp2.Add(new Move(59, 61, 8));
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
                                        Temp2.Add(new Move(59, 57, 7));
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
                                        Temp2.Add(new Move(4, 2, 9));
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
                                        Temp2.Add(new Move(4, 6, 10));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Temp2;
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
                    ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, move.Extra));
                }
                UnmakeCurrentMove(move, inposition, TempStartingSquareSource, TempTargetSquareSource);
            }

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
        }

        private void UnmakeCurrentMove(Move move, ImageSource[] onPosition, ImageSource TempStarting, ImageSource TempTarget)
        {
            onPosition[move.StartingSquare] = TempStarting;
            onPosition[move.TargetSquare] = TempTarget;

            switch (move.Extra)
            {
                case 0: // regular move
                    break;
                case 1: // promotion
                    break;
                case 2: // promotion
                    break;
                case 3: // promotion
                    break;
                case 4: // promotion
                    break;
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
            GeneratedMove = GetBestMove(3);
 
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
                        AISelectedSquare.Source =BlackPieces[0];
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
                //calculate board value
                //todo: give extra points for castling?
                int BoardValue = 0;
                if (IsPlayerWhite)
                {
                    PiecesOnBoard = GenPosition.Count(source => source != NoPiece);
                    for (int i = 0; i < 64; i++)
                    {
                        if (GenPosition[i] != NoPiece)
                        {
                            //White pawn
                            if (GenPosition[i] == WhitePieces[8])
                            {
                                BoardValue += 100 + P2M_PawnExtraValues[i];
                            }
                            //Black pawn
                            else if (GenPosition[i] == BlackPieces[8])
                            {
                                BoardValue -= 100 + AI2M_PawnExtraValues[i];
                            }
                            //White rook
                            else if(GenPosition[i] == WhitePieces[0] || GenPosition[i] == WhitePieces[7])
                            {
                                BoardValue += 500 + P2M_RookExtraValues[i];
                            }
                            //Black rook
                            else if (GenPosition[i] == BlackPieces[0] || GenPosition[i] == BlackPieces[7])
                            {
                                BoardValue -= 500 + AI2M_RookExtraValues[i];
                            }
                            //White bishop
                            else if (GenPosition[i] == WhitePieces[2] || GenPosition[i] == WhitePieces[5])
                            {
                                BoardValue += 330 + BishopExtraValues[i];
                            }
                            //Black bishop
                            else if (GenPosition[i] == BlackPieces[2] || GenPosition[i] == BlackPieces[5])
                            {
                                BoardValue -= 330 + BishopExtraValues[i];
                            }
                            //White queen
                            else if (GenPosition[i] == WhitePieces[3])
                            {
                                BoardValue += 900 + QueenExtraValues[i];
                            }
                            //Black queen
                            else if (GenPosition[i] == BlackPieces[3])
                            {
                                BoardValue -= 900 + QueenExtraValues[i];
                            }
                            //White Knight
                            else if (GenPosition[i] == WhitePieces[1] || GenPosition[i] == WhitePieces[6])
                            {
                                BoardValue += 320 + KnightExtraValues[i];
                            }
                            //Black Knight
                            else if (GenPosition[i] == BlackPieces[1] || GenPosition[i] == BlackPieces[6])
                            {
                                BoardValue -= 320 + KnightExtraValues[i];
                            }
                            //White king
                            else if (GenPosition[i] == WhitePieces[4])
                            {
                                if (PiecesOnBoard > 12)
                                {
                                    BoardValue += P2M_KingExtraValues[i];
                                }
                                else
                                {
                                    BoardValue += Endgame_KingExtraValues[i];
                                }
                            }
                            //Black king
                            else if (GenPosition[i] == BlackPieces[4])
                            {
                                if (PiecesOnBoard > 12)
                                {
                                    BoardValue -= AI2M_KingExtraValues[i];
                                }
                                else
                                {
                                    BoardValue -= Endgame_KingExtraValues[i];
                                }
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
                                if (BoardValue < -100)
                                {
                                    BoardValue = 18000;
                                }
                                else
                                {
                                    BoardValue = -18000;
                                }
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
                                if (BoardValue > 100)
                                {
                                    BoardValue = -18000;
                                }
                                else
                                {
                                    BoardValue = 18000;
                                }
                            }
                        } 
                    }
                }
                else
                {
                    PiecesOnBoard = GenPosition.Count(source => source != NoPiece);
                    for (int i = 0; i < 64; i++)
                    {
                        if (GenPosition[i] != NoPiece)
                        {
                            //White pawn
                            if (GenPosition[i] == WhitePieces[8])
                            {
                                BoardValue -= 100 + AI2M_PawnExtraValues[i];
                            }
                            //Black pawn
                            else if (GenPosition[i] == BlackPieces[8])
                            {
                                BoardValue += 100 + P2M_PawnExtraValues[i];
                            }
                            //White rook
                            else if (GenPosition[i] == WhitePieces[0] || GenPosition[i] == WhitePieces[7])
                            {
                                BoardValue -= 500 + AI2M_RookExtraValues[i];
                            }
                            //Black rook
                            else if (GenPosition[i] == BlackPieces[0] || GenPosition[i] == BlackPieces[7])
                            {
                                BoardValue += 500 + P2M_RookExtraValues[i];
                            }
                            //White bishop
                            else if (GenPosition[i] == WhitePieces[2] || GenPosition[i] == WhitePieces[5])
                            {
                                BoardValue -= 330 + BishopExtraValues[i];
                            }
                            //Black bishop
                            else if (GenPosition[i] == BlackPieces[2] || GenPosition[i] == BlackPieces[5])
                            {
                                BoardValue += 330 + BishopExtraValues[i];
                            }
                            //White queen
                            else if (GenPosition[i] == WhitePieces[3])
                            {
                                BoardValue -= 900 + QueenExtraValues[i];
                            }
                            //Black queen
                            else if (GenPosition[i] == BlackPieces[3])
                            {
                                BoardValue += 900 + QueenExtraValues[i];
                            }
                            //White Knight
                            else if (GenPosition[i] == WhitePieces[1] || GenPosition[i] == WhitePieces[6])
                            {
                                BoardValue -= 320 + KnightExtraValues[i];
                            }
                            //Black Knight
                            else if (GenPosition[i] == BlackPieces[1] || GenPosition[i] == BlackPieces[6])
                            {
                                BoardValue += 320 + KnightExtraValues[i];
                            }
                            //White king
                            else if (GenPosition[i] == WhitePieces[4])
                            {
                                if (PiecesOnBoard > 12)
                                {
                                    BoardValue -= AI2M_KingExtraValues[i];
                                }
                                else
                                {
                                    BoardValue -= Endgame_KingExtraValues[i];
                                }
                            }
                            //Black king
                            else if (GenPosition[i] == BlackPieces[4])
                            {
                                if (PiecesOnBoard > 12)
                                {
                                    BoardValue += P2M_KingExtraValues[i];
                                }
                                else
                                {
                                    BoardValue += Endgame_KingExtraValues[i];
                                }
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
                                if (BoardValue > 100)
                                {
                                    BoardValue = -18000;
                                }
                                else
                                {
                                    BoardValue = 18000;
                                }
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
                                if (BoardValue < -100)
                                {
                                    BoardValue = 18000;
                                }
                                else
                                {
                                    BoardValue = -18000;
                                }
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
                    ValueStack.Push(CurrentValue);

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
                    ValueStack.Push(CurrentValue);

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
        Stack<int> ValueStack = new Stack<int>();
        int CurrentValue;
        private Move GetBestMove(int depth)
        {
            GenerateMoves(possibleMoves, Position);
            int bestEval = int.MaxValue;
            Move bestMove = default(Move);      
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
            //for testing:
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Trace.Write($"{GenPosition[i*8+j]}");
            //    }
            //    Trace.WriteLine(" ");
            //}
            //Trace.WriteLine("Position: ");
            //for (int i = 0; i < 8; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //    {
            //        Trace.Write($"{Position[i * 8 + j]}");
            //    }
            //    Trace.WriteLine(" ");
            //}
            return bestMove;
        }
    }
}
