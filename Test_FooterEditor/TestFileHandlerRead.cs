using FooterEditor;
using System.IO;
using Xunit;

namespace Test_FooterEditor
{
    public class TestFileHandlerExtract
    {
        [Theory]
        [InlineData("[A]123\n456")]
        [InlineData("xxxxxxx[A]123\n456")]
        [InlineData("xxxxxxx[A]123\n456       ")]        
        public void ExtractString(string inputString)
        {
            var reader = new FileHandler("LongTestFile.txt");

            var output = reader.ExctractTextFromSearchToTheEnd(inputString, "[A]");

            Assert.Equal("[A]123\n456", output);
        }
    }
    public class TestFileHandlerRead
    {
        private string localDir { get => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName; }
        
        [Fact]
        public void ReadFileLocalPath()
        {
            var reader = new FileHandler("LongTestFile.txt");

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Equal(1024, ret.Length);
        }

        [Theory]
        [InlineData("LongTestFile.txt")]
        [InlineData("HiddenFile.txt")]
        [InlineData("LongTestFile.pdf")]
        public void ReadExistingLongFile(string fileName)
        {
            var reader = new FileHandler(Path.Combine(localDir, fileName));

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Equal(1024, ret.Length);
        }

        [Fact]
        public void ReadExistingShortFile()
        {
            var reader = new FileHandler(Path.Combine(localDir, "ShortTestFile.txt"));

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Equal(118, ret.Length);
        }

        [Theory]
        [InlineData("ReadOnlyFile.txt")]
        [InlineData("NonExisting.txt")]
        [InlineData("LockedFile.txt")]        
        public void ReadUnaccesableFiles(string fileName)
        {
            var reader = new FileHandler(Path.Combine(localDir, fileName));

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Null(ret);
        }

        [Fact]
        public void ReadFileLockedByAnotherProcess()
        {
            string filePath = Path.Combine(localDir, "LongTestFile.txt");
            //lock the file
            var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

            var reader = new FileHandler(filePath);

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Null(ret);
            file.Close();
        }

        [Fact]
        public void ReadBinary()
        {
            var reader = new FileHandler(Path.Combine(localDir, "LongTestFile.pdf"));

            byte[]? ret = reader.ReadBytes(1024);

            Assert.Equal(1024, ret.Length);
        }


    }
}