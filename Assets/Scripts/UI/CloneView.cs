

namespace ViewCtrl
{
﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Core;
using Model;
 
public class CloneView: MonoBehaviour
{
    #region ui component
    [SerializeField] private TMP_Text text_log;
    [SerializeField] private TMP_Text text_log2;
    [SerializeField] private Button   btn_copy;
    [SerializeField] private Button   btn_deepCopy;
    [SerializeField] private Button   btn_reset;

    private void Reset()
    {
        text_log     = transform.Find("text_log").GetComponent<TMP_Text>();
        text_log2    = transform.Find("text_log2").GetComponent<TMP_Text>();
        btn_copy     = transform.Find("btn_copy").GetComponent<Button>();
        btn_deepCopy = transform.Find("btn_deepCopy").GetComponent<Button>();
        btn_reset    = transform.Find("btn_reset").GetComponent<Button>();
    }

    #endregion

    private void Awake()
    {
        btn_reset.onClick.AddListener(reset);
        btn_copy.onClick.AddListener(copy);
        btn_deepCopy.onClick.AddListener(deepCopy);
    }


    Player _player;
    private void Start()
    {
        reset();
    }
    
    string printPlayer(Player player)
    {
        var res = JsonConvert.SerializeObject(player);
        Debug.Log(res);
        return res;
    }

    void copy()
    {
        // var player2 = _player;
        var player2             = _player.MemberClone();
        player2.Name        = "小红";
        player2.Weapon.Name = "超级可爱宝剑";
        text_log.text       = printPlayer(_player);
        text_log2.text      = printPlayer(player2);
    }
    
    void deepCopy()
    {
        var player2 = _player.DeepCloneByReflection();
        player2.Name        = "小红";
        player2.Weapon.Name = "超级可爱宝剑";
        text_log.text       = printPlayer(_player);
        text_log2.text      = printPlayer(player2);
    }
    
    void reset()
    {
        _player             = new Player();
        _player.Name        = "小明";
        _player.Level       = 1;
        _player.Weapon      = new Equip();
        _player.Weapon.Name = "无敌暴龙宝刀";
        _player.EquipArray  = new Equip[1]{_player.Weapon};
        _player.EquipList   = new List<Equip>{_player.Weapon};
        _player.EquipDict   = new Dictionary<int, Equip>{{_player.Weapon.Id, _player.Weapon}};

        text_log.text = printPlayer(_player);
        text_log2.text = "";
    }
}

}