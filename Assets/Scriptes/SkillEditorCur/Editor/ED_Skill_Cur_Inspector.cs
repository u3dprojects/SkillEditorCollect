using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 当前技能控制模式
/// 作者 : Canyon
/// 日期 : 2016-12-21 16:30:00
/// 功能 : 
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill_Cur), true)]
public class ED_Skill_Cur_Inspector : Editor
{
    ED_Skill_Cur m_entity;

    Transform trsf_entity;
    CharacterController myCtrl;
    Vector3 def_pos = Vector3.zero;

    ED_Ani db_opt_ani = new ED_Ani();
    EDD_GUI_YGame draw_gui = new EDD_GUI_YGame();

    Animator m_ani;

    // 暂停按钮控制
    bool isPauseing = false;

    // 播放按钮的控制值
    bool isPlaying = false;

    // 位移曲线动画
    AnimationCurve[] movCurve = null;
    Vector3 movPos = Vector3.zero;
    bool isCanSpeed = false;
    float movSpeed = 1f;
    float curSpeed = 0.0f;

    // 只是参考时间是否在跑动
    float Repaint_Inverval = 0.2f;
    float Repaint_Progress = 0.0f;

    // DBOpt_Time db_opt_time = new DBOpt_Time(false);

    void OnEnable()
    {
        EDM_Timer.m_instance.DoInit();
        EDM_Particle.m_instance.DoInit();

        EditorApplication.update += OnUpdate;
        EditorApplication.update += EDM_Timer.m_instance.OnUpdate;
        EditorApplication.update += EDM_Particle.m_instance.OnUpdate;

        DoInit();
    }

    void OnDisable()
    {

        EditorApplication.update -= OnUpdate;
        EditorApplication.update -= EDM_Timer.m_instance.OnUpdate;
        EditorApplication.update -= EDM_Particle.m_instance.OnUpdate;

        DoClear();
    }

    void DoClear()
    {
        EDM_Particle.m_instance.DoClear();

        db_opt_ani.DoClear();

        m_entity = null;
        m_ani = null;
        trsf_entity = null;
        myCtrl = null;

        draw_gui.Reset();

        OnResetMember();

        movCurve = null;
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
        m_entity = target as ED_Skill_Cur;
        trsf_entity = m_entity.transform;

        if (trsf_entity) { 
            m_ani = trsf_entity.GetComponent<Animator>();
            myCtrl = trsf_entity.GetComponent<CharacterController>();
            def_pos = trsf_entity.transform.position;
        }

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
        
        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        // db_opt_time.OnResetMemberReckon();
        EDM_Timer.m_instance.DoReset();
        if (trsf_entity)
            trsf_entity.position = def_pos;
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
        // db_opt_time.DoResume();
    }

    void OnUpdate()
    {
        // 重新绘制
        if (Repaint_Inverval < Repaint_Progress)
        {
            this.Repaint();
            Repaint_Progress = 0.0f;
        }
        else
        {
            Repaint_Progress += EDM_Timer.m_instance.DeltaTime;
        }

        if (m_ani == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        // db_opt_time.DoUpdateTime(false);

        float temp = EDM_Timer.m_instance.DeltaTime;

        // Debug.Log("= mono =" + temp + ", = editor = " + db_opt_time.DeltaTime);

        // db_opt_ani.DoUpdateAnimator(db_opt_time.DeltaTime, cur_speed);

        db_opt_ani.DoUpdateAnimator(temp, draw_gui.CurSpeed,
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

        // 执行位移
        movCurve = draw_gui.curCurve;
        if (movCurve != null)
        {
            movPos = Vector3.zero;
            if (isCanSpeed)
            {
                curSpeed = movSpeed * temp;
            }
            else
            {
                curSpeed = movSpeed;
            }

            float nt01 = db_opt_ani.nt01;
            movPos.x = movCurve[0].Evaluate(nt01) * curSpeed;
            movPos.y = movCurve[1].Evaluate(nt01) * curSpeed;
            movPos.z = movCurve[2].Evaluate(nt01) * curSpeed;


            if (myCtrl  && myCtrl.enabled)
            {
                Debug.Log("= move =" + movSpeed + " , " + temp + " , " + curSpeed + " , " + nt01 + " , " + movCurve[0].Evaluate(nt01) + " , " + movPos);
                myCtrl.Move(movPos);
            }
            else
            {
                trsf_entity.position += movPos;
            }
        }
    }

    void OnReady()
    {
        EDM_Particle.m_instance.DoClear();
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

    void DoStop()
    {
        OnReady();
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

        draw_gui.DrawAniListIndex(null);

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

        draw_gui.DrawTimerInfo();

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
                }
                else
                {
                    DoResume();
                }
            }

            if (GUILayout.Button("Stop"))
            {
                DoStop();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
