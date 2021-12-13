using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = await File.ReadAllLinesAsync("input.txt");

            Part1(input);
            Part2(input);

            Console.ReadLine();
        }

        private static void Part2(string[] input)
        {
            var oxygenArray = input;
            var co2Array = input;

            for (var i = 0; i < input[0].Length; i++)
            {
                if (co2Array.Length > 1)
                {

                    var co2Temp = co2Array.GetBitCounts(i, "desc").ToArray();
                    var co2Bit = (co2Temp.First().Bit == '1' || co2Temp.First().Count == co2Temp.Last().Count) ? '1' : '0';

                    co2Array = co2Array.Where(x => x[i] == co2Bit).ToArray();
                }

                if (oxygenArray.Length > 1)
                {
                    var oxygenTemp = oxygenArray.GetBitCounts(i).ToArray();
                    var oxygenBit = (oxygenTemp.First().Bit == '0' || oxygenTemp.First().Count == oxygenTemp.Last().Count) ? '0' : '1';

                    oxygenArray = oxygenArray.Where(x => x[i] == oxygenBit).ToArray();
                }

                if (co2Array.Length == 1 && oxygenArray.Length == 1)
                {
                    break;
                }
            }

            var oxygen = Convert.ToInt32(oxygenArray.First(), 2);
            var co2 = Convert.ToInt32(co2Array.First(), 2);

            Console.WriteLine($"Part2: {oxygen * co2}");
        }

        private static void Part1(string[] input)
        {
            var gammaAnswer = string.Empty;
            var epsilonAnswer = string.Empty;

            for (var i = 0; i < input[0].Length; i++)
            {
                var bitCounts = input.GetBitCounts(i, "desc").ToArray();
                
                gammaAnswer += bitCounts.First().Bit;
                epsilonAnswer += bitCounts.Last().Bit;
            }

            var gamma = Convert.ToInt32(gammaAnswer, 2);
            var epsilon = Convert.ToInt32(epsilonAnswer, 2);

            Console.WriteLine($"Part1: {gamma * epsilon}");
        }
    }

    public static class Extensions
    {
        public static IEnumerable<(char Bit, int Count)> GetBitCounts(this string[] arr, int position, string order = "asc")
        {
            var temp = arr.Select(x => x[position]).GroupBy(x => x).Select(x => (Bit: x.Key, Count: x.Count()));
            temp = order == "desc" ? temp.OrderByDescending(grp => grp.Count) : temp.OrderBy(grp => grp.Count);

            return temp;
        }
    }
}
