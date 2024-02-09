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
        //+ if AI is winning, - if not
        int EvalGameScore = 0;
        int _gameScore = 0;

        public int GameScore
        {
            get => _gameScore;
            set
            {
                if (_gameScore != value)
                {
                    _gameScore = value;
                    OnPropertyChanged(nameof(GameScore));
                    gameScoreLabel.Text = GameScore.ToString();
                }
            }
        }

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
            
            gameScoreLabel.Text = GameScore.ToString();
            this.BindingContext = this;

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
            EvalGameScore = 0;
            GameScore = 0;
            btnReset.Text = "Reset";
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
                int pairValue = Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare).Value;
                GameScore -= pairValue;
                EvalGameScore = GameScore;

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
                
                var selectedMove = Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare);
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
                }

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
        List<Move> CurrentColorMovesOverOpponents = new List<Move>();
        List<Move> Moves = new List<Move>();

        int PiecesOnBoard = 32;
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
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i-16, P2M_PawnExtraValues[i - 16] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (Position[i-8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 0);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (Position[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 1);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 2);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 3);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 4);
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i + 16, AI2M_PawnExtraValues[i + 16] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (Position[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 0);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(Position[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (Position[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 1);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 2);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 3);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 4);
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(Position[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 4);
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    if (IsPlayerWhite == true)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), P2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - P2M_RookExtraValues[i], 0);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), AI2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - AI2M_RookExtraValues[i], 0);
                                    }

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), BishopExtraValues[i + ((k + 1) * DirectionOffsets[j])] - BishopExtraValues[i], 0);

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), QueenExtraValues[i + ((k + 1) * DirectionOffsets[j])] - QueenExtraValues[i], 0);

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
                                    ValueTempMoves(i, i - 15, KnightExtraValues[i - 15] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, KnightExtraValues[i - 6] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, KnightExtraValues[i + 10] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, KnightExtraValues[i + 17] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, KnightExtraValues[i + 15] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, KnightExtraValues[i + 6] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(WhitePieces.Contains(Position[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, KnightExtraValues[i - 10] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(WhitePieces.Contains(Position[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, KnightExtraValues[i - 17] - KnightExtraValues[i], 0);
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
                                            PiecesOnBoard = Position.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == true)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], P2M_KingExtraValues[i + DirectionOffsets[j]] - P2M_KingExtraValues[i], 0);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], AI2M_KingExtraValues[i + DirectionOffsets[j]] - AI2M_KingExtraValues[i], 0);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0);
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
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if ((Position[i - 8] == NoPiece) && (Position[i - 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i - 16, P2M_PawnExtraValues[i - 16] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (Position[i - 8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 0);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 0);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (Position[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 1);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 2);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 3);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 4);
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //double pawn push:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if ((Position[i + 8] == NoPiece) && (Position[i + 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i + 16, AI2M_PawnExtraValues[i + 16] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (Position[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 0);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(Position[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 0);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (Position[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 1);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 2);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 3);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 4);
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 4);
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(Position[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 1);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 2);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 3);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 4);
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(Position[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    if (IsPlayerWhite == false)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), P2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - P2M_RookExtraValues[i], 0);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), AI2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - AI2M_RookExtraValues[i], 0);
                                    }

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), BishopExtraValues[i + ((k + 1) * DirectionOffsets[j])] - BishopExtraValues[i], 0);

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), QueenExtraValues[i + ((k + 1) * DirectionOffsets[j])] - QueenExtraValues[i], 0);

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
                                    ValueTempMoves(i, i - 15, KnightExtraValues[i - 15] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, KnightExtraValues[i - 6] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, KnightExtraValues[i + 10] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, KnightExtraValues[i + 17] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, KnightExtraValues[i + 15] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, KnightExtraValues[i + 6] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(BlackPieces.Contains(Position[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, KnightExtraValues[i - 10] - KnightExtraValues[i], 0);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(BlackPieces.Contains(Position[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, KnightExtraValues[i - 17] - KnightExtraValues[i], 0);
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
                                            PiecesOnBoard = Position.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == false)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], P2M_KingExtraValues[i + DirectionOffsets[j]] - P2M_KingExtraValues[i], 0);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], AI2M_KingExtraValues[i + DirectionOffsets[j]] - AI2M_KingExtraValues[i], 0);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0);
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
            for (int i = 0; i < TempMoves.Count; i++)
            {
                if (TempMoves[i].Extra != 0)
                {
                    if (TempMoves[i].Extra == 1)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + PieceValues[4] - PieceValues[0];
                        TempMoves[i] = move;
                    }
                    else if (TempMoves[i].Extra == 2)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + PieceValues[3] - PieceValues[0];
                        TempMoves[i] = move;
                    }
                    else if (TempMoves[i].Extra == 3)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + PieceValues[2] - PieceValues[0];
                        TempMoves[i] = move;
                    }
                    else if (TempMoves[i].Extra == 4)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + PieceValues[1] - PieceValues[0];
                        TempMoves[i] = move;
                    }
                }
            }
            //---FOR TESTING---
            //for (int i = 0; i < TempMoves.Count; i++)
            //{
            //    Trace.WriteLine($"{i+1}: |{TempMoves[i].StartingSquare}, {TempMoves[i].TargetSquare}|");
            //}
            return TempMoves;
        }
        private void ValueTempMoves(int _StartingSquare, int _TargetSquare, int _Value, int _Extra)
        {
            //P-N-B-R-Q
            if (Position[_TargetSquare]!=NoPiece)
            {
                if (Position[_TargetSquare] == BlackPieces[8] || Position[_TargetSquare] == WhitePieces[8])
                {
                    TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[0], _Extra));
                }

                else if (Position[_TargetSquare] == BlackPieces[1] || Position[_TargetSquare] == BlackPieces[6] || Position[_TargetSquare] == WhitePieces[1] || Position[_TargetSquare] == WhitePieces[6])
                {
                    TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[1], _Extra));
                }

                else if (Position[_TargetSquare] == BlackPieces[2] || Position[_TargetSquare] == BlackPieces[5] || Position[_TargetSquare] == WhitePieces[2] || Position[_TargetSquare] == WhitePieces[5])
                {
                    TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[2], _Extra));
                }

                else if (Position[_TargetSquare] == BlackPieces[0] || Position[_TargetSquare] == BlackPieces[7] || Position[_TargetSquare] == WhitePieces[0] || Position[_TargetSquare] == WhitePieces[7])
                {
                    TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[3], _Extra));
                }

                else
                {
                    TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[4], _Extra));
                }
            }
            else
            {
                TempMoves.Add(new Move(_StartingSquare, _TargetSquare, _Value, _Extra));
            }
        }

        int OpponentMovesCount = 0;
        private bool GenerateMoves()
        {
            Moves.Clear();
            CurrentColorMoves = new List<Move>(PossibleMoves());
            //checking for checks:
            for (int i = 0; i < CurrentColorMoves.Count; i++)
            {
                MakeCurrentMove(i);
                
                if (PlayerToMoveWhite == true)
                {
                    if (!OpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                    {
                        OpponentMovesCount = 0;
                        for (int j = 0; j < OpponentMoves.Count; j++)
                        {
                            MakeMoveNext(j);

                            if (!CurrentColorMovesOverOpponents.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                            {
                                OpponentMovesCount++;
                            }

                            UnmakeMoveNext(j);
                        }
                        if (OpponentMovesCount == 0)
                        {
                            //making this move will result in checkmate/stalemate
                            if (CurrentColorMovesOverOpponents.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                            {
                                //making this move will result in checkmate:
                                Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 20000, CurrentColorMoves[i].Extra));
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                if (IsPlayerWhite == true)
                                {
                                    if (EvalGameScore > 1000)
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 18000, CurrentColorMoves[i].Extra));
                                    }
                                    else
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, -18000, CurrentColorMoves[i].Extra));
                                    }
                                }
                                else
                                {
                                    if (EvalGameScore < -1000)
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 18000, CurrentColorMoves[i].Extra));
                                    }
                                    else
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, -18000, CurrentColorMoves[i].Extra));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //regular move
                            Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, CurrentColorMoves[i].Value, CurrentColorMoves[i].Extra));
                        } 
                    }
                }
                else
                {
                    if (!OpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                    {
                        OpponentMovesCount = 0;
                        for (int j = 0; j < OpponentMoves.Count; j++)
                        {
                            MakeMoveNext(j);

                            if (!CurrentColorMovesOverOpponents.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                            {
                                OpponentMovesCount++;
                            }

                            UnmakeMoveNext(j);
                        }
                        if (OpponentMovesCount == 0)
                        {
                            //making this move will result in checkmate/stalemate
                            if (CurrentColorMovesOverOpponents.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                            {
                                //making this move will result in checkmate:
                                Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 20000, CurrentColorMoves[i].Extra));
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                if (IsPlayerWhite == false)
                                {
                                    if (EvalGameScore > 1000)
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 18000, CurrentColorMoves[i].Extra));
                                    }
                                    else
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, -18000, CurrentColorMoves[i].Extra));
                                    }
                                }
                                else
                                {
                                    if (EvalGameScore < -1000)
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, 18000, CurrentColorMoves[i].Extra));
                                    }
                                    else
                                    {
                                        Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, -18000, CurrentColorMoves[i].Extra));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //regular move
                            Moves.Add(new Move(CurrentColorMoves[i].StartingSquare, CurrentColorMoves[i].TargetSquare, CurrentColorMoves[i].Value, CurrentColorMoves[i].Extra));
                        }
                    }
                }
                UnmakeCurrentMove(i);
            }
            //---FOR TESTING---
            for (int i = 0; i < Moves.Count; i++)
            {
                if (Moves[i].Value<-10000 || Moves[i].Value>10000)
                {
                    Trace.WriteLine($"{i + 1}: |{Moves[i].StartingSquare}, {Moves[i].TargetSquare}, {Moves[i].Value}|");
                }     
            }

            //end of match:
            if (Moves.Count == 0)
            {
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves());
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (PlayerToMoveWhite == true)
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == WhitePieces[4]))
                    {
                        //checkmate:
                        MateSound.Play();
                        btnReset.Text = "CHECKMATE!";
                        return false;
                    }
                    else
                    {
                        //stalemate:
                        MateSound.Play();
                        btnReset.Text = "STALEMATE!";
                        return false;
                    }
                }
                else
                {
                    if (OpponentMoves.Any(move => Position[move.TargetSquare] == BlackPieces[4]))
                    {
                        //checkmate:
                        MateSound.Play();
                        btnReset.Text = "CHECKMATE!";
                        return false;
                    }
                    else
                    {
                        //stalemate:
                        MateSound.Play();
                        btnReset.Text = "STALEMATE!";
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }
        ImageSource TempTargetSquareSource = null;
        ImageSource TempStartingSquareSource = null;
        private void MakeCurrentMove(int i)
        {
            if ((PlayerToMoveWhite == true && IsPlayerWhite == false) || (PlayerToMoveWhite == false && IsPlayerWhite == true))
            {
                //AI moves, evalscore gets higher
                EvalGameScore += CurrentColorMoves[i].Value;
            }
            else
            {
                //Player moves, evalscore gets lower
                EvalGameScore -= CurrentColorMoves[i].Value;
            }
            TempTargetSquareSource = Position[CurrentColorMoves[i].TargetSquare];
            TempStartingSquareSource = Position[CurrentColorMoves[i].StartingSquare];
            if (CurrentColorMoves[i].Extra > 0)
            {
                if (PlayerToMoveWhite == true)
                {
                    //promotion:
                    if (CurrentColorMoves[i].Extra == 1)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = WhitePieces[3];
                    }
                    else if (CurrentColorMoves[i].Extra == 2)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = WhitePieces[0];
                    }
                    else if (CurrentColorMoves[i].Extra == 3)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = WhitePieces[2];
                    }
                    else if (CurrentColorMoves[i].Extra == 4)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = WhitePieces[1];
                    }
                }
                else
                {
                    //promotion:
                    if (CurrentColorMoves[i].Extra == 1)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = BlackPieces[3];
                    }
                    else if (CurrentColorMoves[i].Extra == 2)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = BlackPieces[0];
                    }
                    else if (CurrentColorMoves[i].Extra == 3)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = BlackPieces[2];
                    }
                    else if (CurrentColorMoves[i].Extra == 4)
                    {
                        Position[CurrentColorMoves[i].TargetSquare] = BlackPieces[1];
                    }
                }
            }
            else
            {
                Position[CurrentColorMoves[i].TargetSquare] = Position[CurrentColorMoves[i].StartingSquare];   
            }
            Position[CurrentColorMoves[i].StartingSquare] = NoPiece;
            PlayerToMoveWhite = !PlayerToMoveWhite;
            OpponentMoves = new List<Move>(PossibleMoves());
            PlayerToMoveWhite = !PlayerToMoveWhite;
        }

        private void UnmakeCurrentMove(int i)
        {
            if ((PlayerToMoveWhite == true && IsPlayerWhite == false) || (PlayerToMoveWhite == false && IsPlayerWhite == true))
            {
                EvalGameScore -= CurrentColorMoves[i].Value;
            }
            else
            {
                EvalGameScore += CurrentColorMoves[i].Value;
            }
            Position[CurrentColorMoves[i].StartingSquare] = TempStartingSquareSource;
            Position[CurrentColorMoves[i].TargetSquare] = TempTargetSquareSource;
        }
        ImageSource TempTargetSquareSourceOpponent = null;
        ImageSource TempStartingSquareSourceOpponent = null;
        private void MakeMoveNext(int j)
        {
            TempTargetSquareSourceOpponent = Position[OpponentMoves[j].TargetSquare];
            TempStartingSquareSourceOpponent = Position[OpponentMoves[j].StartingSquare];
            if (OpponentMoves[j].Extra > 0)
            {
                //swapped if
                if (PlayerToMoveWhite == false)
                {
                    //promotion:
                    if (OpponentMoves[j].Extra == 1)
                    {
                        Position[OpponentMoves[j].TargetSquare] = WhitePieces[3];
                    }
                    else if (OpponentMoves[j].Extra == 2)
                    {
                        Position[OpponentMoves[j].TargetSquare] = WhitePieces[0];
                    }
                    else if (OpponentMoves[j].Extra == 3)
                    {
                        Position[OpponentMoves[j].TargetSquare] = WhitePieces[2];
                    }
                    else if (OpponentMoves[j].Extra == 4)
                    {
                        Position[OpponentMoves[j].TargetSquare] = WhitePieces[1];
                    }
                }
                else
                {
                    //promotion:
                    if (OpponentMoves[j].Extra == 1)
                    {
                        Position[OpponentMoves[j].TargetSquare] = BlackPieces[3];
                    }
                    else if (OpponentMoves[j].Extra == 2)
                    {
                        Position[OpponentMoves[j].TargetSquare] = BlackPieces[0];
                    }
                    else if (OpponentMoves[j].Extra == 3)
                    {
                        Position[OpponentMoves[j].TargetSquare] = BlackPieces[2];
                    }
                    else if (OpponentMoves[j].Extra == 4)
                    {
                        Position[OpponentMoves[j].TargetSquare] = BlackPieces[1];
                    }
                }
            }
            else
            {
                Position[OpponentMoves[j].TargetSquare] = Position[OpponentMoves[j].StartingSquare];
            }
            Position[OpponentMoves[j].StartingSquare] = NoPiece;
            CurrentColorMovesOverOpponents = new List<Move>(PossibleMoves());
        }

        private void UnmakeMoveNext(int j)
        {
            Position[OpponentMoves[j].StartingSquare] = TempStartingSquareSourceOpponent;
            Position[OpponentMoves[j].TargetSquare] = TempTargetSquareSourceOpponent;
        }

        ImageButton AISelectedBefore = null;
        ImageButton AISelectedSquare = null;
        ImageSource AISelectedPiece = null;
        int AIselectedIndexBefore = -1;
        int AIselectedIndexSquare = -1;
        int SelectedMoveIndexInList = 0;
        private void AIToMove()
        {
            if (GenerateMoves() == false)
            {
                // The condition was met, so exit the function
                return;
            }
            SelectedMoveIndexInList = Moves
            .Select((move, index) => new { Index = index, move.Value })
            .Aggregate((max, current) => (current.Value > max.Value) ? current : max)
            .Index;

            AISelectedBefore = AllSquares[Moves[SelectedMoveIndexInList].StartingSquare];
            AISelectedPiece = AllSquares[Moves[SelectedMoveIndexInList].StartingSquare].Source;
            AISelectedSquare = AllSquares[Moves[SelectedMoveIndexInList].TargetSquare];

            GameScore += Moves[SelectedMoveIndexInList].Value;
            EvalGameScore = GameScore;

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
            var selectedMove = Moves.First(move => move.StartingSquare == AIselectedIndexBefore && move.TargetSquare == AIselectedIndexSquare);
            if (selectedMove.Extra > 0)
            {
                if (PlayerToMoveWhite == true)
                {
                    //promotion:
                    Position[AIselectedIndexBefore] = NoPiece;
                    AISelectedBefore.Source = NoPiece;
                    if (selectedMove.Extra == 1)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[3];
                        AISelectedSquare.Source = WhitePieces[3];
                    }
                    else if (selectedMove.Extra == 2)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[0];
                        AISelectedSquare.Source = WhitePieces[0];
                    }
                    else if (selectedMove.Extra == 3)
                    {
                        Position[AIselectedIndexSquare] = WhitePieces[2];
                        AISelectedSquare.Source = WhitePieces[2];
                    }
                    else if (selectedMove.Extra == 4)
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
                    if (selectedMove.Extra == 1)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[3];
                        AISelectedSquare.Source = BlackPieces[3];
                    }
                    else if (selectedMove.Extra == 2)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[0];
                        AISelectedSquare.Source =BlackPieces[0];
                    }
                    else if (selectedMove.Extra == 3)
                    {
                        Position[AIselectedIndexSquare] = BlackPieces[2];
                        AISelectedSquare.Source = BlackPieces[2];
                    }
                    else if (selectedMove.Extra == 4)
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
