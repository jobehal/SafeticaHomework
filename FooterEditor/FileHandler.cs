﻿using System.Text;

namespace FooterEditor
{
    public class FileHandler : IFileHandler
    {
        private FileInfo _fileInfo;
        private bool CanAccessFile { get => _fileInfo.Exists && !_fileInfo.IsReadOnly() && !_fileInfo.IsLocked(); }
        //  
        //  CTOR  
        //  

        public FileHandler(string filePath)
        {
            FileInfo tempInfo = new FileInfo(filePath);
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
                throw new IOException($"Can not read the file. {_fileInfo.FullName}");                
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
                    return GetString(buffer);
                }
            }
        }
        
        public Tuple<int, string> SplitBySubstring(string inputString, string searchTag)
        {
            int index = -1;
            if (!string.IsNullOrWhiteSpace(inputString) && !string.IsNullOrWhiteSpace(searchTag))
            {
                index = inputString.LastIndexOf(searchTag);
            }

            string? substring = null;
            int byteIndex = 0;
            if (index != -1)
            {
                string fullsubstring = inputString.Substring(index, inputString.Length-index);
                byteIndex = GetBytes(fullsubstring).Length;
                substring = fullsubstring.Trim();
            }
            return Tuple.Create(byteIndex, substring);
        }

        public void WriteToEnd(string content, int startPossition)
        {   
            if (!CanAccessFile)
            {
                throw new IOException($"Unable to write to the file {_fileInfo.FullName}.\n No changes would be made.");                
            }
            
            // Write to the end if the start pos is not fall into file
            startPossition = _fileInfo.Length <= startPossition ? 0 : startPossition;
            
            using (FileStream fs = new FileStream(_fileInfo.FullName, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                byte[] bytesToWrite = GetBytes(content);
                
                fs.SetLength(_fileInfo.Length - startPossition);
                fs.Seek(0, SeekOrigin.End);                
                fs.Write(bytesToWrite, 0, bytesToWrite.Length);
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
    }
}
