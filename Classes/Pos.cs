﻿namespace SpaceWar.Classes
{
    public struct Pos
    {
        public Pos(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X;
        public int Y;

        public override string ToString()
        {
            return "X: " + X + ", Y:" + Y;
        }

        public bool Equals(int x, int y)
        {
            return X == x && Y == y;
        }
    }
}