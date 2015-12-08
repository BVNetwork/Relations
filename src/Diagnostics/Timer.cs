using System.Diagnostics;
using log4net;

namespace EPiCode.Relations.Diagnostics
{
    public class Timer
    {
        private Stopwatch _stopwatch;
        private string _logMessage;
        public Timer() {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public Timer(string logMessage) {
            Start(logMessage);
        }

        private void Start(string text) {
            _logMessage = text;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Stop() {
            _stopwatch.Stop();
            LogManager.GetLogger("DefaultLogger").Debug("* RelationTimer: " + _stopwatch.Elapsed.Seconds + "s " + _stopwatch.Elapsed.Milliseconds + "ms (" + _logMessage + ")");
        }
    }
}