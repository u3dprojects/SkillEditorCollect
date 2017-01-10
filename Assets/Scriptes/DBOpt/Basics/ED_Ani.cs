using UnityEngine;
using System.Collections;
using UnityEditor.Animations;
using System.Collections.Generic;

[System.Serializable]
public enum AniRunStatus
{
    Fail = 0, Sucess = 1 , Running = 2,
}

/// <summary>
/// 类名 : 当前State的状态数据
/// 作者 : Canyon
/// 日期 : 2016-12-21 10:10:00
/// 功能 :  处理当前的State的数据
/// </summary>
[System.Serializable]
public class ED_Ani : ED_AniBase {
    string cur_state_key = "";
    int cur_layer_index = 0;

    AnimatorState cur_state;

    // 默认速度
    float defSpeed = 1.0f;

    // 动作速度
    float curSpeed = 1.0f;
    
    // 当前帧数
    int cur_FrameCount = 0;

    // 当前帧率
    float cur_FrameRate = 0.0f;

    // 当前时长
    float cur_Length = 0f;

    // 当前动作是否循环
    bool cur_IsLoop = false;

    // 循环/时长 的 关系
    float m_InvLifeTime = 0.0f;

    // 更新true:用条件，false:用Play进度
    bool isUpByCondition = false;

    // 当前state是否需要条件才能播放
    bool cur_isHasCondition = false;

    // 当前状态机
    AnimatorStateInfo cur_stateInfo;

    // 当前状态机的short name
    int cur_shortNameHash = 0;

    // 计算
    
    // 当前阶段(在一个周期中的阶段)[0-1]
    float cur_Phase = 0.0f;

    // 当前状态进度时间 = normalizedTime
    float cur_progressTime = 0.0f;

    // 当前循环次数
    int cur_loop_times = 0;

    // 是否完成了一个周期
    bool isFinishedOneWheel = false;

    // 完成了播放(包括循环播放次数)
    bool isCompletedRound = false;

    // 可循环次数
    public int m_LoopTimes { get; set; }
    
    // 当前StateMatch
    public StateMachineBehaviour cur_state_mache { get; set; }

    // 改变时候调用的函数
    public System.Action callChanged { get; set; }
    
    // 完成一次循环(一个周期)
    public System.Action<bool> callCompleted { get; set; }

    // 动作时间轴时间
    ED_AniTimeEvent stateEvent = new ED_AniTimeEvent();

    // 更新动作回调 - 进度时间 
    System.Action<float> callOnUpdateProgress;

    // 更新动作回调 - 当前阶段(0-1)
    System.Action<float> callOnUpdatePhase;

    public ED_Ani() { }

    public ED_Ani(Animator ani):base(ani)
    {
    }

    public float CurLens
    {
        get
        {
            return cur_Length;
        }
    }

    public int CurFrameCount
    {
        get
        {
            return cur_FrameCount;
        }
    }

    public float CurFrameRate
    {
        get
        {
            return cur_FrameRate;
        }
    }

    public AnimatorState CurState
    {
        get {
            return cur_state;
        }
    }

    public AnimationClip CurClip
    {
        get
        {
            if (cur_state && cur_state.motion)
            {
                return cur_state.motion as AnimationClip;
            }
            return null;
        }
    }

    public bool CurIsHasCondition
    {
        get { return cur_isHasCondition; }
    }

    public bool CurIsInTransition
    {
        get
        {
            return m_ani.IsInTransition(cur_layer_index);
        }
    }

    public float normalizedTime
    {
        get
        {
            return cur_stateInfo.normalizedTime;
        }
    }

    public float nt01
    {
        get
        {
            return Mathf.Repeat(cur_progressTime, 1);
        }
    }

    public bool isLoop
    {
        get { return cur_stateInfo.loop; }
    }

    public bool isEndFirst
    {
        get { return cur_progressTime >=  cur_Length;}
    }

    public int CurLoopCount
    {
        get { return cur_loop_times; }
    }

    public void ResetAniState(int index_key)
    {
        if(index_key < 0)
        {
            Debug.LogError("Key的Index下标从0开始,index = " + index_key);
            return;
        }
        if(index_key >= Keys.Count)
        {
            Debug.LogError("Key的Index下标小其长度,index = " + index_key);
            return;
        }

        string key = Keys[index_key];
        ResetAniState(key);
    }

    public void ResetAniState(string key)
    {
        if (KState.ContainsKey(key))
        {
            cur_state_key = key;
            cur_state = KState[key];
            cur_layer_index = KLayer[key];
            ResetAniState(cur_state);
        }
        else
        {
            Debug.LogError("not has state key = " + key);
        }
    }

    void ResetAniState(AnimatorState state)
    {
        OnResetMember();

        cur_state = state;
        
        if (state.motion)
        {
            AnimationClip clip = state.motion as AnimationClip;
            cur_Length = clip.length;
            cur_FrameRate = clip.frameRate;
            cur_IsLoop = clip.isLooping;

            defSpeed = state.speed;
            curSpeed = defSpeed;

            m_InvLifeTime = 1.0f / cur_Length;
            float count = cur_FrameRate * cur_Length;
            cur_FrameCount = Mathf.FloorToInt(count);

            cur_isHasCondition = ReCurIsHasCondition();

            Debug.Log("key = " + cur_state_key + ",lens = " + cur_Length + ",frameRate = " + cur_FrameRate + ",count = " + count + ",countint = " + cur_FrameCount + ",isLoop = " + cur_IsLoop);
        }
    }

    bool ReCurIsHasCondition()
    {
        if (cur_state)
        {
            foreach (AnimatorStateTransition stateTran in cur_state.transitions)
            {
                if(stateTran.conditions.Length > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public override void OnClear()
    {
        base.OnClear();

        cur_state_key = "";
        cur_layer_index = 0;
        
        OnResetMember();
    }

    void OnResetMember()
    {
        defSpeed = 1.0f;
        curSpeed = 1.0f;

        cur_FrameCount = 0;
        cur_Length = 0.0f;
        cur_FrameRate = 0.0f;
        m_InvLifeTime = 0.0f;
        cur_isHasCondition = false;
        cur_IsLoop = false;

        cur_state = null;
        cur_state_mache = null;
        cur_shortNameHash = 0;

        stateEvent.DoClear();

        callChanged = null;
        callCompleted = null;

        callOnUpdatePhase = null;
        callOnUpdateProgress = null;

        OnResetMemberReckon();
    }

    public void OnResetMemberReckon()
    {
        cur_progressTime = 0.0f;
        cur_Phase = 0.0f;
        cur_loop_times = 0;
        m_LoopTimes = 0;
        isFinishedOneWheel = false;
        isCompletedRound = false;
    }

    // 根据进度来取得上次和此次的间隔时间
    public float GetDelayTime(float m_fPhase)
    {
        m_fPhase = Mathf.Repeat(m_fPhase, 1);
        float curtime = cur_loop_times + m_fPhase;
        return (curtime - cur_progressTime) / curSpeed / m_InvLifeTime;
    }

    // 专门用于拖动进度的时候调用的
    public virtual float DoPlayCurr(float m_fPhase)
    {
        float deltatime = GetDelayTime(m_fPhase);
        OnUpdateTime(deltatime);
        PlayCurr(cur_Phase);
        return deltatime;
    }
    
    public void PlayCurr(float begNormallizedTime, float delta_time = 0)
    {
        if (cur_state)
        {
            Play(cur_state.name, cur_layer_index, begNormallizedTime,delta_time);
        }
    }

    public bool IsChanged4CheckCurStateInfo()
    {
        if (cur_state)
        {
            cur_stateInfo = m_ani.GetCurrentAnimatorStateInfo(cur_layer_index);
            if (cur_shortNameHash != cur_stateInfo.shortNameHash)
            {
                cur_shortNameHash = cur_stateInfo.shortNameHash;
                return true;
            }
        }
        return false;
    }

    
    public void SetCurCondition()
    {
        if (cur_state && isUpByCondition)
        {
            AnimatorControllerParameterType pars_type = AnimatorControllerParameterType.Bool;

            foreach (AnimatorStateTransition stateTran in cur_state.transitions)
            {
                foreach(AnimatorCondition con in stateTran.conditions)
                {
                    pars_type = GetParsType(con.parameter);
                    switch (pars_type)
                    {
                        case AnimatorControllerParameterType.Bool:
                            if(con.mode == AnimatorConditionMode.Equals)
                            {
                                m_ani.SetBool(con.parameter, (con.threshold == 0 ? false : true));
                            } else {
                                m_ani.SetBool(con.parameter, (con.threshold == 0 ? true : false));
                            }
                            break;
                        case AnimatorControllerParameterType.Float:
                            if (con.mode == AnimatorConditionMode.Equals)
                            {
                                m_ani.SetFloat(con.parameter, con.threshold);
                            }
                            else
                            {
                                if(con.mode == AnimatorConditionMode.Greater)
                                {
                                    m_ani.SetFloat(con.parameter, con.threshold + 1f);
                                }
                                else
                                {
                                    m_ani.SetFloat(con.parameter, con.threshold - 1f);
                                }
                            }
                            break;
                        case AnimatorControllerParameterType.Int:
                            if (con.mode == AnimatorConditionMode.Equals)
                            {
                                m_ani.SetInteger(con.parameter, (int)con.threshold);
                            }
                            else
                            {
                                if (con.mode == AnimatorConditionMode.Greater)
                                {
                                    m_ani.SetInteger(con.parameter, (int)(con.threshold + 1f));
                                }
                                else
                                {
                                    m_ani.SetInteger(con.parameter, (int)(con.threshold - 1f));
                                }
                            }
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            m_ani.SetTrigger(con.parameter);
                            break;
                    }
                }
            }
        }
    }

    #region === 以前的模式目前没有用了 ===

    public void DoUpdateAnimator(float deltatime)
    {
        DoUpdateAnimator(deltatime, 1, null, null);
    }

    // 以前的模式(目前没有用了)
    public void DoUpdateAnimator(float deltatime, float speed, System.Action callBackChange, System.Action<bool> callFinished)
    {
        if (m_ani == null)
            return;

        bool isChanged = IsChanged4CheckCurStateInfo();
        if (isChanged)
        {
            if (callBackChange != null)
            {
                callBackChange();
            }
        }

        if (cur_isHasCondition)
        {
            SetSpeed(speed);
            OnAniUpdate(deltatime);
        }
        else
        {
            float progress = 0.0f;
            if (!isChanged)
            {
                progress = normalizedTime + deltatime * speed / CurLens;
            }
            PlayCurr(progress, deltatime);
        }

        if (CurIsInTransition)
        {
            return;
        }

        int runed_loop_times = Mathf.FloorToInt(normalizedTime);
        if (runed_loop_times > cur_loop_times)
        {
            isFinishedOneWheel = true;
            cur_loop_times = runed_loop_times;
        }
        else
        {
            isFinishedOneWheel = false;
        }

        if (isFinishedOneWheel)
        {
            if (callFinished != null)
            {
                callFinished(isLoop);
            }
            isFinishedOneWheel = false;
        }

        // 执行事件
        stateEvent.OnUpdate(nt01);
    }

    #endregion

    public void DoUpdateAnimator(float deltatime,float speed)
    {
        if (m_ani == null)
            return;

        bool isChanged = IsChanged4CheckCurStateInfo();
        if (isChanged)
        {
            OnResetMemberReckon();

            if (this.callChanged != null)
            {
                this.callChanged();
            }
        }

        curSpeed = speed;

        OnUpdateTime(deltatime);

        if (isUpByCondition && cur_isHasCondition)
        {
            SetSpeed(speed);
            OnAniUpdate(deltatime);
        }
        else
        {
            PlayCurr(cur_Phase);
        }
        
        if (CurIsInTransition)
        {
            return;
        }
        
        if (isFinishedOneWheel)
        {
            if (this.callCompleted != null)
            {
                this.callCompleted(isLoop);
            }
            isFinishedOneWheel = false;
        }

        // 执行事件(自带的以前模式)
        stateEvent.OnUpdate(cur_Phase);

        // 新的模式用于外部调用
        OnUpdateCallPhase();
        OnUpdateCallProgress();
    }

    void OnCompleteAllRound()
    {
        cur_progressTime = cur_loop_times * cur_Length;
        cur_Phase = 1.0f;
        isCompletedRound = true;
    }

    protected void OnUpdateTime(float deltatime)
    {
        if (isCompletedRound)
            return;

        if (m_LoopTimes > 0 && m_LoopTimes <= cur_loop_times)
        {
            OnCompleteAllRound();
            return;
        }

        cur_progressTime += deltatime * m_InvLifeTime * curSpeed;
        cur_Phase = cur_progressTime - cur_loop_times;
        if (cur_Phase < 0.0f)
        {
            cur_Phase = 0.0f;
        }
        else if(cur_Phase >= 1.0f)
        {
            cur_loop_times++;
            if(cur_IsLoop || m_LoopTimes > 0)
            {
                cur_Phase -= 1.0f;
            }
            isFinishedOneWheel = true;

            if(!cur_IsLoop && m_LoopTimes <= 0)
            {
                OnCompleteAllRound();
            }
        }
    }

    protected void OnUpdateCallProgress()
    {
        if(this.callOnUpdateProgress != null)
        {
            this.callOnUpdateProgress(cur_progressTime);
        }
    }

    protected void OnUpdateCallPhase()
    {
        if (this.callOnUpdatePhase != null)
        {
            this.callOnUpdatePhase(cur_Phase);
        }
    }

    public void AddCallProgress(System.Action<float> callFunc)
    {
        if (callFunc == null)
            return;

        if(this.callOnUpdateProgress == null)
        {
            this.callOnUpdateProgress = callFunc;
        }else
        {
            this.callOnUpdateProgress += callFunc;
        }
    }

    public void AddCallPhase(System.Action<float> callFunc)
    {
        if (callFunc == null)
            return;

        if (this.callOnUpdatePhase == null)
        {
            this.callOnUpdatePhase = callFunc;
        }
        else
        {
            this.callOnUpdatePhase += callFunc;
        }
    }

    #region === state mache behaviour ==

    public T AddStateMache<T>() where T : StateMachineBehaviour
    {
        if (cur_state)
        {
            return cur_state.AddStateMachineBehaviour<T>();
        }
        return null;
    }

    public T GetStateMache<T>() where T : StateMachineBehaviour
    {
        if (cur_state != null && cur_state.behaviours != null)
        {
            int lens = cur_state.behaviours.Length;
            StateMachineBehaviour temp = null;
            for (int i = 0; i < lens; i++)
            {
                temp = cur_state.behaviours[i];
                if (temp is T)
                {
                    return temp as T;
                }
            }
        }
        return default(T);
    }

    public void RemoveStateMache<T>() where T : StateMachineBehaviour
    {
        if (cur_state != null && cur_state.behaviours != null)
        {
            int lens = cur_state.behaviours.Length;
            List<StateMachineBehaviour> lstMaches = new List<StateMachineBehaviour>();
            StateMachineBehaviour temp = null;
            for (int i = 0; i < lens; i++)
            {
                temp = cur_state.behaviours[i];
                if(temp is T)
                {
                    continue;
                }

                lstMaches.Add(temp);
            }

            cur_state.behaviours = lstMaches.ToArray();
        }
    }

    #endregion

    #region ==== 动作时间轴 ==== 
    public void AddCurEffect()
    {
        stateEvent.AddEffect();
    }

    public List<EA_Effect> curEffects
    {
        get
        {
            return stateEvent.lst_effects;
        }
    }

    public void RemoveEffect(EA_Effect effect)
    {
        stateEvent.RemoveEffect(effect);
    }

    public void ResetEvent(EA_Effect effect, Transform trsfParent)
    {
        stateEvent.ResetOneEvent(effect, trsfParent);
    }

    public void ResetCurEvents()
    {
        stateEvent.ResetEvents();
    }
    #endregion

    public virtual void DoStart(System.Action callChg = null, System.Action<bool> callFinished = null)
    {
        DoResetAniCtrl();

        OnResetMemberReckon();
        ResetCurEvents();

        SetCurCondition();
        this.callChanged = callChg;
        this.callCompleted = callFinished;

        PlayCurr(0);

        // SetApplyRootMotion(false);
    }
}
