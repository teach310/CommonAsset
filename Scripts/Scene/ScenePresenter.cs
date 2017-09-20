using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using TransitionStyle = Const.TransitionStyle;
using System;

// Windowを管理する
// TODO アニメーション対応
public class ScenePresenter : SingletonMonoBehaviour<ScenePresenter>
{

    protected Stack<WindowPresenter> beforeWindowStack = new Stack<WindowPresenter> ();
    public static WindowPresenter CurrentWindow;
    public WindowPresenter initialWindow;
    public ScreenSettings screenSettings;
    public Transform windowContainer;

    void Start ()
    {
#if UNITY_EDITOR
        ClearScene ();
#endif
        //MoveScreen(initialScreen.name);
        GoRootScreen (initialWindow.name);
    }

    #if UNITY_EDITOR
    void ClearScene ()
    {
        foreach (var window in this.GetComponentsInChildren<WindowPresenter>(true)) {
            Destroy (window.gameObject);
        }
    }
    #endif
    #region Comparer

    public static bool IsCurrentWindow<T> ()
        where T:WindowPresenter
    {
        return CurrentWindow.GetType () == typeof(T);
    }

    public static bool IsCurrentScreen<T> ()
        where T:ScreenPresenter
    {
        return CurrentWindow.CurrentScreen.GetType () == typeof(T);
    }

    #endregion


    public IObservable<T> OpenWindow<T> (Action<T> action = null)
        where T : WindowPresenter
    {
        return OpenWindow (typeof(T).Name, action);
    }

    public IObservable<WindowPresenter> OpenWindow (string name)
    {
        return OpenWindow<WindowPresenter> (name);
    }

    public IObservable<T> OpenWindow<T> (string name, Action<T> action = null)
        where T : WindowPresenter
    {
        var openSubject = new AsyncSubject<T> ();
        // 遷移準備
        var previewWindow = CurrentWindow;
        if (previewWindow != null) {
            previewWindow.OnOpenOut ();

            beforeWindowStack.Push (previewWindow);
            previewWindow.gameObject.SetActive (false);
        }

        // 生成処理
        GameObject obj = Instantiate (ResourcesManager.Instance.GetWindow (name), windowContainer) as GameObject;
        T window = obj.GetComponent<T> ();
        CurrentWindow = window;

        window.Initialize ()
            .Do (_ => {
                if(action != null)
                    action (window);
            })
            .SelectMany (_ => window.OnBeforeOpen ())
            .SelectMany (_ => window.OnOpen ())
            .Subscribe (_ => {
        }, () => {
            openSubject.OnNext (window);
            openSubject.OnCompleted ();
        });

        return openSubject;
    }

    /// <summary>
    /// ScreenChangeButton用
    /// </summary>
    public IObservable<ScreenPresenter> GoRootScreen (string windowName,TransitionStyle transitionStyle = TransitionStyle.Null)
    {
        return GoRootScreen<ScreenPresenter> (windowName, transitionStyle);
    }

    // キャッシュを削除してルートへ飛ぶ
    public IObservable<T> GoRootScreen<T> (string windowName,TransitionStyle transitionStyle = TransitionStyle.Null,  Action<T> action = null)
        where T : ScreenPresenter
    {
        var windowEntity = screenSettings.windows.Find (x => x.name == windowName);
        // トップのスクリーンを取得
        var rootScreen = screenSettings.screens.Find (x => x.windowId == windowEntity.id);

        var transition = MoveScreen (rootScreen.name,transitionStyle, action);
        transition.Subscribe (_ => ClearWindowCache ());

        return transition;
    }

    void ClearWindowCache ()
    {
        while (beforeWindowStack.Count > 0) {
            var window = beforeWindowStack.Pop ();
            Destroy (window.gameObject);
        }
    }

    public static IObservable<T> MoveScreen<T> (TransitionStyle transitionStyle = TransitionStyle.Null, Action<T> action = null)
        where T:ScreenPresenter
    {
        return Instance.MoveScreen (typeof(T).Name,transitionStyle, action);
    }

    public IObservable<ScreenPresenter> MoveScreen (string screenName, TransitionStyle transitionStyle = TransitionStyle.Null)
    {
        return MoveScreen<ScreenPresenter> (screenName, transitionStyle);
    }

    /// <summary>
    /// 直接スクリーンを呼ぶ．Backでは用いない
    /// </summary>
    IObservable<T> MoveScreen<T> (string screenName, TransitionStyle transitionStyle = TransitionStyle.Null, Action<T> action = null)
        where T : ScreenPresenter
    {
        // screenの情報取得
        var screenEntity = screenSettings.screens.Find (x => x.name == screenName);
        // windowを取得
        var windowName = screenSettings.windows.Find (x => x.id == screenEntity.windowId).name;

        // screen生成
        if (CurrentWindow != null) {
            if (CurrentWindow.GetType ().Name == windowName) {
                return CurrentWindow.MoveScreen<T> (screenName, transitionStyle);
            }
        }
        var subject = new AsyncSubject<T> ();
        // 別ウインドウの場合
        // ウインドウを表示
        OpenWindow (windowName).Subscribe (x => {
            // スクリーンを表示
            x.MoveScreen (screenName, transitionStyle, action)
               .Subscribe (s => {
                subject.OnNext (s);
                subject.OnCompleted ();
            });
        });
        return subject;
    }

    public IObservable<ScreenPresenter> BackScreen ()
    {
        if (CurrentWindow == null) {
            Debug.LogError ("CurrentWindow is null");
            return null;
        }

        // 現在のWindowのスクリーンのスタックチェック
        if (CurrentWindow.HasScreenStack ()) {
            return CurrentWindow.BackScreen ();
        } else if (HasWindowStack ()) { // 上位のWindowの存在チェック
            var subject = new AsyncSubject<ScreenPresenter> ();
            BackWindow ().Subscribe (x => {
                subject.OnNext (x.CurrentScreen);
                subject.OnCompleted ();
            });
            return subject;
        }

        // Stackがない場合
        return null;
    }

    IObservable<WindowPresenter> BackWindow ()
    {

        var newWindow = beforeWindowStack.Pop ();
        newWindow.gameObject.SetActive (true);
        var subject = new AsyncSubject<WindowPresenter> ();
        CurrentWindow.OnCloseOut ();
        newWindow.OnBeforeCloseIn ()
            .SelectMany (_ => newWindow.OnCloseIn ())
            .Subscribe (_ => {
        }, () => {
                Destroy (CurrentWindow.gameObject);
                CurrentWindow = newWindow;
                subject.OnNext (newWindow);
                subject.OnCompleted ();
        });
        
        return subject;
    }

    public bool HasWindowStack ()
    {
        return beforeWindowStack.Count > 0;
    }


}
