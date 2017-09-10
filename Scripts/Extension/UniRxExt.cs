using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using DG.Tweening;
using System;

public static class UniRxTools{
    // メインスレッドで関数を実行し,IObservable<Unit>を返す関数
    public static IObservable<Unit> ObservableAction(Action action){
        action ();
        return Observable.ReturnUnit ();
    }
}

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
