using Xunit;
using FooterEditor;
using System.Collections.Generic;
using System.Linq;

namespace Test_FooterEditor
{
    public class Test_Footer_EditProperty
    {
        [Fact]
        public void EditExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput, FooterInputs.headTag);
            Dictionary<string, string> expected = new Dictionary<string, string>(FooterInputs.correctProps);
            string prop = expected.Keys.First();
            string newVal = "editedProperty";
            expected[prop] = newVal;

            footer.EditPropety(prop, newVal);

            Assert.Equal(expected, footer.Properties);
        }

        public static IEnumerable<object[]> GetEditNonExistinProperty()
        {
            yield return new object[] { "new property" };
            yield return new object[] { "  new property  " };
        }
        [Theory]
        [MemberData(nameof(GetEditNonExistinProperty))]
        public void EditNonExistinProperty(string prop)
        {
            Footer footer = new Footer(FooterInputs.correctInput, FooterInputs.headTag);
            Dictionary<string, string> expected = new Dictionary<string, string>(FooterInputs.correctProps);
            
            string newVal = "editedProperty";
            expected.Add(prop.Trim(),newVal);

            footer.EditPropety(prop, newVal);

            Assert.Equal(expected, footer.Properties);
        }
    }
}