using System;
namespace ParsingProblem
{
    public class Evaluator
    {   
        private static char[] operators = {'+', '-', '*', '/'};

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

        public static string Evaluate(string input)
        {
            // Because of the parser, this should just be in the form
            // LEFT OP RIGHT
            int operIndex = input.IndexOfAny(operators);
            char oper = input[operIndex];
            int left = Int32.Parse(input.Substring(0, operIndex));
            int right = Int32.Parse(input.Substring(operIndex + 1, input.Length - (operIndex + 1)));
            
            return EvaluateExpression(left, oper, right).ToString();
        }
        
    }
}
