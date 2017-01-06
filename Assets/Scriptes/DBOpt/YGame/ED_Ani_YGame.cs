using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : 融合项目-动作Animator
/// 作者 : Canyon
/// 日期 : 2016-12-29 17:10:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class ED_Ani_YGame : ED_Ani {

    public void DoReInit(GameObject gobj)
    {
        DoClear();
        Animator ani = gobj.GetComponentInChildren<Animator>();
        DoInit(ani);
    }

    public override float DoPlayCurr(float m_fPhase)
    {
        float delaytime = base.DoPlayCurr(m_fPhase);
        //EDM_Particle.m_instance.OnUpdate(delaytime,true);
        //Debug.Log(delaytime);
        return delaytime;
    }
}
