using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    internal delegate void Command(Ship ship);

    internal class RouteFinder
    {
        private readonly Map _map;
        private readonly Ship _ship;

        public RouteFinder(Map map, Ship ship)
        {
            _map = map;
            _ship = ship;
        }

        public Queue<Command> FindRouteToNextTarget()
        {
            return FindRouteToSquareType(SquareType.Target);
        }

        public Queue<Command> FindRouteForExploringUnexploredSquares()
        {
            return FindRouteToSquareType(SquareType.Unexplored);
        }

        private Queue<Command> FindRouteToSquareType(SquareType squareType)
        {
            var exploredPositions = new bool[100, 100];

            var queuedSquares = new List<RouteFinderSquare>();

            var startSquare = new RouteFinderSquare(_ship.Pos, _ship.Direction, "");
            queuedSquares.Add(startSquare);

            while (queuedSquares.Any())
            {
                queuedSquares = queuedSquares.OrderBy(s => s.Route.Length).ToList();
                var squareToEval = queuedSquares.First();
                queuedSquares.RemoveAt(0);
                if (exploredPositions[squareToEval.Pos.X, squareToEval.Pos.Y] || _map.GetSquareType(squareToEval.Pos) != SquareType.Space)
                {
                    continue;
                }

                if (FindsSquareType(squareToEval, squareType))
                {
                    return ConvertToCommands(squareToEval.Route);
                }

                exploredPositions[squareToEval.Pos.X, squareToEval.Pos.Y] = true;

                GetNeighbourSquares(squareToEval)
                    .ForEach(s => queuedSquares.Add(s));
            }

            return new Queue<Command>();
        }

        private Queue<Command> ConvertToCommands(string route)
        {
            Console.WriteLine("Route: " + route);
            return new Queue<Command>(route.Select(ConvertToCommand));
        }

        public Command ConvertToCommand(char command)
        {
            switch (command)
            {
                case 'F': return MoveForwardCommand;
                case 'B': return MoveBackwardCommand;
                case 'L': return TurnLeftCommand;
                case 'R': return TurnRightCommand;
                default: throw new ArgumentOutOfRangeException(); 
            }
        }

        private List<RouteFinderSquare> GetNeighbourSquares(RouteFinderSquare square)
        {
            var neighbours = new List<RouteFinderSquare>();

            neighbours.Add(GetNeighbourSquare(square.Pos, square.Dir, square.Route + "F"));
            neighbours.Add(new RouteFinderSquare(MovementHelper.GetNext(square.Pos, MovementHelper.GetBackOf(square.Dir)), square.Dir, square.Route + "B"));
            neighbours.Add(GetNeighbourSquare(square.Pos, MovementHelper.GetLeftOf(square.Dir), square.Route + "LF"));
            neighbours.Add(GetNeighbourSquare(square.Pos, MovementHelper.GetRightOf(square.Dir), square.Route + "RF"));

            return neighbours;
        }

        private RouteFinderSquare GetNeighbourSquare(Pos pos, Direction dir, string route)
        {
            var newPos = MovementHelper.GetNext(pos, dir);
            return new RouteFinderSquare(newPos, dir, route);
        }

        private static Command MoveForwardCommand { get { return ship => ship.MoveForward(); } }
        private static Command TurnLeftCommand { get { return ship => ship.TurnLeft(); } }
        private Command TurnRightCommand { get { return ship => ship.TurnRight(); } }
        private Command MoveBackwardCommand { get { return ship => ship.MoveBackward(); } }

        private bool FindsSquareType(RouteFinderSquare square, SquareType squareType)
        {            
            return MovementHelper.AllDirections.Any(dir => FindsSquareType(square.Pos, dir, squareType));
        }

        private bool FindsSquareType(Pos pos, Direction dir, SquareType squareType)
        {
            var firstNonSpaceSquare = _map.GetLineOfSquares(pos, dir)
                .SkipWhile(s => s.SquareType == SquareType.Space)
                .First();
            return firstNonSpaceSquare.SquareType == squareType;
        }
    }
}