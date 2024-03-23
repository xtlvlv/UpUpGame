namespace ViewCtrl
{
﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainView: MonoBehaviour
{
   
    #region ui component
    [SerializeField] private Button   btn_add;
    [SerializeField] private TMP_Text text_count;

    private void Reset()
    {
        btn_add    = transform.Find("btn_add").GetComponent<Button>();
        text_count = transform.Find("text_count").GetComponent<TMP_Text>();
    }

    #endregion

    private int      count = 0;
    private void Awake()
    {
        count = 0;
        text_count.text = count.ToString();
        btn_add.onClick.AddListener((() =>
        {
            count++;
            text_count.text = count.ToString();
        }));
    }
}

}