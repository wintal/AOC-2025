namespace Day1;


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
        int result = 0;
        
        var lines = System.IO.File.ReadAllLines(inputFile);
        List<(long, long)> rules = new();
        List<long> values = new();
        foreach (var line in lines)
        {
            var parts = line.Split("-", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                rules.Add((long.Parse(parts[0]), long.Parse(parts[1])));
                continue;
            }
            else if (!string.IsNullOrEmpty(line))
            {
                long value = long.Parse(line);
                values.Add(value);
                foreach (var rule in rules)
                {
                    if (value >= rule.Item1 && value <= rule.Item2)
                    {
                        result++;
                        break;
                    }
                }
            }


        }

        System.Console.WriteLine($"Result {inputFile} is {result}");
    }

    public record struct Range(long MinInclusive, long MaxInclusive)
    {
        public bool Valid()
        {
            return MaxInclusive >= MinInclusive;
        }
    
        public (Range below, Range above) Split(int boundary)
        {

            if (boundary <= MinInclusive)
            {
                return ( new Range(0, -1), this);
            } 
            else if (boundary >= MaxInclusive)
            {
                return (this,  new Range(0, -1) );
            }
            return (new Range(MinInclusive, boundary), new Range(boundary, MaxInclusive));
        }

        public (bool, Range) Combine(Range range)
        {
            if (range.MinInclusive > MaxInclusive)
            {
                return (false, new Range(0,0));
            }

            if (range.MaxInclusive < MinInclusive)
            {
                return (false, new Range(0,0));
            }

            return (true,
                new Range(Math.Min(MinInclusive, range.MinInclusive), Math.Max(MaxInclusive, range.MaxInclusive)));
        }
        
        public bool InRange(int testValue)
        {
            if (testValue < MinInclusive || testValue > MaxInclusive) return false;
            return true;
        }
    }
    
    static void RunPart2(string inputFile)
    {
        long result = 0;
        
        var lines = System.IO.File.ReadAllLines(inputFile);
        List<Range> rules = new();
        List<long> values = new();
        foreach (var line in lines)
        {
            var parts = line.Split("-", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 2)
            {
                Range thisRange = new Range(long.Parse(parts[0]), long.Parse(parts[1]));
                bool combined = false;
                do
                {
                    combined = false;
                    List<int> toRemove = new List<int>();
                    for (int i = 0; i < rules.Count; i++)
                    {
                        (combined, var newRange) = thisRange.Combine(rules[i]);
                        if (combined)
                        {
                            thisRange = newRange;
                            toRemove.Add(i);
                        }
                    }

                    toRemove.Reverse();
                    foreach (var i in toRemove)
                    {
                        rules.RemoveAt(i);
                    }
                } while (combined);

                rules.Add(thisRange);
            }

        }

        foreach (var range in rules)
        {
            result += range.MaxInclusive - range.MinInclusive + 1;
        }


        System.Console.WriteLine($"Result {inputFile} is {result}");
    }
}