using System;
using System.Linq;
using ParsingProblem;
/*
    Parser interface
    All parser classes will have a parse method
*/
interface IParser
{
    public string Parse(string input);
}

/*
    Main Parser class
    Will call all the other parser classes
*/
public class Parser : IParser
{
    // Returns evaluated expression as a string
    public string Parse(string input)
    {
        string result;
        FormatParser formatParser = new FormatParser();
        SubexpressionParser subexpressionParser = new SubexpressionParser();
        ProductParser productParser = new ProductParser();
        SummationParser summationParser = new SummationParser();

        result = formatParser.Parse(input);
        result = subexpressionParser.Parse(result);
        result = productParser.Parse(result);
        result = summationParser.Parse(result);

        return result;
    }
}

/*
    Format Parser class
    Ensures that the input has the proper format
*/
public class FormatParser : IParser
{
    private static char[] operators = {'+', '-', '*', '/', '(', ')'};

    // Checks if input contains illegal input
    private static bool IsIllegalInput(string input)
    {
        foreach (char letter in input)
        {
            // The char can only contain numbers,white space, and approved operators
            if(char.IsDigit(letter) || char.IsWhiteSpace(letter) 
                || operators.Contains(letter))
                return false;
        }
        return true;
    }

    // Returns formated input string
    public string Parse(string input)
    {
        // Make sure the string isn't empty
        if(!input.Any(char.IsDigit))
            throw new FormatException("Input needs a digit.");
        
        // Check if input has the correct chars
 
        if(IsIllegalInput(input))
        {
            throw new FormatException("Input string can only contain digits, " +
            "parentheses, and arithmetic operators");
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
            char ch = input[i];
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
            int[] substringInfo = FindBinaryOperation(input, operators);
            int startIndex = substringInfo[0];
            int substringLength = substringInfo[1] - startIndex;
            string substring = input.Substring(startIndex, substringLength);
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

public class SubexpressionParser : NumericalParser, IParser
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
            char ch = input[i];
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
                    substringInfo[1] = i + 1;
                    break;
                }
            }
        }
        return substringInfo;
    }
    
    public new string AdjustInput(string input, char[] operators)
    {
        while(HasOperators(input, operators))
        {
            Parser parser = new Parser();
            int[] substringInfo = FindSubExpression(input);
            int startIndex = substringInfo[0];
            int substringLength = substringInfo[1] - startIndex;
            string substring = input.Substring(startIndex, substringLength);
            // Remove the parentheses
            substring = substring.Remove(0, 1);
            substring = substring.Remove(substring.Length - 1, 1);
            // Now we have to Parse that substring
            substring = parser.Parse(substring);

            input = input.Remove(startIndex, substringLength).Insert(startIndex, substring);
        }
        return input;
    }
    public string Parse(string input)
    {
        if(!HasOperators(input, parentheses))
            return input;
        
        return AdjustInput(input, parentheses);
    }
}