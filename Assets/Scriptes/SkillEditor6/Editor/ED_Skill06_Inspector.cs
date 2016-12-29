using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : Demo_06
/// 作者 : Canyon
/// 日期 : 2016-12-28 09:40
/// 功能 : 抽出时间管理,添加了粒子系统
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill06), true)]
public class ED_Skill06_Inspector : Editor
{
    ED_Skill06 m_entity;

    DBU3D_Ani db_opt_ani = new DBU3D_Ani();
    DBU3D_GUI draw_gui = new DBU3D_GUI();

    Animator m_ani;

    // 暂停按钮控制
    bool isPauseing = false;

    // 播放按钮的控制值
    bool isPlaying = false;

    void OnEnable()
    {
        EditorApplication.update += OnUpdate;

        EDM_Timer.m_instance.DoInit();

        DoInit();
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
        OnInitAni();
        OnResetMember();
    }

    void OnInitAni()
    {
        m_entity = target as ED_Skill06;
        if (m_entity)
            m_ani = m_entity.GetComponent<Animator>();

        if (m_ani)
        {
            db_opt_ani.DoReInit(m_ani);
            draw_gui.DoInit(db_opt_ani);
        }
    }

    void OnResetMember()
    {
        isPauseing = false;
        isPlaying = false;

        EDM_Particle.m_instance.DoInit();

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        EDM_Timer.m_instance.DoReset();
    }

    void DoPause()
    {
        EDM_Timer.m_instance.DoPause();
        EDM_Particle.m_instance.DoPause();
    }

    void DoResume()
    {
        EDM_Timer.m_instance.DoResume();
        EDM_Particle.m_instance.DoResume();
    }

    void OnUpdate()
    {
        if (m_ani == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        float delta_time = EDM_Timer.m_instance.DeltaTime;
        // db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, cur_speed);

        db_opt_ani.DoUpdateAnimator(delta_time, draw_gui.CurSpeed,
            delegate () { OnResetMemberReckon(); },
            delegate (bool isloop)
            {
                draw_gui.cur_round_times++;
                if (draw_gui.isRound)
                {
                    if (draw_gui.isCompleteRound)
                    {
                        isPlaying = false;
                    }
                }
                else
                {
                    if (isloop)
                    {
                        db_opt_ani.ResetCurEvents();
                    }
                    else
                    {
                        isPlaying = false;
                    }
                }

                OnResetMemberReckon();

                if (isPlaying && !isloop)
                {
                    DoPlay(false);
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
        db_opt_ani.ResetCurEvents();
    }

    void DoPlay(bool isFirst = true)
    {
        if (isFirst)
            draw_gui.cur_round_times = 0;

        OnReady();

        isPlaying = true;

        db_opt_ani.SetCurCondition();
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

        draw_gui.DrawCtrlAniProgress(isPauseing, delegate (bool bl)
        {
            isPauseing = bl;
            if (isPauseing)
            {
                DoPause();
            }
            else
            {
                DoResume();
            }
        });

        draw_gui.DrawAniProgress(isPauseing);

        draw_gui.DrawRoundTimes();

        draw_gui.DrawMovePos();

        draw_gui.DrawEffect();

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Play"))
            {
                DoPlay();
            }

            if (GUILayout.Button(isPauseing ? "ReGo" : "Pause"))
            {
                isPauseing = !isPauseing;
                if (isPauseing)
                {
                    DoPause();
                }else{
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
}