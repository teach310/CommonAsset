using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
public static class AssetDataBaseUtils
{
    public static string GetAssetFullPath(UnityEngine.Object obj)
    {
        return string.Format("{0}{1}",
                             Application.dataPath,
                             AssetDatabase.GetAssetPath(obj).Remove(0, "Assets".Length)
                            );
    }

    /// <summary>
    /// 相対パスをフルパスに変換
    /// </summary>
    public static string GetAssetFullPath(string assetPath)
    {
        return string.Format("{0}{1}",
            Application.dataPath,
            assetPath.Remove(0, "Assets".Length)
        );
    }

    public static bool Exists(string assetPath){
        return File.Exists (GetAssetFullPath (assetPath));
    }
}
