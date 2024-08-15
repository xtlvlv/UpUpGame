using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ViewCtrl.Lobby
{
    public class UILobby: MonoBehaviour
    {
        public Button   btn_play;
        public TMP_Text text_aa;
        public Image    img_aa;
        private void Reset()
        {
            btn_play = transform.Find("btn_play").GetComponent<Button>();
        }

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            
        }

        private int count =0;
        private void Start()
        {
            count        = 0;
            text_aa.text = "aaaa";
            btn_play.onClick.AddListener(OnClick);
        }

        
        public void OnClick()
        {
            count++;
            text_aa.text = "bbbb"+count;
        }

        private void Update()
        {
            // 每帧执行
            // 60 
        }

        private void LateUpdate()
        {
            
        }

        private void FixedUpdate()
        {
            
        }

        private void OnDestroy()
        {
            
        }
    }
}