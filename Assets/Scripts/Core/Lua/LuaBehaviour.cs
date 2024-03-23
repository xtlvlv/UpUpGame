namespace Core
{
using System;
using System.IO;
using UnityEngine;
using XLua;

[Serializable]
public class StringInjection
{
    public string name;
    public string parameter;
}


[Serializable]
public class ObjectInjection
{
    public string             name;
    public GameObject         subject;
    public string             typeName;
    public UnityEngine.Object unityObject;
}

[Serializable]
public class ArrayInjection
{
    public string               name;
    public GameObject[]         subjectArray;
    public string               typeName;
    public UnityEngine.Object[] unityObjectArray;
}

[LuaCallCSharp]
public class LuaBehaviour : MonoBehaviour
{
    public string luaScriptPathFileName;
    public StringInjection[] stringInjectionArray;
    public ObjectInjection[] objectInjectionArray;
    public ArrayInjection[] arrayInjectionArray;

    private Action _luaAwakeAction;
    private Action _luaStartAction;
    private Action _luaUpdateAction;
    private Action _luaOnDestroyAction;

    protected LuaTable _luaTable;

    public void Awake()
    {
        if (!string.IsNullOrEmpty(luaScriptPathFileName))
        {
            _luaTable = LuaManager.luaEnv.NewTable();

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = LuaManager.luaEnv.NewTable();
            meta.Set("__index", LuaManager.luaEnv.Global);
            _luaTable.SetMetaTable(meta);
            meta.Dispose();

            _luaTable.Set("self", this);

            if (stringInjectionArray != null)
            {
                for (int i = 0; i < stringInjectionArray.Length; i++)
                {
                    if (stringInjectionArray[i] != null)
                    {
                        _luaTable.Set(stringInjectionArray[i].name, stringInjectionArray[i].parameter);
                    }
                }
            }

            if (objectInjectionArray != null)
            {
                for (int i = 0; i < objectInjectionArray.Length; i++)
                {
                    if (objectInjectionArray[i] != null)
                    {
                        _luaTable.Set(objectInjectionArray[i].name, objectInjectionArray[i].unityObject);
                    }
                }
            }

            if (arrayInjectionArray != null)
            {
                for (int i = 0; i < arrayInjectionArray.Length; i++)
                {
                    if (arrayInjectionArray[i] != null)
                    {
                        _luaTable.Set(arrayInjectionArray[i].name, arrayInjectionArray[i].unityObjectArray);
                    }
                }
            }

            byte[] byteArray = LuaManager.Instance.LoadScript(luaScriptPathFileName);
            string fileName = Path.GetFileNameWithoutExtension(luaScriptPathFileName);
            LuaManager.luaEnv.DoString(byteArray, fileName, _luaTable);

            _luaTable.Get("Awake", out _luaAwakeAction);
            _luaTable.Get("Start", out _luaStartAction);
            _luaTable.Get("Update", out _luaUpdateAction);
            _luaTable.Get("OnDestroy", out _luaOnDestroyAction);

            if (_luaAwakeAction != null)
            {
                _luaAwakeAction();
            }
        }
    }

    public virtual void Start()
    {
        if (_luaStartAction != null)
        {
            _luaStartAction();
        }
    }

    public virtual void Update()
    {
        if (_luaUpdateAction != null)
        {
            _luaUpdateAction();
        }
    }

    public virtual void OnDestroy()
    {
        if (_luaOnDestroyAction != null)
        {
            _luaOnDestroyAction();
        }

        objectInjectionArray = null;
        arrayInjectionArray = null;

        _luaAwakeAction = null;
        _luaStartAction = null;
        _luaUpdateAction = null;
        _luaOnDestroyAction = null;

        if (_luaTable != null)
        {
            _luaTable.Dispose();
        }
    }

    public LuaTable GetLuaTable()
    {
        return _luaTable;
    }
}
}