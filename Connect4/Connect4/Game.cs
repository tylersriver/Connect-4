//==================================================
// Tyler Sriver
// Connect 4 - Game Class
// October 31, 2014
//==================================================
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Connect4
{
    class Game
    {
        // integers for the height of the board and width
        int BoardWidth, BoardHeight;
        //Booleans for the players 
        public bool player1;
        public bool player2;
        //Color for the color of each game piece
        private Color pieceColor;
        //state Enum for the state of each place on the game board
        public enum state { empty = 0, player1 = 1, player2 = 2 };
        //2D array of states to represent the board 
        private state[,] board = new state[7, 6];
        //array of integers 
        List<int> full = new List<int> { 5, 5, 5, 5, 5, 5, 5 };
        int X;
       
        //Constructor for the game
        public Game()
        {
            BoardWidth = 700;
            BoardHeight = 600;
            player1 = true;
            player2 = false;
            pieceColor = Color.Red;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    board[i, j] = state.empty;
                }
            }            
        }        

        //Constructor for redraw
        public Game(int x, int y, Color pcolor)
        {       
            state STATE = new state();
            if(pcolor == Color.Red)
            {
                player1 = true;
                player2 = false;
                pieceColor = Color.Red;
                STATE = state.player1;
            }
            else if (pcolor == Color.Black)
            {
                player2 = true;
                player1 = false;
                pieceColor = Color.Black;
            }
            
            board[x/100,y/100] = STATE;
            X = x/100;           
            
        }
        //Method for resting the board
        public void Reset()
        {
            full = new List<int>(7) { 5, 5, 5, 5, 5, 5, 5 };
            player1 = true;
            player2 = false;
            pieceColor = Color.Red;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    board[i, j] = state.empty;
                }
            }
        }
        //Method to draw blank game board
        public void drawBoard(PaintEventArgs e)
        {
            Pen line = new Pen(Color.Black);
            int lineXi = 0, lineXf = 700;
            int lineYi = 0, lineYf = 600;
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

            for (int startY = 0; startY <= BoardWidth; startY += 100)
            {
                e.Graphics.DrawLine(line, startY, lineYi, startY, lineYf);
            }

            for (int startX = 0; startX <= BoardHeight; startX += 100)
            {
                e.Graphics.DrawLine(line, lineXi, startX, lineXf, startX);
            }

            for (int y = 0; y <= BoardHeight; y += 100)
            {
                for (int x = 0; x <= BoardWidth; x += 100)
                {
                    e.Graphics.FillEllipse(myBrush, new Rectangle(x, y, 100, 100));
                }
            }
           
        }

        //Method that changes the players turn and game piece color
        private void playerTurn()
        {
            player1 = !player1;
            player2 = !player2;
            if (player1)
            {
                pieceColor = Color.Red;
            }
            else
            {
                pieceColor = Color.Black;
            }
        }

        //Method to draw the individual game pieces
        // Draws red piece if player 1 and black piece if player 2
        public void drawGamePiece(MouseEventArgs e, Graphics f)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(pieceColor);
            int xlocal = (e.X / 100);

            if (full[xlocal] >= 0)
            {

                if (player1 && board[xlocal, full[xlocal]] == state.empty)
                {
                    board[xlocal, full[xlocal]] = state.player1;
                    f.FillEllipse(myBrush, xlocal * 100, full[xlocal] * 100, 100, 100);
                    full[xlocal]--;
                    playerTurn();
                }
                else if (player2 && board[xlocal, full[xlocal]] == state.empty)
                {
                    board[xlocal, full[xlocal]] = state.player2;
                    f.FillEllipse(myBrush, xlocal * 100, full[xlocal] * 100, 100, 100);
                    full[xlocal]--;
                    playerTurn();
                }
            }           
        }

        //Method to redraw the pieces after maximization
        public void redrawGamePiece(Graphics f)
        {
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(pieceColor);
            int xlocal = (X / 100);

            if (full[xlocal] >= 0)
            {

                if (player1)
                {
                    board[xlocal, full[xlocal]] = state.player1;
                    f.FillEllipse(myBrush, xlocal * 100, full[xlocal] * 100, 100, 100);                                        
                }
                else if (player2)
                {
                    board[xlocal, full[xlocal]] = state.player2;
                    f.FillEllipse(myBrush, xlocal * 100, full[xlocal] * 100, 100, 100);                   
                }
            }
        }

        //Method to check if there is a winner
        public Color WinningPlayer()
        {
            bool RedPlayer = false;
            bool BlackPlayer = false;

            int colIndex = 6;
            int rowIndex = 5;

            //For loop to check vertical win
            for (int col = 0; col <= colIndex && !RedPlayer && !BlackPlayer; col++)
            {
                int RedWinner = 1;
                int BlackWinner = 1;
                for (int row = 0; row < rowIndex && !RedPlayer && !BlackPlayer; row++)
                {
                    if (board[col, row] != state.empty)
                    {

                        if ((board[col, row] == state.player1) && (board[col, row + 1] == state.player1))
                        {
                            RedWinner++;
                            if (RedWinner >= 4)
                                RedPlayer = true;
                        }
                        else if ((board[col, row] == state.player2) && (board[col, row + 1] == state.player2))
                        {
                            BlackWinner++;
                            if (BlackWinner >= 4)
                                BlackPlayer = true;
                        }
                        else
                        {
                            RedWinner = 1;
                            BlackWinner = 1;
                        }
                    }

                }
            }

            //For loop to check horizontal win
            for (int row = 0; row < colIndex && !RedPlayer && !BlackPlayer; row++)
            {
                int RedWinner = 1;
                int BlackWinner = 1;

                for (int col = 0; col < colIndex && !RedPlayer && !BlackPlayer; col++)
                {
                    if (board[col, row] != state.empty)
                    {


                        if ((board[col, row] == state.player1) && (board[col + 1, row] == state.player1))
                        {
                            RedWinner++;
                            if (RedWinner >= 4)
                                RedPlayer = true;
                        }
                        else if ((board[col, row] == state.player2) && (board[col + 1, row] == state.player2))
                        {
                            BlackWinner++;
                            if (BlackWinner >= 4)
                                BlackPlayer = true;
                        }
                        else
                        {
                            RedWinner = 1;
                            BlackWinner = 1;
                        }
                    }


                }
            }

            //For loop to check diagonal going up and right from starting piece
            for (int row = 0; row < rowIndex && !RedPlayer && !BlackPlayer; row++)
            {
                for (int col = 0; col < colIndex && !RedPlayer && !BlackPlayer; col++)
                    if (board[col, row] != state.empty)
                    {
                        int RedWinner = 1;
                        int BlackWinner = 1;

                        for (int i = 0; i + row < rowIndex && col - i >= 0; i++)
                        {
                            if ((board[col, row] == state.player1) && (board[col - i, row + i] == state.player1))
                            {
                                RedWinner++;
                                if (RedWinner >= 4)
                                    RedPlayer = true;
                            }
                            else if ((board[col, row] == state.player2) && (board[col - i, row + i] == state.player2))
                            {
                                BlackWinner++;
                                if (BlackWinner >= 4)
                                    BlackPlayer = true;
                            }
                            else
                            {
                                RedWinner = 1;
                                BlackWinner = 1;
                            }

                        }
                    }
            }
            
            //For loop to check diagonal win going up and to the left from starting piece
            for (int row = 0; row < rowIndex && !RedPlayer && !BlackPlayer; row++)
            {
                for (int col = 0; col < colIndex && !RedPlayer && !BlackPlayer; col++)
                {
                    if (board[col, row] != state.empty)
                    {
                        int RedWinner = 1;
                        int BlackWinner = 1;

                        for (int i = 0; i + row < rowIndex && col + i < rowIndex; i++)
                        {
                            if ((board[col, row] == state.player1) && (board[col + i, row + i] == state.player1))
                            {
                                RedWinner++;
                                if (RedWinner >= 4)
                                    RedPlayer = true;
                            }
                            else if ((board[col, row] == state.player2) && (board[col + i, row + i] == state.player2))
                            {
                                BlackWinner++;
                                if (BlackWinner >= 4)
                                    BlackPlayer = true;
                            }
                            else
                            {
                                RedWinner = 1;
                                BlackWinner = 1;
                            }
                        }
                    }
                }
            }
            if (RedPlayer)
                return Color.Red;
            else if (BlackPlayer)
                return Color.Black;
            else
                return Color.Empty;
        }

        //Method to override ToString to write information to a file
       /* public override string ToString()
        {
            
            return string.Format()
        }*/
    }
}
