namespace FooterEditor
{
    internal static class Extensions
    {
        internal static bool IsLocked(this FileInfo fileInfo)
        {
            try
            {                
                using (FileStream stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {                    
                    return false;
                }
            }
            catch (IOException err)
            {
                Console.WriteLine($"File is locked {err}");
                return true;
            }
            catch (UnauthorizedAccessException err)
            {
                Console.WriteLine($"Unauthorzed access {err}");
                return true;
            }
        }

        internal static bool IsReadOnly(this FileInfo fileInfo)
        {
            var attributes = File.GetAttributes(fileInfo.FullName);
            if ((attributes & FileAttributes.ReadOnly) != 0)
            {
                return true;
                Console.WriteLine("File has read-only permission.");
            }
            else
            {
                return false;
            }
        }
    }
}

