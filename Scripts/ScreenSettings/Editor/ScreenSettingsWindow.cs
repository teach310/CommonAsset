﻿using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.IO;

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

        if(treeView == null){
            InitTreeView();
        }

        GUILayout.Space(5f);
        ToolBar();
        GUILayout.Space(3f);

		EditorGUILayout.LabelField("");
		var rect = GUILayoutUtility.GetLastRect();
        treeView.OnGUI(new Rect(rect.x, rect.y, 210f, position.height - rect.height));
    }

    void ToolBar() {
        if (GUILayout.Button("ExportScript")){
            ExportScreenSettings();
        }
    }

    void ExportScreenSettings(){
        string settingsPath = AssetDataBaseUtils.GetAssetFullPath(settings);
        string dir = Path.GetDirectoryName(settingsPath);
        //Window
        string windowDir = dir + "/Window";
        DirectoryUtils.SafeCreateDirectory(windowDir);
        foreach (var item in settings.windows)
        {
            ScriptCreater.Create("WindowTemplate", string.Format("{0}/{1}.cs", windowDir, item.name));
        }

        string screenDir = dir + "/Screen";
        DirectoryUtils.SafeCreateDirectory(screenDir);
        foreach (var item in settings.screens)
		{
            ScriptCreater.Create("ScreenTemplate", string.Format("{0}/{1}.cs", screenDir, item.name));
		}

    }
}
