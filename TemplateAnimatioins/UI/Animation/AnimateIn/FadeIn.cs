using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Common.Tweening
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeIn : AnimateInUI
	{
		[SerializeField]
		private FadeModel model = new FadeModel ();
		public override AnimationModel Model {
			get {
				return model;
			}
		}

		protected override void Reset ()
		{
			model.Init(this.GetComponent<CanvasGroup> ());
			base.Reset ();
		}
	}
}
