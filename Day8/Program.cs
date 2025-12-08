using Utilities;

namespace Day8;

class Program
{
    private const string Sample = "sample.txt";
    private const string Input = "input.txt";

    static void Main(string[] args)
    {
      //  RunPart1(Sample);
        RunPart1(Input);
        RunPart2(Sample);
        RunPart2(Input);
    }

    public record struct Pos(long x, long y, long z);

    static void RunPart1(string inputFile)
    {
        long result = 1;

        var lines = System.IO.File.ReadAllLines(inputFile);

        var map = Map<char>.LoadFromLines(lines, c => c);

        List<Pos> positions = new();

        foreach (var line in lines)
        {
            var point = line.Split(",").Select(long.Parse).ToList();

            positions.Add(new Pos(point[0], point[1], point[2]));
        }

        List<(double, int, int)> distances = new();
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i; j < positions.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var a = positions[i];
                var b = positions[j];

                var distanceSquared = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
                distances.Add((distanceSquared, i, j));
            }
        }

        distances.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        List<(int, int)> edges = new();
        HashSet<int> visited = new();

        List<HashSet<int>> subGraphs = new();
        for (int i = 0; i < 1000; i++)
        {
            var connection = distances[i];
            var left = subGraphs.FirstOrDefault(g => g.Contains(connection.Item2));
            var right = subGraphs.FirstOrDefault(g => g.Contains(connection.Item3));
            if (left == null && right == null)
            {
                subGraphs.Add(new HashSet<int>() { connection.Item2, connection.Item3 });
            }
            else if (left != null && right != null)
            {
                left.Add(connection.Item2);
                left.Add(connection.Item3);
                if (left != right)
                {
                    foreach (var val in right)
                    {
                        left.Add(val);
                    }

                    subGraphs.Remove(right);
                }
            }
            else if (left != null)
            {
                left.Add(connection.Item2);
                left.Add(connection.Item3);
            }
            else
            {
                right.Add(connection.Item2);
                right.Add(connection.Item3);
            }
        }

        var orderedGraphs = subGraphs.ToList();
        orderedGraphs.Sort((a, b) => b.Count.CompareTo(a.Count));
        for (int i = 0; i < 3; i++)
        {
            result = result * orderedGraphs[i].Count;
        }


        System.Console.WriteLine($"Result {inputFile} is {result}");
    }

    private static HashSet<Vector> GetNodes(Map<char> map, List<Vector> locationValue)
    {
        HashSet<Vector> nodes = new();
        for (int i = 0; i < locationValue.Count; i++)
        {
            for (int j = i + 1; j < locationValue.Count; j++)
            {
                var node1 = locationValue[i];
                var node2 = locationValue[j];
                var diff = node1 - node2;
                var location1 = node1 + diff;
                var location2 = node2 - diff;
                if (map.Contains(location1))
                {
                    nodes.Add(location1);
                }

                if (map.Contains(location2))
                {
                    nodes.Add(location2);
                }
            }
        }

        return nodes;
    }

    static void RunPart2(string inputFile)
    {
          long result = 1;

        var lines = System.IO.File.ReadAllLines(inputFile);

        var map = Map<char>.LoadFromLines(lines, c => c);

        List<Pos> positions = new();

        foreach (var line in lines)
        {
            var point = line.Split(",").Select(long.Parse).ToList();

            positions.Add(new Pos(point[0], point[1], point[2]));
        }

        List<(double, int, int)> distances = new();
        for (int i = 0; i < positions.Count; i++)
        {
            for (int j = i; j < positions.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                var a = positions[i];
                var b = positions[j];

                var distanceSquared = (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
                distances.Add((distanceSquared, i, j));
            }
        }

        distances.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        List<(int, int)> edges = new();
        HashSet<int> visited = new();

        List<HashSet<int>> subGraphs = new();
        for (int i = 0; i < distances.Count; i++)
        {
            var connection = distances[i];
            var left = subGraphs.FirstOrDefault(g => g.Contains(connection.Item2));
            var right = subGraphs.FirstOrDefault(g => g.Contains(connection.Item3));
            if (left == null && right == null)
            {
                subGraphs.Add(new HashSet<int>() { connection.Item2, connection.Item3 });
            }
            else if (left != null && right != null)
            {
                left.Add(connection.Item2);
                left.Add(connection.Item3);
                if (left != right)
                {
                    foreach (var val in right)
                    {
                        left.Add(val);
                    }

                    subGraphs.Remove(right);
                }
            }
            else if (left != null)
            {
                left.Add(connection.Item2);
                left.Add(connection.Item3);
            }
            else
            {
                right.Add(connection.Item2);
                right.Add(connection.Item3);
            }


            if (subGraphs.Count == 1)
            {
                var subgraph = subGraphs.First();
                if (subgraph.Count == lines.Length)
                {
                    System.Console.WriteLine($"Found the final connection between nodes {connection.Item2} and {connection.Item3}");
                    long value = positions[connection.Item2].x * positions[connection.Item3].x;
                    System.Console.WriteLine($"Result is {value}");
                    break;
                }
            }
        }

   

        System.Console.WriteLine($"Result {inputFile} is {result}");
    }

}