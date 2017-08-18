using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DialogPresenter : MonoBehaviour {

	// 初期化
    public virtual IObservable<Unit> Initialize(){
        return Observable.ReturnUnit();
    }

    public virtual IObservable<Unit> OnBeforeOpen(){
        return Observable.ReturnUnit();
    }

    // アニメーションとか
    public virtual IObservable<Unit> OnOpen()
    {
        return Observable.ReturnUnit();
    }

    public virtual IObservable<Unit> OnBeforeClose(){
        return Observable.ReturnUnit();
    }

    // Close
    public virtual IObservable<Unit> OnClose(){
        return Observable.ReturnUnit();
    }


}
