using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class ED_DT2 : MonoBehaviour {

    public GameObject gobjEffect;

    bool isInit = false;

    // OnUpdate Update
    public void OnUpdate()
    {
        if (!isInit  && gobjEffect != null)
        {
            isInit = true;
            Debug.Log("=== init ===");
            DoPlay();
        }
    }

    [ContextMenu("dddddd")]
    void DoPlay()
    {
        Debug.Log("=== DoPlay ===");
        EDM_Timer.m_instance.DoReset();
        EDM_Particle.m_instance.DoReady(gobjEffect);
    }
    
#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
        EDM_Timer.m_instance.DoInit();
        EDM_Particle.m_instance.DoInit();
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        EDM_Particle.m_instance.DoPause();
        EDM_Timer.m_instance.DoPause();
    }
#endif
}
