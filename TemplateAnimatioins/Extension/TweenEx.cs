using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public static class TweenEx{

	// EaseをXYで分けたDOScale
	public static Tween DOScale(this Transform tran, Vector3 endValue, float duration, Ease easeX, Ease easeY){
		var sequence = DOTween.Sequence ();
		sequence.Join(tran.DOScaleX(endValue.x, duration).SetEase(easeX));
		sequence.Join(tran.DOScaleY(endValue.y, duration).SetEase(easeY));
		return sequence;
	}
}
