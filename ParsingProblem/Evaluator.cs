using System;

namespace ParsingProblem
{
    /// <summary>
    /// Evaluates arithmetic operations
    /// </summary>
    public class Evaluator
    {   
        private static char[] operators = {'+', '-', '*', '/'};

        /// <summary>
        /// Solves a given expression.
        /// </summary>
        /// <param name="left">Left operand</param>
        /// <param name="oper">Operator</param>
        /// <param name="right">Right operand</param>
        /// <returns>Solution to expression</returns>
        private static int EvaluateExpression(int left, char oper, int right)
        {
            switch(oper)
            {
                case '+': 
                    return left + right;
                case '-':
                    return left - right;
                case '*':
                    return left * right;
                case '/':
                    return left / right;
                default:
                    throw new InvalidOperationException("Unable to use that operator.");
            }
        }

        /// <summary>
        /// Evaluates given input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Solution to expression as a string</returns>
        public static string Evaluate(string input)
        {
            int operIndex = input.IndexOfAny(operators);
            char oper = input[operIndex];
            int left = Int32.Parse(input.Substring(0, operIndex));
            int right = Int32.Parse(input.Substring(operIndex + 1, input.Length - (operIndex + 1)));
            
            return EvaluateExpression(left, oper, right).ToString();
        }
        
    }
}