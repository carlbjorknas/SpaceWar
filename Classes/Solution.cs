namespace SpaceWar.Classes
{
    public class Solution
    {
        private readonly Engine _engine = new Engine();

        public Solution()
        {
        }

        public void Update()
        {
            _engine.Update();
        }
    }
}