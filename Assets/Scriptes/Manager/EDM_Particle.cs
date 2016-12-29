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

    List<DBU3D_Particle> list = new List<DBU3D_Particle>();
    DBU3D_Particle tmp;
    int lens = 0;
    bool isPause = false;

    public void DoInit()
    {
        DoClear();
    }

    public void DoReady(GameObject gobjFab,Transform trsfParent = null)
    {
        UnityEngine.GameObject gobj = GameObject.Instantiate(gobjFab, Vector3.zero, Quaternion.identity) as GameObject;
        if(trsfParent != null)
        {
            gobj.transform.parent = trsfParent;
        }
        DoActive(gobj);
    }

    void DoActive(GameObject go)
    {
        DBU3D_Particle db = new DBU3D_Particle(go);
        db.Simulate(0,false,true);
        list.Add(db);
    }

    public void OnUpdate()
    {
        OnUpdate(EDM_Timer.m_instance.DeltaTime);
    }

    public void OnUpdate(float deltatime)
    {
        if (isPause)
            return;

        // Debug.Log("delta = " + deltatime);

        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            tmp = list[i];
            tmp.Simulate(deltatime,false,false);
        }
        OnClearParticle();
    }

    void OnClearParticle(bool isAll = false)
    {
        bool isCanMv = false;
        for (int i = 0; i < list.Count; i++)
        {
            tmp = list[i];
            isCanMv = tmp.isEndLife;

            if (isAll || isCanMv) { 
                list.Remove(tmp);
                tmp.DoDestory();
            }
        }
        tmp = null;
    }

    public void DoClear()
    {
        OnClearParticle(true);

        list.Clear();
        tmp = null;
        isPause = false;
    }

    public void DoPause()
    {
        isPause = true;
    }

    public void DoResume()
    {
        isPause = false;
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
