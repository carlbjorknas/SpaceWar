using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    internal class Printer
    {
        private readonly Ship _ship;
        private readonly Map _map;

        public Printer(Ship ship, Map map)
        {
            _ship = ship;
            _map = map;
        }
        internal void Print()
        {
            var bottomLeft = GetBottomLeftPosOfExploredPartOfMap();
            var topRight = GetTopRightPosOfExploredPartOfMap();

            //Console.WriteLine("Bottom left: " + bottomLeft);
            //Console.WriteLine("Top right: " + topRight);
            //Console.WriteLine("Ship pos: " + _ship.Pos);
            //Console.WriteLine("Ship dir: " + _ship.Direction);

            //// Add frame of unexplored squares
            //bottomLeft.X--;
            //bottomLeft.Y--;
            //topRight.X++;
            //topRight.Y++;

            for (int y = topRight.Y; y >= bottomLeft.Y; y--)
            {
                var row = "";
                for (var x = bottomLeft.X; x <= topRight.X; x++)
                {                               
                    var type = _map.GetSquareType(x, y);
                    row += _ship.IsOnPos(x, y)
                        ? ToString(_ship.Direction)
                        : ToString(type);
                }
                Console.WriteLine(row);
            }
            Console.WriteLine("");
        }

        private Pos GetBottomLeftPosOfExploredPartOfMap()
        {
            var bottommostExploredSquare = _map.AllSquares(Direction.North, Direction.East)
                .First(square => square.SquareType != SquareType.Unexplored);

            var leftmostExploredSquare = _map.AllSquares(Direction.East, Direction.North)
                .First(square => square.SquareType != SquareType.Unexplored);

            return new Pos(leftmostExploredSquare.Pos.X, bottommostExploredSquare.Pos.Y);
        }

        private Pos GetTopRightPosOfExploredPartOfMap()
        {
            var topmostExploredSquare = _map.AllSquares(Direction.South, Direction.East)
                .First(square => square.SquareType != SquareType.Unexplored);

            var rightmostExploredSquare = _map.AllSquares(Direction.West, Direction.North)
                .First(square => square.SquareType != SquareType.Unexplored);

            return new Pos(rightmostExploredSquare.Pos.X, topmostExploredSquare.Pos.Y);
        }

        private static string ToString(SquareType type)
        {
            switch (type)
            {
                case SquareType.Unexplored: return "?";
                case SquareType.Space: return " ";
                case SquareType.NotSpace: return "@";
                case SquareType.Wall: return "#";
                case SquareType.Target: return "X";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private static string ToString(Direction direction)
        {
            switch (direction)
            {
                case Direction.East: return "→";
                case Direction.West: return "←";
                case Direction.South: return "↓";
                case Direction.North: return "↑";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}