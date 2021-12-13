using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("input.txt").Split(Environment.NewLine + Environment.NewLine);

            var points = input[0].Split(Environment.NewLine)
                .Select(x => (int.Parse(x.Split(',')[0]), int.Parse(x.Split(',')[1]))).ToList();

            var instructions = input[1].Split(Environment.NewLine).Select(x => x.Replace("fold along ", string.Empty))
                .Select(x => (x.Split('=')[0], int.Parse(x.Split('=')[1])));

            var maxCol = points.Max(x => x.Item1);
            var maxRow = points.Max(x => x.Item2);

            var grid = new Dictionary<(int X, int Y), bool>();

            for (var i = 0; i <= maxRow; i++)
            {
                for (var j = 0; j <= maxCol; j++)
                {
                    var point = (j, i);
                    grid.Add(point, false);
                }
            }

            foreach (var point in points)
            {
                grid[point] = true;
            }

            var foldNr = 0;
            foreach (var (axis, line) in instructions)
            {
                foldNr++;

                var pointsToFold = (axis == "y")
                    ? grid.Where(x => x.Key.Y > line)
                    : grid.Where(x => x.Key.X > line);

                var edge = (axis == "y") ? grid.Max(x => x.Key.Y) : grid.Max(x => x.Key.X);

                var newPoints = pointsToFold.Select(x => GetInversePoint(x, axis, edge))
                    .ToDictionary(x => x.Key, x => x.Value);

                foreach (var point in newPoints.Where(point => point.Value))
                {
                    grid[point.Key] = point.Value;
                }

                grid = (axis == "y")
                    ? grid.Where(x => x.Key.Y < line).ToDictionary(x => x.Key, x => x.Value)
                    : grid.Where(x => x.Key.X < line).ToDictionary(x => x.Key, x => x.Value);
               
                var visible = grid.Count(x => x.Value);
                Console.WriteLine($"Visible after fold {foldNr}: {visible}");
            }

            foreach (var (point, value) in grid.OrderBy(x => x.Key.Y))
            {
                if (point.X == 0)
                {
                    Console.WriteLine();
                }

                Console.Write(value ? "#" : ".");
            }

            Console.Read();
        }

        private static KeyValuePair<(int, int), bool> GetInversePoint(KeyValuePair<(int, int), bool> point, string axis, int edge)
        {
            if (axis == "y")
            {
                var distanceFromEdge = edge - point.Key.Item2;
                return new KeyValuePair<(int, int), bool>((point.Key.Item1, distanceFromEdge), point.Value);
            }
            else
            {
                var distanceFromEdge = edge - point.Key.Item1;
                return new KeyValuePair<(int, int), bool>((distanceFromEdge, point.Key.Item2), point.Value);
            }
        }
    }
}
