using System.Diagnostics;

namespace day02;

class Program
{
    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        string filename = "input.txt";
        Directory.SetCurrentDirectory(basedir);

        long totalPaperNeeded = 0;
        long totalRibbonNeeded = 0;

        string[] lines = File.ReadAllLines($"{prefix}{filename}");

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string[] parts = line.Split('x');

            Debug.Assert(parts.Length == 3, "All lines should have 3 parts");
            int l = int.Parse(parts[0]);
            int w = int.Parse(parts[1]);
            int h = int.Parse(parts[2]);

            totalPaperNeeded += paperNeeded(l, w, h);
            totalRibbonNeeded += ribbonNeeded(l, w, h);
        }

        Console.WriteLine($"Part 1: {totalPaperNeeded}");
        Console.WriteLine($"Part 2: {totalRibbonNeeded}");
    }

    static long paperNeeded(int l, int w, int h)
    {
        long a1 = l * w;
        long a2 = w * h;
        long a3 = h * l;

        long amin = Math.Min(Math.Min(a1, a2), a3);
        long area = 2 * a1 + 2 * a2 + 2 * a3 + amin;

        return area;
    }

    static long ribbonNeeded(int l, int w, int h)
    {
        long p1 = 2*l + 2*w;
        long p2 = 2*w + 2*h;
        long p3 = 2*h + 2*l;

        long perimMin = Math.Min(Math.Min(p1, p2), p3);

        return perimMin + l*w*h;
    }
}
