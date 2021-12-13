using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = (await File.ReadAllLinesAsync("input.txt")).Select(x => new Instruction(x)).ToArray();

            Part1(input);
            Part2(input);

            Console.ReadLine();
        }

        private static void Part1(IEnumerable<Instruction> input)
        {
            var depth = 0;
            var horizontal = 0;

            foreach (var instruction in input)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        horizontal += instruction.Distance;
                        break;
                    case "down":
                        depth += instruction.Distance;
                        break;
                    case "up":
                        depth -= instruction.Distance;
                        break;
                }
            }

            Console.WriteLine($"Answer Part1: {depth * horizontal}");
        }

        private static void Part2(IEnumerable<Instruction> input)
        {
            var depth = 0;
            var horizontal = 0;
            var aim = 0;

            foreach (var instruction in input)
            {
                switch (instruction.Direction)
                {
                    case "forward":
                        horizontal += instruction.Distance;
                        depth += (aim * instruction.Distance);
                        break;
                    case "down":
                        aim += instruction.Distance;
                        break;
                    case "up":
                        aim -= instruction.Distance;
                        break;
                }
            }

            Console.WriteLine($"Answer Part2: {depth * horizontal}");
        }
    }

    internal class Instruction
    {
        public Instruction(string input)
        {
            var temp = input.Split(" ");
            Direction = temp[0];
            Distance = int.Parse(temp[1]);
        }
        public string Direction { get; set; }
        public int Distance { get; set; }
    }
}
