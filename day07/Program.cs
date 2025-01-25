using System.Diagnostics;
using System.Text.RegularExpressions;

namespace day07;

class Program
{
    // The imported input file with the wire ID (right side of the line) as
    // the dictionary key and the left expression as the value.
    private static Dictionary<string, string> _circuit =
        new Dictionary<string, string>();

    // Memoized results of calls to Evaluate() with the key being the
    // argument to Evaluate() and the value being the resulting ushort
    private static Dictionary<string, ushort> _memo =
        new Dictionary<string, ushort>();

    // OPERATIONS contains all the patterns we understand for the expressions
    // on the left side of the circuit definition.  For each pattern
    // we have a function that takes the expression and returns the evaluated
    // ushort value.  (Note: this is a list of tuples -- Item1 is the regex,
    // Item2 of the tuple is the function to call for that pattern.)
    private static List<(Regex, Func<string, ushort>)> OPERATIONS = new List<(Regex, Func<string, ushort>)> {

        // Handle operations that are constant numbers, ex, "123", "555"
        (new Regex(@"^\d+$"), ushort.Parse),

        // Handle operations that are wire identifiers, ex, "ad", "a", "b"
        // look the wire up in the _circuit definition and then recursively evaluate
        (new Regex(@"^[a-z]+$"), s => Evaluate(_circuit[s])),

        // Handle operation of this form: "<arg1> AND <arg2>"
        (new Regex(@"^\S+ AND \S+$"), s => DoBinaryOp(s, (arg1, arg2) => (ushort)(arg1 & arg2))),

        // Handle operation of this form: "<arg1> OR <arg2>"
        (new Regex(@"^\S+ OR \S+$"), s => DoBinaryOp(s, (arg1, arg2) => (ushort)(arg1 | arg2))),


        // Handle operation of this form: "<arg1> LSHIFT <arg2>"
        (new Regex(@"^\S+ LSHIFT \S+$"), s => DoBinaryOp(s, (arg1, arg2) => (ushort)(arg1 << arg2))),

        // Handle operation of this form: "<arg1> RSHIFT <arg2>"
        (new Regex(@"^\S+ RSHIFT \S+$"), s => DoBinaryOp(s, (arg1, arg2) => (ushort)(arg1 >> arg2))),

        // Handle operation of this form: "NOT <arg1>"
        (new Regex(@"^NOT \S+$"), DoUnaryOp)
    };


    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        Directory.SetCurrentDirectory(basedir);

        Reset("input.txt");
        ushort part1 = Evaluate("a");
        Console.WriteLine($"Part 1: {part1}");

        Reset("input.txt");
        // override definitino for wire b to the result from part 1
        _circuit["b"] = part1.ToString();
        ushort part2 = Evaluate("a");
        Console.WriteLine($"Part 2: {part2}");
    }

    private static ushort DoBinaryOp(string s, Func<ushort, ushort, ushort> f)
    {
        string[] parts = s.Split();
        ushort arg1 = Evaluate(parts[0]);
        ushort arg2 = Evaluate(parts[2]);

        return f(arg1, arg2);
    }

    private static ushort DoUnaryOp(string s)
    {
        string remaining = s.Replace("NOT ", "");
        return (ushort)~Evaluate(remaining);
    }

    private static ushort Evaluate(string s)
    {
        if (_memo.ContainsKey(s)) return _memo[s];


        foreach (var op in OPERATIONS)
        {
            if (op.Item1.IsMatch(s))
            {
                _memo[s] = op.Item2(s);
                return _memo[s];
            }
        }

        throw new Exception($"Cannot Evaluate string '{s}'");
    }


    private static void Reset(string filename)
    {
        _circuit = new Dictionary<string, string>();
        _memo = new Dictionary<string, ushort>();

        string prefix = "../../../";
        string[] lines = File.ReadAllLines($"{prefix}{filename}");

        foreach (string line in lines)
        {
            // Each input line will be of the form:
            // [value] => [id]
            // where [id] is a wire identifier
            // the [value] could be any of the expression types such as:
            // * a constant number
            // * another wire identifier
            // * an operation AND, OR, LSHIFT, RSHIFT, or NOT each of which
            //   can have arguments that are also [values]
            string[] parts = line.Split(" -> ");
            string id = parts[1];
            string value = parts[0];
            Debug.Assert(!_circuit.ContainsKey(id),
                "More than one input to a wire is invalid!");
            _circuit[id] = value;
        }
    }
}

