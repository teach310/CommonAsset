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

    void Reset()
    {
        screenContainer = this.transform.Find("Screen");
    }

    // 初期化
    public virtual IObservable<Unit> Initialize()
    {
        return Observable.ReturnUnit();
    }

    // OnOpenの直前
    public virtual IObservable<Unit> OnBeforeOpen()
    {

        return Observable.ReturnUnit();
    }

    // 自身が開かれる時
    public virtual IObservable<Unit> OnOpen()
    {
        return Observable.ReturnUnit();
        //DialogManager.Instance.CreateContent (DialogType.Common);
    }

    // 他のWindowが開かれることによって画面から消える時に呼ばれる．
    public virtual void OnOpenOut()
    {
    }

    // OnCloseInの直前
    public virtual IObservable<Unit> OnBeforeCloseIn()
    {
        return Observable.ReturnUnit();
    }

    // 他のWindowが閉じられて，自身が表示される時
    public virtual IObservable<Unit> OnCloseIn()
    {
        return Observable.ReturnUnit();
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
    public IObservable<T> MoveScreen<T>(UnityAction<T> action = null)
        where T : ScreenPresenter
    {
        return MoveScreen(typeof(T).Name, action);
    }

    /// <summary>
    /// スクリーン遷移　型安全でないためここから呼ぶのは非推奨
    /// </summary>
    public IObservable<T> MoveScreen<T>(string screenType, UnityAction<T> action = null)
        where T : ScreenPresenter
    {
        OnScreenWillChange();
        var previewScreen = CurrentScreen;
        if (previewScreen != null)
        {
            previewScreen.OnMoveOut();
            beforeScreenStack.Push(previewScreen);
            previewScreen.gameObject.SetActive(false);
        }


        var transition = MoveTransition(screenType, action);
        transition.Subscribe(
            x => CurrentScreen = x,
            () =>
            {
                OnScreenChanged();
                //// 前のスクリーンを非表示にする
                //if (previewScreen != null)
                //{
                //    previewScreen.gameObject.SetActive(false);
                //}
            });
        // 生成
        return transition;
    }


    public IObservable<ScreenPresenter> BackScreen()
    {
        OnScreenWillChange();
        var nextScreen = beforeScreenStack.Pop();
        var transition = BackTransition(CurrentScreen, nextScreen);
        transition.Subscribe(
            x => CurrentScreen = x,
            () => OnScreenChanged()
        );
        return transition;
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
    IObservable<T> MoveTransition<T>(string screenType, UnityAction<T> action = null)
        where T : ScreenPresenter
    {
        var subject = new AsyncSubject<T>();
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

            subject.OnNext(screen);
            subject.OnCompleted();
        });
        return subject;
    }

    public bool HasScreenStack() {
        return beforeScreenStack.Count > 0;
    }
}
