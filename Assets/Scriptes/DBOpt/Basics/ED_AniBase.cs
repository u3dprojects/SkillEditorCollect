using UnityEngine;
using System.Collections;
using UnityEditor.Animations;
using System.Collections.Generic;

/// <summary>
/// 类名 : E-Editor,D-Data,Ani-Animator,Base 基础
/// 作者 : Canyon
/// 日期 : 2017-01-03 14:30:00
/// 功能 : 管理整个Animator的数据
/// </summary>
[System.Serializable]
public class ED_AniBase : System.Object
{
    public Animator m_ani { get; set; }
    AnimatorController m_ani_ctrl;

    // animator 中key - 对应 - State动画(key = layerIndex_sub_state.name or layerIndex_state.name)
    Dictionary<string, AnimatorState> dic_name_state = new Dictionary<string, AnimatorState>();

    // animator 中key - 对应 - layer层级(key = layerIndex_sub_state.name or layerIndex_state.name)
    Dictionary<string, int> dic_state_layer = new Dictionary<string, int>();

    // 保存key值 key = layerIndex_submatchName_stateName 或者 layerIndex_stateName;
    List<string> lst_keys = new List<string>();

    // 保存state 名字
    Hashtable map_StateName = new Hashtable();

    public ED_AniBase() { }

    public ED_AniBase(Animator ani)
    {
        DoReInit(ani);
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
            Debug.LogError("isHased key = " + key + "nm = " + state.name + ",layer index = " + layer);
            return;
        }

        dic_name_state.Add(key, state);
        dic_state_layer.Add(key, layer);
        lst_keys.Add(key);

        map_StateName[state.name] = true;
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
        if (m_ani != null && m_ani_ctrl != null)
        {
            int _lens = m_ani.parameters.Length;
            if (_lens <= 0)
                return;

            foreach (AnimatorControllerParameter par in m_ani.parameters)
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

    public void SetSpeed(float speed)
    {
        if (m_ani)
            m_ani.speed = speed;
    }

    public void OnAniUpdate(float deltatime)
    {
        if(m_ani)
            m_ani.Update(deltatime);
    }

    public void Play(string stateName, int layer, float begNormallizedTime, float delta_time = 0)
    {
        if (m_ani && map_StateName.ContainsKey(stateName))
        {
            m_ani.Play(stateName, layer, begNormallizedTime);

            // 这个地方有和无，为0和不为0的区别没做测试???
            m_ani.Update(delta_time);
        }
    }

    public List<string> Keys
    {
        get
        {
            return lst_keys;
        }
    }

    public Dictionary<string, AnimatorState> KState
    {
        get
        {
            return dic_name_state;
        }
    }

    public Dictionary<string, int> KLayer
    {
        get
        {
            return dic_state_layer;
        }
    }


    public void DoClear()
    {
        DoResetAniCtrl();

        m_ani = null;
        m_ani_ctrl = null;
        
        dic_name_state.Clear();
        dic_state_layer.Clear();
        lst_keys.Clear();
        map_StateName.Clear();

        OnClear();
    }

    protected AnimatorControllerParameterType GetParsType(string pars_name)
    {
        if (m_ani)
        {
            foreach (AnimatorControllerParameter par in m_ani.parameters)
            {
                if (par.name == pars_name)
                {
                    return par.type;
                }
            }
        }
        return AnimatorControllerParameterType.Bool;
    }

    AnimationClip GetAniClip(string clipName)
    {
        if (m_ani_ctrl)
        {
            foreach (AnimationClip item in m_ani_ctrl.animationClips)
            {
                if (item.name == clipName)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public virtual void OnClear() { }

    public bool IsHasAniCtrl
    {
        get { return this.m_ani_ctrl != null; }
    }

    public void SetApplyRootMotion(bool isApply)
    {
        if (m_ani)
            m_ani.applyRootMotion = isApply;
    }
}
