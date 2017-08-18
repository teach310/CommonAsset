using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneTools : EditorWindow {

    [MenuItem("Tools/SceneTools")]
    static void Open()
    {
        var window = GetWindow<SceneTools>("SceneTools");
    }
}
