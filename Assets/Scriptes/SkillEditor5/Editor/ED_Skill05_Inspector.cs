using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : Demo04-进度
/// 作者 : Canyon
/// 日期 : 2016-12-22 17:20:00
/// 功能 : 抽出绘制区域
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill05), true)]
public class ED_Skill05_Inspector : Editor
{
    DBOpt_Ani db_opt_ani = new DBOpt_Ani();
    DBOpt_Time db_opt_time = new DBOpt_Time();
    DBOpt_GUI draw_gui = new DBOpt_GUI();
    
    ED_Skill05 m_entity;
    Animator m_ani;

    // 更新是否是通过EditorApplication的时间
    bool isUpAniByEdTime = false;
    
    // 暂停按钮控制
    bool isPauseing = false;

    // 播放按钮的控制值
    bool isPlaying = false;
    

    void OnEnable()
    {
        DoInit();
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        
        DoClear();

        draw_gui.DoClear();
    }

    void DoClear()
    {
        db_opt_ani.DoClear();
        m_entity = null;
        m_ani = null;

        draw_gui.Reset();

        OnResetMember();
    }

    void DoReInit()
    {
        DoClear();

        DoInit();
    }

    void DoInit()
    {
        db_opt_time.DoReInit(isUpAniByEdTime);
        OnInitAni();
        OnResetMember();
    }

    void OnInitAni()
    {
        m_entity = target as ED_Skill05;
        if (m_entity)
            m_ani = m_entity.GetComponent<Animator>();

        if (m_ani) { 
            db_opt_ani.DoReInit(m_ani);
            draw_gui.DoInit(db_opt_ani);
        }
    }

    void OnResetMember()
    {
        isPauseing = false;
        isPlaying = false;
        isUpAniByEdTime = false;

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        db_opt_time.OnResetMemberReckon();
    }

    void DoPause()
    {

    }

    void DoResume()
    {
        db_opt_time.DoResume();
    }

    void OnUpdate()
    {
        if (m_ani == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        db_opt_time.DoUpdateTime();

        // db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, cur_speed);

        db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, draw_gui.CurSpeed,
            delegate () { OnResetMemberReckon(); },
            delegate (bool isloop)
            {
                OnResetMemberReckon();
                if (!isloop)
                {
                    isPlaying = false;
                }
            }
        );

        this.Repaint();
    }

    void OnReady()
    {
        OnResetMember();
        OnInitM_Ani();
    }

    void OnInitM_Ani()
    {
        db_opt_ani.DoResetAniCtrl();
        db_opt_ani.OnResetMemberReckon();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        draw_gui.DrawRowLine();

        if (!draw_gui.DrawJudged())
        {
            return;
        }

        draw_gui.DrawRefreshAnimator(DoReInit);

        draw_gui.DrawAniListIndex();

        draw_gui.DrawAniInfo();

        draw_gui.DrawSpeed();

        draw_gui.DrawCtrlAniProgress(isPauseing,delegate(bool bl)
        {
           isPauseing = bl;
            if (!bl)
            {
                DoResume();
            }
        });

        draw_gui.DrawAniProgress(isPauseing);


        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Play"))
            {
                OnReady();

                isPlaying = true;

                db_opt_ani.SetCurCondition();
            }

            if (GUILayout.Button(isPauseing ? "ReGo" : "Pause"))
            {
                isPauseing = !isPauseing;
                if (!isPauseing)
                {
                    DoResume();
                }
            }

            if (GUILayout.Button("Stop"))
            {
                OnReady();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    void OnInspectorUpdate()
    {
        Debug.Log("面板的更新");
    }

    void OnGUI()
    {
        Debug.Log("面板的更新 OnGUI");
    }
}
