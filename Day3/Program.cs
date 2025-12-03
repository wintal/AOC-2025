using System.Text.RegularExpressions;

namespace Day3;

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

        int result = 0;

        foreach (var line in lines)
        {
            int bestFirst = 0;
            int bestIndex = 0;
            for (int i = 0; i < line.Length - 1; i++)
            {
                int thisI = line[i] - '0';
                if (thisI > bestFirst)
                {
                    bestFirst = thisI;
                    bestIndex = i;
                }

                if (bestFirst == 9)
                {
                    break;
                }
            }

            int bestSecond = 0;
            for (int i = bestIndex + 1; i < line.Length; i++)
            {
                int thisI = line[i] - '0';
                if (thisI > bestSecond)
                {
                    bestSecond = thisI;
                }

                if (bestSecond == 9)
                {
                    break;
                }
            }

            result = result + bestFirst * 10 + bestSecond;
        }

        System.Console.WriteLine($"Result {inputFile} is {result}");
    }

    static void RunPart2(string inputFile)
    {
        var lines = System.IO.File.ReadAllLines(inputFile);
        if (!lines.Any())
        {
            return;
        }

        long result = 0;

        foreach (var line in lines)
        {
            var best = new int[12];

            int lastIndex = 0;
            for (int digit = 0; digit < 12; digit++)
            {
                int bestDigit = 0;
                for (int i = lastIndex; i < line.Length - (11 - digit); i++)
                {
                    int thisI = line[i] - '0';
                    if (thisI > bestDigit)
                    {
                        bestDigit = thisI;
                        lastIndex = i + 1;
                        best[digit] = thisI;
                    }

                    if (bestDigit == 9)
                    {
                        break;
                    }
                }
            }

            long total = 0;
            for (int digit = 0; digit < 12; digit++)
            {
                total = total * 10 + best[digit];
            }

            result = result + total;
            
        }

        System.Console.WriteLine($"Result {inputFile} is {result}");
    }
}