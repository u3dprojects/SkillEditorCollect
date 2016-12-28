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
[ExecuteInEditMode]
public class ED_Time_Manager : MonoBehaviour {

    private static ED_Time_Manager _m_instance;

    static public ED_Time_Manager m_instance
    {
        get
        {
            if (_m_instance == null)
            {
                GameObject gobj = GameObject.Find("EDTimeManager");
                if (gobj == null)
                {
                    gobj = new GameObject("EDTimeManager");
                }

                _m_instance = gobj.GetComponent<ED_Time_Manager>();
                if (_m_instance == null)
                {
                    _m_instance = gobj.AddComponent<ED_Time_Manager>();
                }
            }
            return _m_instance;
        }
    }

    DBOpt_Time m_time = new DBOpt_Time();

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += Update;
    }

    void OnDisable()
    {
        EditorApplication.update -= Update;
    }
#endif

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

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_time.DoUpdateTime();
    }
}
