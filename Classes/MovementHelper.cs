﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceWar.Classes
{
    public static class MovementHelper
    {
        public static Pos GetNext(Pos currentPos, Direction dir)
        {
            var getNextPos = GetStepper(dir);
            return getNextPos(currentPos);
        }

        public static List<Pos> MapToPositions(Pos startPos, Direction dir, int distance)
        {
            var getNextPos = GetStepper(dir);

            var currentPos = startPos;
            var positions = Enumerable
                .Range(0, distance)
                .Select(unused => currentPos = getNextPos(currentPos))
                .ToList();
            return positions;
        }

        private static Func<Pos, Pos> GetStepper(Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return oldPos => new Pos { X = oldPos.X, Y = oldPos.Y + 1 };
                case Direction.East: return oldPos => new Pos { X = oldPos.X + 1, Y = oldPos.Y };                    
                case Direction.South: return oldPos => new Pos { X = oldPos.X, Y = oldPos.Y - 1 };                    
                case Direction.West: return oldPos => new Pos { X = oldPos.X - 1, Y = oldPos.Y };
                default: throw new Exception("Invalid direction");
            }
        }

        public static Direction GetLeftOf(Direction direction)
        {
            var newValue = ((int)direction + 3) % 4;
            return (Direction) newValue;
        }
    }
}