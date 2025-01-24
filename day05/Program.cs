namespace day05;

class Program
{
    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        string filename = "input.txt";
        Directory.SetCurrentDirectory(basedir);

        string[] lines = File.ReadAllLines($"{prefix}{filename}");

        int part1count = lines.Count(line => part1isNice(line));
        int part2count = lines.Count(line => part2isNice(line));
         

        //Console.WriteLine($"jchzalrnumimnmhp: {isNice("jchzalrnumimnmhp")}");
        Console.WriteLine($"Part 1: {part1count}");
        Console.WriteLine($"Part 2: {part2count}");
    }

    static bool part1isNice(string line)
    {
        // Rule 1: contain at least 3 vowels (aeiou)
        char[] vowels = { 'a', 'e', 'i', 'o', 'u' };
        int numVowels = 0;

        // Rule 2: contain at least one letter twice
        bool rule2matched = false;

        // scan through all the characters of the string
        // a = the prior character (or null if we're at the start)
        // b = the currently considered character
        for (int i = 0; i < line.Length; i++) {
            char? prev = i > 0 ? line[i-1] : null;
            char curr = line[i];

            // Rule 3 - cannot contain "ab", "cd", "pq", or "xy"
            // Since this is a negative rule and disqualifies the string,
            // matching this rule breaks us out and no longer considers this
            // line further.
            if ((prev == 'a' && curr =='b') ||
                (prev == 'c' && curr == 'd') ||
                (prev == 'p' && curr == 'q') ||
                (prev == 'x' && curr == 'y'))
            {
                return false;
            }

            // Rule 1
            if (vowels.Contains(curr))
            {
                numVowels++;
            }

            if (curr == prev)
            {
                rule2matched = true;
            }
        }

        return (numVowels >= 3 && rule2matched);
    }

    static bool part2isNice(string line)
    {
        // Rule 1: pair of two letters appears at least twice without overlap
        bool rule1matched = false;

        // Rule : a letter repeats with exactly one letter between
        bool rule2matched = false;

        // Scan through all characters in the line (but also have an escape
        // hatch -- once we've satisfied both rule 1 and rule 2 break out)
        for (int i = 0; (i < line.Length) && !(rule1matched && rule2matched); i++)
        {

            // only evaluate rule 1 if it hasn't already been matched
            // and we haven't yet scanned too close to the end where
            // we won't have room for the second match of the pair
            if (rule1matched == false && i <= line.Length - 4)
            {
                // create a substring that starts at i+2 so we can scan for
                // another match of line[i] + line[i+1]
                string pattern = $"{line[i]}{line[i+1]}";
                string remainder = line.Substring(i+2);

                rule1matched = remainder.Contains(pattern);
            }

            // only evaluate rule 2 if it hasn't already been matched
            // and we haven't yet scanned too close to the end where
            // we won't have room for a buffer character and the matching
            // second instance of the character
            if (rule2matched == false && i <= line.Length - 3)
            {
                rule2matched = line[i] == line[i+2];
            }
        }

        return rule1matched && rule2matched;
    }
}
