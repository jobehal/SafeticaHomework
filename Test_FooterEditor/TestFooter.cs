using Xunit;
using FooterEditor;

namespace Test_FooterEditor
{
    public class TestFooter
    {
        private static string correctInput = "[SafeticaProperties]\nproperty1=value1\nproperty2=value2";
        
        [Fact]
        public void ToStringReturnsNullIfNullinputstr()
        {
            Footer footer = new Footer(null);
            
            string? outString = footer.ToString();

            Assert.Null(outString);
        }

        [Fact]
        public void ToStringReturnsSameIfNotModified()
        {
            Footer footer = new Footer(correctInput);
         
            string? outString = footer.ToString();
            
            Assert.Equal(correctInput, outString);
        }
    }
}