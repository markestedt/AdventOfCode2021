using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("testinput.txt");

            var template = input[0];
            var insertionRules = input.Skip(2).ToDictionary(x => x.Split("->")[0].Trim(), x => x.Split("->")[1].Trim());

            Part1(template, insertionRules, 10);
            Part2(template, insertionRules, 40);

            Console.ReadLine();
        }

        private static void Part2(string template, Dictionary<string, string> rules, int steps)
        {
            var pairCount = new Dictionary<string, long>();
            var letterCounts = new Dictionary<string, long>();

            foreach (var pair in template.ToPairs().ToList())
            {
                pairCount[pair] = pairCount.GetValueOrDefault(pair, 0) + 1;
            }

            foreach (var element in template.Select(c => c.ToString()))
            {
                letterCounts[element] = letterCounts.GetValueOrDefault(element, 0) + 1;
            }

            for (var i = 0; i < steps; i++)
            {
                var newPairCount = new Dictionary<string, long>();
                foreach (var pair in pairCount.Keys)
                {
                    var currentCount = pairCount[pair];
                    var added = rules[pair];

                    var newPairs = new List<string> { pair[0] + added, added + pair[1] };
                    newPairs.ForEach(x => newPairCount[x] = newPairCount.GetValueOrDefault(x, 0) + currentCount);

                    letterCounts[added] = letterCounts.GetValueOrDefault(added, 0) + currentCount;
                }

                pairCount = newPairCount.ToDictionary(x => x.Key, x => x.Value);
            }

            Console.WriteLine($"Part 2: {letterCounts.Max(x => x.Value) - letterCounts.Min(x => x.Value)}");
        }

        private static void Part1(string template, Dictionary<string, string> rules, int steps)
        {
            for (var i = 0; i < steps; i++)
            {
                var newTemplate = template[0].ToString();
                for (var j = 1; j < template.Length; j++)
                {
                    var pair = template.Substring(j - 1, 2);

                    newTemplate += rules[pair]; 
                    newTemplate += template[j];
                }

                template = newTemplate;
            }

            var elements = template.GroupBy(x => x).ToList();
            Console.WriteLine($"Part 1: {elements.Max(x => x.Count()) - elements.Min(x => x.Count())}");
        }
    }

    public static class Extensions
        {
            public static IEnumerable<string> ToPairs(this string s)
            {
                for (var i = 0; i < s.Length; i++)
                {
                    if (s.Length - i >= 2)
                    {
                        yield return s.Substring(i, 2);
                    }
                }
            }
        }
    }

