using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    class RouteFinderSquare
    {
        public Pos Pos { get; private set; }
        public Direction Dir { get; private set; }
        public string Route { get; private set; }

        public RouteFinderSquare(Pos pos, Direction dir, string route)
        {
            Pos = pos;
            Dir = dir;
            Route = route;
        }
    }
}