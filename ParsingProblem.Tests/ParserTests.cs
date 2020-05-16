using System;
using ParsingProblem;
using Xunit;

namespace ParsingProblem.Tests
{
    public class ParserTests
    {
        [Fact]
        public void EmptyStringShouldThrowException(){
            var input1 = "";
            var input2 = "  ";

            Assert.Throws<FormatException>(() => Parser.Parse(input1));
            Assert.Throws<FormatException>(() => Parser.Parse(input2));
        }
        [Fact]
        public void AlphaShouldThrowException()
        {
            var input1 = "a";
            var input2 = "3a + 4b";
            Assert.Throws<FormatException>(() => Parser.Parse(input1));
            Assert.Throws<FormatException>(() => Parser.Parse(input2));
        }
    }
}
