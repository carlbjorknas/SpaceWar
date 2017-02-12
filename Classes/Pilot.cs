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

        public Pilot(Ship ship, Map map)
        {
            _ship = ship;
            _map = map;
        }

        public void Steer()
        {
            if (!Fighting())
            {
                Explore();
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
                _ship.TurnRigth();
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
            if (API.LidarFront() > 1)
            {
                _ship.MoveForward();
                return;
            }

            if (API.LidarFront() == 1)
            {
                _ship.TurnLeft();
                return;
            }
        }
    }
}