namespace FileLogger
{
    public interface IFileLogger
    {
        void Log(params object?[] messages);
        void Activate(bool activate);
    }
}