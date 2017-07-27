using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;
using System;

namespace Common.Tweening
{

	[CustomEditor (typeof(UIAnimation), true)]
	[CanEditMultipleObjects]
	public class UIAnimationEditor : Editor
	{
		UIAnimation uIAnimation;

		AnimationModel model;
		protected SerializedProperty modelSO;

		protected SerializedProperty isExtraSettings;
		protected SerializedProperty delayProp;
		protected SerializedProperty easeProp;

		protected virtual void OnEnable(){
			uIAnimation = (UIAnimation)target;
			model = uIAnimation.Model;
			modelSO = serializedObject.FindProperty ("model");

			isExtraSettings = serializedObject.FindProperty("isExtraSettings");
			delayProp = modelSO.FindPropertyRelative ("delay");
			easeProp = modelSO.FindPropertyRelative ("ease");
		}



		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();
			DrawDefaultInspector ();
			GUILayout.Space (5);
			DrawExtraSettings ();
			GUILayout.Space (5);
			DrawButtons ();
			serializedObject.ApplyModifiedProperties ();
		}

		protected virtual void DrawExtraSettings(){
			isExtraSettings.boolValue = AnimationEdGUI.Foldout ("Extra Settings", isExtraSettings.boolValue);
			if (isExtraSettings.boolValue) {
				using (new EditorGUILayout.VerticalScope (GUI.skin.box)) {
					EditorGUILayout.PropertyField (delayProp, new GUIContent("Delay"));
					DrawEaseSettings ();
				}
			}
		}

		// Easeの設定
		protected virtual void DrawEaseSettings(){
			
			EditorGUILayout.PropertyField (easeProp, new GUIContent("Ease"));
		}

		protected virtual void DrawButtons ()
		{
			using (new EditorGUILayout.HorizontalScope ()) {
				string currentLabel = uIAnimation.IsStart ? "Start" : "End";

				if (GUILayout.Button (currentLabel)) {

					Undo.RecordObject (uIAnimation, "Change State");
					uIAnimation.IsStart = !uIAnimation.IsStart;
					if (uIAnimation.IsStart) {
						model.SetStartParams ();
					} else {
						model.SetEndParams ();
					}
				}
				if (GUILayout.Button ("Save")) {
					Undo.RecordObject (uIAnimation, "Save");
					if (uIAnimation.IsStart) {
						model.SaveStartParams ();
					} else {
						model.SaveEndParams ();
					}
				}
			}
		}


	}

	public static class AnimationEdGUI{

		/// <summary>
		/// FoldOutをかっこよく表示
		/// </summary>
		public static bool Foldout(string title, bool display)
		{
			var style = new GUIStyle("ShurikenModuleTitle");
			style.font = new GUIStyle(EditorStyles.label).font;
			style.border = new RectOffset(15, 7, 4, 4);
			style.fixedHeight = 22;
			style.contentOffset = new Vector2(20f, -2f);

			var rect = GUILayoutUtility.GetRect(16f, 22f, style);
			GUI.Box(rect, title, style);

			var clickRect = new Rect(rect);
			clickRect.width -= 180f;

			var e = Event.current;

			var drawRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
			if (e.type == EventType.Repaint)
			{
				EditorStyles.foldout.Draw(drawRect, false, false, display, false);
			}

			if (e.type == EventType.MouseDown && clickRect.Contains(e.mousePosition))
			{
				display = !display;
				e.Use();
			}

			return display;
		}
	}

}
