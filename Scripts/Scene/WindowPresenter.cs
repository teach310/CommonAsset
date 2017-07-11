using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;

public class WindowPresenter : MonoBehaviour
{
    protected Stack<ScreenPresenter> beforeScreenStack = new Stack<ScreenPresenter>();
    [HideInInspector]
    public ScreenPresenter CurrentScreen;
    [SerializeField] Transform screenContainer;

    // 初期化
    public virtual void Initialize()
    {
    }
    // OnOpenの直前
    public virtual void OnBeforeOpen()
    {
    }

    // 自身が開かれる時
    public virtual void OnOpen()
    {
        //DialogManager.Instance.CreateContent (DialogType.Common);
    }

    // 他のWindowが開かれることによって画面から消える時に呼ばれる．
    public virtual void OnOpenOut()
    {
    }

    // OnCloseInの直前
    public virtual void OnBeforeCloseIn()
    {
    }

    // 他のWindowが閉じられて，自身が表示される時
    public virtual void OnCloseIn()
    {
    }

    // 自身を閉じることによって，自身が画面から消える時
    public virtual void OnCloseOut()
    {
    }

    // スクリーンが遷移し始めた時
    public virtual void OnScreenWillChange()
    {
        //CommonBarrier.SetBarrier (true);
    }

    // スクリーン遷移後
    public virtual void OnScreenChanged()
    {
        //CommonBarrier.SetBarrier (false);
    }


    // スクリーン遷移
    public T PushScreen<T>(UnityAction<T> action = null)
        where T : ScreenPresenter
    {
        return PushScreen(typeof(T).Name, action);
    }

    /// <summary>
    /// スクリーン遷移　型安全でないためここから呼ぶのは非推奨
    /// </summary>
    public T PushScreen<T>(string screenType, UnityAction<T> action = null)
        where T : ScreenPresenter
    {
        OnScreenWillChange();
        var previewScreen = CurrentScreen;
        if (previewScreen != null)
        {
            previewScreen.OnMoveOut();
            beforeScreenStack.Push(previewScreen);
        }

        var onEndMoveInSubject = new AsyncSubject<T>();
        onEndMoveInSubject.Subscribe(
            x => CurrentScreen = x,
            () =>
            {
                OnScreenChanged();
                // 前のスクリーンを非表示にする
                if (previewScreen != null)
                {
                    previewScreen.gameObject.SetActive(false);
                }
            });

        // 生成
        return MoveTransition(screenType, action, onEndMoveInSubject);
    }


    public void BackScreen()
    {
        if (beforeScreenStack.Count < 1)
        {
            Debug.LogError("no stack");
            // 前の階層がないので何もしない
            return;
        }
        OnScreenWillChange();
        var nextScreen = beforeScreenStack.Pop();
        BackTransition(CurrentScreen, nextScreen).Subscribe(
            x => CurrentScreen = x,
            () => OnScreenChanged()
        );
    }

    IObservable<ScreenPresenter> BackTransition(ScreenPresenter preview, ScreenPresenter next)
    {
        next.gameObject.SetActive(true);
        var subject = new AsyncSubject<ScreenPresenter>();
        Observable.Concat(
            preview.OnBackOut(),
            next.OnBackIn()
        ).OnComplete(() =>
        {
            Destroy(preview.gameObject);
            subject.OnNext(next);
            subject.OnCompleted();
        });

        return subject;

    }

    // トランジション
    T MoveTransition<T>(string screenType, UnityAction<T> action = null, IObserver<T> observer = null)
        where T : ScreenPresenter
    {
        GameObject obj = Instantiate(ResourcesManager.Instance.GetScreen(screenType), screenContainer) as GameObject;
        T screen = obj.GetComponent<T>();

        var initSubject = screen.Initialize();
        if (action != null)
        {
            initSubject.Subscribe(_ => action(screen));
        }
        Observable.Concat
        (
            initSubject,
            screen.OnBeforeMoveIn(),
            screen.OnMoveIn()
        ).Subscribe(_ =>
        {
        }, () =>
        {
            screen.OnEndMoveIn();
            if (observer != null)
            {
                observer.OnNext(screen);
                observer.OnCompleted();
            }
        });
        return screen;
    }
}
