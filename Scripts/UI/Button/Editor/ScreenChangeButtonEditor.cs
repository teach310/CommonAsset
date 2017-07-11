using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenChangeButton))]
public class ScreenChangeButtonEditor : Editor {
    SerializedProperty windowProperty;
    SerializedProperty screenProperty;

    private void OnEnable()
    {
        windowProperty = serializedObject.FindProperty("window");
        screenProperty = serializedObject.FindProperty("screen");
    }
    public override void OnInspectorGUI()
	{
        //EditorGUILayout.s
	}
}
