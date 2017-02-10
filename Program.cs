using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceWar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    static class API
    {
        public static bool IdentifyTarget() => true;
        public static void FireLaser() {}
        public static void MoveForward() { }
        public static void TurnLeft() { }
        public static int LidarFront() => 0;
    }

    //using System;
    //using System.Collections.Generic;
    //using System.Linq;

    public class Solution
    {
        Engine _engine;

        public Solution()
        {
            _engine = new Engine();
        }

        public void Update()
        {
            _engine.Update();
        }
    }

    public class Map
    {
        const int Width = 100;
        const int Height = 100;
        SquareType[,] _squares = new SquareType[Width, Height];

        public void MarkSquares(Pos startPos, Direction dir, int distance, bool isTarget)
        {
            var positions = MovementHelper.MapToPositions(startPos, dir, distance);
            foreach (var spacePos in positions.Take(positions.Count - 1))
            {
                SetSquareType(spacePos, SquareType.Space);
            }

            var lastPos = positions.Last();
            SetSquareType(lastPos, isTarget ? SquareType.Target : SquareType.Wall);
        }

        private void SetSquareType(Pos pos, SquareType type)
        {
            _squares[pos.X, pos.Y] = type;
        }

        //internal List<Pos> GetAllPosOfType(SquareType type)
        //{
        //    return MapAsSquares()
        //        .Where(square => square.Type == type)
        //        .Select(square => square.Pos)
        //        .ToList();
        //}

        private List<Square> MapAsSquares()
        {
            var squares = new List<Square>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    squares.Add(new Square {
                        Pos = new Pos(x, y),
                        Type = _squares[x, y]
                    });
                }
            }
            return squares;
        }

        internal void Print(Pos pos, Direction dir)
        {
            for (int y = pos.Y - 5; y<pos.Y+5; y++)
            {
                var row = "";
                for (int x = pos.X - 5; x < pos.X + 5; x++)
                {
                    row += ToString(x, y);
                }
                Console.WriteLine(row);
            }
        }

        private string ToString(int x, int y)
        {
            switch (_squares[x, y])
            {
                case SquareType.Unexplored: return "?";
                case SquareType.Space: return " ";
                case SquareType.NotSpace: return "@";
                case SquareType.Wall: return "#";
                case SquareType.Target:return "X";
                default: return "ö";
            }
        }
    }

    public class Engine
    {

        Map _map = new Map();
        Ship _ship = new Ship();

        public void Update()
        {
            UpdateMap();
            _map.Print(_ship.Pos, _ship.Direction);
            //var targets = FindTargets();

            if (API.IdentifyTarget())
            {
                API.FireLaser();
                return;
            }

            if (API.LidarFront() > 1)
            {
                _ship.MoveForward();
                return;
            }

            if (API.LidarFront() == 1)
            {
                API.TurnLeft();
                return;
            }
        }

        private void UpdateMap()
        {
            var distance = API.LidarFront();
            var isTarget = API.IdentifyTarget();

            _map.MarkSquares(_ship.Pos, _ship.Direction, distance, isTarget);
        }

        //private List<Pos> FindTargets()
        //{
        //    return _map.GetAllPosOfType(SquareType.Target);
        //}
    }

    public enum SquareType
    {
        Unexplored = 0,
        Space = 1,
        Wall = 2,
        NotSpace = 3,
        Target = 4
    }

    public enum Direction
    {
        North,
        East,
        South,
        West
    }

    public struct Square
    {
        public Pos Pos { get; set; }
        public SquareType Type { get; set; }
    }

    public class Ship{    
        public Ship()
        {
            Pos = new Pos(50, 50);
            Direction = Direction.North;
        }

        public Pos Pos { get; set; }
        public Direction Direction { get; set; }

        internal void MoveForward()
        {
            Pos = MovementHelper.GetNext(Pos, Direction);
            API.MoveForward();
        }
    }

    public struct Pos
    {
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;
    }

    public static class MovementHelper
    {
        public static Pos GetNext(Pos currentPos, Direction dir)
        {
            return MapToPositions(currentPos, dir, 1).First();
        }

        public static List<Pos> MapToPositions(Pos startPos, Direction dir, int distance)
        {
            Func<Pos, Pos> nextPos;
            switch (dir)
            {
                case Direction.North:
                    nextPos = oldPos => new Pos { X = oldPos.X, Y = oldPos.Y + 1 };
                    break;
                case Direction.East:
                    nextPos = oldPos => new Pos { X = oldPos.X + 1, Y = oldPos.Y };
                    break;
                case Direction.South:
                    nextPos = oldPos => new Pos { X = oldPos.X, Y = oldPos.Y - 1 };
                    break;
                case Direction.West:
                    nextPos = oldPos => new Pos { X = oldPos.X - 1, Y = oldPos.Y };
                    break;
                default: throw new Exception("Invalid direction");
            }

            var currentPos = startPos;
            var positions = Enumerable
                .Range(0, distance)
                .Select(unused => nextPos(currentPos))
                .ToList();
            return positions;
        }
    }
}
