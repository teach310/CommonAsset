using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Common.Transition
{
    public class TransitionFade : ITransition
    {
        public IObservable<Unit> AnimateIn()
        {
            return Fader.FadeIn().OnCompleteAsObservable();
        }

        public IObservable<Unit> AnimateOut()
        {
            return Fader.FadeOut().OnCompleteAsObservable();
        }
    }
}
