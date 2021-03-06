﻿namespace SpaceWar.Classes
{
    public class Ship{

        public Ship()
        {
            Pos = new Pos(50, 50);
            Direction = Direction.North;
        }

        public Pos Pos { get; private set; }
        public Direction Direction { get; private set; }

        internal void MoveForward()
        {
            Pos = MovementHelper.GetNext(Pos, Direction);
            API.MoveForward();
        }

        internal void MoveBackward()
        {
            Pos = MovementHelper.GetNext(Pos, MovementHelper.GetBackOf(Direction));
            API.MoveBackward();
        }

        public void TurnLeft()
        {
            Direction = LeftDir;
            API.TurnLeft();
        }

        public void TurnRight()
        {
            Direction = RightDir;
            API.TurnRight();
        }

        public bool IsOnPos(int x, int y)
        {
            return Pos.Equals(x, y);
        }

        public bool IsDead
        {
            get { return API.CurrentFuel() == 0; }
        }

        public bool IdentifyTarget
        {
            get { return API.IdentifyTarget(); }
        }

        public void FireLaser()
        {
            API.FireLaser();
        }

        public Direction LeftDir
        {
            get { return MovementHelper.GetLeftOf(Direction); }
        }

        public Direction RightDir
        {
            get { return MovementHelper.GetRightOf(Direction); }
        }

        public Direction BackDir
        {
            get { return MovementHelper.GetBackOf(Direction); }
        }

        public int LidarFront{ get { return API.LidarFront(); } }
        public int LidarBack { get { return API.LidarBack(); } }
        public int LidarLeft { get { return API.LidarLeft(); } }
        public int LidarRight { get { return API.LidarRight(); } }

        public void SetPosition(int x, int y, Direction direction)
        {
            Pos = new Pos(x, y);
            Direction = direction;
        }
    }
}