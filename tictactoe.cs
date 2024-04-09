// #nullable enable
using System;

using static System.Console;

namespace Bme121
{
    static class Program
    {
        static string[ , ] NewBoard( int rows, int cols )
        {
            const string blank = " ";
            
            string[ , ] board = new string[ rows, cols ];
            
            for( int row = 0; row < rows; row ++ )
            {
                for( int col = 0; col < cols; col ++ )
                {
                    board[ row, col ] = blank;
                }
            }
            return board;
        }
        
        static void DisplayBoard( string[ , ] board )
        {
            const string h  = "\u2500"; // horizontal line
            const string v  = "\u2502"; // vertical line
            const string tl = "\u250c"; // top left corner
            const string tr = "\u2510"; // top right corner
            const string bl = "\u2514"; // bottom left corner
            const string br = "\u2518"; // bottom right corner
            const string vr = "\u251c"; // vertical join from right
            const string vl = "\u2524"; // vertical join from left
            const string hb = "\u252c"; // horizontal join from below
            const string ha = "\u2534"; // horizontal join from above
            const string hv = "\u253c"; // horizontal vertical cross
            const string mx = "\u256c"; // marked horizontal vertical cross
            const string sp =      " "; // space
            
            int rows = board.GetLength( 0 );
            int cols = board.GetLength( 1 );
            
            for( int row = 0; row < rows; row ++ )
            {
                if( row == 0 )
                {
                    // Labels above top edge.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{0}{1}{0}", sp, col + 1 );
                        else Write( "{0}{0}{1}{0}", sp, col + 1 );
                    }
                    WriteLine( );
                    
                    //Border above top row
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", tl, h );
                        else Write( "{0}{1}{1}{1}", hb, h );
                        if( col == cols - 1 ) Write( "{0}", tr );
                    }
                    WriteLine( );
                }
                else
                {
                    // Border above a row which is not the top row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if(    rows > 5 && cols > 5 && row ==        2 && col ==        2 
                            || rows > 5 && cols > 5 && row ==        2 && col == cols - 2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col ==        2 
                            || rows > 5 && cols > 5 && row == rows - 2 && col == cols - 2 )  
                            Write( "{0}{1}{1}{1}", mx, h );
                        else if( col == 0 ) Write( "   {0}{1}{1}{1}", vr, h );
                        else Write( "{0}{1}{1}{1}", hv, h );
                        if( col == cols - 1 ) Write( "{0}", vl );
                    }
                    WriteLine( );
                }
                
                // Row content displayed column by column.
                for( int col = 0; col < cols; col ++ ) 
                {
                    if( col == 0 ) Write( " {0,-2}", row + 1); // Labels on left side
                    Write( "{0} {1} ", v, board[ row, col ] );
                    if( col == cols - 1 ) Write( "{0}", v );
                }
                WriteLine( );
                
                if( row == rows - 1 )
                {
                    // Border below last row.
                    for( int col = 0; col < cols; col ++ )
                    {
                        if( col == 0 ) Write( "   {0}{1}{1}{1}", bl, h );
                        else Write( "{0}{1}{1}{1}", ha, h );
                        if( col == cols - 1 ) Write( "{0}", br );
                    }
                    WriteLine( );
                }
            }
        }
        
        static void Main( )
        {
            string[ , ] game = NewBoard( rows: 3, cols: 3 );
            string playerX = "X";
            string playerO = "O";
            
            // welcome
            WriteLine( );
            WriteLine("Welcome to Tic Tac Toe!");
            WriteLine( );
            
            // game instructions
            WriteLine();
            WriteLine("This is a game of Tic Tac Toe. To place a tile down, enter the coordinate points of the tile location using row column");
            WriteLine("For example, the top left tile coordinates would be '11' while the top right tile would be '13'");
            WriteLine();
            
            // get player names
            Write("You are Player X. Enter your name or hit <Enter>: ");
            string playerName = ReadLine();
            
            WriteLine();
            DisplayBoard( game );
            WriteLine( );
            
            // start game with playerWhite
            string currentTurn = playerX;
            string response = "";
            
            // gameplay
            while(response != "quit" && CheckGameOn())
            {
                // player X plays
                if (currentTurn == playerX)
                {
                    if(playerName.Length < 1) Write("Your move: ");
                    else Write($"{playerName}'s move: ");
                    
                    response = ReadLine();
                
                    if (response != "quit")
                    {
                        int.Parse(response);
                        
                        if(! CheckMove(response))
                        {
                            WriteLine("Invalid move, try again");
                            DisplayBoard(game);
                        }
                        else
                        {
                            int row = int.Parse(response.Substring(0, 1));
                            int col = int.Parse(response.Substring(1, 1));
                            
                            game[row - 1, col - 1] = currentTurn;
                            Console.Clear();
                            WriteLine($"{playerName}'s move: {game[row - 1, col - 1]}");
                            DisplayBoard(game);
                            if(! CheckGameOn()) break;
                            ChangeTurn();
                        }
                    }
                }
                if (CheckTie()) break;
            
                // computer plays
                else
                {
                    int move = RandomMove();
                    string stringMove = move.ToString();
                    int x = int.Parse(stringMove.Substring(0, 1));
                    int y = int.Parse(stringMove.Substring(1, 1));
                    
                    //if(game[x - 1, y - 1] == "X" || game[x - 1, y - 1] == "O") RandomMove();
                    
                    game[x - 1, y - 1] = currentTurn;
                    Console.Clear();
                    WriteLine($"Opponent's move: {move}");
                    DisplayBoard(game);
                    if(! CheckGameOn()) break;
                    ChangeTurn();
                }
                if (CheckTie()) break;
            }
            
            WriteLine($"{FindWinner()}");
            
            // change turn between moves
            void ChangeTurn()
            {
                if (currentTurn == playerX) currentTurn = playerO;
                else currentTurn = playerX;
            }
            
            // random move generator function
            int RandomMove()
            {
                int move, x, y;
                
                do
                {
                    Random rGen = new Random();
                    
                    x = rGen.Next(1, 4);
                    y = rGen.Next(1, 4);
                    
                    move = int.Parse(x.ToString() + y.ToString());
                }
                while (game[x - 1, y - 1] != " ");
                
                return move;
            }
            
            // return X or O based on current player
            string GetDisc()
            {
                if (currentTurn == playerX) return playerX;
                else return playerO;
            }
            
            // return X or O based on next turn's player
            string NotDisc()
            {
                if (currentTurn == playerX) return playerO;
                else return playerX;
            }
            
            bool CheckGameOn()
            {                
                bool check = false;
                
                if( (game[0, 0] == GetDisc() && game[0, 1] == GetDisc() && game[0, 2] == GetDisc() ) || // row 1
                    (game[1, 0] == GetDisc() && game[1, 1] == GetDisc() && game[1, 2] == GetDisc() ) || // row 2
                    (game[2, 0] == GetDisc() && game[2, 1] == GetDisc() && game[2, 2] == GetDisc() ) || // row 3
                    (game[0, 0] == GetDisc() && game[1, 0] == GetDisc() && game[2, 0] == GetDisc() ) || // column 1
                    (game[0, 1] == GetDisc() && game[1, 1] == GetDisc() && game[2, 1] == GetDisc() ) || // column 2
                    (game[0, 2] == GetDisc() && game[1, 2] == GetDisc() && game[2, 2] == GetDisc() ) || // column 3
                    (game[0, 0] == GetDisc() && game[1, 1] == GetDisc() && game[2, 2] == GetDisc() ) || // top left diagonal
                    (game[0, 2] == GetDisc() && game[1, 1] == GetDisc() && game[2, 0] == GetDisc() ) )  // top right diagonal
                {
                    check = false;
                }
                else if( (game[0, 0] == NotDisc() && game[0, 1] == NotDisc() && game[0, 2] == NotDisc() ) || // row 1
                         (game[1, 0] == NotDisc() && game[1, 1] == NotDisc() && game[1, 2] == NotDisc() ) || // row 2
                         (game[2, 0] == NotDisc() && game[2, 1] == NotDisc() && game[2, 2] == NotDisc() ) || // row 3
                         (game[0, 0] == NotDisc() && game[1, 0] == NotDisc() && game[2, 0] == NotDisc() ) || // column 1
                         (game[0, 1] == NotDisc() && game[1, 1] == NotDisc() && game[2, 1] == NotDisc() ) || // column 2
                         (game[0, 2] == NotDisc() && game[1, 2] == NotDisc() && game[2, 2] == NotDisc() ) || // column 3
                         (game[0, 0] == NotDisc() && game[1, 1] == NotDisc() && game[2, 2] == NotDisc() ) || // top left diagonal
                         (game[0, 2] == NotDisc() && game[1, 1] == NotDisc() && game[2, 0] == NotDisc() ) )  // top right diagonal
                {
                    check = false;
                }
                else check = true;
                
                return check;
            }
            
            bool CheckMove(string response)
            {
                int x = int.Parse(response.Substring(0, 1));
                int y = int.Parse(response.Substring(1, 1));
                
                if (response == "quit")
                {
                    WriteLine("The game is over");
                    return false;
                }
                else
                {
                    if (response.Length != 2)
                    {
                        return false;
                    }
                    else if (x > 0 && x < 4 && y > 0 && y < 4)
                    {
                        if(game[x - 1, y - 1] == "X" || game[x - 1, y - 1] == "O")
                        {
                            return false;
                        }
                        else return true;
                    }
                    else return false;
                }
            }
            
            string FindWinner()
            {                
                if( (game[0, 0] == "X" && game[0, 1] == "X" && game[0, 2] == "X" ) || // row 1
                    (game[1, 0] == "X" && game[1, 1] == "X" && game[1, 2] == "X" ) || // row 2
                    (game[2, 0] == "X" && game[2, 1] == "X" && game[2, 2] == "X" ) || // row 3
                    (game[0, 0] == "X" && game[1, 0] == "X" && game[2, 0] == "X" ) || // column 1
                    (game[0, 1] == "X" && game[1, 1] == "X" && game[2, 1] == "X" ) || // column 2
                    (game[0, 2] == "X" && game[1, 2] == "X" && game[2, 2] == "X" ) || // column 3
                    (game[0, 0] == "X" && game[1, 1] == "X" && game[2, 2] == "X" ) || // top left diagonal
                    (game[0, 2] == "X" && game[1, 1] == "X" && game[2, 0] == "X" ) )  // top right diagonal
                {
                    return "Game over: you win! test :)";
                }
                else if( (game[0, 0] == "O" && game[0, 1] == "O" && game[0, 2] == "O" ) || // row 1
                         (game[1, 0] == "O" && game[1, 1] == "O" && game[1, 2] == "O" ) || // row 2
                         (game[2, 0] == "O" && game[2, 1] == "O" && game[2, 2] == "O" ) || // row 3
                         (game[0, 0] == "O" && game[1, 0] == "O" && game[2, 0] == "O" ) || // column 1
                         (game[0, 1] == "O" && game[1, 1] == "O" && game[2, 1] == "O" ) || // column 2
                         (game[0, 2] == "O" && game[1, 2] == "O" && game[2, 2] == "O" ) || // column 3
                         (game[0, 0] == "O" && game[1, 1] == "O" && game[2, 2] == "O" ) || // top left diagonal
                         (game[0, 2] == "O" && game[1, 1] == "O" && game[2, 0] == "O" ) )  // top right diagonal
                {
                    return "Game over: you lose! :(";
                }
                else return "Game over: you tie :/";
            }
            
            bool CheckTie()
            {
                int count = 0;
                
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 3; j++)
                    {
                        if(game[i, j] == "X" || game[i, j] == "O")
                        {
                            count++;
                        }
                    }
                }
                
                if (count == 9) return true;
                else return false;
            }
        }
    }
}
