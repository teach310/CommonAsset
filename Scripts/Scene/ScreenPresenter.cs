﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ScreenPresenter : MonoBehaviour {

	// 初期化
	public virtual IObservable<Unit> Initialize(){
//        Debug.Log ("initialize" + Time.frameCount);

        return Observable.ReturnUnit();
		// Modelの生成
		// Viewの初期化
		// UIパーツ
		// SetListener
		// 画面の描画に必要な追加通信
	}

	// OnMoveInの直前
	public virtual IObservable<Unit> OnBeforeMoveIn(){
        return Observable.ReturnUnit();
	}

	// 自身に遷移してくる時
    // Transitionの開始と同時に呼ばれる
	public virtual IObservable<Unit> OnMoveIn(){
        
		//Debug.Log ("onMoveIn");
		return Observable.ReturnUnit();
	}

	// OnMoveInの直後
    // TransitionとOnMoveInが両方終わった時に呼ばれる
    public virtual IObservable<Unit> OnEndMoveIn(){
		//Debug.Log ("onEndMoveIn");
        return Observable.ReturnUnit();
	}

	// 次のスクリーンに遷移する時
	public virtual IObservable<Unit> OnMoveOut(){
		//Debug.Log ("onMoveOut");
		return Observable.ReturnUnit();
	}

	// OnBackInの直前
	public virtual IObservable<Unit> OnBeforeBackIn(){

        return Observable.ReturnUnit();
	}

	// 戻るによって遷移してくる時
	public virtual IObservable<Unit> OnBackIn(){

		return Observable.ReturnUnit();
	}

	// 前のスクリーンに戻る際
	public virtual IObservable<Unit> OnBackOut(){
		return Observable.ReturnUnit();
	}
	// Windowごと消された時
	public virtual void OnClose(){
	}

	protected virtual void SetListener(){
	}
}
