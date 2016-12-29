using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ED_DT2),true)]
public class ED_DT2_Inspector : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Debug.Log(" = ED_DT2_Inspector = ");
        // this.Repaint();
    }
}
