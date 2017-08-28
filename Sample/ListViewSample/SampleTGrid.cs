using System.Collections;
using System.Collections.Generic;
using Common.UI;
using UnityEngine;

public class SampleTGrid : TGrid<SampleListItem.Model> {
    public int dataCount = 10;
    public float insertSpace = 20f;

    // Use this for initialization
    void Start () {
        Load();
    }

    void Load(){
        var list = new List<SampleListItem.Model>();
        for (int i = 0; i < dataCount; i++)
        {
            list.Add(new SampleListItem.Model());
        }
        SetData(list);
    }

    protected override float InsertSpace(int i)
    {
        if(i == 6)
            return insertSpace;

        return base.InsertSpace(i);
    }
}
