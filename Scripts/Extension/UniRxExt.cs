using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using DG.Tweening;

public static class UniRxExt{

	public static System.IDisposable OnComplete<T>(this IObservable<T> observable, UnityAction action){
		var ret = observable.Subscribe (x => {
		}, () => action ());
		return ret;
	}


    // TweenをObservableにする
    public static IObservable<Unit> OnCompleteAsObservable(this Tween tween)
    {
        var subject = new AsyncSubject<Unit>();
        subject.OnNext(Unit.Default);
        tween.OnComplete(subject.OnCompleted);
        return subject;
    }
}
