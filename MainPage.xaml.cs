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
        int LastMoveStarting; int LastMoveTarget;
        private void ResetBoard()
        {
            for (int i = 0; i < 6; i++)
            {
                castling[i] = true;
            }

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
                if (currentButton.Source == WhitePieces[3] || currentButton.Source == BlackPieces[3])
                {
                    GameScore -= Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare && move.Extra == 1).Value;
                }
                else if (currentButton.Source == WhitePieces[0] || currentButton.Source == BlackPieces[0])
                {
                    GameScore -= Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare && move.Extra == 2).Value;
                }
                else if (currentButton.Source == WhitePieces[2] || currentButton.Source == BlackPieces[2])
                {
                    GameScore -= Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare && move.Extra == 3).Value;
                }
                else
                {
                    GameScore -= Moves.First(move => move.StartingSquare == selectedIndexBefore && move.TargetSquare == selectedIndexSquare && move.Extra == 4).Value;
                }
                EvalGameScore = GameScore;
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
                if (selectedMove.Extra < 1 || selectedMove.Extra > 4)
                {
                    GameScore -= selectedMove.Value;
                    EvalGameScore = GameScore;
                }
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
                    else if (TempMoves[i].Extra == 5 || TempMoves[i].Extra == 6)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + PieceValues[0];
                        TempMoves[i] = move;
                    }
                    else if (TempMoves[i].Extra == 7 || TempMoves[i].Extra == 8 || TempMoves[i].Extra == 9 || TempMoves[i].Extra == 10)
                    {
                        Move move = TempMoves[i];
                        move.Value = move.Value + 20;
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
                                        ValueTempMoves(i, i - 16, P2M_PawnExtraValues[i - 16] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 1, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 2, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 3, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 4, inposition);
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 4, inposition);
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
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 6, inposition);
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
                                        ValueTempMoves(i, i + 16, AI2M_PawnExtraValues[i + 16] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (BlackPieces.Contains(inposition[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 1, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 2, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 3, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 4, inposition);
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (BlackPieces.Contains(inposition[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 4, inposition);
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
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == BlackPieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 6, inposition);
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

                                    if (IsPlayerWhite == true)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), P2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - P2M_RookExtraValues[i], 0, inposition);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), AI2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - AI2M_RookExtraValues[i], 0, inposition);
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (WhitePieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), BishopExtraValues[i + ((k + 1) * DirectionOffsets[j])] - BishopExtraValues[i], 0, inposition);

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), QueenExtraValues[i + ((k + 1) * DirectionOffsets[j])] - QueenExtraValues[i], 0, inposition);

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
                                    ValueTempMoves(i, i - 15, KnightExtraValues[i - 15] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, KnightExtraValues[i - 6] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, KnightExtraValues[i + 10] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, KnightExtraValues[i + 17] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, KnightExtraValues[i + 15] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, KnightExtraValues[i + 6] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, KnightExtraValues[i - 10] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(WhitePieces.Contains(inposition[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, KnightExtraValues[i - 17] - KnightExtraValues[i], 0, inposition);
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
                                            PiecesOnBoard = inposition.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == true)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], P2M_KingExtraValues[i + DirectionOffsets[j]] - P2M_KingExtraValues[i], 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0, inposition);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], AI2M_KingExtraValues[i + DirectionOffsets[j]] - AI2M_KingExtraValues[i], 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0, inposition);
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
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if ((inposition[i - 8] == NoPiece) && (inposition[i - 16] == NoPiece))
                                    {
                                        ValueTempMoves(i, i - 16, P2M_PawnExtraValues[i - 16] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (inposition[i - 8] == NoPiece && SquaresToEdge[i][0] > 1)
                                {
                                    ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 9]))
                                    {
                                        ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][0] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i - 7]))
                                    {
                                        ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][0] == 1)
                                {
                                    if (inposition[i - 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 1, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 2, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 3, inposition);
                                        ValueTempMoves(i, i - 8, P2M_PawnExtraValues[i - 8] - P2M_PawnExtraValues[i], 4, inposition);
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 7]))
                                        {
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i - 9]))
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 4, inposition);
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
                                            ValueTempMoves(i, i - 7, P2M_PawnExtraValues[i - 7] - P2M_PawnExtraValues[i], 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i - 17 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i - 9, P2M_PawnExtraValues[i - 9] - P2M_PawnExtraValues[i], 6, inposition);
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
                                        ValueTempMoves(i, i + 16, AI2M_PawnExtraValues[i + 16] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (inposition[i + 8] == NoPiece && SquaresToEdge[i][2] > 1)
                                {
                                    ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 0, inposition);
                                }
                                //pawn takes:
                                if (SquaresToEdge[i][3] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 7]))
                                    {
                                        ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                if (SquaresToEdge[i][1] > 0 && SquaresToEdge[i][2] > 1)
                                {
                                    if (WhitePieces.Contains(inposition[i + 9]))
                                    {
                                        ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 0, inposition);
                                    }
                                }
                                //promotion:
                                if (SquaresToEdge[i][2] == 1)
                                {
                                    if (inposition[i + 8] == NoPiece)
                                    {
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 1, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 2, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 3, inposition);
                                        ValueTempMoves(i, i + 8, AI2M_PawnExtraValues[i + 8] - AI2M_PawnExtraValues[i], 4, inposition);
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 7]))
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 4, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][1] > 0)
                                    {
                                        if (WhitePieces.Contains(inposition[i + 9]))
                                        {
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 1, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 2, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 3, inposition);
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 4, inposition);
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
                                            ValueTempMoves(i, i + 9, AI2M_PawnExtraValues[i + 9] - AI2M_PawnExtraValues[i], 5, inposition);
                                        }
                                    }
                                    if (SquaresToEdge[i][3] > 0)
                                    {
                                        if (inposition[i - 1] == WhitePieces[8] && LastMoveStarting == i + 15 && LastMoveTarget == i - 1)
                                        {
                                            ValueTempMoves(i, i + 7, AI2M_PawnExtraValues[i + 7] - AI2M_PawnExtraValues[i], 6, inposition);
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

                                    if (IsPlayerWhite == false)
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), P2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - P2M_RookExtraValues[i], 0, inposition);
                                    }
                                    else
                                    {
                                        ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), AI2M_RookExtraValues[i + ((k + 1) * DirectionOffsets[j])] - AI2M_RookExtraValues[i], 0, inposition);
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
                                for (int k = 0; k < SquaresToEdge[i][j]; k++)
                                {
                                    if (BlackPieces.Contains(inposition[i + ((k + 1) * DirectionOffsets[j])]))
                                    {
                                        break;
                                    }

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), BishopExtraValues[i + ((k + 1) * DirectionOffsets[j])] - BishopExtraValues[i], 0, inposition);

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

                                    ValueTempMoves(i, i + ((k + 1) * DirectionOffsets[j]), QueenExtraValues[i + ((k + 1) * DirectionOffsets[j])] - QueenExtraValues[i], 0, inposition);

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
                                    ValueTempMoves(i, i - 15, KnightExtraValues[i - 15] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][0] >= 1 && SquaresToEdge[i][1] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 6])))
                                {
                                    ValueTempMoves(i, i - 6, KnightExtraValues[i - 6] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 10])))
                                {
                                    ValueTempMoves(i, i + 10, KnightExtraValues[i + 10] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][1] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 17])))
                                {
                                    ValueTempMoves(i, i + 17, KnightExtraValues[i + 17] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][2] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 15])))
                                {
                                    ValueTempMoves(i, i + 15, KnightExtraValues[i + 15] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][2] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i + 6])))
                                {
                                    ValueTempMoves(i, i + 6, KnightExtraValues[i + 6] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 2 && SquaresToEdge[i][0] >= 1)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 10])))
                                {
                                    ValueTempMoves(i, i - 10, KnightExtraValues[i - 10] - KnightExtraValues[i], 0, inposition);
                                }
                            }
                            if (SquaresToEdge[i][3] >= 1 && SquaresToEdge[i][0] >= 2)
                            {
                                if (!(BlackPieces.Contains(inposition[i - 17])))
                                {
                                    ValueTempMoves(i, i - 17, KnightExtraValues[i - 17] - KnightExtraValues[i], 0, inposition);
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
                                            PiecesOnBoard = inposition.Count(source => source != NoPiece);
                                            if (IsPlayerWhite == false)
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], P2M_KingExtraValues[i + DirectionOffsets[j]] - P2M_KingExtraValues[i], 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0, inposition);
                                                }
                                            }
                                            else
                                            {
                                                if (PiecesOnBoard > 12)
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], AI2M_KingExtraValues[i + DirectionOffsets[j]] - AI2M_KingExtraValues[i], 0, inposition);
                                                }
                                                else
                                                {
                                                    ValueTempMoves(i, i + DirectionOffsets[j], Endgame_KingExtraValues[i + DirectionOffsets[j]] - Endgame_KingExtraValues[i], 0, inposition);
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
                                        Temp2.Add(new Move(60, 58, P2M_KingExtraValues[58] - P2M_KingExtraValues[60], 7));
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
                                        Temp2.Add(new Move(60, 62, P2M_KingExtraValues[62] - P2M_KingExtraValues[60], 8));
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
                                        Temp2.Add(new Move(3, 5, AI2M_KingExtraValues[5] - AI2M_KingExtraValues[3], 10));
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
                                        Temp2.Add(new Move(3, 1, AI2M_KingExtraValues[1] - AI2M_KingExtraValues[3], 9));
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
                                        Temp2.Add(new Move(59, 61, P2M_KingExtraValues[61] - P2M_KingExtraValues[59], 8));
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
                                        Temp2.Add(new Move(59, 57, P2M_KingExtraValues[57] - P2M_KingExtraValues[59], 7));
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
                                        Temp2.Add(new Move(4, 2, AI2M_KingExtraValues[2] - AI2M_KingExtraValues[4], 9));
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
                                        Temp2.Add(new Move(4, 6, AI2M_KingExtraValues[6] - AI2M_KingExtraValues[4], 10));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Temp2;
        }
        private void ValueTempMoves(int _StartingSquare, int _TargetSquare, int _Value, int _Extra,ImageSource[] inposition)
        {
            //P-N-B-R-Q
            if (inposition[_TargetSquare]!=NoPiece)
            {
                if (inposition[_TargetSquare] == BlackPieces[8] || inposition[_TargetSquare] == WhitePieces[8])
                {
                    Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[0], _Extra));
                }

                else if (inposition[_TargetSquare] == BlackPieces[1] || inposition[_TargetSquare] == BlackPieces[6] || inposition[_TargetSquare] == WhitePieces[1] || inposition[_TargetSquare] == WhitePieces[6])
                {
                    Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[1], _Extra));
                }

                else if (inposition[_TargetSquare] == BlackPieces[2] || inposition[_TargetSquare] == BlackPieces[5] || inposition[_TargetSquare] == WhitePieces[2] || inposition[_TargetSquare] == WhitePieces[5])
                {
                    Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[2], _Extra));
                }

                else if (inposition[_TargetSquare] == BlackPieces[0] || inposition[_TargetSquare] == BlackPieces[7] || inposition[_TargetSquare] == WhitePieces[0] || inposition[_TargetSquare] == WhitePieces[7])
                {
                    Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[3], _Extra));
                }

                else
                {
                    Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value + PieceValues[4], _Extra));
                }
            }
            else
            {
                Temp1.Add(new Move(_StartingSquare, _TargetSquare, _Value, _Extra));
            }
        }

        int OpponentMovesCount = 0;
        private bool GenerateMoves(List<Move> ListToAddMoves, ImageSource[] inposition)
        {
            ListToAddMoves.Clear();
            CurrentColorMoves = new List<Move>(PossibleMoves(inposition));
            //checking for checks:
            TempLastMoveStarting = LastMoveStarting;
            TempLastMoveTarget = LastMoveTarget;
            foreach (Move move in CurrentColorMoves)
            {
                MakeCurrentMove(move, inposition);
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves(inposition));
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (PlayerToMoveWhite == true)
                {
                    if (!OpponentMoves.Any(move => inposition[move.TargetSquare] == WhitePieces[4]))
                    {
                        TempLastLastMoveStarting = LastMoveStarting;
                        TempLastLastMoveTarget = LastMoveTarget;
                        OpponentMovesCount = 0;
                        foreach (Move nextmove in OpponentMoves)
                        {
                            MakeMoveNext(nextmove, inposition);

                            if (!CurrentColorMovesOverOpponents.Any(move => inposition[move.TargetSquare] == BlackPieces[4]))
                            {
                                OpponentMovesCount++;
                            }

                            UnmakeMoveNext(nextmove, inposition);
                        }
                        if (OpponentMovesCount == 0)
                        {
                            //making this move will result in checkmate/stalemate
                            if (CurrentColorMovesOverOpponents.Any(move => inposition[move.TargetSquare] == BlackPieces[4]))
                            {
                                //making this move will result in checkmate:
                                ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 20000, move.Extra));
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                if (IsPlayerWhite == true)
                                {
                                    if (EvalGameScore > 500)
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 18000, move.Extra));
                                    }
                                    else
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, -18000, move.Extra));
                                    }
                                }
                                else
                                {
                                    if (EvalGameScore < -500)
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 18000, move.Extra));
                                    }
                                    else
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, -18000, move.Extra));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //regular move
                            ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, move.Value, move.Extra));
                        }
                    }
                }
                else
                {
                    if (!OpponentMoves.Any(move => inposition[move.TargetSquare] == BlackPieces[4]))
                    {
                        TempLastLastMoveStarting = LastMoveStarting;
                        TempLastLastMoveTarget = LastMoveTarget;
                        OpponentMovesCount = 0;
                        foreach (Move nextmove in OpponentMoves)
                        {
                            MakeMoveNext(nextmove, inposition);

                            if (!CurrentColorMovesOverOpponents.Any(move => inposition[move.TargetSquare] == WhitePieces[4]))
                            {
                                OpponentMovesCount++;
                            }

                            UnmakeMoveNext(nextmove, inposition);
                        }
                        if (OpponentMovesCount == 0)
                        {
                            //making this move will result in checkmate/stalemate
                            if (CurrentColorMovesOverOpponents.Any(move => inposition[move.TargetSquare] == WhitePieces[4]))
                            {
                                //making this move will result in checkmate:
                                ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 20000, move.Extra));
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                if (IsPlayerWhite == false)
                                {
                                    if (EvalGameScore > 500)
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 18000, move.Extra));
                                    }
                                    else
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, -18000, move.Extra));
                                    }
                                }
                                else
                                {
                                    if (EvalGameScore < -500)
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, 18000, move.Extra));
                                    }
                                    else
                                    {
                                        ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, -18000, move.Extra));
                                    }
                                }
                            }
                        }
                        else
                        {
                            //regular move
                            ListToAddMoves.Add(new Move(move.StartingSquare, move.TargetSquare, move.Value, move.Extra));
                        }
                    }
                }
                UnmakeCurrentMove(move, inposition);
            }
            //---FOR TESTING---
            for (int i = 0; i < ListToAddMoves.Count; i++)
            {
                if (ListToAddMoves[i].Value<-10000 || ListToAddMoves[i].Value>10000)
                {
                    Trace.WriteLine($"{i + 1}: |{ListToAddMoves[i].StartingSquare}, {ListToAddMoves[i].TargetSquare}, {ListToAddMoves[i].Value}|");
                }     
            }

            //end of match:
            if (ListToAddMoves.Count == 0)
            {
                PlayerToMoveWhite = !PlayerToMoveWhite;
                OpponentMoves = new List<Move>(PossibleMoves(inposition));
                PlayerToMoveWhite = !PlayerToMoveWhite;
                if (PlayerToMoveWhite == true)
                {
                    if (OpponentMoves.Any(move => inposition[move.TargetSquare] == WhitePieces[4]))
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
                    if (OpponentMoves.Any(move => inposition[move.TargetSquare] == BlackPieces[4]))
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
        int TempLastMoveStarting; int TempLastMoveTarget; int TempLastLastMoveStarting; int TempLastLastMoveTarget;
        private void MakeCurrentMove(Move move, ImageSource[] onPosition)
        {
            if ((PlayerToMoveWhite == true && IsPlayerWhite == false) || (PlayerToMoveWhite == false && IsPlayerWhite == true))
            {
                //AI moves, evalscore gets higher
                EvalGameScore += move.Value;
            }
            else
            {
                //Player moves, evalscore gets lower
                EvalGameScore -= move.Value;
            }
            TempTargetSquareSource = onPosition[move.TargetSquare];
            TempStartingSquareSource = onPosition[move.StartingSquare];
            if (move.Extra > 0 && move.Extra < 5)
            {
                if (PlayerToMoveWhite == true)
                {
                    //promotion:
                    if (move.Extra == 1)
                    {
                        onPosition[move.TargetSquare] = WhitePieces[3];
                    }
                    else if (move.Extra == 2)
                    {
                        onPosition[move.TargetSquare] = WhitePieces[0];
                    }
                    else if (move.Extra == 3)
                    {
                        onPosition[move.TargetSquare] = WhitePieces[2];
                    }
                    else if (move.Extra == 4)
                    {
                        onPosition[move.TargetSquare] = WhitePieces[1];
                    }
                }
                else
                {
                    //promotion:
                    if (move.Extra == 1)
                    {
                        onPosition[move.TargetSquare] = BlackPieces[3];
                    }
                    else if (move.Extra == 2)
                    {
                        onPosition[move.TargetSquare] = BlackPieces[0];
                    }
                    else if (move.Extra == 3)
                    {
                        onPosition[move.TargetSquare] = BlackPieces[2];
                    }
                    else if (move.Extra == 4)
                    {
                        onPosition[move.TargetSquare] = BlackPieces[1];
                    }
                }
            }
            else
            {
                onPosition[move.TargetSquare] = onPosition[move.StartingSquare];   
            }
            //en passant:
            if (move.Extra == 5)
            {
                onPosition[move.StartingSquare + 1] = NoPiece;
            }
            else if (move.Extra == 6)
            {
                onPosition[move.StartingSquare - 1] = NoPiece;
            }
            //castling:
            else if (move.Extra == 7)
            {
                onPosition[56] = NoPiece;
                if (PlayerToMoveWhite)
                {
                    onPosition[59] = WhitePieces[0];
                }
                else
                {
                    onPosition[58] = BlackPieces[0];
                }
            }
            else if (move.Extra == 8)
            {
                onPosition[63] = NoPiece;
                if (PlayerToMoveWhite)
                {
                    onPosition[61] = WhitePieces[0];
                }
                else
                {
                    onPosition[60] = BlackPieces[0];
                }
            }
            else if (move.Extra == 9)
            {
                onPosition[0] = NoPiece;
                if (PlayerToMoveWhite)
                {
                    onPosition[2] = WhitePieces[0];
                }
                else
                {
                    onPosition[3] = BlackPieces[0];
                }
            }
            else if (move.Extra == 10)
            {
                onPosition[7] = NoPiece;
                if (PlayerToMoveWhite)
                {
                    onPosition[4] = WhitePieces[0];
                }
                else
                {
                    onPosition[5] = BlackPieces[0];
                }
            }
            onPosition[move.StartingSquare] = NoPiece;
            LastMoveStarting = move.StartingSquare;
            LastMoveTarget = move.TargetSquare;
        }

        private void UnmakeCurrentMove(Move move, ImageSource[] onPosition)
        {
            if ((PlayerToMoveWhite == true && IsPlayerWhite == false) || (PlayerToMoveWhite == false && IsPlayerWhite == true))
            {
                EvalGameScore -= move.Value;
            }
            else
            {
                EvalGameScore += move.Value;
            }
            onPosition[move.StartingSquare] = TempStartingSquareSource;
            onPosition[move.TargetSquare] = TempTargetSquareSource;
            if (PlayerToMoveWhite == true)
            {
                //en passant:
                if (move.Extra == 5)
                {
                    onPosition[move.StartingSquare + 1] = BlackPieces[8];
                }
                else if (move.Extra == 6)
                {
                    onPosition[move.StartingSquare - 1] = BlackPieces[8];
                }
                //castling
                else if (move.Extra == 7)
                {
                    onPosition[56] = WhitePieces[0];
                    onPosition[59] = NoPiece;
                }
                else if (move.Extra == 8)
                {
                    onPosition[63] = WhitePieces[0];
                    onPosition[61] = NoPiece;
                }
                else if (move.Extra == 9)
                {
                    onPosition[0] = WhitePieces[0];
                    onPosition[2] = NoPiece;
                }
                else if (move.Extra == 10)
                {
                    onPosition[7] = WhitePieces[0];
                    onPosition[4] = NoPiece;
                }
            }
            else
            {
                //en passant:
                if (move.Extra == 5)
                {
                    onPosition[move.StartingSquare + 1] = WhitePieces[8];
                }
                else if (move.Extra == 6)
                {
                    onPosition[move.StartingSquare - 1] = WhitePieces[8];
                }
                //castling
                else if (move.Extra == 7)
                {
                    onPosition[56] = BlackPieces[0];
                    onPosition[58] = NoPiece;
                }
                else if (move.Extra == 8)
                {
                    onPosition[63] = BlackPieces[0];
                    onPosition[60] = NoPiece;
                }
                else if (move.Extra == 9)
                {
                    onPosition[0] = BlackPieces[0];
                    onPosition[3] = NoPiece;
                }
                else if (move.Extra == 10)
                {
                    onPosition[7] = BlackPieces[0];
                    onPosition[5] = NoPiece;
                }
            }
            LastMoveStarting = TempLastMoveStarting;
            LastMoveTarget = TempLastMoveTarget;
        }
        ImageSource TempTargetSquareSourceOpponent = null;
        ImageSource TempStartingSquareSourceOpponent = null;
        private void MakeMoveNext(Move move, ImageSource[] inposition)
        {
            TempTargetSquareSourceOpponent = inposition[move.TargetSquare];
            TempStartingSquareSourceOpponent = inposition[move.StartingSquare];
            if (move.Extra > 0 && move.Extra < 5)
            {
                //swapped if
                if (PlayerToMoveWhite == false)
                {
                    //promotion:
                    if (move.Extra == 1)
                    {
                        inposition[move.TargetSquare] = WhitePieces[3];
                    }
                    else if (move.Extra == 2)
                    {
                        inposition[move.TargetSquare] = WhitePieces[0];
                    }
                    else if (move.Extra == 3)
                    {
                        inposition[move.TargetSquare] = WhitePieces[2];
                    }
                    else if (move.Extra == 4)
                    {
                        inposition[move.TargetSquare] = WhitePieces[1];
                    }
                }
                else
                {
                    //promotion:
                    if (move.Extra == 1)
                    {
                        inposition[move.TargetSquare] = BlackPieces[3];
                    }
                    else if (move.Extra == 2)
                    {
                        inposition[move.TargetSquare] = BlackPieces[0];
                    }
                    else if (move.Extra == 3)
                    {
                        inposition[move.TargetSquare] = BlackPieces[2];
                    }
                    else if (move.Extra == 4)
                    {
                        inposition[move.TargetSquare] = BlackPieces[1];
                    }
                }
            }
            else
            {
                inposition[move.TargetSquare] = inposition[move.StartingSquare];
            }
            //en passant:
            if (move.Extra == 5)
            {
                inposition[move.StartingSquare + 1] = NoPiece;
            }
            else if (move.Extra == 6)
            {
                inposition[move.StartingSquare - 1] = NoPiece;
            }
            //castling: (swapped if)
            else if (move.Extra == 7)
            {
                inposition[56] = NoPiece;
                if (PlayerToMoveWhite == false)
                {
                    inposition[59] = WhitePieces[0];
                }
                else
                {
                    inposition[58] = BlackPieces[0];
                }
            }
            else if (move.Extra == 8)
            {
                inposition[63] = NoPiece;
                if (PlayerToMoveWhite == false)
                {
                    inposition[61] = WhitePieces[0];
                }
                else
                {
                    inposition[60] = BlackPieces[0];
                }
            }
            else if (move.Extra == 9)
            {
                inposition[0] = NoPiece;
                if (PlayerToMoveWhite == false)
                {
                    inposition[2] = WhitePieces[0];
                }
                else
                {
                    inposition[3] = BlackPieces[0];
                }
            }
            else if (move.Extra == 10)
            {
                inposition[7] = NoPiece;
                if (PlayerToMoveWhite == false)
                {
                    inposition[4] = WhitePieces[0];
                }
                else
                {
                    inposition[5] = BlackPieces[0];
                }
            }
            inposition[move.StartingSquare] = NoPiece;
            LastMoveStarting = move.StartingSquare;
            LastMoveTarget = move.TargetSquare;
            CurrentColorMovesOverOpponents = new List<Move>(PossibleMoves(inposition));
        }

        private void UnmakeMoveNext(Move move, ImageSource[] inposition)
        {
            inposition[move.StartingSquare] = TempStartingSquareSourceOpponent;
            inposition[move.TargetSquare] = TempTargetSquareSourceOpponent;
            //swapped if:
            if (PlayerToMoveWhite == false)
            {
                //en passant:
                if (move.Extra == 5)
                {
                    inposition[move.StartingSquare + 1] = BlackPieces[8];
                }
                else if (move.Extra == 6)
                {
                    inposition[move.StartingSquare - 1] = BlackPieces[8];
                }
                //castling
                else if (move.Extra == 7)
                {
                    inposition[56] = WhitePieces[0];
                    inposition[59] = NoPiece;
                }
                else if (move.Extra == 8)
                {
                    inposition[63] = WhitePieces[0];
                    inposition[61] = NoPiece;
                }
                else if (move.Extra == 9)
                {
                    inposition[0] = WhitePieces[0];
                    inposition[2] = NoPiece;
                }
                else if (move.Extra == 10)
                {
                    inposition[7] = WhitePieces[0];
                    inposition[4] = NoPiece;
                }
            }
            else
            {
                //en passant:
                if (move.Extra == 5)
                {
                    inposition[move.StartingSquare + 1] = WhitePieces[8];
                }
                else if (move.Extra == 6)
                {
                    inposition[move.StartingSquare - 1] = WhitePieces[8];
                }
                //castling
                else if (move.Extra == 7)
                {
                    inposition[56] = BlackPieces[0];
                    inposition[58] = NoPiece;
                }
                else if (move.Extra == 8)
                {
                    inposition[63] = BlackPieces[0];
                    inposition[60] = NoPiece;
                }
                else if (move.Extra == 9)
                {
                    inposition[0] = BlackPieces[0];
                    inposition[3] = NoPiece;
                }
                else if (move.Extra == 10)
                {
                    inposition[7] = BlackPieces[0];
                    inposition[5] = NoPiece;
                }
            }
            LastMoveStarting = TempLastLastMoveStarting;
            LastMoveTarget = TempLastLastMoveTarget;
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
                return;
            }
            GeneratedMove = GetBestMove(3);
 
            AISelectedBefore = AllSquares[GeneratedMove.StartingSquare];
            AISelectedPiece = AllSquares[GeneratedMove.StartingSquare].Source;
            AISelectedSquare = AllSquares[GeneratedMove.TargetSquare];

            GameScore += GeneratedMove.Value;
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
            GenerateMoves(Moves, Position);
        }
        //---------------------
        //minimax eval:
        List<Move> possibleMoves = new List<Move>();
        List<Move> possibleOpponentMoves = new List<Move>();

        private int Minimax(ImageSource[] position, int depth, bool maximizingPlayer, int alpha, int beta)
        {
            if (maximizingPlayer)
            {
                if (IsPlayerWhite)
                {
                    PlayerToMoveWhite = true;
                }
                else
                {
                    PlayerToMoveWhite = false;
                }
            }
            else
            {
                if (IsPlayerWhite)
                {
                    PlayerToMoveWhite = false;
                }
                else
                {
                    PlayerToMoveWhite = true;
                }
            }
            GenerateMoves(possibleMoves, GenPosition);
            //OR CHECKMATE
            if (depth == 0 || possibleMoves.Count == 0)
            {
                int BoardValue = 0;
                //calculate board value
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
                                BoardValue -= 100 + P2M_PawnExtraValues[i];
                            }
                            //Black pawn
                            else if (GenPosition[i] == BlackPieces[8])
                            {
                                BoardValue += 100 + AI2M_PawnExtraValues[i];
                            }
                            //White rook
                            else if(GenPosition[i] == WhitePieces[0] || GenPosition[i] == WhitePieces[7])
                            {
                                BoardValue -= 500 + P2M_RookExtraValues[i];
                            }
                            //Black rook
                            else if (GenPosition[i] == BlackPieces[0] || GenPosition[i] == BlackPieces[7])
                            {
                                BoardValue += 500 + AI2M_RookExtraValues[i];
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
                                    BoardValue -= P2M_KingExtraValues[i];
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
                                    BoardValue += AI2M_KingExtraValues[i];
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
                                if (BoardValue < -500)
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
                                BoardValue = -20000;
                            }
                            else
                            {
                                //making this move will result in stalemate:
                                if (BoardValue < -500)
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
                                BoardValue += 100 + AI2M_PawnExtraValues[i];
                            }
                            //Black pawn
                            else if (GenPosition[i] == BlackPieces[8])
                            {
                                BoardValue -= 100 + P2M_PawnExtraValues[i];
                            }
                            //White rook
                            else if (GenPosition[i] == WhitePieces[0] || GenPosition[i] == WhitePieces[7])
                            {
                                BoardValue += 500 + AI2M_RookExtraValues[i];
                            }
                            //Black rook
                            else if (GenPosition[i] == BlackPieces[0] || GenPosition[i] == BlackPieces[7])
                            {
                                BoardValue -= 500 + P2M_RookExtraValues[i];
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
                                    BoardValue += AI2M_KingExtraValues[i];
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
                                    BoardValue -= P2M_KingExtraValues[i];
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
                                if (BoardValue < -500)
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
                                if (BoardValue < -500)
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
                    MakeCurrentMove(move, position);
                    int eval = Minimax(position, depth - 1, false, alpha, beta);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    UnmakeCurrentMove(move, position);
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
                    int eval = Minimax(position, depth - 1, true, alpha, beta);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    UnmakeCurrentMove(move, position);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return minEval;
            }
        }
        ImageSource[] GenPosition = new ImageSource[64];
        private Move GetBestMove(int depth)
        {
            GenerateMoves(possibleMoves, Position);
            int bestEval = int.MinValue;
            Move bestMove = default(Move);      
            foreach (Move move in possibleMoves.ToList())
            {
                Position.CopyTo(GenPosition, 0);
                MakeCurrentMove(move, GenPosition);
                int eval = Minimax(GenPosition, depth - 1, true, int.MinValue, int.MaxValue);
                if (eval > bestEval)
                {
                    bestEval = eval;
                    bestMove = move;
                }
                UnmakeCurrentMove(move, GenPosition);
            }
            return bestMove;
        }
    }
}
