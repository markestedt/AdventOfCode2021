using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = (await File.ReadAllLinesAsync("input.txt")).Select(int.Parse).ToArray();
            Part1(input);

            Part2(input);
            Console.ReadLine();
        }

        private static void Part2(IReadOnlyList<int> input)
        {
            var queue = new Queue<int>();
            queue.Enqueue(input[0]);
            queue.Enqueue(input[1]);
            queue.Enqueue(input[2]);

            var previousDepthSum = queue.Sum();
            var increased = 0;

            foreach (var depth in input.Skip(3))
            {
                queue.Dequeue();
                queue.Enqueue(depth);

                var depthSum = queue.Sum();
                if (depthSum > previousDepthSum)
                {
                    increased++;
                }

                previousDepthSum = depthSum;
            }
            Console.WriteLine($"Answer Part 2: {increased}");
        }

        private static void Part1(IEnumerable<int> input)
        {
            int? previousDepth = null;
            var increased = 0;

            foreach (var depth in input)
            {
                if (depth > previousDepth)
                {
                    increased++;
                }

                previousDepth = depth;
            }

            Console.WriteLine($"Answer Part 1: {increased}");
        }
    }
}
