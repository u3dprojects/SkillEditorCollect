using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 类名 : 粒子特效管理
/// 作者 : Canyon
/// 日期 : 2016-12-27 10:10
/// 功能 : 
/// </summary>
// [ExecuteInEditMode]
public class EDM_Particle : MonoBehaviour {

    private static EDM_Particle _m_instance;

    static public EDM_Particle m_instance
    {
        get
        {
            if (_m_instance == null)
            {
                GameObject gobj = GameObject.Find("EDM_Particle");
                if (gobj == null)
                {
                    gobj = new GameObject("EDM_Particle");
                }

                _m_instance = gobj.GetComponent<EDM_Particle>();
                if (_m_instance == null)
                {
                    _m_instance = gobj.AddComponent<EDM_Particle>();
                }
            }
            return _m_instance;
        }
    }

    List<DBU3D_Effect> list = new List<DBU3D_Effect>();
    List<DBU3D_Effect> rmList = new List<DBU3D_Effect>();
    DBU3D_Effect tmp;
    int lens = 0;
    bool isPause = false;
    float curSpeed = 1.0f;

    // 添加更新频率限定
    //更新间隔
    float m_InvUpdate = 0.05f;
    // 当前值
    float m_CurInvUp = 0.0f;

    public void DoInit()
    {
        DoClear();
    }
    
    public void DoReady(GameObject gobjFab, Transform trsfParent = null)
    {
        DoReady(gobjFab, Vector3.zero, Vector3.zero, 1, trsfParent);
    }

    public void DoReady(GameObject gobjFab, Vector3 locPos,Vector3 locRotation,float scale = 1,Transform trsfParent = null)
    {
        if (gobjFab == null)
        {
            return;
        }
        UnityEngine.GameObject gobj = GameObject.Instantiate(gobjFab, Vector3.zero, Quaternion.identity) as GameObject;
        Transform trsfGobj = gobj.transform;
        if (trsfParent != null)
        {
            trsfGobj.parent = trsfParent;
        }
        trsfGobj.localPosition = locPos;
        trsfGobj.localEulerAngles = locRotation;
        trsfGobj.localScale = Vector3.one;

        DoActive(gobj,scale);
    }

    public void DoActive(GameObject goEntity, float scale = 1)
    {
        if (goEntity == null)
        {
            return;
        }

        DBU3D_Effect db = new DBU3D_Effect(goEntity);
        db.DoStart();
        db.SetScale(scale);
        list.Add(db);
    }

    public void OnUpdate()
    {
        OnUpdate(EDM_Timer.m_instance.DeltaTime);
    }

    public void OnUpdate(float deltatime,bool isImm = false)
    {
        lens = list.Count;
        if (lens <= 0)
            return;

        if (!isImm) {
            if (isPause)
                return;
        }

        m_CurInvUp += deltatime;
        if (m_CurInvUp < m_InvUpdate)
            return;
        
        // Debug.Log("== EDM_Particle delta = " + m_CurInvUp);

        for (int i = 0; i < lens; i++)
        {
            tmp = list[i];
            tmp.DoUpdate(m_CurInvUp * curSpeed);
        }

        m_CurInvUp = 0.0f;
        OnClearParticle();

        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    void OnClearParticle(bool isAll = false)
    {
        rmList.Clear();

        lens = list.Count;
        bool isCanMv = false;
        for (int i = 0; i < lens; i++)
        {
            tmp = list[i];
            isCanMv = tmp.isEnd;

            if (isAll || isCanMv) {
                rmList.Add(tmp);
            }
        }

        lens = rmList.Count;
        for (int i = 0; i < lens; i++)
        {
            tmp = rmList[i];
            tmp.DoDestory();
            list.Remove(tmp);
        }
        
        tmp = null;
    }

    public void DoClear()
    {
        OnClearParticle(true);

        list.Clear();
        rmList.Clear();

        tmp = null;
        isPause = false;
        curSpeed = 1.0f;
    }

    public void DoPause()
    {
        isPause = true;
    }

    public void DoResume()
    {
        isPause = false;
    }

    public void SetSpeed(float speed)
    {
        curSpeed = speed;
    }

#if UNITY_EDITOR
    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        DoClear();
        EditorApplication.update -= OnUpdate;
    }
#endif

    // Use this for initialization
    void Start () {
        Debug.Log("= EDM_Particle Start =");
    }
	
	// Update is called once per frame
	void Update () {
#if !UNITY_EDITOR
        OnUpdate();
#endif
    }
}
