using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Common.Entity;
using UnityEngine;

public static class ScreenSettingsLoader
{
    public static string assetPath = "Assets/Common/Scripts/ScreenSettings/ScreenSettings.asset";
    static ScreenSettings settings;
    public static ScreenSettings Settings
    {
        get
        {
            if (settings == null)
            {
                settings = AssetDatabase.LoadAssetAtPath<ScreenSettings>(assetPath);
            }
            return settings;
        }
    }

    public static void Save() { 
        EditorUtility.SetDirty(settings);
    }
}

public static class WindowRepository{
	public static WindowEntity FindById(int id)
	{
		return ScreenSettingsLoader.Settings.windows.Find(x => x.id == id);
	}
}

public static class ScreenRepository{
    public static ScreenEntity FindById(int id){
        return ScreenSettingsLoader.Settings.screens.Find(x => x.id == id);
    }
}
