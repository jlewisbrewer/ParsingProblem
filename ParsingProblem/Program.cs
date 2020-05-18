namespace ParsingProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = "4 + 2 * 3 - 8 / 2";
         
            var parser = new Parser();
            System.Console.WriteLine(parser.Parse(input));
        }
    }
}
