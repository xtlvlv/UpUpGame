namespace Core
{
﻿using System;
using System.Diagnostics;

public class Logger: SingleClass<Logger>
{
    private ILog iLog = new UnityLogger();
    public ILog ILog
    {
        set
        {
            this.iLog = value;
            this.iLog.Init();
        }
    }
    
    private const int TraceLevel = 1;
    private const int DebugLevel = 2;
    private const int InfoLevel = 3;
    private const int WarningLevel = 4;

    public int LogLevel = InfoLevel;
    
    public Logger()
    {
        this.iLog.Init();
    }

    private bool CheckLogLevel(int level)
    {
        return LogLevel <= level;
    }
    
    public void Trace(string msg, string tag = "")
    {
        if (!CheckLogLevel(DebugLevel))
        {
            return;
        }
        StackTrace st = new StackTrace(2, true);
        this.iLog.Trace($"{msg}\n{st}", tag);
    }

    public void Debug(string msg, string tag = "")
    {
        if (!CheckLogLevel(DebugLevel))
        {
            return;
        }
        this.iLog.Debug(msg, tag);
    }

    public void Info(string msg, string tag = "")
    {
        if (!CheckLogLevel(InfoLevel))
        {
            return;
        }
        this.iLog.Info(msg, tag);
    }

    public void TraceInfo(string msg, string tag = "")
    {
        if (!CheckLogLevel(InfoLevel))
        {
            return;
        }
        StackTrace st = new StackTrace(2, true);
        this.iLog.Trace($"{msg}\n{st}", tag);
    }

    public void Warning(string msg, string tag = "")
    {
        if (!CheckLogLevel(WarningLevel))
        {
            return;
        }

        this.iLog.Warning(msg, tag);
    }

    public void Error(string msg, string tag = "")
    {
        // StackTrace st = new StackTrace(2, true);
        // this.iLog.Error($"{msg}\n{st}", tag);
        this.iLog.Error($"{tag} {msg}");
    }

    public void Error(Exception e)
    {
        if (e.Data.Contains("StackTrace"))
        {
            this.iLog.Error($"{e.Data["StackTrace"]}\n{e}");
            return;
        }
        string str = e.ToString();
        this.iLog.Error(str);
    }

    public void Trace(string message, params object[] args)
    {
        if (!CheckLogLevel(TraceLevel))
        {
            return;
        }
        StackTrace st = new StackTrace(2, true);
        this.iLog.Trace($"{string.Format(message, args)}\n{st}");
    }

    public void Warning(string message, params object[] args)
    {
        if (!CheckLogLevel(WarningLevel))
        {
            return;
        }
        this.iLog.Warning(string.Format(message, args));
    }

    public void Info(string message, params object[] args)
    {
        if (!CheckLogLevel(InfoLevel))
        {
            return;
        }
        this.iLog.Info(string.Format(message, args));
    }

    public void Debug(string message, params object[] args)
    {
        if (!CheckLogLevel(DebugLevel))
        {
            return;
        }
        this.iLog.Debug(string.Format(message, args));

    }

    public void Error(string message, params object[] args)
    {
        StackTrace st = new StackTrace(2, true);
        string s = string.Format(message, args) + '\n' + st;
        this.iLog.Error(s);
    }
    
    public void Console(string message)
    {
        this.iLog.Debug(message);
    }
    
    public void Console(string message, params object[] args)
    {
        string s = string.Format(message, args);
        this.iLog.Debug(s);
    }
}

}