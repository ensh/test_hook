namespace FileMonitor
{
    public interface IPlugin
    {
        string Extension { get; }
        string Description { get; }
        object Process(System.IO.FileStream dataStream, object context);
    }
}
