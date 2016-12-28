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
[ExecuteInEditMode]
public class ED_Particle_Manager : MonoBehaviour {

    private static ED_Particle_Manager _m_instance;

    static public ED_Particle_Manager m_instance
    {
        get
        {
            if (_m_instance == null)
            {
                GameObject gobj = GameObject.Find("EDParticleManager");
                if (gobj == null)
                {
                    gobj = new GameObject("EDParticleManager");
                }

                _m_instance = gobj.GetComponent<ED_Particle_Manager>();
                if (_m_instance == null)
                {
                    _m_instance = gobj.AddComponent<ED_Particle_Manager>();
                }
            }
            return _m_instance;
        }
    }

    List<DBU3D_Particle> list = new List<DBU3D_Particle>();
    List<DBU3D_Particle> rmList = new List<DBU3D_Particle>();
    DBU3D_Particle tmp;
    int lens = 0;
    bool isPause = false;

    public void DoInit()
    {
        DoClear();
    }

    public void DoActive(GameObject go)
    {
        DBU3D_Particle db = new DBU3D_Particle(go);
        list.Add(db);
        db.Simulate(0);
    }

    public void OnUpdate()
    {
        OnUpdate(ED_Time_Manager.m_instance.DeltaTime);
    }

    public void OnUpdate(float deltatime)
    {
        rmList.Clear();
        if (isPause)
            return;

        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            tmp = list[i];
            tmp.Simulate(deltatime);
        }
        OnClearParticle();
    }

    void OnClearParticle(bool isAll = false)
    {
        lens = list.Count;

        for (int i = 0; i < lens; i++)
        {
            tmp = list[i];
            if (isAll)
            {
                rmList.Add(tmp);
            }else
            {
                if (tmp.isEndLife)
                {
                    rmList.Add(tmp);
                }
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

    void DoClear()
    {
        OnClearParticle(true);

        list.Clear();
        rmList.Clear();
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
        EditorApplication.update += Update;
    }

    void OnDisable()
    {
        DoClear();
        EditorApplication.update -= Update;
    }
#endif

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        OnUpdate();
    }
}
