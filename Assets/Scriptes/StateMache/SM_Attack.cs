using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

/// <summary>
/// 类名 : state mache 的行为
/// 作者 : Canyon
/// 日期 : 2016-12-28 15:21
/// 功能 : 
/// </summary>
public class SM_Attack : StateMachineBehaviour
{
    public enum DebugType
    {
        None = 0,
        All,
        Enter,
        Exit,
        EnterExit,
        IK,
        Mache,
        MacheEnter,
        MacheExit,
        Move,
        Update,
        MoveUpdate,
    }
    
    DebugType m_d_type = DebugType.All;
    public void SetDebugType(DebugType _debugType)
    {
        this.m_d_type = _debugType;
    }

    bool isEnter
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.Enter || m_d_type == DebugType.EnterExit;
        }
    }

    bool isExit
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.Exit || m_d_type == DebugType.EnterExit;
        }
    }

    bool isIK
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.IK;
        }
    }

    bool isMacheEnter
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.Mache || m_d_type == DebugType.MacheEnter;
        }
    }

    bool isMacheExit
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.Mache || m_d_type == DebugType.MacheExit;
        }
    }

    bool isMove
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.MoveUpdate || m_d_type == DebugType.Move;
        }
    }

    bool isUpdate
    {
        get
        {
            return m_d_type == DebugType.All || m_d_type == DebugType.MoveUpdate || m_d_type == DebugType.Update;
        }
    }

    bool isDebug = false;

    void Log(object obj,bool isCanDebug = false)
    {
        if (!isCanDebug)
        {
            if (!isDebug)
                return;
        }
        

        if(obj == null)
        {
            Debug.Log("obj is null !");
            return;
        }

        Debug.Log(obj.ToString());
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        Log("== OnStateEnter == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length,isEnter);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Log("== OnStateExit == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length,isExit);
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateIK(animator, stateInfo, layerIndex);
        Log("== OnStateIK == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length,isIK);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
        Log("== OnStateMachineEnter == animator.name = " + animator.name + ",stateMachinePathHash = " + stateMachinePathHash,isMacheEnter);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
        Log("== OnStateMachineExit == animator.name = " + animator.name + ",stateMachinePathHash = " + stateMachinePathHash,isMacheExit);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        Log("== OnStateMove == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length,isMove);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Log("== OnStateUpdate == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length,isUpdate);
    }
}
