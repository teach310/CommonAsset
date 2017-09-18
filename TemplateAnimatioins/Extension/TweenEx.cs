using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UniRx;

public static class TweenEx{

	// EaseをXYで分けたDOScale
	public static Tween DOScale(this Transform tran, Vector3 endValue, float duration, Ease easeX, Ease easeY){
		var sequence = DOTween.Sequence ();
		sequence.Join(tran.DOScaleX(endValue.x, duration).SetEase(easeX));
		sequence.Join(tran.DOScaleY(endValue.y, duration).SetEase(easeY));
		return sequence;
	}

    // TweenをObservableにする
    public static IObservable<Unit> OnCompleteAsObservable(this Tween tween)
    {
        var subject = new AsyncSubject<Unit>();
        tween.OnComplete(()=>{
            subject.OnNext(Unit.Default);
            subject.OnCompleted();
        });
        return subject;
    }
}
