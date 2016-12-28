using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 类名 : 简单时间管理(放在Update里面调用)
/// 作者 : Canyon
/// 日期 : 2016-12-21 10:10:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class DBOpt_Time : System.Object {

    double cur_ed_time_startup = 0.0d;
    double last_ed_time_startup = 0.0d;
    double delta_ed_time_startup = 0.0d;
    double progress_ed_time_startup = 0.0d;

    float cur_realtime_since_startup = 0.0f;
    float last_realtime_since_startup = 0.0f;
    float delta_realtime_since_startup = 0.0f;
    float progress_realtime_since_startup = 0.0f;

    // 更新是否是通过EditorApplication的时间
    bool _isUpAniByEdTime = false;
    bool isUpAniByEdTime {
        get
        {
#if !UNITY_EDITOR
            _isUpAniByEdTime = false;
#endif
            return _isUpAniByEdTime;
        }
        set
        {
            _isUpAniByEdTime = value;
        }
    }

    // 是否打印
    bool isDebug = false;

    // 是否暂停了
    bool isPause = false;
    
    public DBOpt_Time() {}

    public DBOpt_Time(bool isEdTime)
    {
        DoInit(isEdTime,false);
    }

    public DBOpt_Time(bool isEdTime,bool isDebug)
    {
        DoInit(isEdTime, isDebug);
    }

    public void DoReInit(bool isEdTime)
    {
        DoReInit(isEdTime, this.isDebug);
    }

    public void DoReInit(bool isEdTime, bool isDebug)
    {

        OnResetMemberReckon();
        DoInit(isEdTime, isDebug);
    }

    void DoInit(bool isEdTime,bool isDebug)
    {
        this.isUpAniByEdTime = isEdTime;
        this.isDebug = isDebug;
    }

    public void SetIsEdTime(bool isEdTime)
    {
        this.isUpAniByEdTime = isEdTime;
    }

    public void SetIsDeBug(bool isDebug)
    {
        this.isDebug = isDebug;
    }

    public void SetIsPause(bool isPause)
    {
        this.isPause = isPause;
    }

    public void OnResetMemberReckon()
    {
        cur_ed_time_startup = 0.0d;
        last_ed_time_startup = 0.0d;
        delta_ed_time_startup = 0.0d;
        progress_ed_time_startup = 0.0d;

        cur_realtime_since_startup = 0.0f;
        last_realtime_since_startup = 0.0f;
        delta_realtime_since_startup = 0.0f;
        progress_realtime_since_startup = 0.0f;

        isPause = false;
    }

    void OnUpEDTime()
    {
#if UNITY_EDITOR
        cur_ed_time_startup = EditorApplication.timeSinceStartup;
        if (last_ed_time_startup < 0.001d)
            delta_ed_time_startup = 0.0d;
        else
            delta_ed_time_startup = cur_ed_time_startup - last_ed_time_startup;

        last_ed_time_startup = cur_ed_time_startup;

        progress_ed_time_startup += delta_ed_time_startup;
#endif
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

    void OnDebugLogTime()
    {
        if (!this.isDebug)
            return;

        Debug.Log(
            "ed_cur = " + cur_ed_time_startup + ",ed_pre = " + (cur_ed_time_startup - delta_ed_time_startup) + ",ed_delta = " + delta_ed_time_startup + ",ed_pro = " + progress_ed_time_startup +
            ",@ r_cur = " + cur_realtime_since_startup + ",r_pre = " + (cur_realtime_since_startup - delta_realtime_since_startup) + ",r_delta = " + delta_realtime_since_startup + ",r_pro = " + progress_realtime_since_startup
            + ",@ real_pro = " + ProgressTime + ",@ real_delta = " + DeltaTime);
    }

    public void DoUpdateTime(bool isDebug)
    {
        bool isPreDeBug = this.isDebug;
        this.isDebug = isDebug;
        DoUpdateTime();
        this.isDebug = isPreDeBug;
    }

    public void DoUpdateTime()
    {
        if (this.isPause)
            return;

        OnUpEDTime();
        OnUpRealTime();

        OnDebugLogTime();
    }

    // 暂停
    public void DoPause()
    {
        this.isPause = true;
    }

    // 恢复
    public void DoResume()
    {
        this.isPause = false;
#if UNITY_EDITOR
        last_ed_time_startup = EditorApplication.timeSinceStartup;
#endif
        last_realtime_since_startup = Time.realtimeSinceStartup;
    }

    public float DeltaTime
    {
        get
        {
            if (this.isUpAniByEdTime)
            {
                return (float)delta_ed_time_startup;
            }
            return delta_realtime_since_startup;
        }
    }

    public float ProgressTime
    {
        get
        {
            if (this.isUpAniByEdTime)
            {
                return (float)progress_ed_time_startup;
            }
            return progress_realtime_since_startup;
        }
    }
}
