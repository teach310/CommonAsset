using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Windowを管理する
public class ScenePresenter : SingletonMonoBehaviour<ScenePresenter> {

	public static WindowPresenter CurrentWindow;
    public WindowPresenter initialWindow;

	void Start(){
        OpenWindow<WindowPresenter>(initialWindow.name);
	}

    public T OpenWindow<T>(UnityAction<T> action = null)
        where T : WindowPresenter
    {
        return OpenWindow(typeof(T).Name, action);
    }

    public T OpenWindow<T>(string windowType, UnityAction<T> action = null)
		where T : WindowPresenter
	{
		GameObject obj = Instantiate (ResourcesManager.Instance.GetWindow (windowType), this.transform) as GameObject;
        T window = obj.GetComponent<T>();

		
		window.Initialize ();
        if (action != null)
		{
			action (window);
		}
		window.OnBeforeOpen();
		window.OnOpen();
		CurrentWindow = window;
        return window;
	}
}
