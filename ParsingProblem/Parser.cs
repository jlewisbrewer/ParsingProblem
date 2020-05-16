using System;
using System.Linq;


public class Parser
{
    private static char[] operators = {'+', '-', '*', '/', '(', ')'};

    private static bool IsIllegalInput(char letter)
    {
        // The char can only contain numbers,white space, and approved operators
        if(!char.IsDigit(letter) || !char.IsWhiteSpace(letter) 
            || operators.Contains(letter))
            return true;
        return false;
    }
    public static string Parse(string input)
    {
        // First check if input has the correct chars
        foreach (char letter in input)
        {
            if(Parser.IsIllegalInput(letter))
            {
                throw new FormatException("Input string can only contain digits, " +
                "parentheses, and arithmetic operators");
            }
        }
        // Next, we arrange the operators so we can evaluate

        // First check for parentheses

        // Next check for Mult Div

        // Check for arithmetic
        return input;
    }
}
