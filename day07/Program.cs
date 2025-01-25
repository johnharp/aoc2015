using System.Diagnostics;
using System.Reflection.Metadata;

namespace day07;

class Program
{
    // All wire values in the circuit are stored in a dictionary
    // Key is the wire identifier (a string)
    // Value is an ushort (a 16 bit value)
    private static Dictionary<string, ushort> _circuit = new Dictionary<string, ushort>();

    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        //string filename = "input-sample.txt";
        string filename = "input.txt";
        string filename2 = "input2.txt";
        Directory.SetCurrentDirectory(basedir);

        string[] lines = File.ReadAllLines($"{prefix}{filename}");
        string[] lines2 = File.ReadAllLines($"{prefix}{filename2}");
        ApplyAllLines(lines);

        ushort? part1 = Get("a");
        Console.WriteLine($"Part 1: {part1}");

        Reset();
        ApplyAllLines(lines2);

        ushort? part2 = Get("a");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static void ApplyAllLines(string[] lines)
    {
        bool changes = true;

        while (changes)
        {
            changes = false;

            foreach(string line in lines)
            {
                if (HandleLine(line))
                {
                    changes = true;
                }
            }
        }
    }

    private static void Reset()
    {
        _circuit = new Dictionary<string, ushort>();
    }

    private static void TestLine(string line)
    {
        Console.WriteLine("==============================");
        Console.WriteLine(line);
        HandleLine(line);
        Dump();
    }

    private static void Dump()
    {
        List<String> keys = _circuit.Keys.ToList();
        keys.Sort();
        foreach(string key in keys)
        {
            Console.WriteLine($"{key}: {_circuit[key]}");
        }
    }

    private static bool HandleLine(string line)
    {
        string[] parts = line.Split(" -> ");
        Debug.Assert(parts.Length == 2, $"Bad input format: {line}");

        string operation = parts[0];
        string outWire = parts[1];
    
        ushort? value = ComputeOutput(operation);
        ushort? oldValue = Get(outWire);
        Set(outWire, value);
        return value != oldValue;
    }

    private static ushort? ComputeOutput(string op)
    {
        if (op.Contains(" AND ")) return AND(op);
        else if (op.Contains(" OR ")) return OR(op);
        else if (op.Contains(" LSHIFT ")) return LSHIFT(op);
        else if (op.Contains(" RSHIFT ")) return RSHIFT(op);
        else if (op.Contains("NOT ")) return NOT(op);
        else return CONST(op);
    }

    private static ushort? GetValueOf(string s)
    {
        ushort value;

        if (ushort.TryParse(s, out value)) {
            return value;
        }
        else
        {
            return Get(s);
        }
    }

    private static ushort? AND(string operation)
    {
        string[] parts = operation.Split(" AND ");
        Debug.Assert(parts.Length == 2, "AND operation expects two args");
        ushort? arg1 = GetValueOf(parts[0]);
        ushort? arg2 = GetValueOf(parts[1]);

        if (arg1.HasValue && arg2.HasValue)
        {
            return (ushort) (arg1.Value & arg2.Value);
        }
        else
        {
            return null;
        }
    }

    private static ushort? OR(string operation)
    {
        string[] parts = operation.Split(" OR ");
        Debug.Assert(parts.Length == 2, "OR operation expects two args");
        ushort? arg1 = GetValueOf(parts[0]);
        ushort? arg2 = GetValueOf(parts[1]);

        if (arg1.HasValue && arg2.HasValue)
        {
            return (ushort) (arg1.Value | arg2.Value);
        }
        else
        {
            return null;
        }
    }

    private static ushort? LSHIFT(string operation)
    {
        string[] parts = operation.Split(" LSHIFT ");
        Debug.Assert(parts.Length == 2, "LSHIFT operation expects two args");
        ushort? arg1 = GetValueOf(parts[0]);
        ushort? arg2 = GetValueOf(parts[1]);

        if (arg1.HasValue && arg2.HasValue)
        {
            return (ushort) (arg1.Value << arg2.Value);
        }
        else
        {
            return null;
        }
    }

    private static ushort? RSHIFT(string operation)
    {
        string[] parts = operation.Split(" RSHIFT ");
        Debug.Assert(parts.Length == 2, "RSHIFT operation expects two args");
        ushort? arg1 = GetValueOf(parts[0]);
        ushort? arg2 = GetValueOf(parts[1]);

        if (arg1.HasValue && arg2.HasValue)
        {
            return (ushort) (arg1.Value >> arg2.Value);
        }
        else
        {
            return null;
        }
    }

    private static ushort? NOT(string operation)
    {
        string a = operation.Replace("NOT ", "");
        ushort? arg1 = GetValueOf(a);

        if (arg1.HasValue)
        {
            return (ushort) (~arg1.Value);
        }
        else
        {
            return null;
        }
    }

    private static ushort? CONST(string operation)
    {
        return GetValueOf(operation);
    }

    private static void Set(string id, ushort? value)
    {
        if (value.HasValue)
        {
            _circuit[id] = value.Value;
        }
    }

    private static ushort? Get(string id)
    {
        if (!_circuit.ContainsKey(id))
        {
            return null;
        }
        else
        {
            return _circuit[id];
        }

    }
}
