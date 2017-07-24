using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScreenChangeButton))]
public class ScreenChangeButtonEditor : Editor {

    SerializedProperty goRoot;
    SerializedProperty window;
    SerializedProperty screen;

    void OnEnable()
    {
        goRoot = serializedObject.FindProperty("goRoot");
        window = serializedObject.FindProperty("window");
        screen = serializedObject.FindProperty("screen");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(goRoot);

        if(goRoot.boolValue == true){
            EditorGUILayout.PropertyField(window);    
        }else{
            EditorGUILayout.PropertyField(screen);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
