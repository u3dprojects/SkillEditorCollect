//***************************************************************
// 类名：精灵动作曲线
// 作者：钟汶洁
// 日期：2016.12
// 功能：用做保存动作曲线的数据结构
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
}

