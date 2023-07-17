using System.Text;

namespace FooterEditor
{
    public class FileHandler : IFileHandler
    {
        private FileInfo _fileInfo;
        
        public FileHandler(string filePath)
        {
            var tempInfo = new FileInfo(filePath);
            
            //TODO check folde path not contains forrbide signs                        
            //bool containsInvalidChars = (!string.IsNullOrEmpty(dir) && dir.IndexOfAny(Path.GetInvalidPathChars()) >= 0);
            bool nameContainsInvalid = (!string.IsNullOrEmpty(tempInfo.Name) && tempInfo.Name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0);

            if (string.IsNullOrWhiteSpace(filePath) || nameContainsInvalid)
            {
                throw new ArgumentException($"Inserted file path {filePath} is invalid.");
            }
            string fileInfoPath = Path.IsPathRooted(filePath) ? filePath : Path.Combine(Directory.GetCurrentDirectory(), filePath);                
            _fileInfo = new FileInfo(fileInfoPath);
        }

        public string ReadFromEnd(int byteCount)
        {
            if (!CanAccessFile)
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
                    return GetString(buffer);
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
                    return GetString(buffer);
                }

            }
        }


        private string GetString(byte[] bytes)
        {
            string text = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            return text;
        }

        
        private byte[] GetBytes(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return bytes;
        }
        
        public Tuple<int, string> SplitBySubstring(string inputString, string searchTag)
        {
            int index = inputString.LastIndexOf(searchTag);
            string? substring = null;
            if (index != -1)
            {
                substring = inputString.Substring(index, inputString.Length-index).Trim();
            }
            else
            {
                Console.WriteLine("Search tag not found");                                
            }
            return Tuple.Create(index, substring);

        }

        public void WriteToEnd(string content, int startPossition)
        {   
            if (!CanAccessFile)
            {
                Console.WriteLine("Unable to write to the file. No changes would be made.");
                return;
            }
            startPossition = _fileInfo.Length <= startPossition ? 0 : startPossition;
            using (FileStream fs = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                byte[] bytesToWrite = GetBytes(content);
                
                fs.SetLength(_fileInfo.Length-startPossition);
                fs.Seek(0, SeekOrigin.End);
                
                fs.Write(bytesToWrite, 0, bytesToWrite.Length);
            }

        }

        private bool CanAccessFile { get=> _fileInfo.Exists && !_fileInfo.IsReadOnly() && !_fileInfo.IsLocked(); }

    }
}
