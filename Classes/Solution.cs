namespace SpaceWar.Classes
{
    public class Solution
    {
        private readonly Map _map = new Map();
        private readonly Ship _ship = new Ship();
        private Printer _printer;
        private Pilot _pilot;

        public Solution()
        {
            _printer = new Printer(_ship, _map);
            _pilot = new Pilot(_ship, _map);
        }

        public void Update()
        {
            if (_ship.IsDead) return;

            UpdateMap();
            _printer.Print();
            _pilot.Steer();
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
    }
}