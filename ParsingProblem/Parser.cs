using System;
using System.Linq;

namespace ParsingProblem
{
    interface IParser
    {
        string Parse(string input);
    }

    /// <summary>
    /// Converts a string of arithmetic expressions into an integer.
    /// </summary>
    public class Parser
    {
        private static char[] operators = {'+', '-', '*', '/', '(', ')'};

        /// <summary>
        /// Ensures input is in correct format. Throws format exceptions if
        /// incorrect.
        /// </summary>
        /// <param name="input">Original input string</param>
        /// <returns>Input string with whitespace removed</returns>
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

        /// <summary>
        /// Parses original input string
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Solution to input as a 32 bit int</returns>
        public int Parse(string input)
        {
            ExpressionParser ExpressionParser = new ExpressionParser();
            input = Validate(input);
            input = ExpressionParser.Parse(input);

            return Int32.Parse(input);
        }
    }

    /// <summary>
    /// Parses numerical expressions
    /// </summary>
    public class NumericalParser
    {
        protected char[] parentheses = {'(', ')'};
        protected char[] summationOperators = {'+', '-'};
        protected char[] productOperators = {'*', '/'};

        /// <summary>
        /// Checks if input string has specific arithmetic operators
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="operators">Char list of arithmetic operators</param>
        /// <returns>Bool result</returns>
        protected static bool HasOperators(string input, char[] operators)
        {
            if(operators.Any(input.Contains))
                return true;
            return false;
        }
        
        /// <summary>
        /// Finds the nearest operand to left of operator
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="index">Index of operator</param>
        /// <returns>Index of left operator</returns>
        protected int FindLeftOperandIndex(string input, int index)
        {
            for(int i = index - 1; i >= 0; i--)
            {
                if(!char.IsDigit(input[i]))
                    return i + 1;
            }
            return 0;
        }

        /// <summary>
        /// Finds nearest operand to right of the operator
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="index">Operator index</param>
        /// <returns>Index of right operand</returns>
        protected int FindRightOperandIndex(string input, int index)
        {
            for(int i = index + 1; i < input.Length; i++)
            {
                if(!char.IsDigit(input[i]))
                    return i;
            }
            return input.Length;
        }

        /// <summary>
        /// Finds indices for operation centered around arithmetic operator
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="operators">List of arithmetic operators</param>
        /// <returns>Start and end index of operation</returns>
        protected int[] FindOperation(string input, char[] operators)
        {
            int operIndex = input.IndexOfAny(operators);
            int[] substringInfo = {FindLeftOperandIndex(input, operIndex),
                FindRightOperandIndex(input, operIndex)};

            return substringInfo;
        }

        /// <summary>
        /// Solves operation and replaces it with solution.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="operators">List of arithmetic operators</param>
        /// <returns>Modified input string</returns>
        protected string AdjustInput(string input, char[] operators)
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

    /// <summary>
    /// Parses addition and subtraction expressions
    /// </summary>
    public class SummationParser : NumericalParser, IParser
    {
        /// <summary>
        /// Parses input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Modified input</returns>
        public string Parse(string input)
        {
            if(!HasOperators(input, summationOperators))
                return input;

            return AdjustInput(input, summationOperators);
        }
    }

    /// <summary>
    /// Parses multiplication and division
    /// </summary>
    public class ProductParser : NumericalParser, IParser
    {
        /// <summary>
        /// Parses input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Modified input</returns>
        public string Parse(string input)
        {
            if(!HasOperators(input, productOperators))
                return input;
            
            return AdjustInput(input, productOperators);
        }
    }

    /// <summary>
    /// Parses expressions inside parentheses
    /// </summary>
    public class ExpressionParser : NumericalParser, IParser
    {
        /// <summary>
        /// Finds the expression enclosed in parentheses
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Start and end index of expression</returns>
        private int[] FindExpression(string input)
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
                        return substringInfo;
                    }
                }
            }
            return substringInfo;
        }
        
        /// <summary>
        /// Solves operation and replaces it with solution. Makes recursive
        /// calls to self and calls other Numerical Parsers.
        /// </summary>
        /// <param name="input">Input string</param>
        /// <param name="operators">List of arithmetic operators</param>
        /// <returns>Modified input string</returns>
        private new string AdjustInput(string input, char[] operators)
        {
            ProductParser productParser = new ProductParser();
            SummationParser summationParser = new SummationParser();

            while(HasOperators(input, operators))
            {
                int[] substringInfo = FindExpression(input);
                int startIndex = substringInfo[0];
                int substringLength = substringInfo[1] - startIndex;
                string substring = input.Substring(startIndex, substringLength);

                // Removes the parentheses from the substring
                substring = substring.Remove(0, 1);
                substring = substring.Remove(substring.Length - 1, 1);

                substring = AdjustInput(substring, parentheses);

                input = input.Remove(startIndex, substringLength).Insert(startIndex, substring);
            }
            input = productParser.Parse(input);
            input = summationParser.Parse(input);

            return input;
        }

        /// <summary>
        /// Parses input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Modified input string</returns>
        public string Parse(string input)
        {   
            return AdjustInput(input, parentheses);
        }
    }
}