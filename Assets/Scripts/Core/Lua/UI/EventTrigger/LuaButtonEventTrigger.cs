namespace Core
{
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class LuaButtonEventTrigger : MonoBehaviour
{
    public LuaComponentObject luaComponentObject;
    public LuaButtonEventItem[] eventItemArray;

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

    void Start()
    {

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

    public void OnClick()
    {
        if (eventItemArray != null)
        {
            for (int i = 0; i < eventItemArray.Length; i++)
            {
                if (eventItemArray[i] != null && eventItemArray[i].action != null)
                {
                    eventItemArray[i].action(eventItemArray[i].parameter);
                }
            }
        }
    }
}
}