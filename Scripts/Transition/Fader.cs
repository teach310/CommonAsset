using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Tweening;
using DG.Tweening;

[RequireComponent(typeof(Canvas))]
public class Fader : SingletonMonoBehaviour<Fader>
{

    Canvas canvas;
    Canvas Canvas
    {
        get
        {
            if (canvas == null)
                canvas = this.GetComponent<Canvas>();
            return canvas;
        }
    }

    AnimateInUI fadeInAnim;
    AnimateOutUI fadeOutAnim;

    protected override void Awake()
    {
        base.Awake();
        fadeInAnim = this.GetComponentInChildren<AnimateInUI>(true);
        fadeOutAnim = this.GetComponentInChildren<AnimateOutUI>(true);
        fadeInAnim.Ready();
        Canvas.enabled = false;
    }

    public static Tween FadeIn()
    {
        Instance.Canvas.enabled = true;
        var tween = Instance.fadeInAnim.Play();
        return tween;
    }

    public static Tween FadeOut()
    {
        var tween = Instance.fadeOutAnim.Play();
        tween.OnComplete(() => Instance.Canvas.enabled = false);
        return tween;
    }
}
