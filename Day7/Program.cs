using Utilities;

namespace Day7;


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
        long result = 0;

        var lines = System.IO.File.ReadAllLines(inputFile);

        var map = Utilities.Map<char>.LoadFromLines(lines, (c) => c);
        var start = map.FindEntry('S');

        var downVectors = new HashSet<Vector>();
        downVectors.Add(start);
        int y = 0;
        int split = 0;
        while (y < map.MaxY)
        {
            HashSet<Vector> newVec = new();
            foreach (var downVector in downVectors)
            {
                var vec = new Vector(downVector.X, y);
                if (map[vec] == '^')
                {
                    split++;
                    if (vec.X > 0)
                    {
                        newVec.Add(new Vector(vec.X - 1, vec.Y));
                    }

                    if (vec.X < map.MaxX)
                    {
                        newVec.Add(new Vector(vec.X + 1, vec.Y));
                    }
                }
                else
                {
                    newVec.Add(vec);
                }
            }

            downVectors = newVec;
            y++;
        }
        
        System.Console.WriteLine($"Result {inputFile} is {split}");
    }


    private static int FindSolutions(long equationItem1, long current, long[] ints)
    {
        if (current > equationItem1)
        {
            return 0;
        }
        if (ints.Length == 0)
        {
            if (equationItem1 == current)
            {
                return 1;
            }

            return 0;
        }

        long concated = current;
        long val = ints[0];
        while (val > 0)
        {
            val /= 10;
            concated *= 10;
        }

        return FindSolutions(equationItem1, current + ints[0], ints[1..]) +
               FindSolutions(equationItem1, current * ints[0], ints[1..]);
    }
    
    private static int FindSolutionsConcat(long equationItem1, long current, long[] ints)
    {
        if (current > equationItem1)
        {
            return 0;
        }
        if (ints.Length == 0)
        {
            if (equationItem1 == current)
            {
                return 1;
            }

            return 0;
        }

        long concated = current;
        long val = ints[0];
        while (val > 0)
        {
            val /= 10;
            concated *= 10;
        }

        concated += ints[0];

        return FindSolutionsConcat(equationItem1, current + ints[0], ints[1..]) +
               FindSolutionsConcat(equationItem1, current * ints[0], ints[1..]) + 
               FindSolutionsConcat(equationItem1, concated, ints[1..]);
    }


    static void RunPart2(string inputFile)
    {
        long result = 0;

        var lines = System.IO.File.ReadAllLines(inputFile);

        var map = Utilities.Map<char>.LoadFromLines(lines, (c) => c);
        var start = map.FindEntry('S');

        var downVectors = new Dictionary<Vector,long>();
        downVectors.Add(start, 1);
        int y = 0;
        int split = 0;
        while (y < map.MaxY)
        {
            Dictionary<Vector,long> newVec = new();
            foreach (var downVector in downVectors)
            {
                var pos = downVector.Key;
                var count = downVector.Value;
                var vec = new Vector(pos.X, y);
                if (map[vec] == '^')
                {
                    split++;
                    if (vec.X > 0)
                    {
                        var thisPos = new Vector(vec.X - 1, vec.Y);
                        if (newVec.ContainsKey(thisPos))
                        {
                            newVec[thisPos] += count ;
                        }
                        else
                        {

                            newVec[thisPos] = count ;;
                        }
                    }

                    if (vec.X < map.MaxX)
                    {
                        var thisPos = new Vector(vec.X + 1, vec.Y);
                        if (newVec.ContainsKey(thisPos))
                        {
                            newVec[thisPos] += count ;
                        }
                        else
                        {

                            newVec[thisPos] = count;;
                        }
                    }
                }
                else
                {
                    if (newVec.ContainsKey(vec))
                    {
                        newVec[vec] += count ;
                    }
                    else
                    {

                        newVec[vec] = count;;
                    }
                }
            }

            downVectors = newVec;
            y++;
        }
        
        System.Console.WriteLine($"Result {inputFile} is {downVectors.Values.Sum()}");
    }
}