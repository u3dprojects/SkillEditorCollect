//***************************************************************
// 类名：精灵位移曲线
// 作者：钟汶洁
// 日期：2016.12
// 功能：跟随动作的播放的时间，通过预先编辑的位移曲线，使用角色控制器来控制精灵的位移
//***************************************************************

using UnityEngine;
using System.Collections;

/// <summary>
/// 精灵动作曲线
/// </summary>
public class SpriteAniCurve : StateMachineBehaviour
{
    /// <summary>
    /// x方向曲线
    /// </summary>
    public AnimationCurve x;
    /// <summary>
    /// y方向曲线
    /// </summary>
    public AnimationCurve y;
    /// <summary>
    /// z方向曲线
    /// </summary>
    public AnimationCurve z;

    /// <summary>
    /// 精灵角色控制器
    /// </summary>
    public CharacterController charController
    {
        get
        {
            return m_charController;
        }
        set
        {
            m_charController = value;
            if (m_charController != null)
            {
                m_transform = m_charController.transform;
            }
        }
    }

    private float m_liveTime = 0;
    private CharacterController m_charController;
    private Transform m_transform;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_liveTime = 0;
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        m_liveTime = 0;
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_liveTime += Time.deltaTime;
        Debug.Log("== OnStateMove == animator.name = " + animator.name + ",layerIndex = " + layerIndex + ",normalizedTime = " + stateInfo.normalizedTime + ",length = " + stateInfo.length + "," + m_liveTime);
        if (this.charController != null)
        {
            Vector3 move = Vector3.zero;
            move.x = x.Evaluate(m_liveTime);
            move.y = y.Evaluate(m_liveTime);
            move.z = z.Evaluate(m_liveTime);
            move = m_transform.TransformVector(move);
            m_charController.Move(move);
        }
    }
}

