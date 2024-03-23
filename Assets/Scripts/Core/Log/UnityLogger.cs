namespace Core
{
﻿using System;

public class UnityLogger: ILog
{
    private string _timeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    
    public void Init()
    {
    }
    public void Trace(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
    }

    public void Debug(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
    }

    public void Info(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
    }

    public void Warning(string msg, string tag = "")
    {
        UnityEngine.Debug.LogWarning($"{tag} {msg}");
    }

    public void Error(string msg, string tag = "")
    {
        UnityEngine.Debug.LogError($"{tag} {msg}");
    }

    public void Error(Exception e)
    {
        UnityEngine.Debug.LogError(e);
    }

    public void Trace(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
    }

    public void Warning(string message, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(message, args);
    }

    public void Info(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
    }

    public void Debug(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
    }

    public void Error(string message, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(message, args);
    }
}

}