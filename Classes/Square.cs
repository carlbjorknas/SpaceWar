namespace SpaceWar.Classes
{
    public struct Square
    {
        public Square(int x, int y, SquareType squareType)
        {
            Pos = new Pos(x, y);
            SquareType = squareType;
        }

        public Square(Pos pos, SquareType squareType)
        {
            Pos = pos;
            SquareType = squareType;
        }

        public Pos Pos;
        public SquareType SquareType;
    }
}