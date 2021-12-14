using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var map = new Dictionary<string, List<string>>();
            foreach (var line in input)
            {
                var values = line.Split('-');

                if (map.ContainsKey(values[0]))
                {
                    map[values[0]].Add(values[1]);
                }
                else
                {
                    map.Add(values[0], new List<string> { values[1] });
                }

                if (map.ContainsKey(values[1]))
                {
                    map[values[1]].Add(values[0]);
                }
                else
                {
                    map.Add(values[1], new List<string> { values[0] });
                }
            }

            var paths1 = BuildPaths(map, new List<string> { "start" }, true);
            var paths2 = BuildPaths(map, new List<string> { "start" }, false);
        }

        private static IEnumerable<List<string>> BuildPaths(IReadOnlyDictionary<string, List<string>> map, List<string> currentPath, bool isPart1)
        {
            var lastStep = currentPath.Last();
            if (lastStep == "end")
            {
                return new List<List<string>> { currentPath };
            }

            var possibleSteps = map[lastStep].Where(x => currentPath.IsValidStep(x, isPart1)).ToList();

            if (!possibleSteps.Any())
            {
                return new List<List<string>>();
            }


            // First add each possible step to current path
            // Then build paths upon those.
            return possibleSteps
                .Select(step => new List<List<string>> { currentPath, new List<string> { step } }.SelectMany(x => x).ToList())
                .SelectMany(x => BuildPaths(map, x, isPart1)).ToList();
        }

    }

    public static class Extensions
    {
        public static bool IsValidStep(this IEnumerable<string> path, string step, bool isPart1)
        {
            if (step.All(char.IsUpper))
            {
                return true;
            }

            if (isPart1)
            {
                return !path.Contains(step);
            }

            if (step == "start")
            {
                return false;
            }

            return !path.Contains(step) || path.Where(IsLower).Count() == path.Where(IsLower).Distinct().Count();
        }


        public static bool IsLower(this string val)
        {
            return val.All(char.IsLower);
        }
    }
}
