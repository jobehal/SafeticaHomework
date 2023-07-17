using Xunit;
using FooterEditor;
using System.Linq;

namespace Test_FooterEditor
{
    public class Test_Footer_ToString
    {
        [Fact]
        public void NoEditToString()
        {
            Footer footer = new Footer(FooterInputs.correctInput);

            string result = footer.ToString();

            Assert.Equal(FooterInputs.correctInput, result);
        }

        [Fact]
        public void AddProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            string prop = "   added Property   ";
            string val = "newVal";

            string expectedBase = new string(FooterInputs.correctInput);
            string expected = expectedBase + $"\n{prop.Trim()}={val}";

            footer.AddProperty(prop, val);

            string result = footer.ToString();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void EditExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            string prop = FooterInputs.correctProps.Keys.First();
            string oldVal = FooterInputs.correctProps[prop];
            string newVal  = "new  Val   .";

            string expectedBase = new string(FooterInputs.correctInput);
            string expected = expectedBase.Replace(oldVal, newVal);

            footer.EditPropety(prop, newVal);

            string result = footer.ToString();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void EditNonExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            string prop = "  test non existing...";
            string val = "editedNewValue   ";

            string expectedBase = new string(FooterInputs.correctInput);
            string expected = expectedBase + $"\n{prop.Trim()}={val}";

            footer.EditPropety(prop, val);

            string result = footer.ToString();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void RemoveNonExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);

            footer.RemoveProperty("test remove nonexiting...");

            string result = footer.ToString();

            Assert.Equal(FooterInputs.correctInput, result);
        }

        [Fact]
        public void RemoveExistingProperty()
        {
            Footer footer = new Footer(FooterInputs.correctInput);
            string prop = FooterInputs.correctProps.Keys.First();
            string val  = FooterInputs.correctProps[prop]; 
            
            string expectedBase = new string(FooterInputs.correctInput);
            string expected = expectedBase.Replace($"\n{prop}={val}", "");

            footer.RemoveProperty(prop);

            string result = footer.ToString();

            Assert.Equal(expected, result);
        }

    }
}