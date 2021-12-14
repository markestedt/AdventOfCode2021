using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0].Split(',').Select(int.Parse).ToArray();
            
            Part1(input);
            Part2(input);

            Console.ReadLine();
        }

        public static void Part2(int[] crabs)
        {
            int? fuelConsumedPart2 = null;
            var max = crabs.Max();
            
            for (var i = 0; i <= max; i++)
            {
                var positionCost = crabs.SelectMany(crab => Enumerable.Range(1, Math.Abs(crab - i))).Sum();

                if (!fuelConsumedPart2.HasValue || positionCost < fuelConsumedPart2)
                {
                    fuelConsumedPart2 = positionCost;
                }
                else if (positionCost > fuelConsumedPart2)
                {
                    break;
                }
            }

            Console.WriteLine($"Part2: {fuelConsumedPart2}");
        }

        public static void Part1(int[] crabs)
        {
            var median = GetMedian(crabs);
            var fuelConsumedPart1 = crabs.Sum(crab => Math.Abs(crab - median));

            Console.WriteLine($"Part1: {fuelConsumedPart1}");
        }

        public static int GetMedian(int[] array)
        {
            int medianValue;

            int[] tempArray = array;
            int count = tempArray.Length;

            Array.Sort(tempArray);

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
