using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Common.Tweening
{
	[System.Serializable]
	public class SlideModel : AnimationModel
	{
		public Vector2 startValue;
		public Vector2 endValue;
		[SerializeField]
		private RectTransform rectTransform;

		public SlideModel(){
		}

		public void Init(RectTransform rectTransform){
			this.rectTransform = rectTransform;
		}

		public override void SetStartParams ()
		{
			this.rectTransform.anchoredPosition = startValue;
		}

		public override void SetEndParams ()
		{
			this.rectTransform.anchoredPosition = endValue;
		}

		public override void SaveStartParams ()
		{
			startValue = this.rectTransform.anchoredPosition;
		}

		public override void SaveEndParams ()
		{
			endValue = this.rectTransform.anchoredPosition;
		}

		public override void Ready ()
		{
			base.Ready ();
			this.rectTransform.anchoredPosition = startValue;
		}

		public override Tween Play ()
		{
			base.Play ();
			return rectTransform.DOAnchorPos(endValue, duration).SetEase(ease).SetDelay(delay);
		}
	}
}
