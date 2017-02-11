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

        public void MarkSquares(Pos startPos, Direction dir, int distance, bool? isTarget=null)
        {
            var positions = MovementHelper.MapToPositions(startPos, dir, distance);
            MarkAllButLastPositionAsSpace(positions);
            MarkLastPosition(positions.Last(), isTarget);
        }

        private void MarkLastPosition(Pos lastPos, bool? isTarget)
        {
            var squareType = GetSquareTypeOfLastPosition(lastPos, isTarget);
            SetSquareType(lastPos, squareType);
        }

        private SquareType GetSquareTypeOfLastPosition(Pos lastPos, bool? isTarget)
        {
            if (isTarget.HasValue)
            {
                return isTarget.Value ? SquareType.Target : SquareType.Wall;
            }

            var currentSquareType = GetSquareType(lastPos);
            if (currentSquareType == SquareType.Space)
            {
                return SquareType.Target;
            }

            return currentSquareType == SquareType.Wall || currentSquareType == SquareType.Target
                ? currentSquareType
                : SquareType.NotSpace;
        }


        private void MarkAllButLastPositionAsSpace(List<Pos> positions)
        {
            foreach (var spacePos in positions.Take(positions.Count - 1))
            {
                SetSquareType(spacePos, SquareType.Space);
            }
        }

        private SquareType GetSquareType(Pos pos)
        {
            return GetSquareType(pos.X, pos.Y);
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