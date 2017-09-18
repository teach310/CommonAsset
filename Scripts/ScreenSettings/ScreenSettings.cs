using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Common.Entity;

[CreateAssetMenu(fileName = "ScreenSettings", menuName = "ScreenSettings", order = 1)]
public class ScreenSettings : ScriptableObject
{
    public List<WindowEntity> windows = new List<WindowEntity>();

    public List<ScreenEntity> screens = new List<ScreenEntity>();

    public ScreenSettings(){
        windows.Add (new WindowEntity (1, "NewWindow"));
    }
}
