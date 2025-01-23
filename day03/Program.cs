using System.Diagnostics;

namespace day03;

class Program
{
    private static Dictionary<(int, int), int> deliveries = new Dictionary<(int, int), int>();
    private static Dictionary<(int, int), int> p2deliveries = new Dictionary<(int, int), int>();

    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        string filename = "input.txt";
        Directory.SetCurrentDirectory(basedir);

        string[] lines = File.ReadAllLines($"{prefix}{filename}");
        Debug.Assert(lines.Length == 1, "Expected only one line of input");
        string line = lines[0];

        (int, int) currentLocation = (0, 0);
        (int, int) p2santa = (0, 0);
        (int, int) p2robo = (0, 0);

        incr(deliveries, currentLocation);
        incr(p2deliveries, p2santa);
        incr(p2deliveries, p2robo);


        bool isSantaTurn = true;
        foreach (char c in line)
        {
            currentLocation = move(c, currentLocation);
            incr(deliveries, currentLocation);

            if (isSantaTurn)
            {
                p2santa = move(c, p2santa);
                incr(p2deliveries, p2santa);
            }
            else
            {
                p2robo = move(c, p2robo);
                incr(p2deliveries, p2robo);
            }
            isSantaTurn = !isSantaTurn;
        }

        Console.WriteLine($"Part 1: {deliveries.Keys.Count}");
        Console.WriteLine($"Part 2: {p2deliveries.Keys.Count}");
    }

    static void incr(Dictionary<(int, int), int> dict, (int, int) loc)
    {
        if (!dict.ContainsKey(loc))
        {
            dict[loc] = 0;
        }

        dict[loc]++;
    }

    static (int, int) move(char c, (int, int) loc)
    {
        (int, int) newLoc;

        switch (c)
        {
            case '^':
                newLoc = (
                    loc.Item1,
                    loc.Item2 - 1
                );
                break;
            case 'v':
                newLoc = (
                    loc.Item1,
                    loc.Item2 + 1
                );
                break;
            case '<':
                newLoc = (
                    loc.Item1 - 1,
                    loc.Item2
                );
                break;
            case '>':
                newLoc = (
                    loc.Item1 + 1,
                    loc.Item2
                );
                break;
            default:
                throw new Exception($"Unexpected input character: {c}");
        }

        return newLoc;
    }
}
