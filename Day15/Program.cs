using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var map = new Dictionary<(int, int), int>();

            var maxX = input[0].Length;
            var maxY = input.Length;

            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    map.Add((x, y), int.Parse(input[y][x].ToString()));
                }
            }

            var part1 = GetRisk(map);
            var part2 = GetRisk(ScaleMap(map));

        }

        // TODO FIX
        private static Dictionary<(int, int), int> ScaleMap(Dictionary<(int, int), int> map)
        {
            var maxX = map.Keys.Max(x => x.Item1) +1;
            var maxY = map.Keys.Max(x => x.Item2) +1;

            var xValues = Enumerable.Range(0, maxX * 5);
            var yValues = Enumerable.Range(0, maxY * 5);

            var newMap = new Dictionary<(int, int), int>();
            foreach (var xVal in xValues)
            {
                foreach (var yVal in yValues)
                {
                    var origX = xVal % maxX;
                    var origY = yVal % maxY;

                    var origRisk = map[(origX, origY)];
                    var distance = (yVal / maxY) + (xVal / maxX);

                    var riskLevel = (origRisk + distance - 1) % 9 + 1;

                    newMap.Add((xVal, yVal), riskLevel);
                }
            }

            return newMap;
        }

        private static int GetRisk(Dictionary<(int, int), int> map)
        {
            var startingPoint = (0, 0);
            var endpoint = (map.Keys.Max(x => x.Item1), map.Keys.Max(x => x.Item2));

            var pointQueue = new Dictionary<(int, int), int> { { startingPoint, 0 } };
            var finalMap = new Dictionary<(int, int), int> { { startingPoint, 0 } };

            while (pointQueue.Count > 0)
            {
                var point = pointQueue.FirstOrDefault(x => x.Value == pointQueue.Min(x => x.Value)).Key;
                pointQueue.Remove(point);

                foreach (var adjPoint in GetAdjacentPoints(point))
                {
                    if (map.ContainsKey(adjPoint) && !finalMap.ContainsKey(adjPoint))
                    {
                        var sumRisk = finalMap[point] + map[adjPoint];
                        finalMap[adjPoint] = sumRisk;

                        if (adjPoint == endpoint)
                        {
                            break;
                        }

                        pointQueue.Add(adjPoint, sumRisk);
                    }
                }

            }

            return finalMap[endpoint];
        }

        private static HashSet<(int, int)> GetAdjacentPoints((int, int) sourcePoint)
        {
            var adjacent = new HashSet<(int, int)>
            {
                (sourcePoint.Item1 -1, sourcePoint.Item2),
                (sourcePoint.Item1, sourcePoint.Item2 +1),
                (sourcePoint.Item1 +1, sourcePoint.Item2),
                (sourcePoint.Item1, sourcePoint.Item2 -1),
            };

            return adjacent;
        }
    }
}
