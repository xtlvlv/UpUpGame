namespace Core
{
using UnityEngine.Serialization;
using XLua;

[System.Serializable]
public class LuaComponentObject
{
    public LuaBehaviour luaBehaviour;

    public LuaTable GetLuaTable()
    {
        if (luaBehaviour != null)
        {
            return luaBehaviour.GetLuaTable();
        }

        return null;
    }
}
}