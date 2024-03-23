namespace Core
{
﻿public interface ILog
{
    void Init();
    void Trace(string   message, string          tag = "");
    void Warning(string message, string          tag = "");
    void Info(string    message, string          tag = "");
    void Debug(string   message, string          tag = "");
    void Error(string   message, string          tag = "");
    void Trace(string   message, params object[] args);
    void Warning(string message, params object[] args);
    void Info(string    message, params object[] args);
    void Debug(string   message, params object[] args);
    void Error(string   message, params object[] args);
}

}