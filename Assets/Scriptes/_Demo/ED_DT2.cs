using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ED_DT2 : MonoBehaviour {

    public GameObject gobjEffect;

    public bool isAddToSelf = false;

    [InitializeOnLoadMethod]
    static public void Test()
    {
        Debug.Log("= InitializeOnLoadMethod = test = ");
    }

    // OnUpdate Update
    public void OnUpdate()
    {
        
    }

    [ContextMenu("DoPlay")]
    public void DoPlay()
    {
        if (gobjEffect == null)
            return;

        Debug.Log("=== DoPlay ===");
        EDM_Timer.m_instance.DoReset();
        EDM_Particle.m_instance.DoClear();
        Transform trsfParent = null;
        if (isAddToSelf)
        {
            trsfParent = transform;
        }
        EDM_Particle.m_instance.DoReady(gobjEffect, trsfParent);
    }
    
#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
        EditorApplication.update += EDM_Particle.m_instance.OnUpdate;
        EditorApplication.update += EDM_Timer.m_instance.OnUpdate;

        EDM_Timer.m_instance.DoInit();
        EDM_Particle.m_instance.DoInit();
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        EditorApplication.update -= EDM_Particle.m_instance.OnUpdate;
        EditorApplication.update -= EDM_Timer.m_instance.OnUpdate;

        EDM_Particle.m_instance.DoClear();
    }
#endif
}
