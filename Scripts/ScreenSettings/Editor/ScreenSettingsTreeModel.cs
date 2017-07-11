using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.Entity;

public class ScreenSettingsTreeModel : TreeModel
{
    public ScreenSettingsTreeModel(ScreenEntity screen, TreeModel parent = null)
        : base(screen.id, screen.name, parent)
    {

    }

    public ScreenSettingsTreeModel(WindowEntity window) : base(window.id, window.name)
    {

    }

    public ScreenSettingsTreeModel(int id, string name, TreeModel parent = null) : base(id, name, parent) { }

    //public ScreenSettingsTreeModel(int id, int childCount, string name = null, TreeModel parent = null)
    //    : base(GetId(id, childCount), GetScreenName(name), parent)
    //{

    //}
    // ファックトリメソッド
    public static ScreenSettingsTreeModel CreateSibling(ScreenSettingsTreeModel model){
        if(model.IsScreen())
            return new ScreenSettingsTreeModel(model.parent.id * 100, GetScreenName(), model.parent);
        //else
            //return new ScreenSettingsTreeModel()
        
    }

	public static int GetScreenId(int windowId, int childCount)
	{
		return windowId * 100 + childCount + 1;
	}

	public static string GetScreenName(string name = null)
	{
		if (string.IsNullOrEmpty(name))
			return "newScreen";
		return name;
	}

	public bool IsScreen()
	{
		if (parent != null)
			return true;
		return false;
	}
}
