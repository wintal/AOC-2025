namespace Utilities;

public class Map<T> where T: struct, IEquatable<T>
{
    public T[][] Rows { get; set; }

    public int MaxY
    {
        get
        {
            return Rows.Length;
        }
    }

    public int MaxX
    {
        get
        {
            return Rows[0].Length;
        }
    }

    public Vector FindEntry(T entry)
    {
        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX; x++)
            {
                if (Rows[y][x].Equals(entry))
                {
                    return new Vector(x, y);
                }
            }
        }
        return new Vector(-1, -1);
    }
    public static Map<T> LoadFromLines(string[] lines, Func<char, T> converter = null)
    {
        var map = new Map<T>();
        map.Rows = new T[lines.Length][];
        int row = 0;
        foreach (var line in lines)
        {
            map.Rows[row++] = line.Select(converter).ToArray<T>();
        }

        return map;
    }
    
    public static Map<T> Create(int width, int height)
    {
        var map = new Map<T>();
        map.Rows = new T[height][];
        int row = 0;
        for (int i = 0; i <  height; i++)
        {
            map.Rows[row++] = new T[width];
        }

        return map;
    }

    public T this[Vector location]
    {
        get => Rows[location.Y][location.X];  
        set => Rows[location.Y][location.X] = value; 
    }

    public Map<T> Clone()
    {
        T [][] newRows = new T[Rows.Length][];
        for (int i = 0; i < Rows.Length; i++)
        {
            newRows[i] = Rows[i].Clone() as T[];
        }
        return new Map<T> { Rows = newRows };
    }

    public bool Contains(Vector l)
    {
     return (l.X >= 0 && l.X < MaxX && l.Y >= 0 && l.Y < MaxY);   
    }

    public void Print()
    {
        Console.WriteLine();
        foreach (var row in Rows)
        {
            Console.WriteLine(row);
        }
        Console.WriteLine();
    }

    public Vector? Find(T crop, HashSet<Vector> excluded)
    {
        for (int y = 0; y < MaxY; y++)
        {
            for (int x = 0; x < MaxX; x++)
            {
                Vector pos = new Vector(x, y);
                if (!(excluded?.Contains(pos)??false) && this[pos].Equals(crop))
                {
                    return pos;
                }
            }
        }

        return null;
    }
    
    public static void Print2DArray(T[,] matrix)
    {
        int rows = matrix.GetLength(0); // 0 is first dimension, 1 is 2nd 
        //dimension of 2d array 
        int cols = matrix.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(matrix[i, j]);
            }
            Console.Write('\n');
        }
    }
}