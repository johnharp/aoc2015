using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

namespace day08;

class Program
{
    static char[] hexChars = {
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
        'a', 'b', 'c', 'd', 'e', 'f'
    };

    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        Directory.SetCurrentDirectory(basedir);
        string prefix = "../../../";
        string filename = "input.txt";
        string[] lines = File.ReadAllLines($"{prefix}{filename}");

        int part1 = 0;
        int part2 = 0;

        foreach (string line in lines)
        {
            string lineWithEscapesRemoved = removeEscapes(line);
            int part1diff = line.Length - lineWithEscapesRemoved.Length;
            part1 += part1diff;

            string lineWithEscapesAdded = addEscapes(line);
            int part2diff = lineWithEscapesAdded.Length - line.Length;
            part2 += part2diff;
        }

        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");
    }

    static string removeEscapes(string s)
    {
        Debug.Assert(s[0] == '"', $"Error: should start with \" -- {s}");
        Debug.Assert(s[s.Length - 1] == '"', $"Error: should end with \" -- {s}");

        StringBuilder sb = new StringBuilder();
        int i = 1;
        while (i < s.Length - 1)
        {
            if (s[i] == '\\' && s[i + 1] == '"')
            {
                sb.Append('"');
                i += 2;
            }
            else if (s[i] == '\\' && s[i + 1] == '\\')
            {
                sb.Append('\\');
                i += 2;
            }
            else if (i < s.Length - 3 && s[i] == '\\' && s[i + 1] == 'x' &&
                hexChars.Contains(s[i + 2]) && hexChars.Contains(s[i + 3]))
            {
                string hexString = $"{s[i+2]}{s[i+3]}";
                int intValue = Convert.ToInt32(hexString, 16);
                char asciiChar = (char)intValue;
                sb.Append(asciiChar);
                i += 4;
            }
            else
            {
                sb.Append(s[i]);
                i += 1;
            }
        }

        return sb.ToString();
    }

    static string addEscapes(string s)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append('"');
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];

            if (c == '"') sb.Append("\\\"");
            else if (c == '\\') sb.Append("\\\\");
            else sb.Append(c);
        }
        sb.Append('"');

        return sb.ToString();
    }
}
