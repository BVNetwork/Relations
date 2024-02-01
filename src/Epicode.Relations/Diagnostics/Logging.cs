using EPiServer.Logging;

namespace EPiCode.Relations.Diagnostics
{
    public class Logging
    {
        private static readonly ILogger Instance = LogManager.GetLogger(typeof(Logging));

        public static void Debug(string message) {
            Instance.Debug(message);
        }

        public static void Error(string message)
        {
            Instance.Error(message);
        }
        
        public static void Warning(string message)
        {
            Instance.Warning(message);
        }
    }
}