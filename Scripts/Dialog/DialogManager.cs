using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;




public class DialogManager : SingletonMonoBehaviour<DialogManager>
{

	public class MessageBoxResponse : AsyncRequest<Response>
	{
		public Response Response { get { return data; } }
	}

    public enum Response{
        OK,
        CANCEL,
        YES,
        NO
    }

    public enum Buttons{
        OK,
        OK_CANCEL,
        YES_NO,
        YES_NO_CENCEL,
    }

    //static List<DialogPresenter> dialogStack = new List<DialogPresenter>();

    // 簡潔なやつ
    public static MessageBoxResponse MessageBox(string title, string description, Buttons buttons = Buttons.OK)
    {
        var response = new MessageBoxResponse();
        Open<MessageBox>(x =>
        {
            x.ResetView(title, description);
            x.OnYes.Subscribe(_=>OnClickDialogButton(x, response, Response.OK));
            x.OnNo.Subscribe(_=> OnClickDialogButton(x, response, Response.NO));
        });
        return response;
    }

    // レスポンスを決定して閉じる
    static void OnClickDialogButton(DialogPresenter dialog,  MessageBoxResponse messageBoxResponse, Response response){
        messageBoxResponse.Done(response);
        Close(dialog);
    }

    public static void Open<T>(UnityAction<T> action = null)
        where T: DialogPresenter
    {
        Instance.StartCoroutine(OpenCoroutine(typeof(T).Name, action));
    }

    // SelectManyに変更することで3フレーム早くなる = 0.1秒
    static IEnumerator OpenCoroutine<T>(string dialogName, UnityAction<T> action = null)
        where T:DialogPresenter
    {
        // 生成
        var obj = Instantiate(ResourcesManager.Instance.GetDialog(dialogName), Instance.transform) as GameObject;
        T dialog = obj.GetComponent<T>();
        var canvas = dialog.GetComponent<Canvas>();
        canvas.enabled = false;
        // 一旦削除
//        canvas.overrideSorting = true;
//        canvas.sortingOrder = 200;
        yield return dialog.Initialize().ToYieldInstruction();
        if(action != null)
            action(dialog);
        yield return dialog.OnBeforeOpen().ToYieldInstruction();
        canvas.enabled = true;
        yield return dialog.OnOpen().ToYieldInstruction();
        //dialogStack.Add(dialog);
    }

    public static void Close(DialogPresenter dialog)
    {
        Instance.StartCoroutine(CloseCoroutine(dialog));
    }

    static IEnumerator CloseCoroutine(DialogPresenter dialog){
        yield return dialog.OnBeforeClose().ToYieldInstruction();
        yield return dialog.OnClose().ToYieldInstruction();
        Destroy(dialog.gameObject);
    }
}

