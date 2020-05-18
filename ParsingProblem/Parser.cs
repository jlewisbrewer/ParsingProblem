using System;
using System.Linq;
using ParsingProblem;

interface IParser
{
    string Parse(string input);
}

public class Parser
{
    private static char[] operators = {'+', '-', '*', '/', '(', ')'};

    private string Validate(string input)
    {
        if(string.IsNullOrEmpty(input))
            throw new FormatException("Input cannot be null.");
            
        if(!input.Any(char.IsDigit))
            throw new FormatException("Input needs a digit.");
        
        if(input.Any(char.IsLetter))
        {
            throw new FormatException("Input string can only contain digits, " +
            "parentheses, and arithmetic operators");
        }
    
        return input.Replace(" ", "");
    }

    public int Parse(string input)
    {
        string result;
        ExpressionParser ExpressionParser = new ExpressionParser();
        ProductParser productParser = new ProductParser();
        SummationParser summationParser = new SummationParser();

        result = Validate(input);
        result = ExpressionParser.Parse(result);

        return Int32.Parse(result);
    }
}

public class NumericalParser
{
    public char[] parentheses = {'(', ')'};
    public char[] summationOperators = {'+', '-'};
    public char[] productOperators = {'*', '/'};

    public static bool HasOperators(string input, char[] operators)
    {
        if(operators.Any(input.Contains))
            return true;
        return false;
    }
    public int[] FindOperation(string input, char[] operators)
    {
        int[] substringInfo = {0, input.Length};
        int operIndex = input.IndexOfAny(operators);
        for(int i = operIndex - 1; i >= 0; i--)
        {
            if(!char.IsDigit(input[i]))
            {
                substringInfo[0] = i + 1;
                break;
            }
        }
        for(int i = operIndex + 1; i < input.Length; i++)
        {
            if(!char.IsDigit(input[i]))
            {
                substringInfo[1] = i;
                break;
            }
        }

        return substringInfo;
    }

    public string AdjustInput(string input, char[] operators)
    {
        while(HasOperators(input, operators))
        {
            int[] substringInfo = FindOperation(input, operators);
            int startIndex = substringInfo[0];
            int substringLength = substringInfo[1] - startIndex;
            string substring = input.Substring(startIndex, substringLength);
            
            substring = Evaluator.Evaluate(substring);
            
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

public class ExpressionParser : NumericalParser, IParser
{
    public int[] FindExpression(string input)
    {
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
        ProductParser productParser = new ProductParser();
        SummationParser summationParser = new SummationParser();

        while(HasOperators(input, operators))
        {
            int[] substringInfo = FindExpression(input);
            int startIndex = substringInfo[0];
            int substringLength = substringInfo[1] - startIndex;
            string substring = input.Substring(startIndex, substringLength);

            // Removes the parentheses
            substring = substring.Remove(0, 1);
            substring = substring.Remove(substring.Length - 1, 1);

            substring = AdjustInput(substring, parentheses);

            input = input.Remove(startIndex, substringLength).Insert(startIndex, substring);
        }
        input = productParser.Parse(input);
        input = summationParser.Parse(input);

        return input;
    }
    public string Parse(string input)
    {   
        return AdjustInput(input, parentheses);
    }
}