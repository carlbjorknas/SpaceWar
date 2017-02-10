using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    internal class Printer
    {
        private readonly Ship _ship;
        private readonly Map _map;

        public Printer(Ship ship, Map map)
        {
            _ship = ship;
            _map = map;
        }
        internal void Print()
        {
            var bottomLeft = _map.GetBottomLeft();
            Console.WriteLine("BottomLeft: " + bottomLeft);
            var topRight = _map.GetTopRight();
            Console.WriteLine("TopRight: " + topRight);
            for (int y = topRight.Y; y >= bottomLeft.Y; y--)
            {
                var row = "";
                for (int x = bottomLeft.X; x <= topRight.X; x++)
                {
                    var type = _map.GetSquareType(x, y);
                    row += ToString(type);
                }
                Console.WriteLine(row);
            }
            Console.WriteLine("");
        }

        private static string ToString(SquareType type)
        {
            switch (type)
            {
                case SquareType.Unexplored: return "?";
                case SquareType.Space: return " ";
                case SquareType.NotSpace: return "@";
                case SquareType.Wall: return "#";
                case SquareType.Target: return "X";
                default: return "ö";
            }
        }
    }
}