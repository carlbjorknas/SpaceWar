using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    internal class Pilot
    {
        private readonly Map _map;
        private readonly Ship _ship;
        private readonly RouteFinder _routeFinder;
        private Queue<Command> _commands;

        private bool _isExplorationPossible = true;

        public Pilot(Ship ship, Map map)
        {
            _ship = ship;
            _map = map;
            _routeFinder = new RouteFinder(map);    
            _commands = new Queue<Command>();
        }

        public void Steer()
        {
            if (Fighting())
            {
                _commands = new Queue<Command>();
                return;
            }

            Explore();
        }

        private bool Fighting()
        {
            if (TargetInRangeAhead)
            {
                _ship.FireLaser();
                return true;
            }

            if (TargetInRangeToTheLeft)
            {
                _ship.TurnLeft();
                return true;
            }

            if (TargetInRangeToTheRight)
            {
                _ship.TurnRight();
                return true;
            }

            if (TargetInRangeToTheBack)
            {
                _ship.TurnLeft();
                return true;
            }

            return false;
        }

        private bool TargetInRangeAhead
        {
            get { return _ship.IdentifyTarget; }
        }

        private bool TargetInRangeToTheLeft
        {
            get { return TargetInRangeIn(_ship.LeftDir); }
        }

        private bool TargetInRangeToTheRight
        {
            get { return TargetInRangeIn(_ship.RightDir); }
        }

        private bool TargetInRangeToTheBack
        {
            get { return TargetInRangeIn(_ship.BackDir); }
        }

        private bool TargetInRangeIn(Direction dir)
        {
            var firstNonSpaceSquare = _map.GetLineOfSquares(_ship.Pos, dir)
                .First(square => square.SquareType != SquareType.Space);
            return firstNonSpaceSquare.SquareType == SquareType.Target;
        }

        private void Explore()
        {
            if (!_isExplorationPossible)
            {
                return;
            }

            if (!_commands.Any())
            {
                _commands = _routeFinder.FindRouteForExploringUnexploredSquares(_ship);
                _isExplorationPossible = _commands.Any();
            }

            if (_commands.Any())
            {
                var command = _commands.Dequeue();
                command(_ship);
            }
        }
    }
}