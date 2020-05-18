using System;

namespace ParsingProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = args[0];
            Parser parser = new Parser();
            
            try 
            {
                System.Console.WriteLine(parser.Parse(input));
            }
            catch(FormatException e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
