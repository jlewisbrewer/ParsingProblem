using System;
using System.Linq;


interface IParser
{
    private static char[] operators;

    public string Parse(string input);
}

public class Parser : IParser
{
    public string Parse(string input)
    {
        string result;
        var formatParser = new FormatParser();
        var summationParser = new SummationParser();

        result = formatParser.Parse(input);
        result = summationParser.Parse(result);

        return result;
    }
}

public class FormatParser : IParser
{
    private static char[] operators = {'+', '-', '*', '/', '(', ')'};

    private static bool IsIllegalInput(char letter)
    {
        // The char can only contain numbers,white space, and approved operators
        if(char.IsDigit(letter) || char.IsWhiteSpace(letter) 
            || operators.Contains(letter))
            return false;
        return true;
    }
    public string Parse(string input)
    {
        // Make sure the string isn't empty
        if(!input.Any(char.IsDigit))
            throw new FormatException("Input needs a digit.");
        
        // Check if input has the correct chars
        foreach (char letter in input)
        {
            if(FormatParser.IsIllegalInput(letter))
            {
                throw new FormatException("Input string can only contain digits, " +
                "parentheses, and arithmetic operators");
            }
        }

        return input;
    }
}

public class NumericalParser
{
    public int[] FindBinaryOperation(string input, char[] operators)
    {
        string right = "";
        bool foundLeft = false;
        bool foundOper = false;
        bool foundRight = false;
        int[] indexes = {0, 0};
        int count = 0;

        for(int i = 0; i < input.Length; i++)
        {   
            char ch = input[i];
            System.Console.WriteLine("Char:  " + ch);
            System.Console.WriteLine("Index " + i);
            if(foundOper && foundRight)
            {
                indexes[1] = count;
                break;
            }
            if(char.IsDigit(ch) && !foundLeft)
            {
                System.Console.WriteLine("Print: " + i);
                count = 0;
                foundLeft = true;
                indexes[0] = i;
            }
            if(operators.Contains(ch))
                foundOper = true;
            if(char.IsDigit(ch) && !foundRight && foundOper)
                right += ch;
            if(char.IsWhiteSpace(ch) && !foundRight && !string.IsNullOrEmpty(right))
            {
                System.Console.WriteLine("We are here:");
                foundRight = true;
            }
            count++;
        }

        return indexes;
    }
}

public class SummationParser : NumericalParser, IParser
{
    private static char[] operators = {'+', '-'};

    public static bool HasOperators(string input)
    {
        foreach(char ch in input)
        {
            if(operators.Contains(ch))
                return true;
        }
        return false;
    }

    public string Parse(string input)
    {
        string result;
        if(!HasOperators(input))
            return input;

        // By this point we just have to go left to right
        var indexes = FindBinaryOperation(input, operators);
        System.Console.WriteLine("Indexes: " + indexes[0] + ", " + indexes[1]);
        var substring = input.Substring(indexes[0], indexes[1]);
        System.Console.WriteLine("Substring: " + substring);
        return input;
    }
}
