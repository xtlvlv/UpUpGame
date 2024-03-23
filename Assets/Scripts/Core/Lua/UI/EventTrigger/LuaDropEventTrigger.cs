namespace Core
{
using UnityEngine;
using UnityEngine.EventSystems;
using XLua;

[System.Serializable]
public class LuaDropEventItem
{
    public string eventName = "OnDrop";
    public System.Action<PointerEventData> action;
    public string parameter;
}

[LuaCallCSharp]
public class LuaDropEventTrigger : MonoBehaviour, IDropHandler
{
    public LuaComponentObject luaComponentObject;
    public LuaBeginDragEventItem[] eventItemArray;

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

    public void OnDrop(PointerEventData pointerEventData)
    {
        if (eventItemArray != null)
        {
            for (int i = 0; i < eventItemArray.Length; i++)
            {
                if (eventItemArray[i] != null && eventItemArray[i].action != null)
                {
                    eventItemArray[i].action(pointerEventData);
                }
            }
        }
    }
}
}