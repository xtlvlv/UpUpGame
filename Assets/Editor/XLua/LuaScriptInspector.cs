using System.Collections.Generic;
using Core;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LuaBehaviour), true)]
public class LuaScriptInspector : Editor
{
    private static Dictionary<string, string> uiTypeDic = new Dictionary<string, string>()
    {
        // 只读取以下标签的节点，不是以下标签的节点忽略自己但是会读取子节点，以_开头的忽略自己和子节点
        {"btn","Button"}, // btn_按钮名
        {"img","Image"},
        {"obj","GameObject"},
        {"text","TMP_Text"},
        {"input","TMP_InputField"},
        {"slider","Slider"},
        {"scroll","ScrollRect"},
        {"ui","View"}, // 类型为脚本，命名格式为 ui_脚本名
    };
    
    private LuaBehaviour _luaScript;

    void OnEnable()
    {
        _luaScript = target as LuaBehaviour;
    }
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GetUnityObject"))
        {
            ObjectInjection[] objectInjectionArray = _luaScript.objectInjectionArray;
            if (objectInjectionArray != null)
            {
                for (int i = 0; i < objectInjectionArray.Length; i++)
                {
                    GetUnityObject(objectInjectionArray[i]);
                }
            }

            ArrayInjection[] arrayInjectionArray = _luaScript.arrayInjectionArray;
            if (arrayInjectionArray != null)
            {
                for (int i = 0; i < arrayInjectionArray.Length; i++)
                {
                    GetUnityObjectArray(arrayInjectionArray[i]);
                }
            }

            EditorUtility.SetDirty(_luaScript.gameObject);
        }
    }
    
    public static void GetUnityObject(ObjectInjection objectInjection)
    {
        if (objectInjection != null && objectInjection.subject != null)
        {
            // 根据命名获得类型
            var typeName = GetTypeByName(objectInjection.subject.name);
            if (typeName==null)
            {
                typeName = objectInjection.typeName;    // 自己指定
            }
            else
            {
                objectInjection.typeName = typeName;
            }
            if (!string.IsNullOrEmpty(typeName))
            {
                typeName = typeName.Trim();
            }

            if (string.IsNullOrEmpty(typeName))
            {
                objectInjection.unityObject = objectInjection.subject;
            }
            else
            {
                objectInjection.unityObject = objectInjection.subject.GetComponent(typeName);
            }

            objectInjection.name = objectInjection.subject.name;
        }
    }

    public static string GetTypeByName(string name)
    {
        // name 按照 _ 切割，根据第一个元素返回类型
        string[] uiTypeArr = name.Split('_');
        if (uiTypeArr.Length == 0)
        {
            return null;
        }
        string uiType = uiTypeArr[0];
        if (uiTypeDic.TryGetValue(uiType, out var typeName))
        {
            return typeName;
        }
        return null;
    }

    public static void GetUnityObjectArray(ArrayInjection arrayInjection)
    {
        if (arrayInjection != null && arrayInjection.subjectArray != null)
        {
            arrayInjection.unityObjectArray = new GameObject[arrayInjection.subjectArray.Length];

        
            string typeName = arrayInjection.typeName;
            if (arrayInjection.subjectArray.Length > 0)
            {
                var typeName0 = GetTypeByName(arrayInjection.subjectArray[0].name);
                if (typeName0 != null)
                {
                    typeName = typeName0;
                }
                else
                {
                    arrayInjection.typeName = typeName;
                }
            }
            if (!string.IsNullOrEmpty(typeName))
            {
                typeName = typeName.Trim();
            }

            int length = arrayInjection.subjectArray.Length;
            if (string.IsNullOrEmpty(typeName))
            {
                for (int i = 0; i < length; i++)
                {
                    if (arrayInjection.subjectArray[i] != null)
                    {
                        arrayInjection.unityObjectArray[i] = arrayInjection.subjectArray[i];
                    }
                }
            }
            else
            {
                List<Component> componentList = new List<Component>();
                for (int i = 0; i < length; i++)
                {
                    if (arrayInjection.subjectArray[i] != null)
                    {
                        Component component = arrayInjection.subjectArray[i].GetComponent(typeName);
                        componentList.Add(component);
                    }
                }

                arrayInjection.unityObjectArray = componentList.ToArray();
            }
        }
    }


    
}