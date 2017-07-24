using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

[RequireComponent(typeof(Button))]
public class BackButton : MonoBehaviour
{
#if UNITY_EDITOR
    void Reset()
    {
        var button = this.GetComponent<Button>();
        RemoveAllPersistentListener(button.onClick);
        UnityEventTools.AddPersistentListener(button.onClick, BackScreen);
    }

    void RemoveAllPersistentListener(UnityEventBase e){
        while(e.GetPersistentEventCount() > 0){
            UnityEventTools.RemovePersistentListener(e, 0);
        }
    }
#endif

    void BackScreen() { 
        ScenePresenter.Instance.BackScreen();
    }
}
