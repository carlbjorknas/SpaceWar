namespace SpaceWar.Classes
{
    public class Solution
    {
        readonly Engine _engine;

        public Solution()
        {
            _engine = new Engine();
        }

        public void Update()
        {
            _engine.Update();
        }
    }
}