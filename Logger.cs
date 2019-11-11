using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace ClearLog
{  
    public sealed class Logger
    {
        #region Log File Writing
        public FileInfo TargetLogFile { get; private set; }
        public DirectoryInfo TargetDirectory
        {
            get 
            {
                if (TargetLogFile != null)
                {
                    return TargetLogFile.Directory;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool LogToConsole = false;
        public int BatchInterval = 1000;
        public bool IgnoreDebug = false;

        private readonly Timer Timer;
        private readonly StringBuilder LogQueue = new StringBuilder();


        static List<Logger> _all_obj = new List<Logger>();

        static HashSet<string> AllPathes = new HashSet<string>();
        public Logger(string folder_name,string file_name = "log.txt")
        {
            string path_desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string path_folder = path_desktop + "\\" + folder_name;
            string path_log = path_folder + "\\"+ file_name;

            if (AllPathes.Contains(path_log))
            {
                throw new Exception("two instances use same log file");
            }
            else
            {
                AllPathes.Add(path_log);
            }

            _all_obj.Add(this);

            Timer = new Timer(FlushMsg);

            TargetLogFile = new FileInfo(path_log);
            VerifyTargetDirectory();
            File.Delete(path_log);

            Timer.Change(BatchInterval, Timeout.Infinite); // A one-off tick event that is reset every time.
        }

        private void VerifyTargetDirectory()
        {
            if (TargetDirectory == null)
                throw new DirectoryNotFoundException("Target logging directory not found.");

            TargetDirectory.Refresh();
            if (!TargetDirectory.Exists)
                TargetDirectory.Create();
        }
        private object lockFile = new object();


        public static void ShutDown()
        {
            foreach (Logger obj in _all_obj)
            {
                obj.FlushMsg(null);
            }
        }

        public void FlushMsg(object state)
        {
            try
            {
                var logMessage = "";
                lock (LogQueue)
                {
                    logMessage = LogQueue.ToString();
                    LogQueue.Length = 0;
                }

                if (string.IsNullOrEmpty(logMessage))
                    return;

                if (LogToConsole)
                    Console.Write(logMessage);

                if (false)
                {
                    VerifyTargetDirectory(); // File may be deleted after initialization.
                }

                lock (lockFile)
                {
                    File.AppendAllText(TargetLogFile.FullName, logMessage);
                }
            }
            finally
            {
                Timer.Change(BatchInterval, Timeout.Infinite); // Reset timer for next tick.
            }
        }

        #endregion

        public EventHandler<LogMessageInfo> LogMessageAdded;
        private bool _startedErrorShown = false;

        public const string DEBUG = "DEBUG";
        public const string INFO = "INFO";
        public const string WARN = "WARN";
        public const string ERROR = "ERROR";


        public void Debug(string message,  [CallerLineNumber] int lineNumber = 0)
        {
            if (IgnoreDebug)
                return;

            Log(DEBUG, "[" + lineNumber + "]" + message);
        }
        public void Debug_Flush(string message,  [CallerLineNumber] int lineNumber = 0)
        {
            if (IgnoreDebug)
                return;

            Log(DEBUG, "[" + lineNumber + "]" + message);
            FlushMsg(null);
        }

        public void Info(string message,  [CallerLineNumber] int lineNumber = 0)
        {
            Log(INFO, "[" + lineNumber + "]" + message);
        }



        public void Info_Flush(string message,  [CallerLineNumber] int lineNumber = 0)
        {
            Log(INFO, "[" + lineNumber + "]" + message);
            FlushMsg(null);
        }

        public void Error(string message,  [CallerLineNumber] int lineNumber = 0)
        {
            Log(ERROR, "[" + lineNumber + "]" + message);
        }
        public void Error_Flush(string message, [CallerLineNumber] int lineNumber = 0)
        {
            Log(ERROR, "[" + lineNumber + "]" + message);
            FlushMsg(null);
        }
        public void Warn(string message, Exception ex = null,  [CallerLineNumber] int lineNumber = 0)
        {
            Log(WARN, "[" + lineNumber + "]" + message);
        }

        public void Warn_Flush(string message, Exception ex = null, [CallerLineNumber] int lineNumber = 0)
        {
            Log(WARN, "[" + lineNumber + "]" + message);
            FlushMsg(null);
        }

        public void Log(string level, string message, Exception ex = null)
        {
            if (ex != null)
                message += string.Format("\r\n{0}\r\n{1}", ex.Message, ex.StackTrace);

            var info = new LogMessageInfo(level, message);
            var msg = info.ToString();

            lock (LogQueue)
            {
                LogQueue.AppendLine(msg);
            }

            //var evnt = LogMessageAdded;
            //if(evnt != null)
            //    evnt.Invoke(this, info); // Block caller.
        }

    }

    public sealed class LogMessageInfo : EventArgs
    {
        public readonly DateTime Timestamp;
        public readonly string ThreadId;
        public readonly string Level;
        public readonly string Message;

        public bool IsError { get { return Logger.ERROR.Equals(Level, StringComparison.Ordinal); } }
        public bool IsWarning { get { return Logger.WARN.Equals(Level, StringComparison.Ordinal); } }
        public bool IsInformation { get { return Logger.INFO.Equals(Level, StringComparison.Ordinal); } }
        public bool IsDebug { get { return Logger.DEBUG.Equals(Level, StringComparison.Ordinal); } }

        public LogMessageInfo(string level, string message)
        {
            Timestamp = DateTime.Now;
            var thread = Thread.CurrentThread;
            ThreadId =
                string.IsNullOrEmpty(thread.Name) ?
                thread.ManagedThreadId.ToString() :
                thread.Name + "(" + thread.ManagedThreadId.ToString() + ")";
            Level = level;
            Message = message;
        }

        public override string ToString()
        {
            return string.Format("{0:yyyy/MM/dd HH:mm:ss.fff} thr:{1}\t{2}\t{3}",
                Timestamp, ThreadId, Level, Message);
        }
    }
}

