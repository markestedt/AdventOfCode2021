using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = (await File.ReadAllLinesAsync("input.txt"))[0].Split(',').Select(int.Parse).ToArray();

            var part1 = AgeFish(input, 80);
            var part2 = AgeFish(input, 256);

            Console.WriteLine($"Part1: {part1}");
            Console.WriteLine($"Part2: {part2}");

            Console.ReadLine();
        }

        private static long AgeFish(IEnumerable<int> input, int days)
        {
            var fishCount = new Dictionary<int, long>
            {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0},
                {4, 0},
                {5, 0},
                {6, 0},
                {7, 0},
                {8, 0}
            };

            input.GroupBy(x => x).ToList().ForEach(x => fishCount[x.Key] = x.Count());

            for (var i = 0; i < days; i++)
            {
                long? tempVal = null;
                foreach (var key in fishCount.Keys.OrderByDescending(x => x))
                {
                    var count = tempVal ?? fishCount[key];
                    var newKey = key == 0 ? 6 : key - 1;

                    if (key == 0)
                    {
                        fishCount[8] = count;
                        fishCount[6] += count;
                    }
                    else
                    {
                        tempVal = fishCount[newKey];
                        fishCount[newKey] = count;
                    }
                }
            }

            return fishCount.Values.Sum();
        }
    }
}


