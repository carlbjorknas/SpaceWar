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

            // Add frame of unexplored squares
            bottomLeft.X--;
            bottomLeft.Y--;
            topRight.X++;
            topRight.Y++;

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
            var bottommostExploredSquare = _map.AllSquares(SquareVisitorDirection.DownUp, SquareVisitorDirection.LeftToRight)
                .First(square => square.SquareType != SquareType.Unexplored);

            var leftmostExploredSquare = _map.AllSquares(SquareVisitorDirection.LeftToRight, SquareVisitorDirection.DownUp)
                .First(square => square.SquareType != SquareType.Unexplored);

            return new Pos(leftmostExploredSquare.Pos.X, bottommostExploredSquare.Pos.Y);
        }

        private Pos GetTopRightPosOfExploredPartOfMap()
        {
            var topmostExploredSquare = _map.AllSquares(SquareVisitorDirection.TopDown, SquareVisitorDirection.LeftToRight)
                .First(square => square.SquareType != SquareType.Unexplored);

            var rightmostExploredSquare = _map.AllSquares(SquareVisitorDirection.RightToLeft, SquareVisitorDirection.DownUp)
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