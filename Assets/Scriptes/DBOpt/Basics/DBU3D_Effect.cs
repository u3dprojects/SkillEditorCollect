using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 类名 : U3D - 特效管理
/// 作者 : Canyon
/// 日期 : 2017-01-04 11:10
/// 功能 :  特效包含粒子和Animator
/// </summary>
[System.Serializable]
public class DBU3D_Effect : System.Object
{
    // 当前操作对象
    GameObject gobjEdtity = null;

    // 多个粒子特效
    DBU3D_Particle m_particle = new DBU3D_Particle();

    // 多个Animator
    List<ED_Ani> list = new List<ED_Ani>();
    int lens = 0;

    public float curSpeed { get; set; }

    float up_deltatime = 0.0f;

    public bool isEnd
    {
        get
        {
            return m_particle.isEnd && IsEndAnimator();
        }
    }

    public DBU3D_Effect() { }

    public DBU3D_Effect(GameObject gobj)
    {
        DoReInit(gobj);
    }

    public void DoReInit(GameObject gobj)
    {
        DoClear();
        DoInit(gobj);
    }

    void DoInit(GameObject gobj)
    {
        m_particle.DoReInit(gobj);

        Animator[] anis = gobj.GetComponentsInChildren<Animator>();
        lens = anis.Length;
        ED_Ani tmp = null;
        for (int i = 0; i < lens; i++)
        {
            tmp = new ED_Ani(anis[i]);
            tmp.ResetAniState(0);
            if(tmp.CurState == null)
            {
                continue;
            }

            list.Add(tmp);
        }
    }

    public void DoClear()
    {
        this.gobjEdtity = null;
        curSpeed = 1;
        m_particle.DoClear();
        OnClearAnimator();
    }

    void OnClearAnimator()
    {
        lens = list.Count;
        for(int i = 0;i < lens; i++)
        {
            (list[i]).DoClear();
        }
        list.Clear();
        lens = 0;
    }

    public void DoStart()
    {
        m_particle.DoStart();
        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            (list[i]).DoStart();
        }
    }

    public void DoUpdate(float deltatime)
    {
        up_deltatime = deltatime * curSpeed;
        lens = list.Count;
        Debug.Log("== DBU3D_Effect delta = " + up_deltatime);
        for (int i = 0; i < lens; i++)
        {
            (list[i]).DoUpdateAnimator(up_deltatime,1);
        }
        m_particle.DoUpdate(up_deltatime);
    }

    public void SetScale(float _scale)
    {
        m_particle.SetScale(_scale);
    }

    public bool IsEndAnimator()
    {
        lens = list.Count;
        for (int i = 0; i < lens; i++)
        {
            if(!(list[i]).isEndFirst)
            {
                return false;
            }
        }
        return true;
    }

    public void DoDestory()
    {
        m_particle.DoDestory();
        DoClear();
    }
}
