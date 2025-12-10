using Utilities;
using Range = System.Range;

namespace Day1;

class Program
{
    private const string Sample = "sample.txt";
    private const string Input = "input.txt";

    static void Main(string[] args)
    {
        Console.WriteLine("Part 1");
        RunPart1(Sample);
        RunPart1(Input);
        Console.WriteLine("Part 2");
        RunPart2(Sample);
        RunPart2(Input);
    }

    static void RunPart1(string inputFile)
    {
        long result = 0;

        var start = DateTime.Now;
        var lines = System.IO.File.ReadAllLines(inputFile);

        List<Vector> array = new List<Vector>();

        int fileIndex = 0;
        bool isFile = true;
        foreach (var line in lines)
        {
            var parts = line.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            array.Add(new Vector(int.Parse(parts[0]), int.Parse(parts[1])));
        }

        long max = 0;
        for (int i = 0; i < array.Count; i++)
        {
            for (int j = 1 + 1; j < array.Count; j++)
            {
                var left = array[i] - array[j];
                var size = (left.X + 1) * (left.Y + 1);
                if (size > max) max = size;
            }
        }

        System.Console.WriteLine($"Result {inputFile} is {max}");
    }

    static bool RectangleIntersectHoriz(int minX, int maxX, int minY, int maxY, int left, int right, int y)
    {
        if (maxX <= left) return false;
        if (minX >= right) return false;
        if (y <= minY || y >= maxY) return false;
        if (left <= maxX && right >= maxX) return true;
        if (left <= minX && right >= minX) return true;

        if (left > minX && right < maxX) return true;
        if (left <= maxX && left >= minX) return true;
        if (right <= maxX && right >= minX) return true;
        return false;
    }

    static bool RectangleIntersectVert(int minX, int maxX, int minY, int maxY, int bottom, int top, int x)
    {
        if (maxY <= bottom) return false;
        if (minY >= top) return false;
        if (x <= minX || x >= maxX) return false;
        if (bottom <= maxY && top >= maxY) return true;
        if (bottom <= minY && top >= minY) return true;
        if (bottom > minY && top < maxY) return true;
        if (bottom <= maxY && bottom >= minY) return true;
        if (top <= maxY && top >= minY) return true;
        return false;
    }

    public static bool Intersects(int minX, int maxX, int minY, int maxY, Vector start, Vector end)
    {
        var segMinX = Math.Min(start.X, end.X);
        var segMaxX = Math.Max(start.X, end.X);
        var segMinY = Math.Min(start.Y, end.Y);
        var segMaxY = Math.Max(start.Y, end.Y);

        return segMaxX > minX && segMinX < maxX && segMaxY > minY && segMinY < maxY;
    }

    static void RunPart2(string inputFile)
    {
        long result = 0;


        var start = DateTime.Now;
        var lines = System.IO.File.ReadAllLines(inputFile);

        List<Vector> array = new List<Vector>();

        List<(Vector, Vector)> segments = new();
        int fileIndex = 0;
        bool isFile = true;
        foreach (var line in lines)
        {
            var parts = line.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            array.Add(new Vector(int.Parse(parts[0]), int.Parse(parts[1])));
            if (array.Count > 1)
            {
                segments.Add((array[array.Count - 2], array[array.Count - 1]));
            }
        }

        segments.Add((array[^1], array[0]));


        long max = 0;
        for (int i = 0; i < array.Count - 1; i++)
        {
            for (int j = 1 + 1; j < array.Count; j++)
            {
                var topRight = array[i];
                var bottomLeft = array[j];
                var top = (int)Math.Max(topRight.Y, bottomLeft.Y);
                var bottom = (int)Math.Min(topRight.Y, bottomLeft.Y);
                var left = (int)Math.Min(topRight.X, bottomLeft.X);
                var right = (int)Math.Max(topRight.X, bottomLeft.X);
                var diag = array[i] - array[j];
                var size = (Math.Abs(diag.X) + 1) * (Math.Abs(diag.Y) + 1);
                if (size < max) continue;
                bool intersects = false;
                for (int seg = 0; seg < segments.Count; seg++)
                {
                    var segment = segments[seg];
                    var segmentA = segment.Item1;
                    var segmentB = segment.Item2;
                    if (segmentA.X != segmentB.X)
                    {
                        // horizontal line, check if any of our up down lines intersect it
                        if (RectangleIntersectHoriz(left, right, bottom, top, (int)Math.Min(segmentA.X, segmentB.X), (int)Math.Max(segmentA.X, segmentB.X),
                                (int)segmentA.Y))
                        {
                            intersects = true;
                        }
                    }
                    else
                    {
                        // vertical line
                        // doesn't have zero width or end on the line
                        if (RectangleIntersectVert(left, right, bottom, top, (int)Math.Min(segmentA.Y, segmentB.Y), (int)Math.Max(segmentA.Y, segmentB.Y),
                                (int)segmentA.X))
                        {
                            intersects = true;
                        }
                    }

                    if (intersects) break;
                }

                if (!intersects)
                {
                    if (size > max) max = size;
                }
            }
        }

        System.Console.WriteLine($"Result {inputFile} is {max}");
    }
}