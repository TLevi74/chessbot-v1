using System.Diagnostics;
using System.Linq;

namespace chessbot
{
    public partial class MainPage : ContentPage
    {
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

        private void ResetBoard()
        {
            HasPlayerSelectedFromSquare = false;
            SelectedPiece = null;
            SelectedSquare = null;
            SelectedBefore = null;
            AISelectedPiece = null;
            AISelectedBefore = null;
            AISelectedSquare = null;
            PlayerToMoveWhite = true;
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
                GenerateMoves();
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
        Boolean HasWhiteKingMoved = false;
        Boolean HasBlackKingMoved = false;
        public struct Move
        {
            public int StartingSquare;
            public int TargetSquare;
            public Move(int startingSquare, int targetSquare)
            {
                StartingSquare = startingSquare;
                TargetSquare = targetSquare;
            }
        }
        List<Move> TempMoves = new List<Move>();
        List<Move> CurrentColorMoves = new List<Move>();
        List<Move> OpponentMoves = new List<Move>();
        List<Move> Moves = new List<Move>();
        private List<Move> PossibleMoves()
        {
            TempMoves.Clear();
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
                                if ((SquaresToEdge[i][2] == 1))
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        TempMoves.Add(new Move(i, i-16)); 
                                    }
                                }
                                //!!!!!after someone moved with a pawn, we need to check if it's not in the first line, because it has to promote!!!!!!
                                if (Position[i-8] == NoPiece)
                                {
                                    TempMoves.Add(new Move(i, i - 8));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0)
                                {
                                    if (BlackPieces.Contains(Position[i - 9]))
                                    {
                                        TempMoves.Add(new Move(i, i - 9));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0)
                                {
                                    if (BlackPieces.Contains(Position[i - 7]))
                                    {
                                        TempMoves.Add(new Move(i, i - 7));
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if ((SquaresToEdge[i][0] == 1))
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        TempMoves.Add(new Move(i, i + 16));
                                    }
                                }
                                //!!!!!after someone moved with a pawn, we need to check if it's not in the first line, because it has to promote!!!!!!
                                if (Position[i + 8] == NoPiece)
                                {
                                    TempMoves.Add(new Move(i, i + 8));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0)
                                {
                                    if (BlackPieces.Contains(Position[i + 7]))
                                    {
                                        TempMoves.Add(new Move(i, i + 7));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0)
                                {
                                    if (BlackPieces.Contains(Position[i + 9]))
                                    {
                                        TempMoves.Add(new Move(i, i + 9));
                                    }
                                }
                            }
                        }
                        //WHITE ROOK
                        else if (Position[i] == WhitePieces[0] || Position[i] == WhitePieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                            if (SquaresToEdge[i][0] >= 2 && SquaresToEdge[i][1] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i - 15])))
                                {
                                    TempMoves.Add(new Move(i, i - 15));
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 6])))
                                {
                                    TempMoves.Add(new Move(i, i - 6));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 10])))
                                {
                                    TempMoves.Add(new Move(i, i + 10));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 17])))
                                {
                                    TempMoves.Add(new Move(i, i + 17));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 15])))
                                {
                                    TempMoves.Add(new Move(i, i + 15));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 6])))
                                {
                                    TempMoves.Add(new Move(i, i + 6));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i - 10])))
                                {
                                    TempMoves.Add(new Move(i, i - 10));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 17])))
                                {
                                    TempMoves.Add(new Move(i, i - 17));
                                }
                            }
                        }
                        //WHITE KING
                        else if (Position[i] == WhitePieces[4])
                        {
                            //TODO: castleing (maybe haswhitekingmoved variable)
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i][j] > 0))
                                {
                                    if (!(WhitePieces.Contains(Position[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j]][k] > 0))
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
                                            TempMoves.Add(new Move(i, i + DirectionOffsets[j]));
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
                                if ((SquaresToEdge[i][2] == 1))
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        TempMoves.Add(new Move(i, i - 16));
                                    }
                                }
                                //!!!!!after someone moved with a pawn, we need to check if it's not in the first line, because it has to promote!!!!!!
                                if (Position[i - 8] == NoPiece)
                                {
                                    TempMoves.Add(new Move(i, i - 8));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0)
                                {
                                    if (WhitePieces.Contains(Position[i - 9]))
                                    {
                                        TempMoves.Add(new Move(i, i - 9));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0)
                                {
                                    if (WhitePieces.Contains(Position[i - 7]))
                                    {
                                        TempMoves.Add(new Move(i, i - 7));
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if ((SquaresToEdge[i][0] == 1))
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        TempMoves.Add(new Move(i, i + 16));
                                    }
                                }
                                //!!!!!after someone moved with a pawn, we need to check if it's not in the first line, because it has to promote!!!!!!
                                if (Position[i + 8] == NoPiece)
                                {
                                    TempMoves.Add(new Move(i, i + 8));
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0)
                                {
                                    if (WhitePieces.Contains(Position[i + 7]))
                                    {
                                        TempMoves.Add(new Move(i, i + 7));
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0)
                                {
                                    if (WhitePieces.Contains(Position[i + 9]))
                                    {
                                        TempMoves.Add(new Move(i, i + 9));
                                    }
                                }
                            }
                        }
                        //BLACK ROOK
                        else if (Position[i] == BlackPieces[0] || Position[i] == BlackPieces[7])
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    TempMoves.Add(new Move(i, i + ((k + 1) * DirectionOffsets[j])));

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
                            if (SquaresToEdge[i][0] >= 2 && SquaresToEdge[i][1] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i - 15])))
                                {
                                    TempMoves.Add(new Move(i, i - 15));
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 6])))
                                {
                                    TempMoves.Add(new Move(i, i - 6));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 10])))
                                {
                                    TempMoves.Add(new Move(i, i + 10));
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 17])))
                                {
                                    TempMoves.Add(new Move(i, i + 17));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 15])))
                                {
                                    TempMoves.Add(new Move(i, i + 15));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 6])))
                                {
                                    TempMoves.Add(new Move(i, i + 6));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i - 10])))
                                {
                                    TempMoves.Add(new Move(i, i - 10));
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 17])))
                                {
                                    TempMoves.Add(new Move(i, i - 17));
                                }
                            }
                        }
                        //BLACK KING
                        else if (Position[i] == BlackPieces[4])
                        {
                            //TODO: castleing (maybe haswhitekingmoved variable)
                            for (int j = 0; j < 8; j++)
                            {
                                if ((SquaresToEdge[i][j] > 0))
                                {
                                    if (!(BlackPieces.Contains(Position[i + DirectionOffsets[j]])))
                                    {
                                        for (int k = 0; k < 8; k++)
                                        {
                                            if ((SquaresToEdge[i + DirectionOffsets[j]][k] > 0))
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
                                            TempMoves.Add(new Move(i, i + DirectionOffsets[j]));
                                        }
                                        IsKingNextTo = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //---FOR TESTING---
            //for (int i = 0; i < TempMoves.Count; i++)
            //{
            //    Trace.WriteLine($"{i}: |{TempMoves[i].StartingSquare}, {TempMoves[i].TargetSquare}|");
            //}
            return TempMoves;
        }

        private void GenerateMoves()
        {
            Moves.Clear();
            CurrentColorMoves = new List<Move>(PossibleMoves());
            for (int i = 0; i < CurrentColorMoves.Count; i++)
            {
                MakeMove(i);
                
                if (PlayerToMoveWhite == true)
                {
                    if (!OpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                    {
                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare));
                    }
                }
                else
                {
                    if (!OpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                    {
                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare));
                    }
                }
                UnmakeMove(i);
            }
        }
        ImageSource TempTargetSquareSource = null;
        private void MakeMove(int i)
        {
            PlayerToMoveWhite = !PlayerToMoveWhite;
            TempTargetSquareSource = Position[CurrentColorMoves[i].TargetSquare];
            Position[CurrentColorMoves[i].TargetSquare] = Position[CurrentColorMoves[i].StartingSquare];
            Position[CurrentColorMoves[i].StartingSquare] = NoPiece;
            OpponentMoves = new List<Move>(PossibleMoves());
            PlayerToMoveWhite = !PlayerToMoveWhite;
        }

        private void UnmakeMove(int i)
        {
            Position[CurrentColorMoves[i].StartingSquare] = Position[CurrentColorMoves[i].TargetSquare];
            Position[CurrentColorMoves[i].TargetSquare] = TempTargetSquareSource;
        }

        ImageButton AISelectedBefore = null;
        ImageButton AISelectedSquare = null;
        ImageSource AISelectedPiece = null;
        int AIselectedIndexBefore = -1;
        int AIselectedIndexSquare = -1;
        private void AIToMove()
        {
            GenerateMoves();
            //Select a random move:
            Random random = new Random();
            int CurrentRan = random.Next(0, Moves.Count);
            
            AISelectedBefore = AllSquares[Moves[CurrentRan].StartingSquare];
            AISelectedPiece = AllSquares[Moves[CurrentRan].StartingSquare].Source;
            AISelectedSquare = AllSquares[Moves[CurrentRan].TargetSquare];
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

            Position[AIselectedIndexBefore] = NoPiece;
            Position[AIselectedIndexSquare] = AISelectedPiece;
            AISelectedBefore.Source = NoPiece;
            AISelectedSquare.Source = AISelectedPiece;

            if (IsPlayerWhite == true)
            {
                PlayerToMoveWhite = true;
            }
            else
            {
                PlayerToMoveWhite = false;
            }

            //At the end after everything is set so that it's the player's turn, we need to run possiblemoves again for the other color:
            GenerateMoves();
        }
    }

}
