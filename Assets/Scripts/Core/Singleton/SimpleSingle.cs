namespace Core
{
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模板，这个需要自己挂载到一个物体上
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleObject<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance != null)
            {
                _instance = FindObjectOfType<SingleObject<T>>() as T;
            }
            else
            {
                GameObject obj = new GameObject { name = nameof(T) };
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }
}

/// <summary>
/// 单例类 不继承MonoBehaviour
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleClass<T> where T : class, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();
            }

            return _instance;
        }
    }
}


}