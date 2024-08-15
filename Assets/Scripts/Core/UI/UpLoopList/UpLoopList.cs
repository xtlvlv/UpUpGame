using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace ViewCtrl.UILoopList
{
    [RequireComponent(typeof(ScrollRect))]
    public class UpLoopList: MonoBehaviour
    {
        public bool IsVertial = true;   // TODO: 暂不支持水平滚动
        public GameObject ItemPrefab;

        private ScrollRect                                   _scrollRect;
        private RectTransform                                _viewPortRectTransform;
        private RectTransform                                _contentRectTransform;
        private int                                          _itemCount;
        private Func<UpLoopList, int, UpLoopListItem> _onItemRefresh;
        private List<UpLoopListItem>                         _itemList = new();

        private void Awake()
        {
            _scrollRect                  = gameObject.GetComponent<ScrollRect>();
            _viewPortRectTransform       = _scrollRect.viewport;
            _contentRectTransform        = _scrollRect.content;
            _viewPortRectTransform.pivot = new Vector2(0, 1);
            _contentRectTransform.pivot  = new Vector2(0, 1);

            AdjustAnchor(_contentRectTransform);
        }


        public void Init(Func<UpLoopList, int, UpLoopListItem> onItemRefresh)
        {
            _onItemRefresh = onItemRefresh;
            InitItem();
            UpdateContentSize();
        }
        
        public void SetItemCount(int itemCount)
        {
            _itemCount = itemCount;
            if (_itemCount==0)
            {
                RecycleAllItem();
            }
            
            UpdateContentSize();
        }
        void RecycleAllItem()
        {
            foreach (var item in _itemList)
            {
                SimplePool.Despawn(item.gameObject);
            }
            _itemList.Clear();
        }
        
        void InitItem()
        {
            if (ItemPrefab == null)
            {
                Log.Error("item prefab is null ");
                return;
            }
            RectTransform rtf = ItemPrefab.GetComponent<RectTransform>();
            AdjustAnchor(rtf);
            AdjustPivot(rtf);
            UpLoopListItem tItem = ItemPrefab.GetComponent<UpLoopListItem>();
            if (tItem == null)
            {
                Log.Error($"{ItemPrefab.name}ItemPrefab is not set UpLoopListItem component");
                return;
            }
        }
        
        void AdjustAnchor(RectTransform rtf)
        {
            if (IsVertial)
            {
                rtf.anchorMin = new Vector2(0f, 1);
                rtf.anchorMax = new Vector2(0f, 1);
            }
            else
            {
                rtf.anchorMin = new Vector2(0f, 1);
                rtf.anchorMax = new Vector2(0f, 1);
            }
        }
        
        void AdjustPivot(RectTransform rtf)
        {
            if (IsVertial)
            {
                rtf.pivot = new Vector2(0f, 1);
            }
            else
            {
                rtf.pivot = new Vector2(0, 1f);
            }
        }
        
        void UpdateContentSize()
        {
            float size = GetAllItemSize();
            if (IsVertial)
            {
                if(Math.Abs(_contentRectTransform.rect.height - size) > 0.001)
                {
                    _contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
                }
            }
            else
            {
                if(Math.Abs(_contentRectTransform.rect.width - size) > 0.001)
                {
                    _contentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                }
            }
        }

        public void RefreshAllShowItem()
        {
            int count = _itemCount;
            if (count == 0)
            {
                return;
            }
            RecycleAllItem();
            for (int i = 0; i < count; ++i)
            {
                UpLoopListItem item = GetNewItemByIndex(i);
                if (item == null)
                {
                    continue;
                }
                item.CacheRectTransform.SetParent(_contentRectTransform);
                item.CacheRectTransform.localScale = Vector3.one;
                item.CacheRectTransform.anchoredPosition3D = Vector3.zero;
                item.CacheRectTransform.sizeDelta = ItemPrefab.GetComponent<RectTransform>().sizeDelta;
                item.CacheRectTransform.SetSiblingIndex(i);
                _itemList.Add(item);
            }
            UpdateContentSize();
            UpdateAllShownItemsPos();
        }
        
        void UpdateAllShownItemsPos()
        {
            int count = _itemList.Count;
            if (count == 0)
            {
                return;
            }

            float pos = 0;
           
            float pos1 = _itemList[0].CacheRectTransform.anchoredPosition3D.y;
            float d    = pos - pos1;
            float curY = pos;
            for (int i = 0; i < count; ++i)
            {
                var item = _itemList[i];
                item.CacheRectTransform.anchoredPosition3D = new Vector3(0, curY, 0);
                curY = curY - item.CacheRectTransform.rect.height - item.Padding;
            }
            if(d != 0)
            {
                Vector2 p = _contentRectTransform.anchoredPosition3D;
                p.y                                      = p.y - d;
                _contentRectTransform.anchoredPosition3D = p;
            }
                
        }
        
        public UpLoopListItem NewListViewItem()
        {
            UpLoopListItem item = SimplePool.SpawnByObj(ItemPrefab).GetComponent<UpLoopListItem>();
            RectTransform  rf   = item.GetComponent<RectTransform>();
            rf.SetParent(_contentRectTransform);
            rf.localScale         = Vector3.one;
            rf.anchoredPosition3D = Vector3.zero;
            rf.localEulerAngles   = Vector3.zero;
            item.OwnLoopList   = this;
            return item;
        }
        
        UpLoopListItem GetNewItemByIndex(int index)
        {
            if(_itemCount > 0 && index >= _itemCount)
            {
                return null;
            }
            UpLoopListItem newItem = _onItemRefresh(this, index);
            if (newItem == null)
            {
                return null;
            }
            newItem.Index                  = index;
            return newItem;
        }
        
        float GetAllItemSize()
        {
            int count = _itemList.Count;
            if (count == 0)
            {
                return 0;
            }
            float s = 0;
            for (int i = 0; i < count - 1; ++i)
            {
                s += _itemList[i].Padding + _itemList[i].ItemSize;
            }
            s += _itemList[count - 1].ItemSize;
            return s;
        }
        
      
    }
}