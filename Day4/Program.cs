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

        bool[][] rolls = new bool[lines.Length][];
        
        int xMax = lines[0].Length;
        int yMax = lines.Length;
        for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            rolls[lineIdx] = new bool[lines[lineIdx].Length];
            for (int x = 0; x < lines[lineIdx].Length; x++)
            {
                rolls[lineIdx][x] = lines[lineIdx][x] == '@';
            }
        }

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {

                if (rolls[y][x])
                {
                    int ContainsRoll(int x, int y)
                    {
                        if (x < 0 || y < 0 || x >= xMax || y >= yMax)
                        {
                            return 0;
                        }
                        return rolls[y][x] ? 1 : 0;
                    }
                    int count = 0;
                    count += ContainsRoll(x - 1, y - 1);
                    count += ContainsRoll(x - 1, y );
                    count += ContainsRoll(x - 1, y + 1);
                    count += ContainsRoll(x , y - 1);
                    count += ContainsRoll(x , y + 1);
                    count += ContainsRoll(x + 1, y - 1);
                    count += ContainsRoll(x + 1, y );
                    count += ContainsRoll(x + 1, y + 1);
                    if (count < 4)
                    {
                        result++;
                    }
                }
            }
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

        int result = 0;

        bool[][] rolls = new bool[lines.Length][];
        
        int xMax = lines[0].Length;
        int yMax = lines.Length;
        for (int lineIdx = 0; lineIdx < lines.Length; lineIdx++)
        {
            rolls[lineIdx] = new bool[lines[lineIdx].Length];
            for (int x = 0; x < lines[lineIdx].Length; x++)
            {
                rolls[lineIdx][x] = lines[lineIdx][x] == '@';
            }
        }

        int totalRemoved;
        do
        {
            totalRemoved = 0;
            HashSet<(int, int)> toRemove = new HashSet<(int, int)>();
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {

                    if (rolls[y][x])
                    {
                        int ContainsRoll(int x, int y)
                        {
                            if (x < 0 || y < 0 || x >= xMax || y >= yMax)
                            {
                                return 0;
                            }

                            return rolls[y][x] ? 1 : 0;
                        }

                        int count = 0;
                        count += ContainsRoll(x - 1, y - 1);
                        count += ContainsRoll(x - 1, y);
                        count += ContainsRoll(x - 1, y + 1);
                        count += ContainsRoll(x, y - 1);
                        count += ContainsRoll(x, y + 1);
                        count += ContainsRoll(x + 1, y - 1);
                        count += ContainsRoll(x + 1, y);
                        count += ContainsRoll(x + 1, y + 1);
                        if (count < 4)
                        {
                            toRemove.Add((x, y));
                            result++;
                        }
                    }
                }
            }
            foreach (var r in toRemove)
            {
                rolls[r.Item2][r.Item1] = false;
                totalRemoved++;
            }
        } while (totalRemoved > 0);


        System.Console.WriteLine($"Result {inputFile} is {result}");
    }
}