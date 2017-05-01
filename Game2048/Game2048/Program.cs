using System;

namespace Game2048
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }

    public class Game
    {
        public int Score { get; private set; }
        public int[,] Board { get; set; }
        public int[,] BoardTemp { get; set; }
        public bool HasUpdated { get; set; }

        private readonly Random _random = new Random();
        private readonly int _rows = 4;
        private readonly int _cols = 4;
        private readonly int k = 0;

        public int[,] temp { get; set; }

        public Game()
        {
            Board = new int[_rows, _cols];
            BoardTemp = new int[_rows, _cols];
            InitalizeBoard(Board);
            _rows = Board.GetLength(0);
            _cols = Board.GetLength(1);
        }

        public void Run()
        {
            Display();
            do
            {
                BoardTemp = CopyBoard(Board);
                Update();
                GenerateNewIndex();
                Display();
            } while (k == 0);
        }

        public void Display()
        {
            Console.Clear();
            Console.WriteLine();
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    Color(i, j);
                    Console.Write("|{0,3}|", Board[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine("Score: {0}", Score);
            Console.WriteLine();
        }

        private void Update()
        {
            var key = Console.ReadKey().Key;

            if (key == ConsoleKey.LeftArrow)
            {
                Move();
                Summing();
            }
            else if (key == ConsoleKey.DownArrow)
            {
                Board = RotateBoard(Board, 1);
                Move();
                Summing();
                Board = RotateBoard(Board, 3);
            }
            else if (key == ConsoleKey.RightArrow)
            {
                Board = RotateBoard(Board, 2);
                Move();
                Summing();
                Board = RotateBoard(Board, 2);
            }
            else if (key == ConsoleKey.UpArrow)
            {
                Board = RotateBoard(Board, 3);
                Move();
                Summing();
                Board = RotateBoard(Board, 1);
            }
            else
                Update();

        }

        private void GenerateNewIndex()
        {
            this.HasUpdated = ContentEquals(Board, BoardTemp);
            if (HasUpdated == false)
            {
                int x, y;
                int flag = 1;
                do
                {
                    x = _random.Next(0, 4);
                    y = _random.Next(0, 4);

                    if (Board[x, y] == 0)
                    {
                        Board[x, y] = 2;
                        flag = 0;
                        Console.WriteLine(x);
                        Console.WriteLine(y);
                    }
                } while (flag == 1);
            }
        }

        private void InitalizeBoard(int[,] board)
        {

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                    board[i, j] = 0;
            }
            for (int i = 0; i < 2; i++)
            {
                int randomRowFirst = _random.Next(1, 4);
                int randomColFirst = _random.Next(1, 4);

                if (Board[randomRowFirst, randomColFirst] == 0)
                    board[randomRowFirst, randomColFirst] = 2;
            }
        }

        public void Move()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols - 1; j++)
                {
                    if (Board[i, j] == 0)
                    {
                        int temp = Board[i, j + 1];
                        Board[i, j] = temp;
                        Board[i, j + 1] = 0;
                    }
                }
            }

        }

        public void Summing()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols - 1; j++)
                {
                    if (Board[i, j] == Board[i, j + 1])
                    {
                        Board[i, j] *= 2;
                        Board[i, j + 1] = 0;
                        Score += Board[i, j];
                        Move();
                    }
                }
            }
        }

        private int[,] RotateBoard(int[,] board, int iterations)
        {
            int[,] temp = new int[_cols, _rows];
            int[,] temp2 = new int[_cols, _rows];
            temp2 = CopyBoard(board);

            for (int k = 0; k < iterations; k++)
            {
                for (int i = 3; i >= 0; --i)
                {
                    for (int j = 0; j < 4; ++j)
                    {
                        temp[j, 3 - i] = temp2[i, j];
                    }
                }
                temp2 = CopyBoard(temp);
            }
            return temp;
        }

        enum Colors
        {
            DarkGray = 0,
            Blue = 2,
            Red = 4,
            Magenta = 8,
            Yellow = 16,
            Green = 32,
            Cyan = 64,
            Grey = 128,
            DarkBlue = 256,
            DarkRed = 512,
            DarkMagenta = 1024,
            White = 2048
        }

        public void Color(int i, int j)
        {
            int value = Board[i, j];
            string myValue = ((Colors)value).ToString();
            Console.ForegroundColor = (ConsoleColor)Enum
                .Parse(typeof(ConsoleColor), myValue);
        }

        public static bool ContentEquals<T>(T[,] arr, T[,] other) where T : IComparable
        {
            if (arr.GetLength(0) != other.GetLength(0) ||
                arr.GetLength(1) != other.GetLength(1))
                return false;
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    if (arr[i, j].CompareTo(other[i, j]) != 0)
                        return false;
            return true;
        }

        public int[,] CopyBoard(int[,] Board)
        {
            int[,] temp = new int[_cols, _rows];
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    temp[i, j] = Board[i, j];
            return temp;
        }
    }
}
