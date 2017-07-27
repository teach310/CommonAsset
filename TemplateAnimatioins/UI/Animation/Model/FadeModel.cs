using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Common.Tweening
{
	[System.Serializable]
	public class FadeModel : AnimationModel
	{

		public float startValue;
		public float endValue;

		[SerializeField]
		private CanvasGroup canvasGroup;

		public FadeModel(){}

		public void Init(CanvasGroup canvasGroup){
			this.canvasGroup = canvasGroup;
		}

		public override void SetStartParams ()
		{
			canvasGroup.alpha = startValue;
		}

		public override void SetEndParams ()
		{
			canvasGroup.alpha = endValue;
		}

		public override void SaveStartParams ()
		{
			startValue = canvasGroup.alpha;
		}

		public override void SaveEndParams ()
		{
			endValue = canvasGroup.alpha;
		}

		public override void Ready ()
		{
			base.Ready ();
			canvasGroup.alpha = startValue;
		}

		public override Tween Play ()
		{
			return canvasGroup.DOFade (endValue, duration).SetEase (ease).SetDelay (delay);
		}
	}
}