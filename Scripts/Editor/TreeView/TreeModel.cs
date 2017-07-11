using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeModel{
    public int id;
    public string displayName;
    public TreeModel parent;

    public TreeModel(int id, string displayName, TreeModel parent = null){
        this.id = id;
        this.displayName = displayName;
        this.parent = parent;
    }
}
