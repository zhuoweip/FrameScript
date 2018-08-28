using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameDebugLog
{
    /// <summary>
    /// Log level .定义LOG的类型：正常、警告、错误、异常
    /// </summary>
    public enum LogLevel : byte
    {
        /// <summary>
        /// The normal.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// The warning.
        /// </summary>
        Warning,
        /// <summary>
        /// The error.
        /// </summary>
        Error,
        /// <summary>
        /// The exception.
        /// </summary>
        Exception,
    }

    /// <summary>
    /// Log the specified msg and objs.
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="objs">Objects.</param>
    public static void Log(string msg, params object[] objs)
    {
        OutputLog(LogLevel.Normal, msg, objs);
    }

    /// <summary>
    /// Logs the warning.
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="objs">Objects.</param>
    public static void LogWarning(string msg, params object[] objs)
    {
        OutputLog(LogLevel.Warning, msg, objs);
    }

    /// <summary>
    /// Logs the error.
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="objs">Objects.</param>
    public static void LogError(string msg, params object[] objs)
    {
        OutputLog(LogLevel.Error, msg, objs);
    }

    /// <summary>
    /// Logs the exception.
    /// </summary>
    /// <param name="msg">Message.</param>
    /// <param name="objs">Objects.</param>
    public static void LogException(System.Exception e)
    {
        OutputExceptionLog(e);
    }

    /// <summary>
    /// Log content.
    /// </summary>
    public class LogContent
    {
        public string Msg { get; set; }
        public LogLevel Level { get; set; }
    }

    /// <summary>
    /// The logs.
    /// </summary>
    public static List<LogContent> logs;

    /// <summary>
    /// Adds the log centent.
    /// </summary>
    /// <param name="level">Level.</param>
    /// <param name="msg">Message.</param>
    public static void AddLogCentent(LogLevel level, string msg)
    {
        if (logs == null) logs = new List<LogContent>();
        if (logs != null)
        {
            if (logs.Count > 20) logs.RemoveAt(0);
            LogContent log = new LogContent();
            log.Msg = msg;
            log.Level = level;
            logs.Add(log);
        }
    }

    public static bool isEditor = Application.isEditor;

    /// <summary>
    /// Outputs the log.
    /// </summary>
    /// <param name="level">Level.</param>
    /// <param name="msg">Message.</param>
    /// <param name="objs">Objects.</param>
    public static void OutputLog(LogLevel level, string msg, params object[] objs)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat(msg, objs);

#if DEBUG_LOG_EDITOR
			if (Application.isPlaying && !isEditor)
#else
        if (Application.isPlaying)
#endif
        {
            AddLogCentent(level, sb.ToString());
        }
        else
        {
            if (level == LogLevel.Normal)
                Debug.Log(sb.ToString());
            else if (level == LogLevel.Warning)
                Debug.LogWarning(sb.ToString());
            else if (level == LogLevel.Error)
                Debug.LogError(sb.ToString());
        }
    }

    /// <summary>
    /// Outputs the exception log.
    /// </summary>
    /// <param name="e">E.</param>
    public static void OutputExceptionLog(System.Exception e)
    {
#if DEBUG_LOG_EDITOR
			if (Application.isPlaying && !isEditor)
#else
        if (Application.isPlaying)
#endif
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} \n{1}", e.Message, e.StackTrace);

            AddLogCentent(LogLevel.Exception, sb.ToString());
        }
        else
        {
            Debug.LogError(e);
        }
        Debug.LogError(e);
    }

    /// <summary>
    /// The log position.
    /// </summary>
    public static Vector2 logPos;
    /// <summary>
    /// The log rect.
    /// </summary>
    public static Rect logRect;
    /// <summary>
    /// The level setting.
    /// </summary>
    public class LevelSetting
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }
        /// <summary>
        /// Gets or sets the color of the title.
        /// </summary>
        /// <value>The color of the title.</value>
        public Color TitleColor { get; set; }
    }
    /// <summary>
    /// Level settings.
    /// </summary>

}


