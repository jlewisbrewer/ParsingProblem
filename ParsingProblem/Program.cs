namespace ParsingProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = args[0];
            Parser parser = new Parser();
            System.Console.WriteLine(parser.Parse(input));
        }
    }
}
