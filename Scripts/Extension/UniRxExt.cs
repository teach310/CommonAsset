using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;

public static class UniRxExt{

	public static System.IDisposable OnComplete<T>(this IObservable<T> observable, UnityAction action){
		var ret = observable.Subscribe (x => {
		}, () => action ());
		return ret;
	}
}
