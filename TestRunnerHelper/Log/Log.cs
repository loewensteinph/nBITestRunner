using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestrunnerHelper.Log
{
    public static class LogFactory
    {
        public const string Log4NetConfig = "log4net.config";

        public static ILog GetLogger(Type classType)
        {
            var uri =
                new Uri(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), Log4NetConfig));
            var configFile = new FileInfo(Path.GetFullPath(uri.LocalPath));
            XmlConfigurator.ConfigureAndWatch(configFile);
            var log = LogManager.GetLogger(classType);
            return log;
        }
    }
        public class Log
    {
        public int Id { get; set; }
        public DateTime ServerDate { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
