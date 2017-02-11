using System.Runtime.InteropServices;

namespace SpaceWar.Classes
{
    public class Engine
    {
        private readonly Map _map = new Map();
        private readonly Ship _ship = new Ship();
        private readonly Printer _printer;

        public Engine()
        {
            _printer = new Printer(_ship, _map);
        }

        public void Update()
        {
            if (API.CurrentFuel() == 0)
            {
                return;
            }

            UpdateMap();
            _printer.Print();
            //var targets = FindTargets();

            if (API.IdentifyTarget())
            {
                API.FireLaser();
                return;
            }

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

        private void UpdateMap()
        {
            var isTarget = API.IdentifyTarget();
            _map.MarkSquares(_ship.Pos, _ship.Direction, API.LidarFront(), isTarget);
            
            var dir = MovementHelper.GetLeftOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, API.LidarLeft());
            
            dir = MovementHelper.GetRightOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, API.LidarRight());
            
            dir = MovementHelper.GetBackOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, API.LidarBack());
        }

        //private List<Pos> FindTargets()
        //{
        //    return _map.GetAllPosOfType(SquareType.Target);
        //}
    }
}