using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 粒子特效脚本-Inspector
/// 作者 : Canyon
/// 日期 : 2016-12-26 10:35
/// 功能 : 通过Play来驱动的
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Particle01), true)]
public class ED_Particle01_Inspector : Editor
{
    ED_Particle01 m_target;
    DBU3D_Particle m_db_particle = new DBU3D_Particle();

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
        m_target = target as ED_Particle01;
        m_db_particle.DoReInit(m_target.gameObject);
    }

    void DoPause()
    {
        m_db_particle.ChangeState(2);
    }

    void DoResume()
    {
        m_db_particle.ChangeState(1);
    }

    void OnUpdate()
    {
        if (m_target == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }
        m_db_particle.SetScale(cur_scale);

        m_db_particle.SetSpeed(cur_speed);

        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
    }

    void DoPlay(bool isFirst = true)
    {
        OnReady();

        isPlaying = true;

        m_db_particle.ChangeState(1);
    }

    void OnReady()
    {
        isPlaying = false;
        isPauseing = false;
        m_db_particle.ChangeState(0);
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
                }else
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
            // cur_speed = GUILayout.HorizontalSlider(cur_speed,min_speed, max_speed);
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
