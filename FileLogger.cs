using System;
using System.IO;

namespace NotificationTaskScheduler
{
    public class FileLogger : ILogger,IDisposable
    {
        private readonly FileStream _file;
        private readonly StreamWriter _writer;

        public FileLogger(string fileName)
        {
            _file = new FileStream(fileName, FileMode.OpenOrCreate);
            _writer = new StreamWriter(_file);
        }
        public void Write(string message)
        {
            _writer.Write(message);
            _writer.Write(Environment.NewLine);
        }

        public void Dispose()
        {
            _writer.Flush();
            _writer.Dispose();
            _file.Dispose();
        }

    }
}