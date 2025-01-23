using System.Diagnostics;

namespace day01;

class Program
{
    static void Main(string[] args)
    {
        // Allow this app to be run:
        //
        // *  From VS Code "Run" => "Start Debugging"
        // *  From VS Code "Run" => "Run Without Debugging"
        // *  From the terminal via "dotnet run"
        // 
        // The current working directory is different based on the method,
        // so accomodate both by setting the working directory to the 
        // bin/obj for the app.
        string basedir = AppDomain.CurrentDomain.BaseDirectory;

        Directory.SetCurrentDirectory(basedir);
        string prefix = "../../../";
        string filename = "input.txt";

        int part1 = 0;
        int part2 = 0;

        // step in the simulation (0 for the first step)
        int stepNum = 0;
        // The current floor = 0 is the ground floor
        int currentFloor = 0;
        int maxFloorReached = 0;
        // simulation step number where we go negative for the first time
        // note: this is 0 based also -- the answer to the puzzle will be
        // this +1 since it considers the first step as 1
        // -1 indicates we've never entered the basement
        int stepWhereBasementEntered = -1;

        string[] lines = File.ReadAllLines($"{prefix}{filename}");
        Debug.Assert(lines.Length == 1, "Expected exactly one line of input");
        string line = lines[0];

        for (stepNum = 0; stepNum < line.Length; stepNum++)
        {
            char c = line[stepNum];

            if (c == '(') currentFloor++;
            else if (c == ')') currentFloor--;

            if (currentFloor > maxFloorReached) maxFloorReached = currentFloor;
            if (currentFloor < 0 && stepWhereBasementEntered == -1) stepWhereBasementEntered = stepNum;
        }

        part1 = maxFloorReached;
        part2 = stepWhereBasementEntered + 1;
        
        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {part2}");

    }
}
