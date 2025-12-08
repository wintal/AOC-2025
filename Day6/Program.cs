using Utilities;
using Range = System.Range;

namespace Day6;

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

        List<List<long>> values = new List<List<long>>();

      

        for (int line = 0; line < lines.Length - 1; line++)
        {
            var numbers = lines[line]
                .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            values.Add(numbers.Select(long.Parse).ToList());
        }

        var operands = lines[lines.Length - 1]
            .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);


        long[] results = new long[values[0].Count];

        for (int i = 0; i < operands.Length; i++)
        {
            var operand = operands[i];
                switch (operand)
                {
                    case "*":
                        results[i] = 1;
                        break;
                    case "+":
                        results[i] = 0;
                        break;
                }
        }

        foreach (var line in values)
        {
            for (int i = 0; i < line.Count; i++)
            {
                var operand = operands[i];
                switch (operand)
                {
                    case "*":
                        results[i] *= line[i];
                        break;
                    case "+":
                        results[i] += line[i];
                        break;
                }
            }
        }
       
        

        System.Console.WriteLine($"Result {inputFile} is {results.Sum()}");
    }

    static void RunPart2(string inputFile)
    {
        long result = 0;

        var lines = System.IO.File.ReadAllLines(inputFile);



        int width = lines[0].Length;
        var operand = lines[lines.Length - 1][0];
        for (int i = 0; i < width; )
        {
            List<long> numbers = new();
            var empty = false;
            while (!empty && i < width)
            {
                empty = true;
                long number = 0;
                for (int j = 0; j < lines.Length - 1; j++)
                {
                    if (lines[j][i] != ' ')
                    {
                        number = number * 10 + lines[j][i] - '0';
                        empty = false;
                    }
                }

                if (!empty ||  i == width -1)
                {
                    numbers.Add(number);
                }

                if (empty || i == width - 1)
                {
                    switch (operand)
                    {
                        case '*':
                            long thisVal = 1;
                            foreach (var thisNumber in numbers)
                            {
                                thisVal *= thisNumber;
                            }

                            result += thisVal;
                            break;
                        case '+':
                            result += numbers.Sum();
                            break;
                    }

                    if (i < width - 1)
                    {
                        operand = lines[lines.Length - 1][i + 1];
                    }

                }

                i++;
            }
           
            
        }
        
       System.Console.WriteLine($"Result {inputFile} is {result}");
    }
    
  
}