using System.Collections.Generic;
using Core;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using Logger = Core.Logger;

// 启动入口，挂在初始prefab上
public class GameApp: MonoBehaviour
{
    public static long         FrameCount = 0;

    private void Awake()
    {
        // 处理当前应用程序域中未被捕获的异常
        System.AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Log.Error(e.ExceptionObject.ToString());
        };
        Logger.Instance.ILog = new UnityLogger();

        Log.Info("Game awake");
        
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(Camera.main);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        FrameCount++;
    }

    private void LateUpdate()
    {
        
    }
    
    private void OnDestroy()
    {
        Log.Warning("GameApp OnDestroy");
    }

}
