using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = await File.ReadAllLinesAsync("input.txt");
            var points = new Dictionary<(int Row, int Col), int>();

            var rowNumber = 0;
            foreach (var row in input)
            {
                var colNumber = 0;
                foreach (var number in row.ToList())
                {
                    points.Add((rowNumber, colNumber), number.ToInt());
                    colNumber++;
                }

                rowNumber++;
            }

            var maxRow = points.Keys.Max(p => p.Row);
            var maxCol = points.Keys.Max(p => p.Col);

            var lowPoints = points.Where(point => point.GetAdjacentPoints(points, maxCol, maxRow).IsLowPoint(point)).ToList();
            var answerPart1 = lowPoints.Select(point => point.Value + 1).Sum();

            var basinCounts = lowPoints.Select(point => GetValidAdjacentPointsCount(point, points, maxRow, maxCol)).ToList();
            var answerPart2 = basinCounts.OrderByDescending(count => count).Take(3).Aggregate((a, b) => a * b);

            Console.ReadLine();
        }


        private static int GetValidAdjacentPointsCount(KeyValuePair<(int Row, int Col), int> sourcePoint, Dictionary<(int Row, int Col), int> allPoints, int maxRow, int maxCol)
        {
            var validPoints = 1;

            var adjacentPoints = sourcePoint
                .GetAdjacentPoints(allPoints, maxCol, maxRow)
                .Where(x => x.Value != 9)
                .ToList();

            allPoints.Remove(sourcePoint.Key);

            if (!adjacentPoints.Any()) return validPoints;
            foreach (var point in adjacentPoints.Where(adjacentPoint => allPoints.ContainsKey(adjacentPoint.Key)))
            {
                validPoints += GetValidAdjacentPointsCount(point, allPoints, maxRow, maxCol);
            }

            return validPoints;
        }
    }

    public static class Extensions
    {
        private enum Directions
        {
            Up, Down, Right, Left
        }

        private static (int, int) GetAdjacentKey((int Row, int Col) source, Directions direction)
        {
            switch (direction)
            {
                case Directions.Up:
                    return (source.Row - 1, source.Col);
                case Directions.Down:
                    return (source.Row + 1, source.Col);
                case Directions.Right:
                    return (source.Row, source.Col + 1);
                case Directions.Left:
                    return (source.Row, source.Col - 1);
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        private static void AddIfExistsIn(this Dictionary<(int Row, int Col), int> dic, Dictionary<(int Row, int Col), int> source,
            (int Row, int Col) key)
        {
            if (source.TryGetValue(key, out var val))
            {
                dic.Add(key, val);
            }
        }

        public static Dictionary<(int Row, int Col), int> GetAdjacentPoints(
            this KeyValuePair<(int Row, int Col), int> point, Dictionary<(int Row, int Col), int> allPoints, int maxCol,
            int maxRow)
        {
            var adjacentPoints = new Dictionary<(int, int), int>();
            var directionsToAdd = new List<Directions>();

            if (point.Key.Row > 0)
            {
                directionsToAdd.Add(Directions.Up);
            }

            if (point.Key.Row < maxRow)
            {
                directionsToAdd.Add(Directions.Down);
            }

            if (point.Key.Col > 0)
            {
                directionsToAdd.Add(Directions.Left);
            }

            if (point.Key.Col < maxCol)
            {
                directionsToAdd.Add(Directions.Right);
            }

            foreach (var direction in directionsToAdd)
            {
                var key = GetAdjacentKey(point.Key, direction);
                adjacentPoints.AddIfExistsIn(allPoints, key);
            }

            return adjacentPoints;
        }

        public static bool IsLowPoint(this Dictionary<(int Row, int Col), int> d, KeyValuePair<(int Row, int Col),
            int> point)
        {
            if (point.Value == 9)
            {
                return false;
            }

            return d.All(ap => ap.Value > point.Value);
        }

        public static int ToInt(this char c)
        {
            return (int)(c - '0');
        }
    }
}
