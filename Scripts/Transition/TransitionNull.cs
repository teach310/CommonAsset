using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Common.Transition
{
    public class TransitionNull : ITransition
    {
        public IObservable<Unit> AnimateIn()
        {
            Debug.LogError ("AnimateIn");
            return Observable.ReturnUnit();
        }

        public IObservable<Unit> AnimateOut()
        {
            Debug.LogError ("AnimateOut");
            return Observable.ReturnUnit();
        }
    }
}
