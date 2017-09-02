using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Common.UI
{
    public class ListViewBaseEditor<T> : Editor
    {
        public override void OnInspectorGUI()
        {
            var listView = (ListViewBase<T>)target;
            DrawDefaultInspector();
            if(GUILayout.Button("ResetAllItems")){
                listView.ResetAllItems();
            }
        }
    }
}
