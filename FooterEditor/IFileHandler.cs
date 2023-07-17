namespace FooterEditor
{
    public interface IFileHandler
    {
        public string ReadFromEnd(int byteCount);
        public Tuple<int, string> SplitBySubstring(string inputString, string searchTag);
        public void WriteToEnd(string content, int startPossition);
    }
}
