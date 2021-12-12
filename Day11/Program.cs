using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var octopuses = new Dictionary<(int, int), int>();

            var maxCol = 0;
            var rowCount = 0;
            foreach (var row in input)
            {
                var colCount = 0;
                foreach (var o in row)
                {
                    octopuses.Add((rowCount, colCount), o.ToInt());
                    colCount++;
                }
                rowCount++;
                maxCol = colCount - 1;
            }

            var maxRow = rowCount - 1;
            Part1(octopuses, maxRow, maxCol);
            Part2(octopuses, maxRow, maxCol);

            Console.ReadLine();
        }

        private static void Part2(Dictionary<(int, int), int> octopuses, int maxRow, int maxCol)
        {
            var lastStep = 0;
            var stepFlashCount = 0;


            while(stepFlashCount < octopuses.Count)
            {
                lastStep++;
                var hasFlashed = new HashSet<(int, int)>();

                // Increase energy level
                octopuses = octopuses.ToDictionary(o => o.Key, o => o.Value + 1);

                // If greater than 9, flash. This raises adjacent with 1. If any adjacent greater than 9, then flash and so on. Can only flash once per step.
                var flashing = octopuses.Where(x => x.Value > 9).ToDictionary(x => x.Key, x => x.Value);
                hasFlashed.UnionWith(flashing.Select(x => x.Key));

                foreach (var octo in flashing)
                {
                    Flash(octopuses, octo.Key, maxRow, maxCol, hasFlashed);
                }
                // Any that flashed, resets to 0.
                foreach (var point in hasFlashed)
                {
                    octopuses[point] = 0;
                }

                stepFlashCount = hasFlashed.Count();
            }

            Console.WriteLine($"Part 2: {lastStep}");
        }

        private static void Part1(Dictionary<(int, int), int> octopuses, int maxRow, int maxCol)
        {
            var steps = 100;
            var totalFlashCount = 0;


            for (int i = 0; i < steps; i++)
            {
                var hasFlashed = new HashSet<(int, int)>();

                // Increase energy level
                octopuses = octopuses.ToDictionary(o => o.Key, o => o.Value + 1);

                // If greater than 9, flash. This raises adjacent with 1. If any adjacent greater than 9, then flash and so on. Can only flash once per step.
                var flashing = octopuses.Where(x => x.Value > 9).ToDictionary(x => x.Key, x => x.Value);
                hasFlashed.UnionWith(flashing.Select(x => x.Key));

                foreach (var octo in flashing)
                {
                    Flash(octopuses, octo.Key, maxRow, maxCol, hasFlashed);
                }
                // Any that flashed, resets to 0.
                foreach (var point in hasFlashed)
                {
                    octopuses[point] = 0;
                }

                totalFlashCount += hasFlashed.Count();
            }

            Console.WriteLine($"Part 1: {totalFlashCount}");
        }

        private static void Flash(Dictionary<(int, int), int> allOctos, (int, int) flashingOctoPoint, int maxRow, int maxCol, HashSet<(int, int)> hasFlashed)
        {
            var adjacent = GetAdjacentPoints(flashingOctoPoint, maxRow, maxCol);

            foreach (var point in adjacent)
            {
                allOctos[point] = allOctos[point] + 1;
                if (allOctos[point] > 9 && !hasFlashed.Contains(point))
                {
                    hasFlashed.Add(point);
                    Flash(allOctos, point, maxRow, maxCol, hasFlashed);
                }
            }

        }


        private static HashSet<(int, int)> GetAdjacentPoints((int, int) sourcePoint, int maxRow, int maxCol)
        {
            var adjacent = new HashSet<(int, int)>
            {
                (sourcePoint.Item1 -1, sourcePoint.Item2 -1),
                (sourcePoint.Item1 -1, sourcePoint.Item2),
                (sourcePoint.Item1 -1, sourcePoint.Item2 +1),
                (sourcePoint.Item1, sourcePoint.Item2 +1),
                (sourcePoint.Item1 +1, sourcePoint.Item2 +1),
                (sourcePoint.Item1 +1, sourcePoint.Item2),
                (sourcePoint.Item1+1, sourcePoint.Item2 -1),
                (sourcePoint.Item1, sourcePoint.Item2 -1),
            };

            adjacent.RemoveWhere(x => x.Item1 < 0 || x.Item2 < 0 || x.Item1 > maxRow || x.Item2 > maxCol);
            return adjacent;

        }
    }

    public static class Extensions
    {
        public static int ToInt(this char c)
        {

            return (int)c - '0';
        }
    }
}
