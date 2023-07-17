using FooterEditor;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Test_FooterEditor
{
    public class Test_FileHandler_Write
    {
        private string localDir { get => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName; }
        
        private static string ReadFileEnd(string filePath, int size)
        {
            byte[] buffer = new byte[size];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(-size, SeekOrigin.End);
                fs.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }

        [Theory]
        [InlineData("ReadOnlyFile.txt")]
        [InlineData("NonExisting.txt")]
        [InlineData("LockedFile.txt")]
        public void WriteUnaccesableFiles(string fileName)
        {
            var filePath = new FileInfo(Path.Combine(localDir, fileName));
            long expectedLenght = filePath.Exists ? filePath.Length : -1;
            FileHandler reader = new FileHandler(filePath.FullName);

            reader.WriteToEnd("123456", 0);            

            long newLength = filePath.Exists ? filePath.Length : -1;
            Assert.Equal(expectedLenght, newLength);
        }


        [Theory]
        [InlineData("LongTestFile.txt")]
        [InlineData("ShortTestFile.txt")]
        [InlineData("Empty.txt")]
        [InlineData("HiddenFile.txt")]
        public void WriteNewFooter(string fileName)
        {
            FileInfo originalFile = new FileInfo(Path.Combine(localDir, fileName));
            FileInfo newFile = originalFile.CopyTo(originalFile.FullName.Replace(".txt","_new.txt"), true);

            var reader = new FileHandler(newFile.FullName);
            string newText = "\nThis is new appended text";
            reader.WriteToEnd(newText, 0);

            Thread.Sleep(5);
            string fileEndText = ReadFileEnd(newFile.FullName, newText.Length);
            Assert.Equal(newText, fileEndText);
            Assert.Equal(originalFile.Length + newText.Length, newFile.Length);
        }

        [Theory]
        [InlineData("LongTestFile.txt")]
        [InlineData("ShortTestFile.txt")]
        [InlineData("Empty.txt")]
        [InlineData("HiddenFile.txt")]
        public void OverWriteFooter(string fileName)
        {
            FileInfo originalFile = new FileInfo(Path.Combine(localDir, fileName));
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

            string fileEndText = ReadFileEnd(newFile.FullName, newText.Length);
            Assert.Equal(newText, fileEndText);
            Assert.Equal(expectdLength, newFile.Length);
        }

    }
}