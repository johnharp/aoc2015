using System.Collections;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace day06;

class Program
{
    const int WIDTH = 1_000;
    const int HEIGHT = 1_000;
    // in C# bool arrays are initialized to false
    private static BitArray _bits = new BitArray(WIDTH * HEIGHT);
    // in C# int arrays are initialized to 0
    private static int[] _brightness = new int[WIDTH * HEIGHT];

    const string PATTERN = @"^(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)$";
    private static Regex REGEX  = new Regex(PATTERN);

    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        string filename = "input.txt";
        Directory.SetCurrentDirectory(basedir);

        string[] lines = File.ReadAllLines($"{prefix}{filename}");
        foreach(string line in lines)
        {
            HandleInstruction(line);
        }

        int part1 = CountAllOn();
        Console.WriteLine($"Part 1: {part1}");

        int part2 = TotalBrightness();
        Console.WriteLine($"Part 1: {part2}");
    }

    private static void HandleInstruction(string line)
    {
        Match match = REGEX.Match(line);
        if (!match.Success) throw new Exception("Input format problem: {line}");

        string instruction = match.Groups[1].Value;
        int x1 = int.Parse(match.Groups[2].Value);
        int y1 = int.Parse(match.Groups[3].Value);
        int x2 = int.Parse(match.Groups[4].Value);
        int y2 = int.Parse(match.Groups[5].Value);

        ValidateRange(x1, y1, x2, y2);

        Action<int, int> func;

        switch (instruction)
        {
            case "turn on":
                func = On;
                break;
            case "turn off":
                func = Off;
                break;
            case "toggle":
                func = Toggle;
                break;
            default:
                throw new Exception($"Unknown instruction: {instruction}");
        }

        ApplyFunctionToRange(func, x1, y1, x2, y2);
    }

    private static void ApplyFunctionToRange(Action<int, int> func, int x1, int y1, int x2, int y2)
    {
        for (int y = y1; y <= y2; y++)
        {
            for (int x = x1; x <= x2; x++)
            {
                func(x, y);
            }
        }
    }

    public static int CountAllOn()
    {
        int count = 0;

        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                if (_bits[y*WIDTH + x]) count++;
            }
        }

        return count;
    }

    public static int TotalBrightness()
    {
        int total = 0;

        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                total += _brightness[y*WIDTH + x];
            }
        }

        return total;
    }

    private static void ValidatePoint(int x, int y)
    {
        if (x < 0 || x > WIDTH-1 || y < 0 || y > HEIGHT-1)
        {
            throw new Exception($"Out of bounds point ({x}, {y})");
        }
    }

    private static void ValidateRange(int x1, int y1, int x2, int y2)
    {
        ValidatePoint(x1, y1);
        ValidatePoint(x2, y2);
        if (x2 < x1 || y2 < y1)
        {
            throw new Exception($"Invalid range ({x1}, {y1}) through ({x2}, {y2})");
        }
    }

    private static void On(int x, int y)
    {
        _bits[y*WIDTH + x] = true;
        _brightness[y*WIDTH + x] += 1;
    }

    private static void Off(int x, int y)
    {
        _bits[y*WIDTH + x] = false;
        _brightness[y*WIDTH + x] = Math.Max(0, _brightness[y*WIDTH +x] - 1);
    }

    private static void Toggle(int x, int y)
    {
        _bits[y*WIDTH + x] = !_bits[y*WIDTH + x];
        _brightness[y*WIDTH + x] += 2;
    }
}
