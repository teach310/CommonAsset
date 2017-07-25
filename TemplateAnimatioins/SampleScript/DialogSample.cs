using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zeke.Tweening; // 1
using DG.Tweening; // 2
namespace Zeke
{
	public class DialogSample : MonoBehaviour
	{
		public Button closeButton;

		public AnimateInUI animateIn;
		public AnimateOutUI animateOut;

		void Awake(){
			closeButton.onClick.AddListener (Close);
		}

		public void Close(){
			animateOut.Play ().OnComplete (()=>this.gameObject.SetActive(false));
		}
	}
}
