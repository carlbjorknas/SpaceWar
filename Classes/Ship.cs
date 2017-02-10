namespace SpaceWar.Classes
{
    public class Ship{    
        public Ship()
        {
            Pos = new Pos(50, 50);
            Direction = Direction.South;
        }

        public Pos Pos { get; set; }
        public Direction Direction { get; set; }

        internal void MoveForward()
        {
            Pos = MovementHelper.GetNext(Pos, Direction);
            API.MoveForward();
        }

        public void TurnLeft()
        {
            Direction = MovementHelper.GetLeftOf(Direction);
            API.TurnLeft();
        }
    }
}