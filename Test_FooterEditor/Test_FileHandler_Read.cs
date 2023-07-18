using FooterEditor;
using System.IO;
using System.Threading;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileHandler_Read
    {   
        [Fact]
        public void ReadFileLocalPath()
        {
            var reader = new FileHandler(FileHandlerTestInputs.longFile);

            string ret = reader.ReadFromEnd(1024);

            Assert.Equal(1024, ret.Length);
        }

        [Theory]
        [InlineData(FileHandlerTestInputs.longFile)]
        [InlineData(FileHandlerTestInputs.hiddenFile)]        
        public void ReadExistingLongFile(string fileName)
        {
            var reader = new FileHandler(FileHandlerTestInputs.GetFilePath(fileName));

            var ret = reader.ReadFromEnd(1024);

            Assert.Equal(1024, ret.Length);
        }

        [Theory]
        [InlineData(1, FileHandlerTestInputs.emptyFile)]
        [InlineData(116, FileHandlerTestInputs.shortFile)]
        public void ReadExistingShortFile(int expectedLength, string fileName)
        {
            Thread.Sleep(5);
            var fileInfo = new FileInfo(FileHandlerTestInputs.GetFilePath(fileName));
            var reader = new FileHandler(fileInfo.FullName);

            var ret = reader.ReadFromEnd(1024);

            Assert.Equal(expectedLength, ret.Length);
        }

        [Theory]
        [InlineData(FileHandlerTestInputs.readOnlyFile)]
        [InlineData(FileHandlerTestInputs.nonExistingFile)]
        [InlineData(FileHandlerTestInputs.lockedFile)]        
        public void ReadUnaccesableFiles(string fileName)
        {
            var reader = new FileHandler(FileHandlerTestInputs.GetFilePath(fileName));

            Assert.Throws<IOException>(() => reader.ReadFromEnd(1024));
        }

        [Fact]
        public void ReadFileLockedByAnotherProcess()
        {
            string filePath = Path.Combine(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.longFile));
            //lock the file
            var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

            var reader = new FileHandler(filePath);

            Assert.Throws<IOException>(() => reader.ReadFromEnd(1024));
            
            file.Close();
            Thread.Sleep(5);
        }

    }
}