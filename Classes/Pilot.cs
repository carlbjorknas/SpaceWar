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

        private bool _canExplore = true;
        private bool _canSearchForTargets = false;

        public Pilot(Ship ship, Map map)
        {
            _ship = ship;
            _map = map;
            _routeFinder = new RouteFinder(map, ship);    
            _commands = new Queue<Command>();
        }

        public bool IdentifyLevel()
        {
            if (LevelIdentifier.IdentifyLevel(_ship, _map))
            {
                _canExplore = false;
                _canSearchForTargets = true;
            }

            return _canSearchForTargets;
        }

        public void Steer()
        {
            if (Fighting())
            {
                _commands = new Queue<Command>();
                return;
            }

            SearchTarget();
            Explore();
        }

        private void SearchTarget()
        {
            if (!_canSearchForTargets)
            {
                return;
            }

            if (!_commands.Any())
            {
                _commands = _routeFinder.FindRouteToNextTarget();
                _canSearchForTargets = _commands.Any();
                _canExplore = !_canSearchForTargets;
            }

            if (_commands.Any())
            {
                var command = _commands.Dequeue();
                command(_ship);
            }
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
            if (!_canExplore)
            {
                return;
            }

            if (!_commands.Any())
            {
                _commands = _routeFinder.FindRouteForExploringUnexploredSquares();
                _canExplore = _commands.Any();
            }

            if (_commands.Any())
            {
                var command = _commands.Dequeue();
                command(_ship);
            }
        }
    }
}