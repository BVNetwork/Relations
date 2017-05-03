using EPiServer.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPiCode.Relations.Diagnostics
{
    public class Logging
    {
        private static readonly ILogger _instance = LogManager.GetLogger(typeof(Logging));

        public static void Debug(string message) {
            _instance.Debug(message);
        }

        public static void Error(string message)
        {
            _instance.Error(message);
        }
        public static void Warning(string message)
        {
            _instance.Warning(message);
        }



    }
}