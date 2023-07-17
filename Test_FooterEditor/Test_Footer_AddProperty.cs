using Xunit;
using FooterEditor;
using System.Collections.Generic;
using System.Linq;

namespace Test_FooterEditor
{
    public class Test_Footer_AddProperty
    {
        public static IEnumerable<object[]> GetAddPropertyInConstructorData()
        {
            yield return new object[] { FooterInputs.correctProps,          FooterInputs.correctInput };
            yield return new object[] { FooterInputs.duplicateProps,        FooterInputs.duplicatePropsInput };
            yield return new object[] { FooterInputs.corruptedProps,        FooterInputs.corruptedPropsInput };
            yield return new object[] { FooterInputs.corruptedOnlyValProps, FooterInputs.corruptedOnlyValInput };
            yield return new object[] { FooterInputs.emptyProps,            FooterInputs.emptyPropInput };
        }        
        [Theory]
        [MemberData(nameof(GetAddPropertyInConstructorData))]
        public void AddPropertyInConstructor(Dictionary<string, string> expectedResult, string input)
        {
            Footer footer = new Footer(input, FooterInputs.headTag);

            Assert.Equal(expectedResult, footer.Properties);
        }

        public static IEnumerable<object[]> GetAddPropertyData()
        {
            yield return new object[] { "new property" };
            yield return new object[] { "  new property  " };
        }       

        [Theory]
        [MemberData(nameof(GetAddPropertyData))]
        public void AddNewProperty(string propName)
        {
            Footer footer = new Footer(FooterInputs.correctInput, FooterInputs.headTag);
            Dictionary<string, string> expected = new Dictionary<string, string>(FooterInputs.correctProps);
            expected.Add("new property", "newValue");

            footer.AddProperty(propName, "newValue");

            Assert.Equal(expected, footer.Properties);
        }

        public static IEnumerable<object[]> GetAddExistingOrEmptyPropertyData()
        {
            yield return new object[] { FooterInputs.correctProps.Keys.First()};
            yield return new object[] { ""};
            yield return new object[] { "  "};
            yield return new object[] { null};
        }
        [Theory]
        [MemberData(nameof(GetAddExistingOrEmptyPropertyData))]
        public void AddExistingOrEmptyProperty(string propName)
        {
            Footer footer = new Footer(FooterInputs.correctInput, FooterInputs.headTag);
                        
            footer.AddProperty(propName, "newValue");

            Assert.Equal(FooterInputs.correctProps, footer.Properties);
        }
    }
}