using System;
using UnityEngine;

namespace ViewCtrl.UILoopList
{
    public class UpLoopListItem: MonoBehaviour
    {
        public float Padding;

        [NonSerialized]
        public UpLoopList OwnLoopList;

        private RectTransform _rectTransform;
        public RectTransform CacheRectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = gameObject.GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
        public float ItemSize
        {
            get
            {
                if (OwnLoopList.IsVertial)
                {
                    return CacheRectTransform.rect.height;
                }
                else
                {
                    return CacheRectTransform.rect.width;
                }
            }
        }

        private int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
    }
}