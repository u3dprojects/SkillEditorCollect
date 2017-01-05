using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// 类名 : 技能测试第一种模式Update(非录制模式)
/// 作者 : Canyon
/// 日期 : 2016-12-21 09:20:00
/// 功能 : 通过设置Animator的Parmeters的某个值来调用动画更新
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ED_Skill01),true)]
public class ED_Skill01_Inspector : Editor {

    ED_Skill01 m_entity;
    Animator m_ani;

    bool isPauseing = false;
    bool isPlaying = false;

    double cur_ed_time_startup = 0.0d;
    double last_ed_time_startup = 0.0d;
    double delta_ed_time_startup = 0.0d;
    double progress_ed_time_startup = 0.0d;

    float cur_realtime_since_startup = 0.0f;
    float last_realtime_since_startup = 0.0f;
    float delta_realtime_since_startup = 0.0f;
    float progress_realtime_since_startup = 0.0f;

    // 更新是否是通过EditorApplication的时间
    bool isUpAniByEdTime = false;
    float progress_time = 0.0f;
    float delta_time = 0.0f;

    AnimatorStateInfo cur_ani_stateInfo;
    int cur_ani_shortNameHash = 0;
    int cur_ani_frame_count = 0;
    float cur_ani_length = 0f;

    int cur_ani_frame_count_reckon = 0;

    // 帧率(每秒显示帧数 30fps/s)
    const float ani_frameRate = 30f;
    
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

    // Use this for initialization
    void Start () {
        Debug.Log("== ED_Skill01_Inspector Start ==");
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log("== ED_Skill01_Inspector Update ==");
    }

    void DoClear()
    {
        m_entity = null;
        m_ani = null;
        OnResetMember();
    }

    void DoInit()
    {
        m_entity = target as ED_Skill01;
        if (m_entity)
            m_ani = m_entity.GetComponent<Animator>();

        OnResetMember();
    }

    void OnResetMember()
    {
        isPauseing = false;
        isPlaying = false;
        isUpAniByEdTime = false;

        cur_ani_shortNameHash = 0;
        cur_ani_frame_count = 0;
        cur_ani_length = 0.0f;

        OnResetMemberReckon();
    }

    void OnResetMemberReckon()
    {
        cur_ed_time_startup = 0.0d;
        last_ed_time_startup = 0.0d;
        delta_ed_time_startup = 0.0d;
        progress_ed_time_startup = 0.0d;

        cur_realtime_since_startup = 0.0f;
        last_realtime_since_startup = 0.0f;
        delta_realtime_since_startup = 0.0f;
        progress_realtime_since_startup = 0.0f;

        progress_time = 0.0f;
        delta_time = 0.0f;

        cur_ani_frame_count_reckon = 0;
    }

    void DoUpdateTime()
    {
        OnUpEDTime();
        OnUpRealTime();

        // OnDebugLogTime();
    }

    void DoPause()
    {

    }

    void DoResume()
    {
        last_ed_time_startup = EditorApplication.timeSinceStartup;
        last_realtime_since_startup = Time.realtimeSinceStartup;
    }

    void OnDebugLogTime()
    {
        Debug.Log(
            "ed_cur = " + cur_ed_time_startup + ",ed_pre = " + (cur_ed_time_startup - delta_ed_time_startup) + ",ed_delta = " + delta_ed_time_startup + ",ed_pro = " + progress_ed_time_startup +
            ",@ r_cur = " + cur_realtime_since_startup + ",r_pre = " + (cur_realtime_since_startup - delta_realtime_since_startup) + ",r_delta = " + delta_realtime_since_startup + ",r_pro = " + progress_realtime_since_startup
            + ",@ real_pro = " + progress_time + ",@ real_delta = " + delta_time);
    }

    void OnUpEDTime()
    {
        cur_ed_time_startup = EditorApplication.timeSinceStartup;
        if (last_ed_time_startup < 0.001d)
            delta_ed_time_startup = 0.0d;
        else
            delta_ed_time_startup = cur_ed_time_startup - last_ed_time_startup;

        last_ed_time_startup = cur_ed_time_startup;

        progress_ed_time_startup += delta_ed_time_startup;
    }

    void OnUpRealTime()
    {
        cur_realtime_since_startup = Time.realtimeSinceStartup;
        if (last_realtime_since_startup < 0.001f)
            delta_realtime_since_startup = 0.0f;
        else
            delta_realtime_since_startup = cur_realtime_since_startup - last_realtime_since_startup;

        last_realtime_since_startup = cur_realtime_since_startup;

        progress_realtime_since_startup += delta_realtime_since_startup;
    }

    void DoUpdateAnimator()
    {
        if (isUpAniByEdTime)
        {
            delta_time = (float)(delta_ed_time_startup);
            progress_time = (float)progress_ed_time_startup;
        }
        else
        {
            delta_time = delta_realtime_since_startup;
            progress_time = progress_realtime_since_startup;
        }

        m_ani.Update(delta_time);

        cur_ani_frame_count_reckon++;

        // new GameObject("frame_" + cur_ani_frame_count_reckon);
    }

    void OnUpdate()
    {
        if(m_ani == null || isPauseing || !isPlaying || Application.isPlaying || EditorApplication.isPlaying)
        {
            return;
        }

        DoUpdateTime();

        DoUpdateAnimator();

        cur_ani_stateInfo = m_ani.GetCurrentAnimatorStateInfo(0);
        if(cur_ani_shortNameHash != cur_ani_stateInfo.shortNameHash)
        {
            OnResetMemberReckon();

            cur_ani_shortNameHash = cur_ani_stateInfo.shortNameHash;
            cur_ani_length = cur_ani_stateInfo.length;
            cur_ani_frame_count = Mathf.CeilToInt(cur_ani_length * ani_frameRate);
        }

        // Debug.Log("fph = "+cur_ani_stateInfo.fullPathHash + ",hc = " + cur_ani_stateInfo.GetHashCode() + ",nh = " + cur_ani_stateInfo.nameHash + ",snh = " + cur_ani_stateInfo.shortNameHash + ",tag = " + cur_ani_stateInfo.tagHash);

        if (m_ani.IsInTransition(0))
        {
            return;
        }

        if(cur_ani_stateInfo.normalizedTime >= 1f)
        {
            if (cur_ani_stateInfo.loop)
            {
                OnResetMemberReckon();
            }
            else
            {
                isPlaying = false;
            }
        }

        Debug.Log("snh = " + cur_ani_shortNameHash + ",lens = " + cur_ani_length + ",frameCount = " + cur_ani_frame_count + ",curCount = " + cur_ani_frame_count_reckon);
    }

    void OnInitM_Ani()
    {
        if (m_ani)
        {
            m_ani.SetBool("Skill1", false);
            m_ani.SetBool("Skill2", false);
            m_ani.SetBool("Skill3", false);
            m_ani.SetBool("Skill4", false);
            m_ani.SetBool("Run", false);
            m_ani.speed = 1;
            m_ani.Update(0);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Play"))
            {
                //isEditorUp = true;
                OnResetMember();
                OnInitM_Ani();
                isPlaying = true;

                m_ani.SetBool("Skill2", true);
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
                OnInitM_Ani();
                OnResetMember();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}
