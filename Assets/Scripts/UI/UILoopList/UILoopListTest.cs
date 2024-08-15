using System;
using System.Collections.Generic;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewCtrl.UILoopList
{
    public class UpLoopListTest: MonoBehaviour
    {
       
        #region ui component
        [SerializeField] private TMP_Text text_info;
        [SerializeField] private Button   btn_add;

        private void Reset()
        {
            text_info = transform.Find("text_info").GetComponent<TMP_Text>();
            btn_add   = transform.Find("btn_add").GetComponent<Button>();
        }

        #endregion
        
        public UpLoopList list_btn;

        private int _curCount = 0;

        private void Start()
        {
            list_btn.Init(OnItemRefresh);
            btn_add.onClick.RemoveAllListeners();
            btn_add.onClick.AddListener((() =>
            {
                list_btn.SetItemCount(++_curCount);
                list_btn.RefreshAllShowItem();
            }));
        }

        private UpLoopListItem OnItemRefresh(UpLoopList loopList, int index)
        {
            UpLoopListItem item = loopList.NewListViewItem();
            item.transform.GetComponentInChildren<TMP_Text>().text = index.ToString();
            return item;
        }
    }
}