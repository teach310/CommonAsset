using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

public class ScreenSettingsWindow : EditorWindow
{
    
    [MenuItem("Tools/ScreenSettingsWindow")]
    public static void Open()
    {
        GetWindow<ScreenSettingsWindow>("ScreenSettings");
    }


    SerializedObject so = null;
    ScreenSettings settings;

    Vector2[] scroll = { Vector2.zero, Vector2.zero };



    TreeViewState state;
    ScreenTreeView treeView;
    SearchField searchField;


    //void OnSelectionChange()
    //{
    //    var treeAsset = Selection.activeObject as ScreenSettings;
    //    if (treeAsset != null && treeAsset != settings)
    //    {
    //        settings = treeAsset;
    //    }
    //}

    void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedoPerformed;


        state = new TreeViewState();
        treeView = new ScreenTreeView(state, ScreenSettingsUseCase.Load());
        treeView.Reload();
        // ScriptableObject取得

        searchField = new SearchField();
        searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
    }

    private void OnDisable()
    {
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
    }

    void OnUndoRedoPerformed()
    {
        if (treeView != null)
        {
            // 読み込み
            treeView.Reload();
        }
    }
    void OnGUI()
    {
        GUILayout.Space(5f);
        ToolBar();
        GUILayout.Space(3f);
		//const float topToolbarHeight = 20f;
		//Rect toolbarRect = new Rect()

		EditorGUILayout.LabelField("");
		var rect = GUILayoutUtility.GetLastRect();
        treeView.OnGUI(new Rect(rect.x, rect.y, 210f, position.height - rect.height));
        //using (new EditorGUILayout.VerticalScope())
        //{
        //    TopPanel();
        //    using (new EditorGUILayout.HorizontalScope())
        //    {
        //        LeftPabel();
        //        GUILayout.Space(5);
        //        RightPanel();
        //    }
        //}
    }

    void ToolBar() { 
    }
    // void TopPanel()
    // {
    //     using (new EditorGUILayout.HorizontalScope())
    //     {
    //         // ScreenのPath
    //         GUILayout.Label("Path");
    //         //settings.directoryPath = EditorGUILayout.TextField(settings.directoryPath, GUILayout.Width(80));
    //         GUILayout.FlexibleSpace();
    //         // Load
    //         if (GUILayout.Button("Load", GUILayout.Width(80)))
    //         {
    //             Debug.Log("Load");
    //         }
    //         // Save
    //         if (GUILayout.Button("Save", GUILayout.Width(80)))
    //         {
    //             Debug.Log("Save");
    //         }
    //     }
    // }

     //void LeftPabel()
     //{
         //// Window名
         //using (new EditorGUILayout.VerticalScope(GUI.skin.box, GUILayout.Width(220f)))
         //{
             //GUILayout.Space(5);
    //if (GUILayout.Button("Add", GUILayout.Width(80)))
    //{
    //    // Window加算
    //}

    //EditorGUILayout.LabelField("");
    //var rect = GUILayoutUtility.GetLastRect();
        //    treeView.OnGUI(new Rect(rect.x, rect.y, 210f, position.height - rect.height));
        //    GUILayout.FlexibleSpace();
        //}

    //}

    //void RightPanel()
    //{
    //    // Screen名
    //}
}
