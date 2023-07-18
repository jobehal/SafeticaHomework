using FooterEditor;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Xunit;

namespace Test_FooterEditor
{

    public class Test_FileHandler_Write
    {
        [Fact]
        public void WriteToSecuredFiles()
        {
            FileInfo fileInfo = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.readOnlyFile));
            FileHandlerTestInputs.SetSecurity(fileInfo, AccessControlType.Deny);
            FileHandler reader = new FileHandler(fileInfo.FullName);

            Assert.Throws<UnauthorizedAccessException>(() => reader.WriteToEnd("123456", 0));
            FileHandlerTestInputs.SetSecurity(fileInfo, AccessControlType.Allow);
        }

        [Fact]
        public void WriteToReadOnlyFiles()
        {
            FileInfo filePath = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.readOnlyFile));
            File.SetAttributes(filePath.FullName, FileAttributes.ReadOnly);
            FileHandler reader = new FileHandler(filePath.FullName);

            Assert.Throws<UnauthorizedAccessException>(() => reader.WriteToEnd("123456", 0));            
            File.SetAttributes(filePath.FullName, FileAttributes.Normal);
        }
        
        [Fact]
        public void WriteToNonExistingFiles()
        {
            var filePath = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.nonExistingFile));
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
        [InlineData(FileHandlerTestInputs.longFile, 0)]
        [InlineData(FileHandlerTestInputs.longFile, 200)]
        [InlineData(FileHandlerTestInputs.shortFile, 200)]
        public void WriteEmptyFooter(string fileName, int writePossition)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(fileName));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_noFooter.txt"), true);
            string newText = "";
            long expecteLength = originalFile.Length>writePossition ? originalFile.Length - writePossition : originalFile.Length;

            var reader = new FileHandler(newFile.FullName);           
            reader.WriteToEnd(newText, writePossition);

            Thread.Sleep(5);
            string fileEndText = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, newText.Length);


            Assert.Equal(newText, fileEndText);
            Assert.Equal(expecteLength, newFile.Length);

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