using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Day2;


class Program
{
    private const string Sample = "sample.txt";
    private const string Input = "input.txt";
    static void Main(string[] args)
    {
        RunPart1(Sample);
        RunPart1(Input);
        RunPart2(Sample);
        RunPart2(Input);
    }

    static void RunPart1(string inputFile)
    {
        var lines = System.IO.File.ReadAllLines(inputFile);
        if (!lines.Any())
        {
            return;
        }

        long total = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(',').Select(s => s.Trim()).Where(s => !String.IsNullOrEmpty(s));
            foreach (var part in parts)
            {
                var ranges = part.Split('-');
                long min = long.Parse(ranges[0]);
                long max = long.Parse(ranges[1]);
                for (long i = min; i <= max; i++)
                {
                    var str = i.ToString();
                    var minBegin = str.Substring(0, str.Length / 2);
                    var minEnd = str.Substring(str.Length / 2);
                    if (minBegin == minEnd)
                    {
                        total += i;
                    }
                }
            }
        }
        System.Console.WriteLine($"Result {inputFile} is {total}");
    }

    static void RunPart2(string inputFile)
    {
        var lines = System.IO.File.ReadAllLines(inputFile);
        if (!lines.Any())
        {
            return;
        }

        long total = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(',').Select(s => s.Trim()).Where(s => !String.IsNullOrEmpty(s));
            foreach (var part in parts)
            {
                var ranges = part.Split('-');
                long min = long.Parse(ranges[0]);
                long max = long.Parse(ranges[1]);
                for (long i = min; i <= max; i++)
                {
                    var str = i.ToString();
                    for (int length = 1; length <= str.Length / 2; length++)
                    {
                        
                        if (str.Length % length == 0)
                        {
                            var first = str.Substring(0, length);
                            int index = length;
                            var  broken = false;
                            while (index < str.Length)
                            {
                                var second = str.Substring(index, length);
                                if (first != second)
                                {
                                    broken = true;
                                    break;
                                }

                                index += length;
                            }
                            if (!broken)
                            {
                                total += i;
                                break;
                            }
                        }
                    }
                }
            }
        }
        System.Console.WriteLine($"Result {inputFile} is {total}");
    }
}