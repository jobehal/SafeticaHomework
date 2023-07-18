using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

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

        internal static void SetSecurity(FileInfo fileInfo, AccessControlType type)
        {
            FileSecurity fileSecurity = fileInfo.GetAccessControl();
            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            SecurityIdentifier currentUserSid = currentUser.User;
            FileSystemAccessRule ownerRule = new FileSystemAccessRule(currentUserSid, FileSystemRights.FullControl, type);
            fileSecurity.PurgeAccessRules(currentUserSid);
            fileSecurity.AddAccessRule(ownerRule);

            fileInfo.SetAccessControl(fileSecurity);
        }

        internal static string GetFilePath(string filePath)
        {
            var solDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            
            return Path.Combine(solDir, "TestFiles", filePath);
        }

        internal static string ReadFileEnd(string filePath, int size)
        {
            byte[] buffer = new byte[size];
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                fs.Seek(-size, SeekOrigin.End);
                fs.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer);
            }
        }
    }
}