using System;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0].Split(',').Select(int.Parse).ToArray();
            var median = GetMedian(input);
            var mode = GetMode(input);

            var min = input.Min();
            var max = input.Max();

            long Part1() => Enumerable.Range(min, max - min + 1).Min(i => input.Select(x => Math.Abs(x - i)).Sum());
            long Part2() => Enumerable.Range(min, max - min + 1).Min(i => input.Select(x => Math.Abs(x - i)).Select(x => x * (x + 1) / 2).Sum());

            Console.WriteLine($"Part1: {Part1()}");
            Console.WriteLine($"Part2: {Part2()}");
            Console.ReadLine();
        }

        public static int GetMode(int[] input)
        {
            return input.GroupBy(x => x).OrderByDescending(x => x.Count()).ThenBy(x => x.Key).Select(x => x.Key).First();
        }

        public static decimal GetMedian(int[] array)
        {
            int[] tempArray = array;
            int count = tempArray.Length;

            Array.Sort(tempArray);

            decimal medianValue = 0;

            if (count % 2 == 0)
            {
                // count is even, need to get the middle two elements, add them together, then divide by 2
                int middleElement1 = tempArray[(count / 2) - 1];
                int middleElement2 = tempArray[(count / 2)];
                medianValue = (middleElement1 + middleElement2) / 2;
            }
            else
            {
                // count is odd, simply get the middle element.
                medianValue = tempArray[(count / 2)];
            }

            return medianValue;
        }
    }
}
