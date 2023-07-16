using System.Text;

namespace FooterEditor
{
    public class FileHandler : IFileHandler
    {
        private FileInfo _fileInfo;
        
        public FileHandler(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && filePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                throw new ArgumentException("Inserted file path contains invalid characters");
            }
            string fileInfoPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(Directory.GetCurrentDirectory(), filePath);
            _fileInfo = new FileInfo(fileInfoPath);
        }

        public byte [] ReadBytes(int byteCount)
        {
            if (!CanWriteFile)
            {
                Console.WriteLine("Can not read the file.");
                return null;
            }

            using (FileStream fs = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs.Length <= byteCount)
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    return buffer;
                }
                else
                {
                    byte[] buffer = new byte[byteCount];
                    long bytesRead = 0;
                    long totalBytesRead = 0;

                    while (totalBytesRead < byteCount)
                    {
                        long remainingBytes = byteCount - totalBytesRead;
                        int chunkSize = (int)Math.Min(remainingBytes, buffer.Length);

                        fs.Seek(-chunkSize, SeekOrigin.End);
                        bytesRead = fs.Read(buffer, 0, chunkSize);

                        if (bytesRead == 0)
                        {
                            // Reached the start of the file before reading the desired byte count
                            break;
                        }

                        totalBytesRead += bytesRead;
                    }

                    //return Encoding.UTF8.GetString(buffer, 0, (int)totalBytesRead);
                    return buffer;
                }

            }
        }


        private string DecodeBytes(byte[] bytes)
        {
            string text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return text;
        }

        public string ExctractTextFromSearchToTheEnd(string inputString, string searchTag)
        {
            // int firsIndex = inputString.IndexOf(searchString);
            // int count = 1;
            // while (index >= 0)
            // {
            //     Console.WriteLine($"Found at index {index}");
            //     index = inputString.IndexOf(searchString, index + 1);
            // }
            string trimmedString = inputString.Trim();

            int index = trimmedString.LastIndexOf(searchTag);
            if (index != -1)
            {
                return trimmedString.Substring(index, trimmedString.Length-index);
            }
            else
            {
                Console.WriteLine("Search tag not found");
                return null;
            }

        }

        public void Write(string content)
        {
            throw new NotImplementedException();
        }

        private bool CanWriteFile { get=> _fileInfo.Exists && !_fileInfo.IsReadOnly() && !_fileInfo.IsLocked(); }

    }
}
