#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.UI
{
    public class RaycastDetector : Graphic{

        protected override void OnPopulateMesh(VertexHelper v)
        {
            base.OnPopulateMesh(v);
            v.Clear();
        }
        #if UNITY_EDITOR

        [CustomEditor(typeof(RaycastDetector))]
        class GraphicCastEditor : Editor
        {
            public override void OnInspectorGUI() {
            }
        }

        #endif
    } 
}