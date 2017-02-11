using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar.Classes
{
    public class Map
    {
        private const int Size = 100;
        private readonly SquareType[,] _squares = new SquareType[Size, Size];

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
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    squares.Add(new Square {
                        Pos = new Pos(x, y),
                        SquareType = _squares[x, y]
                    });
                }
            }
            return squares;
        }

        public IEnumerable<Square> AllSquares(SquareVisitorDirection rowDir, SquareVisitorDirection cellDir)
        {
            var reverseDirs = new[] {SquareVisitorDirection.TopDown, SquareVisitorDirection.RightToLeft};

            var rowIds = Enumerable.Range(0, Size).ToList();
            if (reverseDirs.Contains(rowDir))
            {
                rowIds.Reverse();
            }

            var cellIds = Enumerable.Range(0, Size).ToList();
            if (reverseDirs.Contains(cellDir))
            {
                cellIds.Reverse();
            }

            var xAxisScan = cellDir == SquareVisitorDirection.LeftToRight || cellDir == SquareVisitorDirection.RightToLeft;

            foreach (var rowId in rowIds)
            {
                foreach (var cellId in cellIds)
                {
                    var x = xAxisScan ? cellId : rowId;
                    var y = xAxisScan ? rowId : cellId;
                    var squareType = GetSquareType(x, y);
                    yield return new Square(x, y, squareType);
                }
            }
        }

        public SquareType GetSquareType(int x, int y)
        {
            return _squares[x, y];
        }
    }
}