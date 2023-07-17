using Xunit;
using FooterEditor;
using System.Collections.Generic;
using System.Linq;

namespace Test_FooterEditor
{
    public class Test_FooterRemove_Property
    {
        [Fact]
        public void RemoveExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            Dictionary<string, string> expected = new Dictionary<string, string>(FooterInputs.correctProps);
            string prop = expected.Keys.First();
            
            expected.Remove(prop);

            footer.RemoveProperty(prop);

            Assert.Equal(expected, footer.Properties);
        }

        [Fact]
        public void RemoveNonExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            Dictionary<string, string> expected = new Dictionary<string, string>(FooterInputs.correctProps);
            
            footer.RemoveProperty("   test non existing");

            Assert.Equal(expected, footer.Properties);
        }

    }
}