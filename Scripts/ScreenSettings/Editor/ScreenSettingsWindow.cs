﻿using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;

public class ScreenSettingsWindow : EditorWindow
{
    
    [MenuItem("Tools/ScreenSettingsWindow")]
    public static void Open()
    {
        GetWindow<ScreenSettingsWindow>("ScreenSettings");
    }

    ScreenSettings settings;

    TreeViewState state;
    ScreenTreeView treeView;
    SearchField searchField;


    void OnSelectionChange()
    {
        var treeAsset = Selection.activeObject as ScreenSettings;
        if (treeAsset != null && treeAsset != settings)
        {
            settings = treeAsset;
            InitTreeView();
            Repaint();
        }
    }

    void OnEnable()
    {
        Undo.undoRedoPerformed += OnUndoRedoPerformed;
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

    void InitTreeView(){
        if (settings == null)
            return;

		state = new TreeViewState();
        treeView = new ScreenTreeView(state, settings);
		treeView.Reload();
		// ScriptableObject取得

		searchField = new SearchField();
		searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
    }

    void OnGUI()
    {
        if (settings == null)
        {
            EditorGUILayout.LabelField("ScreenSettingsを選択してください");
            return;
        }

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
