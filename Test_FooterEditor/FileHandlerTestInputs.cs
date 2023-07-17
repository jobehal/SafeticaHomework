using System.IO;

namespace Test_FooterEditor
{
    internal static class FileHandlerTestInputs
    {
        internal const string longFile = "LongTestFile.txt";
        internal const string shortFile = "ShortTestFile.txt";
        internal const string readOnlyFile = "ReadOnlyTestFile.txt";
        internal const string nonExistingFile = "NonExisting.txt";
        internal const string lockedFile = "LockedTestFile.txt";
        internal const string emptyFile = "EmptyTestfile.txt";
        internal const string hiddenFile = "HiddenTestFile.txt";

        internal static string GetFilePath(string filePath)
        {
            var solDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            
            return Path.Combine(solDir, "TestFiles", filePath);
        }
    }
}