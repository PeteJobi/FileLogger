namespace FileLogger
{
    public interface IFileLogger
    {
        void Log(object? message);
        void Activate(bool activate);
    }
}