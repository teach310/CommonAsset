using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Zeke.Tweening
{
	public class ScaleIn : AnimateInUI
	{
		[SerializeField]
		private ScaleModel model = new ScaleModel ();
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
