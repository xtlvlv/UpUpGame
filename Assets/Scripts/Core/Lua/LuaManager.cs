namespace Core
{
using System;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using XLua;

public class LuaManager : MonoBehaviour
{
    public static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
    public static LuaManager Instance;
    public string LuaPath = "lua_script";   

    private float    _lastGCTime = 0;
    private LuaTable _luaTable;
    private Action   _luaOnOpenAction;
    private Action   _luaOnCloseAction;
    private Action   _luaOnLoadingEndAction;

    public void Awake()
    {
        Instance = this;
        luaEnv.AddLoader(CustomLoader);

        _luaTable = luaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        _luaTable.SetMetaTable(meta);
        meta.Dispose();

        _luaTable.Set("self", this);

        // 如果有热更，热更后再调用
        AfterHotfixInitialize();    
    }
    
    public void Update()
    {
        float time = Time.time;
        if (time - _lastGCTime > 1) // 1s 一次
        {
            luaEnv.Tick();
            _lastGCTime = time;
        }
    }

    public void Destrory()
    {
        _luaOnOpenAction = null;
        _luaOnCloseAction = null;
    }

    public void AfterHotfixInitialize()
    {
        byte[] scriptByteArray = LoadScript("LuaEntry");
        luaEnv.DoString(scriptByteArray, "LuaEntry", _luaTable);

        _luaTable.Get("OnOpen", out _luaOnOpenAction);
        _luaTable.Get("OnClose", out _luaOnCloseAction);
        _luaTable.Get("OnLoadingEnd", out _luaOnLoadingEndAction);
        this.OpenLua();
    }

    public void OpenLua()
    {
        if (_luaOnOpenAction != null)
        {
            _luaOnOpenAction();
        }
    }

    public void CloseLua()
    {
        if (_luaOnCloseAction != null)
        {
            _luaOnCloseAction();
        }
    }
    
    public void OnLoadingEnd()
    {
        if (_luaOnLoadingEndAction != null)
        {
            _luaOnLoadingEndAction();
        }
    }

    public byte[] LoadScript(string scriptName)
    {
        var scriptPath = Path.Combine(LuaPath, scriptName);
        var luaCode      = Resources.Load<TextAsset>(scriptPath);
        if (luaCode != null)
        {
            byte[] bytes = luaCode.bytes;
            return bytes;
        }
        return null;
    }

    private byte[] CustomLoader(ref string pathFileName)
    {
        return LoadScript(pathFileName);
    }

    #region lua call csharp

    private GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }

    #endregion
}
}