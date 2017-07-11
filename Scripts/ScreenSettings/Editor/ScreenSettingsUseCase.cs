using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common.Entity;

public static class ScreenSettingsUseCase
{

    public static void SaveName(ScreenSettingsTreeModel model)
    {
        if (model.IsScreen())
        {
            ScreenRepository.FindById(model.id).name = model.displayName;
        }
        else
        {
            WindowRepository.FindById(model.id).name = model.displayName;
        }
        ScreenSettingsLoader.Save();
    }

    public static void Save(List<ScreenSettingsTreeModel> models)
    {
        var windows = models
            .Where(x => !x.IsScreen())
            .Select(x => new WindowEntity(x.id, x.displayName))
            .ToList();
        var screens = models
            .Where(x => x.IsScreen())
            .Select(x => new ScreenEntity(x.id, x.parent.id, x.displayName))
            .ToList();
        ScreenSettingsLoader.Settings.windows = windows;
        ScreenSettingsLoader.Settings.screens = screens;
        ScreenSettingsLoader.Save();
    }

    public static List<ScreenSettingsTreeModel> Load()
    {
        var list = new List<ScreenSettingsTreeModel>();
        var settings = ScreenSettingsLoader.Settings;
        for (int i = 0; i < settings.windows.Count(); i++)
        {
            var window = settings.windows[i];
            var windowModel = new ScreenSettingsTreeModel(window);
            list.Add(windowModel);

            var screens = settings.screens.Where(x => x.windowId == window.id);
            foreach (var screen in screens)
            {
                var screenItem = new ScreenSettingsTreeModel(screen, windowModel);
                list.Add(screenItem);
            }
        }
        return list;
    }

    public static void AddChild(ScreenSettingsTreeModel parent) {
        
    }
	
    public static void SetupIds(List<ScreenSettingsTreeModel> list){
        
    }
}
