using Xunit;
using FooterEditor;
using System;

namespace Test_FooterEditor
{

    public class TestFooterConstructor
    {        
        [Theory]
        [InlineData(FooterInputs.noHeadInput)]
        [InlineData(FooterInputs.wrongHeadInput)]
        public void CorruptedHeadRaisException(string input)
        {
            Assert.Throws<ArgumentException>(() => new Footer(input));
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void NullOrEmptyInputRiseException(string input)
        {
            Assert.Throws<ArgumentNullException>(() => new Footer(input));
        }

        [Theory]
        [InlineData(FooterInputs.correctInput)]
        [InlineData(FooterInputs.corruptedPropsInput)]
        public void NotNullAfterCorrectInput(string input)
        {            
            Footer footer = new Footer(input);
         
            Assert.NotNull(footer);
        }
    }
}