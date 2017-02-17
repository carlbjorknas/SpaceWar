using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceWar.Classes
{
    internal static class LevelIdentifier
    {
        public static bool IdentifyLevel(Ship ship, Map map)
        {
            if (ship.LidarFront < 6)
            {
                Console.WriteLine("Level 1 identified!");
                SetStartPositionOnLevel1(ship);
                map.SetMap(Level1());
                return true;
            }

            Console.WriteLine("Level 2 identified!");
            SetStartPositionOnLevel2(ship);
            map.SetMap(Level2());

            return true;
        }

        private static void SetStartPositionOnLevel1(Ship ship)
        {            
            if (ship.LidarFront == 4)
            {
                Console.WriteLine("Starting on upper half!");
                var x = ship.LidarLeft == 3 ? 3 : 18;
                ship.SetPosition(x, 7, Direction.North);
            }
            else
            {
                Console.WriteLine("Starting on lower half!");
                var x = ship.LidarRight == 3 ? 3 : 18;
                ship.SetPosition(x, 5, Direction.South);
            }
        }

        private static void SetStartPositionOnLevel2(Ship ship)
        {
            if (ship.LidarRight == 5)
            {
                Console.WriteLine("Starting in top left corner!");
                ship.SetPosition(1, 10, Direction.East);
            }
            else if (ship.LidarLeft == 5)
            {
                Console.WriteLine("Starting in top right corner!");
                ship.SetPosition(20, 10, Direction.West);
            }
            else if (ship.LidarLeft == 4)
            {
                Console.WriteLine("Starting in bottom left corner!");
                ship.SetPosition(1, 1, Direction.East);
            }
            else if (ship.LidarRight == 4)
            {
                Console.WriteLine("Starting in bottom right corner!");
                ship.SetPosition(20, 1, Direction.West);
            }
        }

        private static List<string> Level1()
        {
            var map = new StringBuilder();
            map.AppendLine("######################");
            map.AppendLine("#   X   X    X   X   #");
            map.AppendLine("#          #         #");
            map.AppendLine("#          #         #");
            map.AppendLine("#          #         #");
            map.AppendLine("#  ################  #");
            map.AppendLine("#          #         #");
            map.AppendLine("#          #         #");
            map.AppendLine("#          #         #");
            map.AppendLine("#   X   X    X   X   #");
            map.AppendLine("#                    #");
            map.AppendLine("######################");

            return map.ToString()
                .Split(new []{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }

        private static List<string> Level2()
        {
            var map = new StringBuilder();
            map.AppendLine("######################");
            map.AppendLine("#        X#X         #");
            map.AppendLine("#   #    X#X     #   #");
            map.AppendLine("#   ##############   #");
            map.AppendLine("#                    #");
            map.AppendLine("#                    #");
            map.AppendLine("### X            X ###");
            map.AppendLine("#                    #");
            map.AppendLine("#   ##############   #");
            map.AppendLine("#   #    X#X     #   #");
            map.AppendLine("#        X#X         #");
            map.AppendLine("######################");

            return map.ToString()
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}