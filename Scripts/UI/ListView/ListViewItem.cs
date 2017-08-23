using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListViewItem<T> : UIBase {

    public int DataIndex{get;set;}

    public abstract void UpdateData(T data);
}
