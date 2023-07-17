using FooterEditor;
using System;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileHandler_Constructor
    {
        [Theory]
        [InlineData("")]
        [InlineData(@"o?myfile.txt")]
        //[InlineData(@"C:\*Us<ers\nmyfile.txt")]
        public void InvalidInputPath(string inputStr)
        {   
            Assert.Throws<ArgumentException>(()=> new FileHandler(inputStr));
        }

        [Theory]
        [InlineData(@"C:\Users\myfile.txt")]
        [InlineData(@"myfile.txt")]
        public void ValidInputPath(string inputStr)
        {
            var reader = new FileHandler(inputStr);

            Assert.NotNull(reader);
        }
    }
}