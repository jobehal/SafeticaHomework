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
                throw new IOException($"File is locked {err.Message}");
            }
            catch (UnauthorizedAccessException err)
            {
                throw new UnauthorizedAccessException($"Unauthorzed access {err.Message}");                
            }
        }

        internal static bool IsReadOnly(this FileInfo fileInfo)
        {
            var attributes = File.GetAttributes(fileInfo.FullName);
            if ((attributes & FileAttributes.ReadOnly) != 0)
            {
                throw new UnauthorizedAccessException("File has read-only permission.");
            }
            else
            {
                return false;
            }
        }
    }
}

