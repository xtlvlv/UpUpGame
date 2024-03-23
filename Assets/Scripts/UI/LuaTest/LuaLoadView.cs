using Core;

namespace ViewCtrl
{
﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XLua;

public class LuaLoadView: MonoBehaviour
{
    #region ui component
    [SerializeField] private Button   btn_load;

    private void Reset()
    {
        btn_load = transform.Find("btn_load").GetComponent<Button>();
    }

    #endregion

    private void Awake()
    {
        btn_load.onClick.AddListener((() =>
        {
            LuaManager.Instance.OnLoadingEnd();
        }));
    }

}

}