using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar.Classes
{
    public class Map
    {
        public const int Size = 100;
        private readonly SquareType[,] _squares = new SquareType[Size, Size];

        public void SetStartPos(Pos pos)
        {
            _squares[pos.X, pos.Y] = SquareType.Space;
        }

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

        public SquareType GetSquareType(int x, int y)
        {
            return _squares[x, y];
        }

        public SquareType GetSquareType(Pos pos)
        {
            return GetSquareType(pos.X, pos.Y);
        }

        private void SetSquareType(Pos pos, SquareType type)
        {
            _squares[pos.X, pos.Y] = type;
        }

        private Square GetSquare(int x, int y)
        {
            return new Square(x, y, _squares[x, y]);
        }

        public IEnumerable<Square> AllSquares()
        {
            return AllSquares(Direction.North, Direction.East);
        }

        public IEnumerable<Square> AllSquares(Direction rowDir, Direction cellDir)
        {
            var rowIndexes = GetIndexes(rowDir);
            var cellIndexes = GetIndexes(cellDir);

            var xAxisScan = cellDir == Direction.East || cellDir == Direction.West;

            foreach (var rowIndex in rowIndexes)
            {
                foreach (var cellIndex in cellIndexes)
                {
                    var x = xAxisScan ? cellIndex : rowIndex;
                    var y = xAxisScan ? rowIndex : cellIndex;
                    var squareType = GetSquareType(x, y);
                    yield return new Square(x, y, squareType);
                }
            }
        }

        private static List<int> GetIndexes(Direction dir)
        {
            var reverseDirs = new[] { Direction.South, Direction.West };
            var indexes = Enumerable.Range(0, Size).ToList();

            if (reverseDirs.Contains(dir))
            {
                indexes.Reverse();
            }
            
            return indexes;
        } 

        public IEnumerable<Square> GetLineOfSquares(Pos pos, Direction dir)
        {
            var indexes = GetIndexes(dir);
            switch (dir)
            {
                case Direction.North:
                    return GetSquaresInColumn(pos.X, indexes.Skip(pos.Y + 1));
                case Direction.East:
                    return GetSquaresInRow(indexes.Skip(pos.X + 1), pos.Y);
                case Direction.South:
                    return GetSquaresInColumn(pos.X, indexes.Skip(Size - pos.Y));
                case Direction.West:
                    return GetSquaresInRow(indexes.Skip(Size - pos.X), pos.Y);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IEnumerable<Square> GetSquaresInRow(IEnumerable<int> xIndexes, int yIndex)
        {
            return xIndexes.Select(xIndex => GetSquare(xIndex, yIndex));
        }

        private IEnumerable<Square> GetSquaresInColumn(int xIndex, IEnumerable<int> yIndexes)
        {
            return yIndexes.Select(yIndex => GetSquare(xIndex, yIndex));
        }

        public void SetMap(List<string> newMap)
        {
            var yIndex = newMap.Count-1;
            foreach (var row in newMap)
            {
                var xIndex = 0;
                foreach (var squareSymbol in row)
                {
                    _squares[xIndex, yIndex] = MapToSquareType(squareSymbol);
                    xIndex++;
                }
                yIndex--;
            }          
        }

        private SquareType MapToSquareType(char squareSymbol)
        {
            switch (squareSymbol)
            {
                case '#': return SquareType.Wall;
                case ' ': return SquareType.Space;
                case 'X': return SquareType.Target;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}