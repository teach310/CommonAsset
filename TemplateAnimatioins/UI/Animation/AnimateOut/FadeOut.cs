using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Zeke.Tweening
{
	[RequireComponent(typeof(CanvasGroup))]
	public class FadeOut : AnimateOutUI
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
