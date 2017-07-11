using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Scene上でGridのサイズを変更できるようにする拡張
/// </summary>
public class CustomGrid : GridLayoutGroup {
	#if UNITY_EDITOR
	// 子のRectTransformを更新するタイミング
	public override void SetLayoutHorizontal ()
	{
		UpdateCellSize ();
		base.SetLayoutHorizontal ();
	}

	// CellSizeを更新
	void UpdateCellSize(){
		// シーン上のオブジェクトであるか
		var obj = Selection.activeGameObject;
		if (!obj) {
			return;
		}

		// RectTransformを持っているか
		var tran = obj.GetComponent<RectTransform> ();
		if (!tran) {
			return;
		}

		// 親が自身であるか
		if (tran.parent == this.transform) {
			cellSize = new Vector2 (tran.rect.width, tran.rect.height);
		}
	}
	#endif
}
