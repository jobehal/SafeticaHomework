using FooterEditor;
using System;
using System.IO;
using System.Threading;
using System.Security.Principal;
using System.Security.AccessControl;
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

        [Fact]
        public void ReadSecuredFile()
        {
            var fileInfo = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.lockedFile));

            FileHandlerTestInputs.SetSecurity(fileInfo,AccessControlType.Deny);
            var reader = new FileHandler(fileInfo.FullName);

            Assert.Throws<UnauthorizedAccessException>(() => reader.ReadFromEnd(1024));
            FileHandlerTestInputs.SetSecurity(fileInfo,AccessControlType.Allow);
        }

        [Fact]
        public void ReadReadOnlyFile()
        {
            string filePath = FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.readOnlyFile);
            File.SetAttributes(filePath, FileAttributes.ReadOnly);
            
            var reader = new FileHandler(filePath);

            Assert.Throws<UnauthorizedAccessException>  (() => reader.ReadFromEnd(1024));
            File.SetAttributes(filePath, FileAttributes.Normal);
        }

        [Fact]
        public void NonExistingFile()
        {
            var reader = new FileHandler(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.nonExistingFile));

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