namespace FooterEditor
{
    public interface IFileHandler
    {
        public byte[] ReadBytes(int byteCount);
        public void Write(string content);
    }
}
