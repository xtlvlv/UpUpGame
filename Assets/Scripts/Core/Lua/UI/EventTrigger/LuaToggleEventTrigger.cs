namespace Core
{
using UnityEngine;
using XLua;

[System.Serializable]
public class LuaToggleEventItem
{
    public string eventName = "OnValueChanged";
    public System.Action<bool> action;
    public int parameter = -1;
}

[LuaCallCSharp]
public class LuaToggleEventTrigger : MonoBehaviour
{
    public LuaComponentObject luaComponentObject;
    public LuaToggleEventItem[] eventItemArray;

    void Awake()
    {
        if (luaComponentObject != null)
        {
            LuaTable luaTable = luaComponentObject.GetLuaTable();
            if (luaTable != null)
            {
                if (eventItemArray != null)
                {
                    for (int i = 0; i < eventItemArray.Length; i++)
                    {
                        if (eventItemArray[i] != null)
                        {
                            luaTable.Get(eventItemArray[i].eventName, out eventItemArray[i].action);
                        }
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        if (eventItemArray != null)
        {
            for (int i = 0; i < eventItemArray.Length; i++)
            {
                if (eventItemArray[i] != null)
                {
                    eventItemArray[i].action = null;
                }
            }
        }
    }

    public void OnValueChanged(bool isOn)
    {
        if (eventItemArray != null)
        {
            for (int i = 0; i < eventItemArray.Length; i++)
            {
                if (eventItemArray[i] != null && eventItemArray[i].action != null)
                {
                    if (eventItemArray[i].parameter < 0)
                    {
                        eventItemArray[i].action(isOn);
                    }
                    else
                    {
                        if (eventItemArray[i].parameter == 0)
                        {
                            eventItemArray[i].action(false);
                        }
                        else
                        {
                            eventItemArray[i].action(true);
                        }
                    }
                }
            }
        }
    }
}

}