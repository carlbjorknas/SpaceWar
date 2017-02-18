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
        private bool _canSearchForAliens = false;

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
                _canSearchForAliens = true;
            }

            return _canSearchForAliens;
        }

        public void Steer()
        {
            if (AvoidEnemies())
            {
                _commands = new Queue<Command>();
                return;
            }

            if (FightAliens())
            {
                _commands = new Queue<Command>();
                return;
            }

            SearchAliens();
            //Explore();
        }

        private bool AvoidEnemies()
        {
            if (EnemyInRangeAhead || EnemyInRangeToTheBack)
            {
                if (_ship.LidarLeft >= _ship.LidarRight)
                {
                    _ship.TurnLeft();
                }
                else
                {
                    _ship.TurnRight();
                }
                return true;
            }

            if (EnemyInRangeToTheLeft || EnemyInRangeToTheRight)
            {
                if (_ship.LidarFront >= _ship.LidarBack)
                {
                    _ship.MoveForward();
                }
                else
                {
                    _ship.MoveBackward();
                }
                return true;
            }

            return false;
        }

        private void SearchAliens()
        {
            if (!_canSearchForAliens)
            {
                return;
            }

            if (!_commands.Any())
            {
                _commands = _routeFinder.FindRouteToNextAlien();
                _canSearchForAliens = _commands.Any();
                _canExplore = !_canSearchForAliens;
            }

            if (_commands.Any())
            {
                var command = _commands.Dequeue();
                command(_ship);
            }
        }

        private bool FightAliens()
        {
            if (AlienInRangeAhead)
            {
                _ship.FireLaser();
                return true;
            }

            if (AlienInRangeToTheLeft)
            {
                _ship.TurnLeft();
                return true;
            }

            if (AlienInRangeToTheRight)
            {
                _ship.TurnRight();
                return true;
            }

            if (AlienInRangeToTheBack)
            {
                _ship.TurnLeft();
                return true;
            }

            return false;
        }

        private bool EnemyInRangeAhead
        {
            get { return SquareTypeInRangeIn(_ship.Direction, SquareType.Enemy); }
        }

        private bool EnemyInRangeToTheLeft
        {
            get { return SquareTypeInRangeIn(_ship.LeftDir, SquareType.Enemy); }
        }

        private bool EnemyInRangeToTheRight
        {
            get { return SquareTypeInRangeIn(_ship.RightDir, SquareType.Enemy); }
        }

        private bool EnemyInRangeToTheBack
        {
            get { return SquareTypeInRangeIn(_ship.BackDir, SquareType.Enemy); }
        }

        private bool AlienInRangeAhead
        {
            get { return SquareTypeInRangeIn(_ship.Direction, SquareType.Alien); }
        }

        private bool AlienInRangeToTheLeft
        {
            get { return SquareTypeInRangeIn(_ship.LeftDir, SquareType.Alien); }
        }

        private bool AlienInRangeToTheRight
        {
            get { return SquareTypeInRangeIn(_ship.RightDir, SquareType.Alien); }
        }

        private bool AlienInRangeToTheBack
        {
            get { return SquareTypeInRangeIn(_ship.BackDir, SquareType.Alien); }
        }

        private bool SquareTypeInRangeIn(Direction dir, SquareType squareType)
        {
            var firstNonSpaceSquare = _map.GetLineOfSquares(_ship.Pos, dir)
                .First(square => square.SquareType != SquareType.Space);
            return firstNonSpaceSquare.SquareType == squareType;
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