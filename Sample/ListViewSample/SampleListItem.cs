using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleListItem : ListViewItem<SampleListItem.Model> {

    public Text text;

    public class Model{
        public string text = "a";
    }

	public override void UpdateData(SampleListItem.Model data)
    {
        text.text = DataIndex.ToString();
    }
}
