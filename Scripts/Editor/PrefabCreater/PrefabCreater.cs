using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class PrefabCreater {

	// テンプレートがあるディレクトリへのパス
    public static readonly string TEMPLATE_PREFAB_DIRECTORY_PATH = "/Common/Scripts/Editor/PrefabCreater/Templates";

    /// <summary>
    /// Assetからの相対パスで指定する．
    /// </summary>
    public static void Create(string templateName, string filePath, System.Action<GameObject> setParam)
    {
        if (AssetDataBaseUtils.Exists(filePath)) {
            return;
        }

        string dir = System.IO.Path.GetDirectoryName (filePath);
        DirectoryUtils.SafeCreateDirectory (dir);
        if (AssetDatabase.CopyAsset (GetTemplatePath(templateName), filePath)) {
            var go = AssetDatabase.LoadAssetAtPath<GameObject> (filePath);
            setParam (go);
        }
    }

    static string GetTemplatePath(string templateName){
        string[] lookFor = new string[] { "Assets" + TEMPLATE_PREFAB_DIRECTORY_PATH };
        var guids = AssetDatabase.FindAssets (templateName, lookFor);
        if (guids.Length == 0) {
            throw new System.IO.FileNotFoundException (templateName + " does not found");
        }
        var path = AssetDatabase.GUIDToAssetPath (guids [0]);
        return path;
    }
}
