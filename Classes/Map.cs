using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar.Classes
{
    public class Map
    {
        private const int Width = 100;
        private const int Height = 100;
        private readonly SquareType[,] _squares = new SquareType[Width, Height];

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
}