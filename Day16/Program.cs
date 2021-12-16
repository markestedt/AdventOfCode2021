using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day16
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt")[0];

            var binaryString = input.Aggregate(string.Empty,
                (current, c) => current + Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'));


            var test = ReadPackage(binaryString, 0);


        }

        private static (long, int, long) ProcessLiteralValues(string package, long versionSum)
        {
            var packageContent = package[6..];
            var groupNumber = 0;

            var keepProcessing = true;
            var actualValue = new StringBuilder();

            while (keepProcessing)
            {
                keepProcessing = packageContent[groupNumber] == '1';
                actualValue.Append(packageContent[(groupNumber + 1)..(groupNumber + 5)]);
                groupNumber += 5;
            }

            return (Convert.ToInt64(actualValue.ToString(), 2), groupNumber + 6, versionSum);
        }

        private static (long, int, long) ReadPackage(string package, long versionSum)
        {
            var version = Convert.ToInt64(package[..3], 2);
            var typeId = Convert.ToInt64(package.Substring(3, 3), 2);

            versionSum += version;

            if (typeId == 4)
            {
                return ProcessLiteralValues(package, versionSum);
            }

            var values = new List<long>();
            var lengthTypeId = Convert.ToInt32(package[6].ToString(), 2);

            var bitSize = lengthTypeId == 0 ? 15 : 11;
            var processedBitsCount = 0;

            const int packageContentStartIndex = 7;

            if (lengthTypeId == 0)
            {
                var bitsToProcess = Convert.ToInt32(package.Substring(packageContentStartIndex, bitSize), 2);

                while (bitsToProcess != processedBitsCount)
                {
                    var (value, bitsUsed, subVersionSum) = ReadPackage(
                        package.Substring(packageContentStartIndex + bitSize + processedBitsCount, bitsToProcess - processedBitsCount),
                        versionSum);

                    processedBitsCount += bitsUsed;
                    values.Add(value);
                    versionSum = subVersionSum;
                }
            }
            else
            {
                var numberOfSubpackets = Convert.ToInt32(package.Substring(packageContentStartIndex, bitSize), 2);

                for (var i = 0; i < numberOfSubpackets; i++)
                {
                    var (value, bitsUsed, subVersionSum) = ReadPackage(package[(packageContentStartIndex + bitSize + processedBitsCount)..], versionSum);

                    processedBitsCount += bitsUsed;
                    values.Add(value);
                    versionSum = subVersionSum;
                }
            }
            var totalBitsUsed = processedBitsCount + bitSize + 7;

            Func<List<long>, long> resultFunc = typeId switch
            {
                0 => val => val.Sum(),
                1 => val => val.Aggregate((x, y) => x * y),
                2 => val => val.Min(),
                3 => val => val.Max(),
                5 => val => val[0] > val[1] ? 1 : 0,
                6 => val => val[0] < val[1] ? 1 : 0,
                7 => val => val[0] == val[1] ? 1 : 0,
                _ => throw new Exception()
            };

            return (resultFunc(values), totalBitsUsed, versionSum);
        }
    }
}
