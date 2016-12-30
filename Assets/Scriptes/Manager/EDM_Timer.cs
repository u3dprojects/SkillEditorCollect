using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 类名 : 时间管理
/// 作者 : Canyon
/// 日期 : 2016-12-28 09:20
/// 功能 : 
/// </summary>
/// [ExecuteInEditMode]
public class EDM_Timer : MonoBehaviour {

    private static EDM_Timer _m_instance;

    static public EDM_Timer m_instance
    {
        get
        {
            if (_m_instance == null)
            {
                GameObject gobj = GameObject.Find("EDM_Timer");
                if (gobj == null)
                {
                    gobj = new GameObject("EDM_Timer");
                }

                _m_instance = gobj.GetComponent<EDM_Timer>();
                if (_m_instance == null)
                {
                    _m_instance = gobj.AddComponent<EDM_Timer>();
                }
            }
            return _m_instance;
        }
    }

    DBOpt_Time m_time = new DBOpt_Time();

//#if UNITY_EDITOR
//    void OnEnable()
//    {
//        EditorApplication.update += OnUpdate;
//    }

//    void OnDisable()
//    {
//        EditorApplication.update -= OnUpdate;
//    }
//#endif

    public void DoPause()
    {
        m_time.DoPause();
    }

    public void DoResume()
    {
        m_time.DoResume();
    }

    public void DoInit()
    {
        m_time.DoReInit(false);
    }

    public void DoReset()
    {
        m_time.OnResetMemberReckon();
    }

    public float DeltaTime
    {
        get
        {
            return m_time.DeltaTime;
        }
    }

    public float ProgressTime
    {
        get
        {
            return m_time.ProgressTime;
        }
    }

    public void OnUpdate()
    {
        m_time.DoUpdateTime();
    }

    // Use this for initialization
    void Start () {
        Debug.Log("= EDM_Timer Start =");
    }
	
	// Update is called once per frame
	void Update () {
#if !UNITY_EDITOR
        OnUpdate();
#endif
    }
}
