using System.Collections;
using UnityEngine;
using UnityEditor;
public static class AssetDataBaseUtils
{

    public static string GetAssetFullPath(UnityEngine.Object obj)
    {
        return string.Format("{0}{1}",
                             Application.dataPath,
                             AssetDatabase.GetAssetPath(obj).Remove(0, "Assets".Length)
                            );
    }
}
