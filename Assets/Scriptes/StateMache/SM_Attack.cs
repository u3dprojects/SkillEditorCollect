using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : state mache 的行为
/// 作者 : Canyon
/// 日期 : 2016-12-28 15:21
/// 功能 : 
/// </summary>
public class SM_Attack : StateMachineBehaviour
{
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
        Log("== OnStateEnter == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Log("== OnStateExit == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length);
    }

    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateIK(animator, stateInfo, layerIndex);
        Log("== OnStateIK == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
        Log("== OnStateMachineEnter == animator.name = " + animator.name + ",stateMachinePathHash = " + stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
        Log("== OnStateMachineExit == animator.name = " + animator.name + ",stateMachinePathHash = " + stateMachinePathHash);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateMove(animator, stateInfo, layerIndex);
        Log("== OnStateMove == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        Log("== OnStateUpdate == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length);
    }
}
