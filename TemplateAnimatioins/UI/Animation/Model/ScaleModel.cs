using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Common.Tweening
{
	[System.Serializable]
	public class ScaleModel : AnimationModel
	{
		public Vector3 startValue;
		public Vector3 endValue;
		[SerializeField]
		private RectTransform rectTransform;

		public Ease EaseX{
			get{ return ease; }
			set{ ease = value; }
		}

		[SerializeField, HideInInspector]
		private Ease easeY;
		public Ease EaseY {
			get{ return easeY; }
			set{ easeY = value; }
		}

		public ScaleModel(){
		}

		public void Init(RectTransform rectTransform){
			this.rectTransform = rectTransform;
		}

		public override void SetStartParams ()
		{
			this.rectTransform.localScale = startValue;
		}

		public override void SetEndParams ()
		{
			this.rectTransform.localScale = endValue;
		}

		public override void SaveStartParams ()
		{
			startValue = this.rectTransform.localScale;
		}

		public override void SaveEndParams ()
		{
			endValue = this.rectTransform.localScale;
		}

		public override void Ready ()
		{
			base.Ready ();
			this.rectTransform.localScale = startValue;
		}

		public override Tween Play ()
		{
			return rectTransform.DOScale(endValue, duration,EaseX,EaseY);
		}
	}
}
