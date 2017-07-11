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

	// Completeのタイミングのみを通知するSubjectに変換
	public static IObservable<Unit> ToUnitObservable<T>(this IObservable<T> observable){
		var subject = new AsyncSubject<Unit> ();
		subject.OnNext (Unit.Default);
		observable.OnComplete (() => subject.OnCompleted ());
		return subject;
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
