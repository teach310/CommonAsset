using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class UIBase : MonoBehaviour {

	private RectTransform _RectTransform;
	public RectTransform rectTransform{
		get{
			if(_RectTransform == null)
				_RectTransform = this.GetComponent<RectTransform>();
			return _RectTransform;
		}
	}
}
