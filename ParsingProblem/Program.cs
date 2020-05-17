using System;
namespace ParsingProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "12 + 4 + 89";
            var parser = new Parser();
            System.Console.WriteLine(parser.Parse(input));
        }
    }
}
