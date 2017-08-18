using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Tweening;
public static class DialogTools {

    public static void AddDefaultAnimation(GameObject target){
        target.AddComponent<FadeIn>();
        target.AddComponent<ScaleIn>();
        target.AddComponent<FadeOut>();
        target.AddComponent<FadeOut>();
    }
}
