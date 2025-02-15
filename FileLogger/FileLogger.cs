using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    public class FileLogger : IFileLogger
    {
        private string _logPath;
        private readonly string _logFolder;
        private readonly StringBuilder _logBuilder = new();
        private bool _active;
        private readonly string _dateTimeFormat;
        private DateTime _currentDate;
        private Action<Exception, StringBuilder> _onException;

        public FileLogger(string folderName, string? folderDirectory = null, string? dateTimeFormat = null, Action<Exception, StringBuilder> onException = null)
        {
            folderDirectory ??= Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            _dateTimeFormat = dateTimeFormat ?? "dddd, dd MMMM yyyy";
            _logFolder = Path.Combine(folderDirectory, folderName);
            _onException = onException;
            Setup();
        }

        [MemberNotNull("_logPath")]
        private void Setup()
        {
            _currentDate = DateTime.Today;
            _logPath = Path.Combine(_logFolder, $"{_currentDate.ToString(_dateTimeFormat)}.log");
            if (!Directory.Exists(_logFolder))
            {
                Directory.CreateDirectory(_logFolder);
            }
            Activate(true);
        }

        public void Activate(bool activate)
        {
            _active = activate;
            if (activate) _ = UpdateLog();
        }

        public void Log(object? message)
        {
            _logBuilder.AppendLine($"{DateTime.Now.ToString("HH:mm:ss:fff")}: {message}");
        }

        private async Task UpdateLog()
        {
            while (_active)
            {
                await Task.Delay(5000); //5 seconds
                if (_currentDate != DateTime.Today)
                {
                    Setup();
                    return;
                }
                if (_logBuilder.Length <= 0 || !_active) continue;
                try
                {
                    await File.AppendAllTextAsync(_logPath, _logBuilder.ToString());
                }
                catch (FileNotFoundException)
                {
                    Setup();
                    return;
                }
                catch (Exception ex)
                {
                    _onException?.Invoke(ex, _logBuilder);
                }

                _logBuilder.Clear();
            }
        }
    }
}
