using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Tweening;
using UniRx;

public class RedWindow : WindowPresenter
{
    public AnimateInUI animateIn;

    public override IObservable<Unit> OnBeforeOpen()
    {
        animateIn.Ready();
        return base.OnBeforeOpen();
    }

    public override IObservable<UniRx.Unit> OnOpen()
    {
        return animateIn.Play().OnCompleteAsObservable();
    }

}
