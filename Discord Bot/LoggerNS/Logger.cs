
using System;
using System.Data;
using System.Threading.Tasks;
using Discord;

namespace Discord_Bot.LoggerNS
{

    public class Level
    {
        public static Level Success = new Level(-1, "Success", LoggerFormat.Green);
        public static Level Info = new Level(0, "Info", LoggerFormat.White);
        public static Level Warning = new Level(1, "Warning", LoggerFormat.Yellow);
        public static Level Error = new Level(2, "Error", LoggerFormat.LightRed);
        public static Level Critical = new Level(3, "Critical", LoggerFormat.Red);
        public static Level Verbose = new Level(4, "Verbose", LoggerFormat.LightMagenta); //TODO Add functionality
        public static Level Debug = new Level(5, "Debug", LoggerFormat.LightMagenta); //TODO Add functionality
        
        public int Rank { get; }
        public string Name { get; }
        public LoggerFormat LoggerFormat { get; }
        
        public Level(int rank, string name, LoggerFormat loggerFormat)
        {
            Rank = rank;
            Name = name;
            LoggerFormat = loggerFormat;
        }
        
        public static Level LogSeverityToLevel(LogSeverity logSeverity)
        {
            switch (logSeverity)
            {
                case LogSeverity.Warning:
                    return Warning;
                case LogSeverity.Debug:
                    return Debug;
                case LogSeverity.Info:
                    return Info;
                case LogSeverity.Error:
                    return Error;
                case LogSeverity.Verbose:
                    return Verbose;
                case LogSeverity.Critical:
                    return Critical;
                default:
                    throw new DataException("Invalid LogSeverity");
            }
        }

        public override string ToString() => Name;
    }   
    
    
    
    public class Logger
    {
        public void Log(Level level, string message, Exception exception=null)
        {
            LoggerFormat.Write(String.Format("[{0:G}] [{1}]: {2}", DateTime.Now, 
                LoggerFormat.Bold + "" + level.LoggerFormat + level.Name + LoggerFormat.Reset, message + LoggerFormat.Reset) + "\n");

            if (exception != null)
            {
                Log(Level.Error, LoggerFormat.Red + "An " + exception + " exception has occured.\n");
                Log(Level.Error, LoggerFormat.Red + exception.Message + "\n" + exception.StackTrace);
            }
        }
    }
}