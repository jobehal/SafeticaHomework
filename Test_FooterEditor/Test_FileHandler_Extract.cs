using FooterEditor;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileHandler_Extract
    {
        [Theory]
        [InlineData(10, "[A]123\n456")]
        [InlineData(10, "    [A]123\n456")]
        [InlineData(10, "xxxx[A]123\n456")]
        [InlineData(17, "xxxx[A]123\n456       ")]        
        public void ExtractString(int expectedIndex, string inputString)
        {
            var reader = new FileHandler("LongTestFile.txt");

            var output = reader.SplitBySubstring(inputString, "[A]");
            int index = output.Item1;
            string substring = output.Item2;

            Assert.Equal("[A]123\n456", substring);
            Assert.Equal(expectedIndex, index);
        }
    }
}