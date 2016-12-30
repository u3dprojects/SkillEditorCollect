using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 粒子特效脚本-Inspector
/// 作者 : Canyon
/// 日期 : 2016-12-26 14:35
/// 功能 : 通过 Simulate 来驱动的
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Particle02), true)]
public class ED_Particle02_Inspector : Editor
{
    ED_Particle02 m_target;
    DBU3D_Particle m_db_particle = new DBU3D_Particle();
    DBOpt_Time m_db_time = new DBOpt_Time();
    
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
    }

    void DoClear()
    {
        m_db_particle.DoClear();
        m_target = null;
    }

    void DoInit()
    {
        m_target = target as ED_Particle02;
        m_db_particle.DoReInit(m_target.gameObject);

        m_db_time.DoReInit(false);
    }

    void DoPause()
    {
        
    }

    void DoResume()
    {
        m_db_time.DoResume();
    }

    void OnUpdate()
    {
        if (m_target == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        if(m_db_time.ProgressTime > m_db_particle.maxTime)
        {
            isPlaying = false;
            return;
        }

        m_db_time.DoUpdateTime(true);

        m_db_particle.SetScale(cur_scale);

        m_db_particle.SetSpeedRate(cur_speed);

        // 用delta time 播放 无效??
        // m_db_particle.Simulate(m_db_time.ProgressTime,false,true);
        // m_db_particle.Simulate(m_db_time.DeltaTime, false, false);

    }

    void DoPlay(bool isFirst = true)
    {
        OnReady();

        m_db_particle.DoStart();

        isPlaying = true;
    }

    void OnReady()
    {
        isPlaying = false;
        isPauseing = false;
        m_db_particle.Stop();

        m_db_time.DoReInit(false);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawScale();

        DrawSpeed();

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
                OnReady();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    float space_row_interval = 10.0f;

    float min_scale = 0.0f;
    float max_scale = 3.0f;
    float cur_scale = 1.0f;

    public void DrawScale()
    {
        EditorGUILayout.BeginHorizontal();
        {
            cur_scale = EditorGUILayout.Slider("当前缩放:", cur_scale, min_scale, max_scale);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }


    float min_speed = 0.0f;
    float max_speed = 3.0f;
    float cur_speed = 1.0f;
    public void DrawSpeed()
    {
        EditorGUILayout.BeginHorizontal();
        {
            cur_speed = EditorGUILayout.Slider("当前速度:", cur_speed, min_speed, max_speed);
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(space_row_interval);
    }
}

