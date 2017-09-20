using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using System;
using Common.Transition;
using TransitionStyle = Const.TransitionStyle;

public class WindowPresenter : MonoBehaviour
{
    protected Stack<ScreenPresenter> beforeScreenStack = new Stack<ScreenPresenter> ();
    [HideInInspector]
    public ScreenPresenter CurrentScreen;
    [SerializeField] Transform screenContainer;

    void Reset ()
    {
        screenContainer = this.transform.Find ("Screen");
    }

    // 初期化
    public virtual IObservable<Unit> Initialize ()
    {
        return Observable.ReturnUnit ();
    }

    // OnOpenの直前
    public virtual IObservable<Unit> OnBeforeOpen ()
    {

        return Observable.ReturnUnit ();
    }

    // 自身が開かれる時
    public virtual IObservable<Unit> OnOpen ()
    {
        return Observable.ReturnUnit ();
        //DialogManager.Instance.CreateContent (DialogType.Common);
    }

    // 他のWindowが開かれることによって画面から消える時に呼ばれる．
    public virtual void OnOpenOut ()
    {
    }

    // OnCloseInの直前
    public virtual IObservable<Unit> OnBeforeCloseIn ()
    {
        return Observable.ReturnUnit ();
    }

    // 他のWindowが閉じられて，自身が表示される時
    public virtual IObservable<Unit> OnCloseIn ()
    {
        return Observable.ReturnUnit ();
    }

    // 自身を閉じることによって，自身が画面から消える時
    public virtual void OnCloseOut ()
    {
    }

    // スクリーンが遷移し始めた時
    public virtual void OnScreenWillChange ()
    {
        //CommonBarrier.SetBarrier (true);
    }

    // スクリーン遷移後
    public virtual void OnScreenChanged ()
    {
        //CommonBarrier.SetBarrier (false);
    }


    // スクリーン遷移
    public IObservable<T> MoveScreen<T> (TransitionStyle transitionStyle = TransitionStyle.Null, Action<T> action = null)
        where T : ScreenPresenter
    {
        return MoveScreen (typeof(T).Name, transitionStyle, action: action);
    }

    /// <summary>
    /// スクリーン遷移　型安全でないためここから呼ぶのは非推奨
    /// </summary>
    public IObservable<T> MoveScreen<T> (string screenType, TransitionStyle transitionStyle = TransitionStyle.Null, Action<T> action = null)
        where T : ScreenPresenter
    {
        OnScreenWillChange ();
        var previewScreen = CurrentScreen;
        if (previewScreen != null) {
            previewScreen.OnMoveOut ();
            beforeScreenStack.Push (previewScreen);
            previewScreen.gameObject.SetActive (false);
        }

        // 生成
        var transition = MoveTransition (screenType, transitionStyle, action);
        transition.Subscribe (
            x => CurrentScreen = x,
            () => {
                OnScreenChanged ();
                //// 前のスクリーンを非表示にする
                //if (previewScreen != null)
                //{
                //    previewScreen.gameObject.SetActive(false);
                //}
            });
        return transition;
       
    }


    public IObservable<ScreenPresenter> BackScreen ()
    {
        OnScreenWillChange ();
        var nextScreen = beforeScreenStack.Pop ();
        var transition = BackTransition (CurrentScreen, nextScreen);
        transition.Subscribe (
            x => CurrentScreen = x,
            () => OnScreenChanged ()
        );
        return transition;
    }

    IObservable<ScreenPresenter> BackTransition (ScreenPresenter preview, ScreenPresenter next)
    {
        next.gameObject.SetActive (true);
        var subject = new AsyncSubject<ScreenPresenter> ();
        preview.OnBackOut ()
            .SelectMany (_ => next.OnBackIn ())
            .Subscribe (_ => {
            }, () => {
                Destroy (preview.gameObject);
                subject.OnNext (next);
                subject.OnCompleted ();
            });

        return subject;

    }

    /// コルーチンバージョン，yield returnの仕様上使うために1フレーム待たなくてはならないのがトランジションのない
    /// 遷移だと致命的なため使用しない
    [Obsolete]
    IEnumerator MoveTransitionCoroutine<T> (string screenType, IObserver<T> observer, TransitionStyle transitionStyle = TransitionStyle.Null, Action<T> action = null)
            where T : ScreenPresenter
    {
        var effect = TransitionFactory.Create (transitionStyle);
        yield return effect.AnimateIn ().ToYieldInstruction ();

        // 生成
        GameObject obj = Instantiate (ResourcesManager.Instance.GetScreen (screenType), screenContainer) as GameObject;
        T screen = obj.GetComponent<T> ();
    
    
        // 初期化
        yield return screen.Initialize ().ToYieldInstruction ();
        // モデルの注入
        if (action != null)
            action (screen);
        // 遷移
        yield return screen.OnBeforeMoveIn ().ToYieldInstruction ();
        //Debug.LogError ("BeforeMoveIn終了");
        yield return Observable.WhenAll (
            screen.OnMoveIn (),
            effect.AnimateOut ()
        ).ToYieldInstruction ();
        yield return screen.OnEndMoveIn ().ToYieldInstruction ();
        // 副作用
        CurrentScreen = screen;
        observer.OnNext (screen);
        observer.OnCompleted ();
    }


    // トランジション
    IObservable<T> MoveTransition<T> (string screenType, TransitionStyle transitionStyle, Action<T> action = null)
            where T : ScreenPresenter
    {
        
        var subject = new AsyncSubject<T> ();
        GameObject obj = Instantiate (ResourcesManager.Instance.GetScreen (screenType), screenContainer) as GameObject;
        var canvas = obj.GetComponent<Canvas> ();
        T screen = obj.GetComponent<T> ();
        var effect = TransitionFactory.Create (transitionStyle);
    
        Action<Unit> setParam = _ => {
            if (action != null)
                action (screen);
        };

        canvas.enabled = false; // 生成して非表示にしておく
        effect.AnimateIn ()
            .SelectMany (_=>screen.Initialize ())
            .Do (setParam)
            .SelectMany (_=>screen.OnBeforeMoveIn ())
            .Do (_ => canvas.enabled = true)
            .SelectMany (_=> Observable.WhenAll (effect.AnimateOut (), screen.OnMoveIn ()))
            .SelectMany (_=>screen.OnEndMoveIn ())
            .Subscribe (_ => {}, () => {
            subject.OnNext (screen);
            subject.OnCompleted ();
        });
        return subject;
    }

    public bool HasScreenStack ()
    {
        return beforeScreenStack.Count > 0;
    }
}
