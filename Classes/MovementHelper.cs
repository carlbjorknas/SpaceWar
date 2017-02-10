using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar.Classes
{
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