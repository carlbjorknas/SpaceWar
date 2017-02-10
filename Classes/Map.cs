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

        public void MarkSquares(Pos startPos, Direction dir, int distance, bool isTarget=false)
        {
            var positions = MovementHelper.MapToPositions(startPos, dir, distance);
            foreach (var spacePos in positions.Take(positions.Count - 1))
            {
                SetSquareType(spacePos, SquareType.Space);
            }

            var lastPos = positions.Last();
            SetSquareType(lastPos, isTarget ? SquareType.Target : SquareType.NotSpace);
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

        public Pos GetBottomLeft()
        {
            int bottomMost = -1;
            for (int y = 0; y < Height && bottomMost < 0; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_squares[x, y] != SquareType.Unexplored)
                    {
                        bottomMost = y-1;
                        break;
                    }
                }
            }

            int leftMost = -1;
            for (int x = 0; x < Width && leftMost < 0; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_squares[x, y] != SquareType.Unexplored)
                    {
                        leftMost = x-1;
                        break;
                    }
                }
            }

            return new Pos(leftMost, bottomMost);
        }

        public Pos GetTopRight()
        {
            int topMost = Height;
            for (int y = Height-1; y >= 0 && topMost == Height; y--)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_squares[x, y] != SquareType.Unexplored)
                    {
                        topMost = y+1;
                        break;
                    }
                }
            }

            int rightMost = Width;
            for (int x = Width-1; x >= 0 && rightMost == Width; x--)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_squares[x, y] != SquareType.Unexplored)
                    {
                        rightMost = x+1;
                        break;
                    }
                }
            }

            return new Pos(rightMost, topMost);
        }

        public SquareType GetSquareType(int x, int y)
        {
            return _squares[x, y];
        }
    }
}