using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Common.Tweening
{
    [RequireComponent(typeof(CanvasGroup))]
    public class YoyoFade : MonoBehaviour
    {
        public bool playOnAwake = false;

        public float duration = 1.0f;
        public float delay = 0f;

        CanvasGroup canvasGroup;
        CanvasGroup CanvasGroup{
            get{
                if(canvasGroup == null)
                    canvasGroup = this.GetComponent<CanvasGroup>();
                return canvasGroup;
            }
        }

        private void Awake()
        {
            if(playOnAwake){
                Ready();
                Play();
            }
        }

        public void Ready(){
            CanvasGroup.alpha = 0f;
        }

        public Tween Play()
        {
            return CanvasGroup
                .DOFade(1f, duration)
                .SetLoops (-1, LoopType.Yoyo)
                .SetEase (Ease.OutCirc)
                .SetDelay(delay);
        }
    }
}
