using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(ED_DT2),true)]
public class ED_DT2_Inspector : Editor {

    ED_DT2 entity;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Debug.Log(" = ED_DT2_Inspector = ");
        entity = target as ED_DT2;

        if (GUILayout.Button("PlayEffect"))
        {
            if (entity.gobjEffect == null)
            {
                EditorUtility.DisplayDialog("提示", "请选特效Prefab", "Okey");
                return;
            }
            entity.DoPlay();
        }
    }
}
