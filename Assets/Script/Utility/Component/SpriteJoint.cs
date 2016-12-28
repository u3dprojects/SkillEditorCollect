//***************************************************************
// 类名：精灵挂接点
// 作者：钟汶洁
// 日期：2016.12
// 功能：编辑、获取精灵挂接点
//***************************************************************

using UnityEngine;
using System.Collections;

/// <summary>
/// 精灵挂接点
/// </summary>
public class SpriteJoint : MonoBehaviour
{
    /// <summary>
    /// 挂接点类型
    /// 注意：新的枚举只能定义在最后（Length字段前），严禁在中间插入！
    /// </summary>
    public enum JointType
    {
        /// <summary>
        /// 默认（原点）
        /// </summary>
        Default = 0,
        /// <summary>
        /// 胸口
        /// </summary>
        Chest,
        /// <summary>
        /// 腰部
        /// </summary>
        Side,
        /// <summary>
        /// 头部
        /// </summary>
        Head,
        /// <summary>
        /// 左手
        /// </summary>
        LeftHand,
        /// <summary>
        /// 右手
        /// </summary>
        RightHand,
        /// <summary>
        /// 左武器
        /// </summary>
        LeftWeapon,
        /// <summary>
        /// 右武器
        /// </summary>
        RightWeapon,
        /// <summary>
        /// 该字段仅仅用来计算枚举值个数
        /// </summary>
        Length
    }

    /// <summary>
    /// 挂接点数组
    /// </summary>
    public Transform[] jointArray = new Transform[(int)JointType.Length];
}

