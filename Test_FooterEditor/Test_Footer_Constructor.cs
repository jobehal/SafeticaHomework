using Xunit;
using FooterEditor;
using System;
using System.IO;
using System.Linq;

namespace Test_FooterEditor
{

    public class Test_Footer_Constructor
    {        
        [Theory]
        [InlineData(FooterInputs.noHeadInput)]
        [InlineData(FooterInputs.wrongHeadInput)]
        [InlineData(FooterInputs.corruptedPropsInput)]        
        [InlineData(FooterInputs.corruptedOnlyValInput)]
        public void CorruptedRaisException(string input)
        {            
            Assert.Throws<ArgumentException>(() => new Footer(input, FooterInputs.headTag));
        }

        [Theory]
        [InlineData(FooterInputs.correctInput)]
        public void NotNullAfterCorrectInput(string input)
        {            
            Footer footer = new Footer(input, FooterInputs.headTag);
         
            Assert.NotNull(footer);
        }
    }
}