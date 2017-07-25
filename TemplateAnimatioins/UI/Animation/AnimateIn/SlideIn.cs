using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Zeke.Tweening
{
	public class SlideIn : AnimateInUI
	{
		[SerializeField]
		private SlideModel model = new SlideModel ();
		public override AnimationModel Model {
			get {
				return model;
			}
		}

		protected override void Reset ()
		{
			model.Init (rectTransform);
			base.Reset ();
		}
	}
}
