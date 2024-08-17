using System;
using TMPro;
using UnityEngine;

namespace ViewCtrl.UIFps
{
    public class UIFps: MonoBehaviour
    {
        public TMP_Text text_fps1;
        public TMP_Text text_fps2;

        private static readonly float _fpsSmoonthParam        = 1f / 100;
        private                 int   _frameCount      = 0;
        private                 float _averageDuration = 0.05f;
        private                 int   _fpsSmoonthly              = 0;
        
        private void Update()
        {
            _frameCount++;
            if (_frameCount % 100 == 0)
            {
                getFpsSmoothly();
                var deltaTime = Time.deltaTime;
                var fps       = 1.0f / deltaTime;
                text_fps1.text = $"FPS: {fps:F0}";
                text_fps2.text = $"FPS: {_fpsSmoonthly}";
            }

        }
        
        private void getFpsSmoothly()
        {
            _averageDuration = _averageDuration * (1 - _fpsSmoonthParam) + Time.deltaTime * _fpsSmoonthParam;

            _fpsSmoonthly = (int)(1f / _averageDuration);
        }
    }
}