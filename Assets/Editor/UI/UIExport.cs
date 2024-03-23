using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class UIExport
{
    private static string exportString="";
    private static Dictionary<string, string> uiTypeDic = new Dictionary<string, string>()
    {
        // 只读取以下标签的节点，不是以下标签的节点忽略自己但是会读取子节点，以_开头的忽略自己和子节点
        {"btn","Button"},   // btn_按钮名
        {"img","Image"},
        {"obj","GameObject"},
        {"text","TMP_Text"},
        {"input","TMP_InputField"},
        {"slider","Slider"},
        {"scroll","ScrollRect"},
        {"ui","View"}, // 类型为脚本，命名格式为 ui_脚本名
    };

    [MenuItem("Tools/UI/快捷导出UI组件名 #&%Q")]
    private static void exportUIComp()
    {
        var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage == null)
        {
            Debug.LogWarning("当前没有编辑预设");
            return;
        }
        exportString = "";


        GameObject basePrebabModal = AssetDatabase.LoadAssetAtPath(prefabStage.assetPath, typeof(GameObject)) as GameObject;

        exportString += "\n#region ui component\n";
        exportUICompList(basePrebabModal.transform, "");
        exportString += "\nprivate void Reset()\n{\n";
        exportResetFunc(basePrebabModal.transform, "");
        exportString += "}\n";
        exportString += "\n#endregion\n";


        GUIUtility.systemCopyBuffer = exportString;
        Debug.Log(exportString);
        Debug.Log("已保存到剪贴板");
    }


    /// <summary>
    /// 导出ui组件列表文本，ui命名规范为 "{uiTypeDic中的前缀}_{任意名字}"
    /// </summary>
    private static void exportUICompList(Transform transform,string path = "")
    {
        var len = transform.childCount;
        for(var i=0; i < len; i++)
        {
            var    childTrans = transform.GetChild(i);
            var    name       = childTrans.name;
            var    uiTypeArr  = name.Split('_');
            string uiType     = uiTypeArr[0];
            bool   forChild   = true;       // 是否递归子节点
            bool   ignore     = false;      // 是否忽略当前节点
            string typeName   = "";
            if (!uiTypeDic.TryGetValue(uiType, out typeName))
            {
                ignore = true;
                // Debug.LogWarning("未知的ui类型:" + uiType);
            }
            // 需要特殊处理的类型
            switch (uiType)
            {
                case "ui":
                    typeName = uiTypeArr[1];
                    forChild = false;
                    break;
                case "scroll":
                    forChild = false;
                    break;
                case "":        // _name 
                    ignore = true;
                    forChild = false;
                    break;
            }
        
            if (!ignore)
            {
                string targetStr = $"[SerializeField] private {typeName} {name};\n";
                if (exportString.IndexOf(targetStr) == -1)
                {
                    exportString += targetStr;
                }
                else
                {
                    Debug.LogError($"重复的属性名={name} path={path}");
                }
                
            }
            if (forChild)
            {
                exportUICompList(childTrans, path + "/" + name);
            }
            
        }
    }
    
    private static void exportResetFunc(Transform a_trans, string a_path)
    {
        var len = a_trans.childCount;
        for (var i = 0; i < len; i++)
        {
            var childTrans = a_trans.GetChild(i);
            var name = childTrans.name;
            var uiTypeArr = name.Split('_');
            string uiType = uiTypeArr[0];
            bool forChild = true;
            bool ignore = false;
            string typeName = "";
            if (!uiTypeDic.TryGetValue(uiType, out typeName))
            {
                ignore = true;
                // Debug.LogWarning("未知的ui类型:" + uiType);
            }
            switch (uiType)
            {
                case "ui":
                    typeName = uiTypeArr[1];
                    forChild = false;
                    break;
                case "scroll":
                    forChild = false;
                    break;
                case "": // 跳过
                    ignore = true;
                    forChild = false;
                    break;
            }

            if (!ignore)
            {
                string targetStr;
                
                if (a_path == "")
                {
                    if (uiType == "obj")
                    {
                        targetStr = $"{name} = transform.Find(\"{name}\").gameObject;\n";
                    }
                    else
                    {
                        targetStr = $"{name} = transform.Find(\"{name}\").GetComponent<{typeName}>();\n";
                    }
                }
                else
                {
                    if (uiType == "obj")
                    {
                        targetStr = $"{name} = transform.Find(\"{a_path}/{name}\").gameObject;\n";
                    }
                    else
                    {
                        targetStr = $"{name} = transform.Find(\"{a_path}/{name}\").GetComponent<{typeName}>();\n";
                    }
                }
                
                if (exportString.IndexOf(targetStr) == -1)
                {
                    exportString += targetStr;
                }
                else
                {
                    Debug.LogError($"重复的属性名={name} path={a_path}");
                }
            }
            if (forChild)
            {
                if (a_path=="")
                {
                    exportResetFunc(childTrans, name);
                }
                else
                {
                    exportResetFunc(childTrans, a_path + "/" + name);
                }
            }
        }
    }
    
}
