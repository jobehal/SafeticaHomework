using System;
using System.IO;
using System.Threading;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileFooterEditor
    {
        private const string longExpected_add                = "[SafeticaProperties]\nproperty1=value1\nproperty2=value2\naddedProp=123";
        private const string longExpected_edit               = "[SafeticaProperties]\nproperty1=edited\nproperty2=value2";
        private const string longExpected_remove_nonExisting = "[SafeticaProperties]\nproperty1=value1\nproperty2=value2";
        private const string longExpected_remove_exising     = "[SafeticaProperties]\nproperty2=value2";
        private const int    longBytesLength_add = 68;
        private const int    longBytesLength_edit = 54;
        private const int    longBytesLength_remove_existing = 37;
        private const int    longBytesLength_remove_nonexisting = 54;
        
        
        private const string emptyExpected_add = "﻿[SafeticaProperties]\naddedProp=123";
        private const int emptybytesLength_add = 37;
        [Theory]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.longFile)]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.shortFile)]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.hiddenFile)]
        [InlineData(emptyExpected_add, emptybytesLength_add, FileHandlerTestInputs.emptyFile)]
        public void AddMethod(string expected, int bytesLength, string inputFile)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(inputFile));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_prog_add.txt"), true);


            FileFooterEditor footerEditor = new FileFooterEditor(newFile.FullName);
            footerEditor.Execute("add", "addedProp=123");

            var fileEndString = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, bytesLength);
            Assert.Equal(expected, fileEndString);

        }

        [Theory]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.longFile)]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.shortFile)]
        [InlineData(longExpected_add, longBytesLength_add, FileHandlerTestInputs.hiddenFile)]
        [InlineData(emptyExpected_add, emptybytesLength_add, FileHandlerTestInputs.emptyFile)]
        public void EditNewProp(string expected, int bytesLength, string inputFile)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(inputFile));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_prog_edit_new.txt"), true);


            FileFooterEditor footerEditor = new FileFooterEditor(newFile.FullName);
            footerEditor.Execute("edit", "addedProp=123");

            var fileEndString = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, bytesLength);
            Assert.Equal(expected, fileEndString);
        }

        [Theory]
        [InlineData(longExpected_edit, longBytesLength_edit, FileHandlerTestInputs.longFile)]
        [InlineData(longExpected_edit, longBytesLength_edit, FileHandlerTestInputs.shortFile)]
        [InlineData(longExpected_edit, longBytesLength_edit, FileHandlerTestInputs.hiddenFile)]        
        public void EditExistingProp(string expected, int bytesLength, string inputFile)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(inputFile));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_prog_edit_existing.txt"), true);

            FileFooterEditor footerEditor = new FileFooterEditor(newFile.FullName);
            footerEditor.Execute("edit", "property1=edited");

            var fileEndString = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, bytesLength);
            Assert.Equal(expected, fileEndString);
        }


        [Theory]
        [InlineData(longExpected_remove_exising, longBytesLength_remove_existing, FileHandlerTestInputs.longFile)]
        [InlineData(longExpected_remove_exising, longBytesLength_remove_existing, FileHandlerTestInputs.shortFile)]
        [InlineData(longExpected_remove_exising, longBytesLength_remove_existing, FileHandlerTestInputs.hiddenFile)]
        
        public void RemoveExistingProp(string expected, int bytesLength, string inputFile)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(inputFile));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_prog_remove_existing.txt"), true);

            FileFooterEditor footerEditor = new FileFooterEditor(newFile.FullName);
            footerEditor.Execute("remove", "property1");

            var fileEndString = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, bytesLength);
            Assert.Equal(expected, fileEndString);
        }

        [Theory]
        [InlineData(longExpected_remove_nonExisting, longBytesLength_remove_nonexisting, FileHandlerTestInputs.longFile)]
        [InlineData(longExpected_remove_nonExisting, longBytesLength_remove_nonexisting, FileHandlerTestInputs.shortFile)]
        [InlineData(longExpected_remove_nonExisting, longBytesLength_remove_nonexisting, FileHandlerTestInputs.hiddenFile)]
        public void RemoveNonExistingProp(string expected, int bytesLength, string inputFile)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(inputFile));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt", "_prog_remove_nonexisting.txt"), true);

            FileFooterEditor footerEditor = new FileFooterEditor(newFile.FullName);
            footerEditor.Execute("remove", "nonExisting");

            var fileEndString = FileHandlerTestInputs.ReadFileEnd(newFile.FullName, bytesLength);
            Assert.Equal(expected, fileEndString);
        }

        [Fact]
        public void CallNonExistingMethod()
        {   
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.longFile));            

            FileFooterEditor footerEditor = new FileFooterEditor(originalFile.FullName);
            Assert.Throws<ArgumentException>(() => footerEditor.Execute("nonExisting", "nonExisting"));
        }
    
        [Theory]    
        [InlineData("add")]
        [InlineData("edit")]
        public void NotDefinedValueThrowException(string method)
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.longFile));

            FileFooterEditor footerEditor = new FileFooterEditor(originalFile.FullName);
            Assert.Throws<ArgumentException>(() => footerEditor.Execute(method, "onlyProp"));
        }

        [Fact]
        public void WriteOversizedFooter()
        {
            FileInfo originalFile = new FileInfo(FileHandlerTestInputs.GetFilePath(FileHandlerTestInputs.longFile));

            FileFooterEditor footerEditor = new FileFooterEditor(originalFile.FullName);
            string longProp = new string('a', 1024);

            Assert.Throws<ArgumentException>(() => footerEditor.Execute("add", $"long={longProp}"));

        }

    }
}