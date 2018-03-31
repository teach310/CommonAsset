using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// 順番に非同期処理を処理していくクラス
/// </summary>
public class ObservableQueueWorker : IDisposable{

	bool running = false;
	List<Func<IObservable<Unit>>> eventQueue = new List<Func<IObservable<Unit>>> ();
	bool isDisposed = false;

	/// <summary>
	/// 非同期処理を追加
	/// </summary>
	public void Enqueue(Func<IObservable<Unit>> asyncAction){
		eventQueue.Add (asyncAction);
		if (!running) {
			running = true;
			SubscribeNext ();
		}
	}

	/// <summary>
	/// 先頭に割り込み
	/// </summary>
	public void InsertFirst(Func<IObservable<Unit>> asyncAction){
		if (running) {
			eventQueue.Insert (0, asyncAction);
		}else
			Enqueue (asyncAction);
	}

	/// <summary>
	/// 現在進行しているイベントが終わったらDisposeする
	/// </summary>
	public void EndLoop(){
		Debug.Log ("End Loop");
		InsertFirst (()=>Observable.Start(Dispose));
	}

	// 次のObservableを発火させる
	void SubscribeNext(){
		if (eventQueue.Count == 0 || isDisposed){
			Debug.Log ("Stop Running");
			running = false;
			return;
		}

		var e = eventQueue [0];
		eventQueue.RemoveAt (0);

		e ()
			.Finally (() => SubscribeNext ())
			.Subscribe ();
	}

	public void Dispose(){
		isDisposed = true;
	}
}
