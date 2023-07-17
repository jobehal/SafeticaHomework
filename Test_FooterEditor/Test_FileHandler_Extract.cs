using FooterEditor;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileHandler_Extract
    {
        private static FileHandler dummyReader = new FileHandler("LongTestFile.txt");
        [Theory]
        [InlineData(10, "[A]123\n456")]
        [InlineData(10, "    [A]123\n456")]
        [InlineData(10, "xxxx[A]123\n456")]
        [InlineData(17, "xxxx[A]123\n456       ")]        
        public void ExtractString(int expectedIndex, string inputString)
        {

            var output = dummyReader.SplitBySubstring(inputString, "[A]");
            int index = output.Item1;
            string substring = output.Item2;

            Assert.Equal("[A]123\n456", substring);
            Assert.Equal(expectedIndex, index);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void NullSearchTag(string tag)
        {
            var output = dummyReader.SplitBySubstring("123", tag);
            int index = output.Item1;
            string substring = output.Item2;

            Assert.Null(substring);
            Assert.Equal(0, index);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void NullInputString(string inp)
        {
            var output = dummyReader.SplitBySubstring(inp, "[A]");
            int index = output.Item1;
            string substring = output.Item2;

            Assert.Null(substring);
            Assert.Equal(0, index);
        }

        [Fact]
        public void TagNotFound()
        {
            var output = dummyReader.SplitBySubstring("123", "[A]");
            int index = output.Item1;
            string substring = output.Item2;

            Assert.Null(substring);
            Assert.Equal(0, index);
        }
    }
}