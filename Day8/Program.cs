using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var uniqueLengths = new[] { 2, 3, 4, 7 };

            var part1 = input.Select(x =>
                x.Split(new[] {'|', ' '}, StringSplitOptions.RemoveEmptyEntries)[10..]
                    .Count(x => uniqueLengths.Contains(x.Length))).Sum();
            Console.WriteLine($"Part 1: {part1}");
            
            Part2(input);

            Console.ReadLine();
        }

        private static void Part2(IEnumerable<string> entries)
        {
            long sum = 0;

            foreach (var line in entries)
            {
                Dictionary<int, List<char>> mapping = new Dictionary<int, List<char>>();
                var tmp = line.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var wires = tmp[..10].Select(x => x.OrderBy(c => c).ToList()).ToList();
                var outputs = tmp[10..].Select(x => x.OrderBy(c => c).ToList());


                 //* Section counts
                 //* 0:6
                 //* 1:2
                 //* 2:5
                 //* 3:5
                 //* 4:4
                 //* 5:5
                 //* 6:6
                 //* 7:3
                 //* 8:7
                 //* 9:6
                

                //Uniques
                mapping[1] = wires.Single(x => x.Count == 2);
                mapping[4] = wires.Single(x => x.Count == 4);
                mapping[7] = wires.Single(x => x.Count == 3);
                mapping[8] = wires.Single(x => x.Count == 7);

                mapping[9] = wires.Single(x => x.Count == 6 && x.Intersect(mapping[4]).Count() == 4);
                mapping[6] = wires.Single(x => x.Count == 6 && !mapping[1].All(x.Contains));
                mapping[0] = wires.Single(x => x.Count == 6 && !mapping.ContainsValue(x));

                mapping[5] = wires.Single(x => x.Count == 5 && x.Intersect(mapping[6]).Count() == 5);
                mapping[3] = wires.Single(x => x.Count == 5 && x.Intersect(mapping[7]).Count() == 3 && !mapping.ContainsValue(x));
                mapping[2] = wires.Single(x => x.Count == 5 && !mapping.ContainsValue(x));


                var ans = string.Empty;
                foreach (var output in outputs)
                {
                    var number = mapping.Single(x => x.Value.SequenceEqual(output)).Key;
                    ans += number;
                }

                sum += long.Parse(ans);
            }

            Console.WriteLine($"Part 2: {sum}");
        }
    }
}
