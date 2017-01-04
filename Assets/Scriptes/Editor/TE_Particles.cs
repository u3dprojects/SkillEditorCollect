using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 技能播放测试文件
/// </summary>
[CanEditMultipleObjects]
public class TE_Particles : Editor {
    [MenuItem("Tools/RunningEffect")]
    static void ToolsRunSelectEffectInScene()
    {
        GameObject gobj = Selection.activeGameObject;
        if (gobj == null) {
            EditorUtility.DisplayDialog("提示", "当前没有选中任何对象!!!", "Okey");
            return;
        }
        PrefabType  pfType = PrefabUtility.GetPrefabType(gobj);
        if(pfType != PrefabType.PrefabInstance && pfType != PrefabType.None)
        {
            EditorUtility.DisplayDialog("提示", "请选Hierarchy窗口的对象!!!", "Okey");
            return;
        }

        bool isCurPs = false;
        ParticleSystem ps = gobj.GetComponent<ParticleSystem>();
        isCurPs = ps != null;

        if (ps == null)
            ps = gobj.GetComponentInChildren<ParticleSystem>();

        if (ps == null)
        {
            EditorUtility.DisplayDialog("提示", "选中的对象没有粒子特效!!!", "Okey");
            return;
        }

        ps.Stop(true);
        ps.Simulate(0, true, true);

        if (isCurPs)
        {    
            ps.Play(true);
        }else
        {
            // EDM_Timer.m_instance.DoInit();
            // EDM_Particle.m_instance.DoInit();
            // EDM_Particle.m_instance.DoActive(gobj);
        }
    }
}
