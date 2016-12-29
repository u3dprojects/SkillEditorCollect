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
/// 类名 : E-Editor,D-Data,Ani-Animator
/// 作者 : Canyon
/// 日期 : 2016-12-21 10:10:00
/// 功能 : 
/// </summary>
[System.Serializable]
public class ED_Ani : System.Object {

    public Animator m_ani { get; set; }
    AnimatorController m_ani_ctrl;

    // animator 中key - 对应 - State动画(key = layer_sub_state.name or layer_state.name)
    Dictionary<string, AnimatorState> dic_name_state = new Dictionary<string, AnimatorState>();

    // animator 中key - 对应 - layer层级(key = layer_sub_state.name or layer_state.name)
    Dictionary<string, int> dic_state_layer = new Dictionary<string, int>();

    List<string> lst_keys = new List<string>();

    string cur_state_key = "";
    int cur_layer_index = 0;
    AnimatorState cur_state;
    
    int cur_state_frame_count = 0;
    float cur_state_length = 0f;

    // 当前state是否需要条件才能播放
    bool cur_is_HasCondition = false;

    AnimatorStateInfo cur_stateInfo;
    int cur_state_shortNameHash = 0;

    // 控制循环
    int runed_loop_times = 0;
    int cur_loop_times = 0;
    bool isFinished_OneWheel = false;

    // 动作时间轴时间
    DBU3D_AniState_TimeEvent stateEvent = new DBU3D_AniState_TimeEvent();

    // 当前StateMatch
    public StateMachineBehaviour cur_state_mache { get; set; }

    public ED_Ani() { }

    public ED_Ani(Animator ani)
    {
        DoInit(ani);
    }

    public void DoReInit(Animator ani)
    {
        DoClear();
        DoInit(ani);
    }

    public void DoInit(Animator ani)
    {
        m_ani = ani;

        if (m_ani)
            m_ani_ctrl = m_ani.runtimeAnimatorController as AnimatorController;

        if (m_ani_ctrl)
        {
            int lens_layer = m_ani_ctrl.layers.Length;
            AnimatorControllerLayer layer;

            for (int index = 0; index < lens_layer; index++)
            {
                layer = m_ani_ctrl.layers[index];
                // 第一层
                InitByMachine(index, layer.stateMachine);
            }
        }
    }

    void InitByMachine(int layer, AnimatorStateMachine machine, string subname = "")
    {
        InitByChildStates(layer, machine.states, subname);

        string sname = "";

        // 子层
        foreach (ChildAnimatorStateMachine childMachine in machine.stateMachines)
        {
            sname = childMachine.stateMachine.name;
            if (!string.IsNullOrEmpty(subname))
            {
                sname = subname + "_" + sname;
            }

            InitByMachine(layer, childMachine.stateMachine, sname);
        }
    }

    void InitByChildStates(int layer, ChildAnimatorState[] cms, string subname = "")
    {
        AnimatorState oneState;
        foreach (ChildAnimatorState childState in cms)
        {
            oneState = childState.state;
            InitAniState(layer, oneState, subname);
        }
    }

    void InitAniState(int layer, AnimatorState state, string subname = "")
    {
        if (state.motion == null)
            return;

        string key = "";
        if (string.IsNullOrEmpty(subname))
            key = layer + "_" + state.name;
        else
            key = layer + "_" + subname + "_" + state.name;

        if (dic_name_state.ContainsKey(key))
        {
            Debug.Log("isHased key = " + key + "nm = " + state.name + ",layer index = " + layer);
            return;
        }

        dic_name_state.Add(key, state);
        dic_state_layer.Add(key, layer);
        lst_keys.Add(key);
    }

    public void DoResetAniCtrl()
    {
        if (m_ani)
        {
            m_ani.Rebind();
            m_ani.speed = 1;
        }
        
        OnResetPars();
        OnResetLayers();
    }

    void OnResetPars()
    {
        if (m_ani)
        {
            foreach(AnimatorControllerParameter par in m_ani.parameters)
            {
                switch (par.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        m_ani.SetBool(par.name, par.defaultBool);
                        break;
                    case AnimatorControllerParameterType.Float:
                        m_ani.SetFloat(par.name, par.defaultFloat);
                        break;
                    case AnimatorControllerParameterType.Int:
                        m_ani.SetInteger(par.name, par.defaultInt);
                        break;
                    case AnimatorControllerParameterType.Trigger:
                        m_ani.ResetTrigger(par.name);
                        break;
                }
            }
        }
    }

    AnimatorControllerParameterType GetParsType(string pars_name)
    {
        if (m_ani)
        {
            foreach (AnimatorControllerParameter par in m_ani.parameters)
            {
               if(par.name == pars_name)
                {
                    return par.type;
                }
            }
        }
        return AnimatorControllerParameterType.Bool;
    }


    void OnResetLayers()
    {
        if (m_ani != null && m_ani_ctrl != null)
        {
            AnimatorControllerLayer layer;
            for (int index = 0; index < m_ani.layerCount; index++)
            {
                layer = m_ani_ctrl.layers[index];
                Play(layer.stateMachine.defaultState.name, index, 0);
            }
        }
    }

    AnimationClip GetAniClip(string name)
    {
        if (m_ani_ctrl)
        {
            foreach (AnimationClip item in m_ani_ctrl.animationClips)
            {
                if (item.name == name)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public void ResetAniState(int index_key)
    {
        if(index_key < 0)
        {
            Debug.Log("Key的Index下标从0开始,index = " + index_key);
            return;
        }
        if(index_key >= lst_keys.Count)
        {
            Debug.Log("Key的Index下标小其长度,index = " + index_key);
            return;
        }

        string key = lst_keys[index_key];
        ResetAniState(key);
    }

    public void ResetAniState(string key)
    {
        if (dic_name_state.ContainsKey(key))
        {
            cur_state_key = key;
            cur_state = dic_name_state[key];
            cur_layer_index = dic_state_layer[key];
            ResetAniState(cur_state);
        }
        else
        {
            Debug.Log("not has state key = " + key);
        }
    }

    void ResetAniState(AnimatorState state)
    {
        cur_state = state;
        cur_state_mache = null;
        cur_state_frame_count = 0;
        cur_state_length = 0.0f;
        cur_is_HasCondition = false;

        // 动作时间事件
        stateEvent.DoClear();

        if (state.motion)
        {
            AnimationClip clip = CurClip();
            cur_state_length = clip.length;
            float count = clip.frameRate * clip.length;
            cur_state_frame_count = Mathf.FloorToInt(count);

            cur_is_HasCondition = ReCurIsHasCondition();

            Debug.Log("key = " + cur_state_key + ",lens = " + clip.length + ",frameRate = " + clip.frameRate + ",count = " + count + ",countint = " + cur_state_frame_count);
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

    public List<string> Keys
    {
        get
        {
            return lst_keys;
        }
    }

    public AnimationClip CurClip()
    {
        if (cur_state && cur_state.motion)
        {
            return cur_state.motion as AnimationClip;
        }
        return null;
    }

    public AnimatorState CurState
    {
        get { return cur_state;}
    }

    public float CurLens
    {
        get
        {
            return cur_state_length;
        }
    }

    public int CurFrameCount
    {
        get
        {
            return cur_state_frame_count;
        }
    }

    public float CurFrameRate
    {
        get
        {
            AnimationClip clip = CurClip();
            if (clip)
            {
                return clip.frameRate;
            }
            return 0.0f;
        }
    }

    public void DoClear()
    {
        DoResetAniCtrl();

        m_ani = null;
        m_ani_ctrl = null;
        cur_state = null;
        cur_state_mache = null;
        cur_state_key = "";
        cur_layer_index = 0;

        dic_name_state.Clear();
        dic_state_layer.Clear();
        lst_keys.Clear();
        
        OnResetMember();

        stateEvent.DoClear();
    }

    void OnResetMember()
    {
        cur_state_frame_count = 0;
        cur_state_length = 0.0f;
        cur_is_HasCondition = false;

        cur_state_shortNameHash = 0;

        OnResetMemberReckon();
    }

    public void OnResetMemberReckon()
    {
        runed_loop_times = 0;
        cur_loop_times = 0;
        isFinished_OneWheel = false;
    }

    public void PlayCurr(float begNormallizedTime, float delta_time = 0)
    {
        if (cur_state)
        {
            Play(cur_state.name, cur_layer_index, begNormallizedTime,delta_time);
        }
    }

    public void Play(string stateName,int layer,float begNormallizedTime,float delta_time = 0)
    {
        if (m_ani){ 
            m_ani.Play(stateName, layer, begNormallizedTime);
            m_ani.Update(delta_time);
        }
    }

    public bool IsChanged4CheckCurStateInfo()
    {
        if (cur_state)
        {
            cur_stateInfo = m_ani.GetCurrentAnimatorStateInfo(cur_layer_index);
            if (cur_state_shortNameHash != cur_stateInfo.shortNameHash)
            {
                cur_state_shortNameHash = cur_stateInfo.shortNameHash;
                return true;
            }
        }
        return false;
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
            return Mathf.Repeat(normalizedTime, 1);
        }
    }

    public bool isLoop
    {
        get { return cur_stateInfo.loop; }
    }

    public void SetCurCondition()
    {
        if (cur_state)
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

    public bool CurIsHasCondition
    {
        get { return cur_is_HasCondition;}
    }

    public bool CurIsInTransition
    {
        get
        {
            return m_ani.IsInTransition(cur_layer_index);
        }
    }

    public void OnAniUpdate(float deltatime)
    {
        m_ani.Update(deltatime);
    }

    public void SetSpeed(float speed)
    {
        m_ani.speed = speed;
    }

    public void DoUpdateAnimator(float deltatime, float speed)
    {
        DoUpdateAnimator(deltatime, speed, null, null);
    }

    public void DoUpdateAnimator(float deltatime,float speed,System.Action callBackChange,System.Action<bool> callFinished)
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

        if (cur_is_HasCondition)
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

        cur_loop_times = Mathf.FloorToInt(normalizedTime);
        if (cur_loop_times > runed_loop_times)
        {
            isFinished_OneWheel = true;
            runed_loop_times = cur_loop_times;
        }
        else
        {
            isFinished_OneWheel = false;
        }

        if (isFinished_OneWheel)
        {
            if (callFinished != null)
            {
                callFinished(isLoop);
            }
        }

        // 执行事件
        stateEvent.OnUpdate(nt01);
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

    public void ResetEvent(EA_Effect effect)
    {
        stateEvent.ResetOneEvent(effect);
    }

    public void ResetCurEvents()
    {
        stateEvent.ResetEvents();
    }
    #endregion
}
