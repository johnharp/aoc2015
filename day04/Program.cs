using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace day04;

class Program
{
    private static MD5 md5Hasher = MD5.Create();


    static void Main(string[] args)
    {
        string basedir = AppDomain.CurrentDomain.BaseDirectory;
        string prefix = "../../../";
        string filename = "input.txt";
        Directory.SetCurrentDirectory(basedir);

        string[] lines = File.ReadAllLines($"{prefix}{filename}");
        Debug.Assert(lines.Length == 1, "Expected only one line of input");
        string puzzleInput = lines[0];

        int i = 0;
        bool solvedPart1 = false;
        bool solvedPart2 = false;

        while (!solvedPart2)
        {
            i++;
            //if  (i%10000 == 0) Console.WriteLine($"i = {i}");
            string startOfHash = doHash(puzzleInput, i);
            if (!solvedPart1 && startOfHash.StartsWith("00000"))
            {
                Console.WriteLine($"Part 1: {i}");
                solvedPart1 = true;
            }

            if (!solvedPart2 && startOfHash.StartsWith("000000"))
            {
                Console.WriteLine($"Part 2: {i}");
                solvedPart2 = true;
            }
        }
    }

    private static string doHash(string puzzleInput, int suffixNumber)
    {
        string toHash = $"{puzzleInput}{suffixNumber}";
        byte[] result = md5Hasher.ComputeHash(Encoding.Default.GetBytes($"{puzzleInput}{suffixNumber}"));
        string startOfString = 
            result[0].ToString("x2") +
            result[1].ToString("x2") +
            result[2].ToString("x2");
        
        return startOfString;
    }
}
