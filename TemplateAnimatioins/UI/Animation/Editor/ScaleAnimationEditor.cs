using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

namespace Common.Tweening
{
	/// <summary>
	/// Scale アニメーションの共通処理
	/// </summary>
	public class ScaleAnimationEditor : UIAnimationEditor
	{
		SerializedProperty easeYprop;

		protected override void OnEnable ()
		{
			base.OnEnable ();
			easeYprop = modelSO.FindPropertyRelative ("easeY");
		}

		protected override void DrawEaseSettings ()
		{
			EditorGUILayout.PropertyField (easeProp, new GUIContent ("EaseX"));
			EditorGUILayout.PropertyField(easeYprop, new GUIContent("EaseY"));
		}
	}

	[CustomEditor (typeof(ScaleIn), true)]
	[CanEditMultipleObjects]
	public class ScaleInEditor:ScaleAnimationEditor
	{
	}

	[CustomEditor (typeof(ScaleOut), true)]
	[CanEditMultipleObjects]
	public class ScaleOutEditor:ScaleAnimationEditor
	{
	}
}
