using FooterEditor;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Test_FooterEditor
{

    public class Test_FileHandler_Write
    {   
        [Theory]
        [InlineData(FileHandlerTestInputs.readOnlyFile)]
        [InlineData(FileHandlerTestInputs.nonExistingFile)]
        [InlineData(FileHandlerTestInputs.lockedFile)]
        public void WriteUnaccesableFiles(string fileName)
        {
            var filePath = new FileInfo(FileHandlerTestInputs.GetFilePath(fileName));            
            FileHandler reader = new FileHandler(filePath.FullName);

            Assert.Throws<IOException>(() => reader.WriteToEnd("123456", 0));            
        }

        [Theory]
        [InlineData(FileHandlerTestInputs.longFile)]
        [InlineData(FileHandlerTestInputs.shortFile)]
        [InlineData(FileHandlerTestInputs.emptyFile)]
        [InlineData(FileHandlerTestInputs.hiddenFile)]
        public void WriteNewFooter(string fileName)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(fileName));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt","_new.txt"), true);

            var reader = new FileHandler(newFile.FullName);
            string newText = "\nThis is new appended text";
            reader.WriteToEnd(newText, 0);

            Thread.Sleep(5);
            string fileEndText = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, newText.Length);
            Assert.Equal(newText, fileEndText);
            Assert.Equal(originalFile.Length + newText.Length, newFile.Length);
        }

        [Theory]
        [InlineData(FileHandlerTestInputs.longFile)]
        [InlineData(FileHandlerTestInputs.shortFile)]
        [InlineData(FileHandlerTestInputs.emptyFile)]
        [InlineData(FileHandlerTestInputs.hiddenFile)]
        public void OverWriteFooter(string fileName)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(fileName));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_ovr.txt"), true);

            var reader = new FileHandler(newFile.FullName);
            string newText = "\nThis is new overwritten text";
            int start = 200;
            long expectdLength = -1;
            if (originalFile.Length < start)
            {
                expectdLength = originalFile.Length + newText.Length;
            }
            else
            {
                expectdLength = originalFile.Length - start + newText.Length;
            }
            
            reader.WriteToEnd(newText, start);

            string fileEndText = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, newText.Length);
            Assert.Equal(newText, fileEndText);
            Assert.Equal(expectdLength, newFile.Length);
        }
        [Fact]
        public void ReadFileLockedByAnotherProcess()
        {
            string filePath = Path.Combine(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.longFile));
            //lock the file
            var file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);

            var reader = new FileHandler(filePath);

            Assert.Throws<IOException>(() => reader.WriteToEnd("123",0));

            file.Close();
            Thread.Sleep(5);
        }

    }
}