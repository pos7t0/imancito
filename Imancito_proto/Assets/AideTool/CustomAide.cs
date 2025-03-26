using AideTool.Extensions;

namespace AideTool
{
    public static partial class Aide
    {
        private static partial bool IsInDevelopment() => true;
        
        private static partial LogLevel LevelLogued()
        {
            if (IsInDevelopment())
                return LogLevel.Verbose;
            return LogLevel.Error;
        }

        private static partial void DefineLevelFormat(string log, LogLevel logLevel)
        {
            if (logLevel > LevelLogued())
                return;

            switch(logLevel)
            {
                case LogLevel.Warning:
                    LogColor(log, AideColors.DarkYellow);
                    break;
                case LogLevel.Error:
                    LogColor(log, AideColors.LightLila);
                    break;
                case LogLevel.Exception:
                    LogColor(log, AideColors.Red);
                    break;
                case LogLevel.Debug:
                    LogColor(log, AideColors.Rosa);
                    break;
                case LogLevel.Verbose:
                    LogColor(log, AideColors.Orange);
                    break;
                default:
                    LogColor(log, AideColors.LightDark);
                    break;
            }
        }

        public enum Environment { Development, Production }

        public enum LogLevel { Info, Warning, Error, Exception, Debug, Verbose }

    }
}
