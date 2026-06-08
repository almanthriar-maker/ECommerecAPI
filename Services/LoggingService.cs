using System.Text;

namespace E_CommerceSystem_API.Services
{
    public class LoggingService
    {
        private readonly string _filePath;

        public LoggingService()
        {
            _filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Logs",
                "log.txt");

            Directory.CreateDirectory(
                Path.GetDirectoryName(_filePath)!);
        }

        public void Log(string input)
        {
            string message =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {input}";

            File.AppendAllText(
                _filePath,
                message + Environment.NewLine,
                Encoding.UTF8);
        }


    }
}
