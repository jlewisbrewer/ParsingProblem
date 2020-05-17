using System;
using System.Linq;
using ParsingProblem;

interface IParser
{
    public string Parse(string input);
}

public class Parser : IParser
{
    public string Parse(string input)
    {
        string result;
        var formatParser = new FormatParser();
        var parenthesesParser = new ParentheseParser();
        var productParser = new ProductParser();
        var summationParser = new SummationParser();

        result = formatParser.Parse(input);
        result = parenthesesParser.Parse(result);
        result = productParser.Parse(result);
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
        // Removes whitespace
        return input.Replace(" ", "");
    }
}

public class NumericalParser
{
    public char[] parentheses = {'(', ')'};
    public char[] summationOperators = {'+', '-'};
    public char[] productOperators = {'*', '/'};

    public static bool HasOperators(string input, char[] operators)
    {
        foreach(char ch in input)
        {
            if(operators.Contains(ch))
                return true;
        }
        return false;
    }
    public int[] FindBinaryOperation(string input, char[] operators)
    {
        // We need to find the first operation we want, and then
        // iter back to find the beginning of the digit
        bool hasOper = false;
        int[] substringInfo = {0, input.Length};

        for(int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if(hasOper && !char.IsDigit(ch))
            {
                substringInfo[1] = i;
                break;
            }
            if(operators.Contains(ch))
            {
                hasOper = true;
                for(int j = i - 1; j >= 0; j--)
                {
                    if(!char.IsDigit(input[j]))
                    {
                        substringInfo[0] = j + 1;
                        break;
                    }
                }
            }
        }

        return substringInfo;
    }

    public string AdjustInput(string input, char[] operators)
    {
        while(HasOperators(input, operators))
        {
            var substringInfo = FindBinaryOperation(input, operators);
            var startIndex = substringInfo[0];
            var substringLength = substringInfo[1] - startIndex;
            var substring = input.Substring(startIndex, substringLength);
            // Now we evaluate the substring
            substring = Evaluator.Evaluate(substring);
            // Bring them back together
            input = input.Remove(startIndex, substringLength).Insert(startIndex, substring);
        }
        return input;
    }

}

public class SummationParser : NumericalParser, IParser
{
    public string Parse(string input)
    {
        if(!HasOperators(input, summationOperators))
            return input;

        // By this point we just have to go left to right, starting from
        // the beginning of the string
        return AdjustInput(input, summationOperators);
    }
}

public class ProductParser : NumericalParser, IParser
{
    public string Parse(string input)
    {
        if(!HasOperators(input, productOperators))
            return input;
        
        return AdjustInput(input, productOperators);
    }
}

public class ParentheseParser : NumericalParser, IParser
{
    // This class will be different, it will have to recursively call
    // Parser with it's results
    public int[] FindSubExpression(string input)
    {
        // Assume that it has proper number of parentheses

        // We need to count the left parentheses and stop when we
        // Find the same amount of right parentheses
        bool foundStartIndex = false;
        int leftParenCount = 0;
        int rightParenCount = 0;
        int[] substringInfo = {0, input.Length};
        
        for(int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if(ch == '(')
            {
                if (!foundStartIndex)
                {
                    foundStartIndex = true;
                    substringInfo[0] = i;
                }
                leftParenCount++;
            }
            if (ch == ')')
            {
                rightParenCount++;
                if(leftParenCount == rightParenCount)
                {
                    // Here we are done
                    substringInfo[1] = i + 1;
                    break;
                }
            }
        }
        return substringInfo;
    }
    public string Parse(string input)
    {
        if(!HasOperators(input, parentheses))
            return input;
        
        while(HasOperators(input, parentheses))
        {
            var parser = new Parser();
            var substringInfo = FindSubExpression(input);
            var startIndex = substringInfo[0];
            var substringLength = substringInfo[1] - startIndex;
            var substring = input.Substring(startIndex, substringLength);
            // Remove the parentheses
            substring = substring.Remove(0, 1);
            substring = substring.Remove(substring.Length - 1, 1);
            // Now we have to Parse that substring

            substring = parser.Parse(substring);
            // We need to remove two more because of the parenthese we removed
            input = input.Remove(startIndex, substringLength).Insert(startIndex, substring);
        }
        return input;
    }
}
