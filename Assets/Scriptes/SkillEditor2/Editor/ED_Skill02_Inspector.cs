using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;

/// <summary>
/// 类名 : 第二种模式Record(录制模式)
/// 作者 : Canyon
/// 日期 : 2016-12-21 10:30:00
/// 功能 : 先录制,后回播的模式,(猜测得到达那个状态需要条件才行，测试失败),没有任何条件线是可以的;
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill02),true)]
public class ED_Skill02_Inspector : Editor {
    DBU3D_Ani db_opt_ani = new DBU3D_Ani();
    DBOpt_Time db_opt_time = new DBOpt_Time();

    ED_Skill02 m_entity;
    Animator m_ani;

    // 更新是否是通过EditorApplication的时间
    bool isUpAniByEdTime = false;

    // 暂停按钮控制
    bool isPauseing = false;

    // 播放按钮的控制值
    bool isPlaying = false;
    
    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 进度条slider
    float sld_progress = 0.0f;
    float m_RecorderStopTime = 1.0f;

    void OnEnable()
    {
        DoInit();
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
        DoClear();
    }

    void DoReInit()
    {
        DoClear();
        
        DoInit();
    }

    void DoClear()
    {
        db_opt_ani.DoClear();

        m_entity = null;
        m_ani = null;

        ind_popup = 0;
        pre_ind_popup = -1;

        OnResetMember();
    }

    void DoInit()
    {
        db_opt_time.DoReInit(isUpAniByEdTime);
        OnInitAni();
        OnResetMember();
    }

    void OnInitAni()
    {
        m_entity = target as ED_Skill02;
        if (m_entity)
            m_ani = m_entity.GetComponent<Animator>();
        
        if (m_ani)
            db_opt_ani.DoReInit(m_ani);
    }

    void OnResetMember()
    {
        isPauseing = false;
        isPlaying = false;

        sld_progress = 0.0f;
        m_RecorderStopTime = 1.0f;

        if (m_ani)
        {
            m_ani.Rebind();
            m_ani.StopPlayback();
        }

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        db_opt_time.OnResetMemberReckon();
    }

    void DoBake()
    {
        OnResetMember();
        // m_ani.Rebind();
        db_opt_ani.PlayCurr(0);

        int frameCount = db_opt_ani.CurFrameCount + 2;
        float frameRate = db_opt_ani.CurFrameRate;

        m_ani.StopPlayback();
        m_ani.recorderStartTime = 0.0f;
        m_ani.StartRecording(frameCount);
        for (var i = 0; i < frameCount - 1; i++)
        {
            // 这里可以在指定的时间触发新的动画状态
            // 记录每一帧
            m_ani.Update(1.0f / frameRate);
        }

        m_ani.StopRecording();

        m_RecorderStopTime = m_ani.recorderStopTime;

        m_ani.StartPlayback();
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

        sld_progress = db_opt_time.ProgressTime;
        sld_progress = Mathf.Min(sld_progress, m_RecorderStopTime);
        m_ani.playbackTime = sld_progress;
        m_ani.Update(0);
        
        if (db_opt_time.ProgressTime >= m_RecorderStopTime)
        {
            OnResetMemberReckon();
            // isPlaying = false;
        }
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(m_ani == null)
        {
            EditorGUILayout.HelpBox("请将脚本绑定到一个带Animator的物体！", MessageType.Error);
            return;
        }

        if (GUILayout.Button("刷新Animator动画列表"))
        {
            DoReInit();
        }

        if (db_opt_ani.Keys.Count <= 0)
        {
            EditorGUILayout.HelpBox("该AnimatorController里面没有任何动画，请添加动画！", MessageType.Error);
            return;
        }
        
        if (!isPlaying) {
            ind_popup = EditorGUILayout.Popup("动画列表", ind_popup, db_opt_ani.Keys.ToArray());
            if (pre_ind_popup != ind_popup)
            {
                pre_ind_popup = ind_popup;
                db_opt_ani.ResetAniState(ind_popup);
            }
        }

        EditorGUILayout.BeginHorizontal();
        {
            // EditorGUILayout.LabelField("总帧数: " + db_opt_ani.AniFrameCount);
            GUILayout.Label("总帧数: " + db_opt_ani.CurFrameCount);

            // EditorGUILayout.Space();
            GUILayout.Space(10);
            GUILayout.Label("进度: ");

            GUILayout.Label("cur : " + sld_progress);

            sld_progress = EditorGUILayout.Slider(sld_progress, 0, m_RecorderStopTime);

            GUILayout.Label("all : " + m_RecorderStopTime);
        }
        EditorGUILayout.EndHorizontal();

        
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Play"))
            {
                OnResetMember();
                DoBake();
                isPlaying = true;
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
                OnResetMember();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
