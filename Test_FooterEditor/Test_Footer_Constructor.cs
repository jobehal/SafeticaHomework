using Xunit;
using FooterEditor;
using System;

namespace Test_FooterEditor
{

    public class Test_Footer_Constructor
    {        
        [Theory]
        [InlineData(FooterInputs.noHeadInput)]
        [InlineData(FooterInputs.wrongHeadInput)]
        public void CorruptedHeadRaisException(string input)
        {
            Assert.Throws<ArgumentException>(() => new Footer(input, FooterInputs.headTag));
        }
        
        // [Theory]
        // [InlineData(null)]
        // [InlineData("")]
        // [InlineData("  ")]
        // public void NullOrEmptyInputRiseException(string input)
        // {
        //     Assert.Throws<ArgumentNullException>(() => new Footer(input, FooterInputs.headTag));
        // }

        [Theory]
        [InlineData(FooterInputs.correctInput)]
        [InlineData(FooterInputs.corruptedPropsInput)]
        public void NotNullAfterCorrectInput(string input)
        {            
            Footer footer = new Footer(input, FooterInputs.headTag);
         
            Assert.NotNull(footer);
        }
    }
}