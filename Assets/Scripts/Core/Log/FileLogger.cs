namespace Core
{
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class FileLogger : ILog
{
    private string       _timeFormat = "yyyy-MM-dd HH:mm:ss.fff";
    private List<string> _logArray;
    private string       _logPath = null;

    private int    _logMaxCapacity = 500;
    private int    _currLogCount   = 0;
    private string reporterPath    = Application.persistentDataPath + "//Reporter";


    private int _logBufferMaxNumber = 10;

    public void Init()
    {
        _logArray = new List<string>();

        if (string.IsNullOrEmpty(_logPath))
        {
            _logPath = reporterPath + "//" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-log.txt";
        }
    }

    #region 日志接口



    public void Trace(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
        _logArray.Add($"{tag} {msg}");
    }

    public void Debug(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
        _logArray.Add($"{tag} {msg}");
    }

    public void Info(string msg, string tag = "")
    {
        UnityEngine.Debug.Log($"{tag} {msg}");
        _logArray.Add($"{tag} {msg}");
    }

    public void Warning(string msg, string tag = "")
    {
        UnityEngine.Debug.LogWarning($"{tag} {msg}");
        _logArray.Add($"{tag} {msg}");
    }

    public void Error(string msg, string tag = "")
    {
        UnityEngine.Debug.LogError($"{tag} {msg}");
        _logArray.Add($"{tag} {msg}");
    }

    public void Error(Exception e)
    {
        UnityEngine.Debug.LogException(e);
        _logArray.Add(e.ToString());
    }

    public void Trace(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
        _logArray.Add(String.Format(message, args));
    }

    public void Warning(string message, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(message, args);
        _logArray.Add(String.Format(message, args));
    }

    public void Info(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
        _logArray.Add(String.Format(message, args));
    }

    public void Debug(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
        _logArray.Add(String.Format(message, args));
    }

    public void Error(string message, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(message, args);
        _logArray.Add(String.Format(message, args));
    }

    #endregion

    #region 写入文件操作

    public void Write(string writeFileData, LogType type)
    {
        if (_currLogCount >= _logMaxCapacity)
        {
            _logPath        = reporterPath + "//" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-log.txt";
            _logMaxCapacity = 0;
        }

        _currLogCount++;

        if (!string.IsNullOrEmpty(writeFileData))
        {
            writeFileData = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff") + "|" + type.ToString() + "|" +
                            writeFileData + "\r\n";
            AppendDataToFile(writeFileData);
        }
    }

    private void AppendDataToFile(string writeFileDate)
    {
        if (_logArray == null) return;
        if (!string.IsNullOrEmpty(writeFileDate))
        {
            _logArray.Add(writeFileDate);
        }

        if (_logArray.Count % _logBufferMaxNumber == 0)
        {
            SyncLog();
        }
    }

    public void SyncLog()
    {
        if (!string.IsNullOrEmpty(_logPath))
        {
            int len = _logArray.Count;
            for (int i = 0; i < len; i++)
            {
                CreateFile(_logPath, _logArray[i]);
            }

            ClearLogArray();
        }
    }

    private void CreateFile(string pathAndName, string info)
    {
        if (!Directory.Exists(reporterPath)) Directory.CreateDirectory(reporterPath);

        StreamWriter sw;
        FileInfo     t = new FileInfo(pathAndName);
        if (!t.Exists)
        {
            sw = t.CreateText();
        }
        else
        {
            sw = t.AppendText();
        }

        sw.WriteLine(info);

        sw.Close();

        sw.Dispose();
    }

    private void ClearLogArray()
    {
        if (_logArray != null)
        {
            _logArray.Clear();
        }
    }

    #endregion

}


}