using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using log4net;
using log4net.Config;

namespace Logger
{
    public static class Log
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Log));

        public static void Init()
        {
            //XmlDocument log4netConfig = new XmlDocument();
            //log4netConfig.Load(File.OpenRead("log4net.config"));
            //var repo = LogManager.GetRepository(Assembly.GetEntryAssembly());
            //log4net.Config.XmlConfigurator.Configure(repo, new FileInfo("log4net.config"));

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var configFile = new FileInfo("log4net.config");
            XmlConfigurator.Configure(logRepository, configFile);
        }

        public static void Debug(
            string msg,
            Exception e = null,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            msg = file.Split(Path.DirectorySeparatorChar).Last() + ":" + line + "(" + member + ") " + msg;
            if (e != null)
                msg += " \r\n  " + e.Message + "\r\n  " + e.StackTrace;
            Logger.Debug(msg);
        }

        public static void Info(string msg,
            Exception e = null,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            msg = file.Split(Path.DirectorySeparatorChar).Last() + ":" + line + "(" + member + ") " + msg;
            if (e != null)
                msg += " \r\n  " + e.Message + "\r\n  " + e.StackTrace;
            Logger.Info(msg);
        }

        public static void Warn(string msg,
            Exception e = null,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            msg = file.Split(Path.DirectorySeparatorChar).Last() + ":" + line + "(" + member + ") " + msg;
            if (e != null)
                msg += " \r\n  " + e.Message + "\r\n  " + e.StackTrace;
            Logger.Warn(msg);
        }

        public static void Error(string msg,
            Exception e = null,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            msg = file.Split(Path.DirectorySeparatorChar).Last() + ":" + line + "(" + member + ") " + msg;
            if (e != null)
                msg += " \r\n  " + e.Message + "\r\n  " + e.StackTrace;
            Logger.Error(msg);
        }

        public static void Fatal(string msg, Exception e = null, [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            msg = file.Split(Path.DirectorySeparatorChar).Last() + ":" + line + "(" + member + ") " + msg;
            if (e != null)
                msg += " \r\n  " + e.Message + "\r\n  " + e.StackTrace;
            Logger.Fatal(msg);
        }

    }
}
