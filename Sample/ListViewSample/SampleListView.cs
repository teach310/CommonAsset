using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common.UI;
public class SampleListView : ListView<SampleListItem.Model> {

    public int dataCount = 10;

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
}
