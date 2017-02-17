namespace SpaceWar.Classes
{
    public class Solution
    {
        private readonly Map _map = new Map();
        private readonly Ship _ship = new Ship();
        private Printer _printer;
        private Pilot _pilot;

        private bool _firstRun = true;

        public Solution()
        {
            _printer = new Printer(_ship, _map);
            _pilot = new Pilot(_ship, _map);                        
        }

        public void Update()
        {
            if (_firstRun)
            {
                if (!_pilot.IdentifyLevel())
                {
                    _map.SetStartPos(_ship.Pos);                    
                }
                _firstRun = false;
            }

            if (_ship.IsDead) return;

            UpdateMap();
            _printer.Print();
            _pilot.Steer();
        }

        private void UpdateMap()
        {
            _map.MarkSquares(_ship.Pos, _ship.Direction, _ship.LidarFront, _ship.IdentifyTarget);

            var dir = MovementHelper.GetLeftOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, _ship.LidarLeft);

            dir = MovementHelper.GetRightOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, _ship.LidarRight);

            dir = MovementHelper.GetBackOf(_ship.Direction);
            _map.MarkSquares(_ship.Pos, dir, _ship.LidarBack);
        }
    }
}