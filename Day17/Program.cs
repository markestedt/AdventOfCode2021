using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Schema;

namespace Day17
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0];

            input = input.Replace("target area: ", string.Empty);
            var areas = input.Split(',').Select(x => x.Trim()[2..])
                .Select(x => (int.Parse(x.Split("..")[0]), int.Parse(x.Split("..")[1]))).ToArray();

            var (xMin, xMax) = areas[0];
            var (yMin, yMax) = areas[1];

            var targetArea = new HashSet<(int, int)>();

            for (var x = xMin; x <= xMax; x++)
            {
                for (var y = yMin; y <= yMax; y++)
                {
                    targetArea.Add((x, y));
                }
            }

            var result = Solve(targetArea);

            Console.WriteLine($"Part1: {result.Item1}");
            Console.WriteLine($"Part2: {result.Item2}");
            Console.Read();
        }

        private static (int,int) Solve(HashSet<(int, int)> targetArea)
        {
            var hits = 0;
            var yMax = 0;
            var boundaries = (targetArea.Max(x => x.Item1), targetArea.Min(x => x.Item2));

            // x min 0 beacause we need to move forward. x max boundary +1, if larger we overshoot on first step.
            for (var xVelocity = 1; xVelocity < boundaries.Item1 + 1; xVelocity++)
            {
                // y min lower boundary, if starting lower we will under shoot. Abs of y min, beacause other wise it'll blow past target area on way down.
                for (var yVelocity = boundaries.Item2; yVelocity < Math.Abs(boundaries.Item2); yVelocity++)
                {
                    var velocity = (xVelocity, yVelocity);
                    var (hit, y) = FireShot(targetArea, velocity, boundaries);

                    if (hit)
                    {
                        yMax = Math.Max(yMax, y);
                        hits++;
                    }
                }
            }

            return (yMax,hits);
        }

        private static (bool, int) FireShot(HashSet<(int, int)> targetArea, (int, int) velocity, (int,int) boundaries)
        {
            var currentPosition = (0, 0);
            var highestY = 0;
            var hitTarget = false;

            while (true)
            {
                currentPosition.Item1 += velocity.Item1;
                currentPosition.Item2 += velocity.Item2;

                highestY = Math.Max(highestY, currentPosition.Item2);

                velocity.Item1 = (velocity.Item1 > 0) ? velocity.Item1 - 1 :
                    (velocity.Item1 < 0) ? velocity.Item1 + 1 : velocity.Item1;
                velocity.Item2--;

                if (currentPosition.Item1 > boundaries.Item1 || currentPosition.Item2 < boundaries.Item2)
                {
                    break;
                }

                if (targetArea.Contains(currentPosition))
                {
                    hitTarget = true;
                    break;
                }
            }
            return (hitTarget, highestY);
        }
    }
}
