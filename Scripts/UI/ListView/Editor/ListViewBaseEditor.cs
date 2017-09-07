using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace Common.UI
{
    [CustomEditor(typeof(ListViewEdInterface), true)]
    public class ListViewEditor : Editor
    {
        ListViewEdInterface listView = null;

        private void OnEnable()
        {
            listView = (ListViewEdInterface) target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("ResetContent")){
                listView.ResetContent();
            }
        }
    }
}
