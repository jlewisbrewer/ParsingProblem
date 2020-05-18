using System;
using ParsingProblem;
using Xunit;

namespace ParsingProblem.Tests
{
    public class ParserTests
    {
        public Parser parser = new Parser();

        [Fact]
        public void EmptyStringShouldThrowException(){
            string input1 = "";
            string input2 = "  ";

            Assert.Throws<FormatException>(() => parser.Parse(input1));
            Assert.Throws<FormatException>(() => parser.Parse(input2));
        }
        [Fact]
        public void AlphaShouldThrowException()
        {
            string input1 = "a";
            string input2 = "3a + 4b";
            Assert.Throws<FormatException>(() => parser.Parse(input1));
            Assert.Throws<FormatException>(() => parser.Parse(input2));
        }
        [Fact]
        public void NonDigitShouldThrowException()
        {
            string input1 = "&#!";
            
            Assert.Throws<FormatException>(() => parser.Parse(input1));
        }
        
        [Fact]
        public void DigitShouldReturnSelf()
        {
            string input1 = "1";
            string input2 = "  12 ";

            int expectedResult1 = 1;
            int expectedResult2 = 12;
            
            int result1 = parser.Parse(input1);
            int result2 = parser.Parse(input2);

            Assert.Equal(expectedResult1, result1);
            Assert.Equal(expectedResult2, result2);
        }

        [Fact]
        public void SolvesSimpleExpressions()
        {
            string input1 = "1 + 1";
            string input2 = "1 - 1";
            string input3 = "2 * 2";
            string input4 = "2 / 2";

            int expectedResult1 = 2;
            int expectedResult2 = 0;
            int expectedResult3 = 4;
            int expectedResult4 = 1;

            int result1 = parser.Parse(input1);
            int result2 = parser.Parse(input2);
            int result3 = parser.Parse(input3);
            int result4 = parser.Parse(input4);

            Assert.Equal(expectedResult1, result1);
            Assert.Equal(expectedResult2, result2);
            Assert.Equal(expectedResult3, result3);
            Assert.Equal(expectedResult4, result4);
        }

        [Fact]
        public void SolvesComplexExpressions()
        {
            string input1 = "((3 + 5) / 2) * (4 - 1)";
            string input2 = "4 + 2 * 3 - 8 / 2";

            int expectedResult1 = 12;
            int expectedResult2 = 6;

            int result1 = parser.Parse(input1);
            int result2 = parser.Parse(input2);

            Assert.Equal(expectedResult1, result1);
            Assert.Equal(expectedResult2, result2);
        }
    }
}
