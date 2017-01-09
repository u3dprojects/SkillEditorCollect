using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

/// <summary>
/// 类名 : P-Partial,S-Skill 美术工具:动作-特效预览
/// 作者 : Canyon
/// 日期 : 2017-01-06 09:10
/// 功能 : 
/// </summary>
public class PS_MidLeft{

    #region  == Member Attribute ===

    EDW_Skill m_wSkill;

    ED_Ani_YGame m_curAni
    {
        get
        {
            return m_wSkill.me_ani;
        }
    }

    CharacterController m_myCtrl
    {
        get { return m_wSkill.m_myCtrl; }
    }

    Transform trsfEntity
    {
        get { return m_wSkill.trsfEntity; }
    }

    DBOpt_Time m_curTime;

    // popup 列表选择值
    int ind_popup = 0;
    int pre_ind_popup = -1;

    // 速度控制
    float cur_speed = 1.0f;
    bool isCanSetMinMaxSpeed = false;
    float min_speed = 0.0f;
    float max_speed = 3.0f;

    // 动作进度
    bool isCtrlProgress = false;
    float cur_progress = 0.0f;
    float min_progress = 0.0f;
    float max_progress = 1.0f;

    // 循环次数
    bool isRound = false;
    int round_times = 1;

    // 位移
    bool isOpenMovPos = false;
    AnimationCurve x_curve;
    AnimationCurve y_curve;
    AnimationCurve z_curve;
    Vector3 movPos = Vector3.zero;

    // 特效事件
    
    List<bool> m_event_fodeOut = new List<bool>();
    // 特效挂节点
    bool isEffectJoinSelf = false;

    // 暂停按钮控制
    bool isPauseing = false;

    // 是否运行
    bool isRunnging = false;

    #endregion

    public void DoInit(EDW_Skill org)
    {
        this.m_wSkill = org;
        this.m_wSkill.AddCall4Update(OnUpdate);

        OnInitTime();
    }


    void OnInitTime()
    {
        if (m_curTime == null)
            m_curTime = new DBOpt_Time();

        m_curTime.DoReInit(false);
    }

    public void DoReset()
    {
        ind_popup = 0;
        pre_ind_popup = -1;
    }

    void OnResetProgress()
    {
        max_progress = m_curAni.CurLens;
        max_progress = max_progress > 0 ? max_progress : 1.0f;
    }

    public void DrawShow()
    {
        EG_GUIHelper.FEG_BeginVArea();
        {
            _DrawDesc();

            if (this.m_wSkill.gobjEntity)
            {
                _DrawFreshBtn();

                if (_DrawAniJudged())
                {
                    _DrawAniList();

                    _DrawAniStateInfo();

                    _DrawAniMinMaxSpeed();

                    _DrawAniCurSpeed();

                    _DrawCtrlAniStateProgress();

                    _DrawRoundTimes();

                    _DrawMovPos();

                    _DrawEffects();

                    _DrawOptBtns();
                }
            }
        }
        EG_GUIHelper.FEG_EndV();
    }

    void _DrawDesc()
    {
        EG_GUIHelper.FEG_BeginH();

        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleCenter;
        string txtDecs = "美术工具:动作预览(可添加特效)";
        GUILayout.Label(txtDecs, style);

        EG_GUIHelper.FEG_EndH();
    }

    void _DrawFreshBtn()
    {
        EG_GUIHelper.FEG_BeginH();
        if (GUILayout.Button("刷新Animator动作列表List"))
        {
            m_wSkill.OnInitEnAni();
        }
        EG_GUIHelper.FEG_EndH();
    }

    bool _DrawAniJudged()
    {
        bool ret = false;
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginV();
            {
                if (m_wSkill == null || m_curAni == null)
                {
                    EditorGUILayout.HelpBox("请拖动Prefab到Model Prefab中！", MessageType.Error);
                }else if (!m_curAni.IsHasAniCtrl)
                {
                    EditorGUILayout.HelpBox("该Animator里面没有AnimatorController \n\n请添加动画控制器-AnimatorController！", MessageType.Error);
                }
                else if (m_curAni.Keys.Count <= 0)
                {
                    EditorGUILayout.HelpBox("该AnimatorController里面没有任何动画，请添加State动画！", MessageType.Error);
                }else
                {
                    ret = true;
                }
            }
            EG_GUIHelper.FEG_EndV();
        }
        EG_GUIHelper.FEG_EndH();
        return ret;
    }

    void _DrawAniList()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;
        EditorGUILayout.LabelField("动画列表:",style);

        style = EditorStyles.popup;
        style.alignment = TextAnchor.MiddleRight;
        ind_popup = EditorGUILayout.Popup(ind_popup, m_curAni.Keys.ToArray(), style);
        if (pre_ind_popup != ind_popup)
        {
            pre_ind_popup = ind_popup;
            m_curAni.SetSpeed(1.0f);
            m_curAni.ResetAniState(ind_popup);

            OnResetProgress();
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniStateInfo()
    {
        EG_GUIHelper.FEG_BeginH();
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;

        GUILayoutOption minW = GUILayout.MinWidth(90);
        GUILayout.Label("动画帧数: " + m_curAni.CurFrameCount, style, minW);

        GUILayout.Label("动画时长: " + m_curAni.CurLens + " s", style, minW);

        style.alignment = TextAnchor.MiddleRight;
        EditorGUILayout.LabelField("动画帧率: " + m_curAni.CurFrameRate + " 帧/s", style);
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniMinMaxSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("MinMax速度??", ref isCanSetMinMaxSpeed);
            min_speed = EditorGUILayout.FloatField("Min速度:", min_speed);
            EG_GUIHelper.FG_Space(3);
            max_speed = EditorGUILayout.FloatField("Max速度:", max_speed);
            EG_GUIHelper.FEG_EndToggleGroup();

            if (max_speed < min_speed)
            {
                max_speed = min_speed;
            }
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawAniCurSpeed()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField("当前速度:", style);
            
            cur_speed = EditorGUILayout.Slider(cur_speed, min_speed, max_speed);
        }
        EG_GUIHelper.FEG_EndH();
    }

    float Round(float org, int acc)
    {
        float pow = Mathf.Pow(10, acc);
        float temp = org * pow;
        return Mathf.RoundToInt(temp) / pow;
    }

    void ReckonProgress(float normalizedTime)
    {
        cur_progress = (normalizedTime % 1);
        cur_progress = cur_progress * max_progress;
        // cur_progress = Round(cur_progress, 6);
    }

    void _DrawCtrlAniStateProgress()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("控制动作进度??", ref isCtrlProgress);
            
            float cur_progress01 = cur_progress / max_progress;
            if (isCtrlProgress)
            {
                ReckonProgress(cur_progress01);
                m_curAni.DoPlayCurr(cur_progress01);
            }
            else {
                ReckonProgress(m_curAni.normalizedTime);
            }

            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleRight;
            EditorGUILayout.LabelField("当前动作进度: " + cur_progress01, style);

            EG_GUIHelper.FG_Space(3);

            cur_progress = EditorGUILayout.Slider(cur_progress, min_progress, max_progress);
            EG_GUIHelper.FEG_EndToggleGroup();

            //GUIStyle style = new GUIStyle();
            //style.alignment = TextAnchor.MiddleRight;
            //style.normal.textColor = Color.yellow;
            //EditorGUILayout.LabelField("(勾选时，才可控制 [当前进度]！！！)", style);
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawRoundTimes()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("控制循环次数??", ref isRound);
            GUIStyle style = new GUIStyle();
            style.alignment = TextAnchor.MiddleRight;
            style.normal.textColor = Color.cyan;

            string desc = string.Format("已播放了{0:D2}次!", m_curAni.CurLoopCount);
            EditorGUILayout.LabelField(desc, style);

            EG_GUIHelper.FG_Space(3);

            round_times = EditorGUILayout.IntField("循环次数:", round_times);

            EG_GUIHelper.FEG_EndToggleGroup();
            
        }
        EG_GUIHelper.FEG_EndH();
    }

    void InitMovPosCurve()
    {
        DefCurve();
    }

    void DefCurve()
    {
        x_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        y_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
        z_curve = new AnimationCurve(new Keyframe(0, 0, 0, 0), new Keyframe(1, 1, 0, 0));
    }

    void SaveMache() { }

    void RemoveMache() { }

    void _DrawMovPos()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("开启位移??", ref isOpenMovPos);
            {
                InitMovPosCurve();

                EG_GUIHelper.FEG_BeginV();

                EG_GUIHelper.FEG_BeginH();
                GUI.color = Color.cyan;
                if (GUILayout.Button("SaveCurveMache"))
                {
                    SaveMache();
                }

                GUI.color = Color.red;
                if (GUILayout.Button("RemoveCurveMache"))
                {
                    RemoveMache();
                }
                GUI.color = Color.white;
                EG_GUIHelper.FEG_EndH();

                EG_GUIHelper.FG_Space(5);

                EG_GUIHelper.FEG_BeginH();
                x_curve = EditorGUILayout.CurveField("x", x_curve);
                EG_GUIHelper.FEG_EndH();

                EG_GUIHelper.FG_Space(5);

                EG_GUIHelper.FEG_BeginH();
                y_curve = EditorGUILayout.CurveField("y", y_curve);
                EG_GUIHelper.FEG_EndH();

                EG_GUIHelper.FG_Space(5);

                EG_GUIHelper.FEG_BeginH();
                z_curve = EditorGUILayout.CurveField("z", z_curve);
                EG_GUIHelper.FEG_EndH();

                EG_GUIHelper.FEG_EndV();
            }
            EG_GUIHelper.FEG_EndToggleGroup();
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawEffects()
    {
        EG_GUIHelper.FG_BeginVAsArea();
        {
            {
                // 上
                EG_GUIHelper.FEG_BeginH();
                Color def = GUI.backgroundColor;
                GUI.backgroundColor = Color.black;
                GUI.color = Color.white;

                EditorGUILayout.LabelField("特效列表", EditorStyles.textArea);

                GUI.backgroundColor = def;

                GUI.color = Color.green;
                if (GUILayout.Button("+", GUILayout.Width(50)))
                {
                    m_curAni.AddCurEffect();
                }
                GUI.color = Color.white;
                EG_GUIHelper.FEG_EndV();
            }

            {
                // 中
                int lens = m_curAni.curEffects.Count;
                if (lens > 0)
                {
                    for (int i = 0; i < lens; i++)
                    {
                        lens = m_curAni.curEffects.Count;
                        if (i > lens - 1)
                        {
                            i = lens - 1;
                        }
                        m_event_fodeOut.Add(false);

                        _DrawOneEffect(i, m_curAni.curEffects[i]);
                    }
                }
                else
                {
                    m_event_fodeOut.Clear();
                }
            }
        }
        EG_GUIHelper.FG_EndV();
    }

    void _DrawOneEffect(int index,EA_Effect effect)
    {
        bool isEmptyName = string.IsNullOrEmpty(effect.name);

        EG_GUIHelper.FEG_BeginV();
        {
            EG_GUIHelper.FEG_BeginH();
            {
                m_event_fodeOut[index] = EditorGUILayout.Foldout(m_event_fodeOut[index], "特效 - " + (isEmptyName ? "未指定" : effect.name));
                GUI.color = Color.red;
                if (GUILayout.Button("X", EditorStyles.miniButton, GUILayout.Width(50)))
                {
                    m_curAni.RemoveEffect(effect);
                    m_event_fodeOut.RemoveAt(index);
                }
                GUI.color = Color.white;
            }
            EG_GUIHelper.FEG_EndH();

            EG_GUIHelper.FG_Space(5);

            if (m_event_fodeOut[index])
            {
                _DrawOneEffectAttrs(effect);
            }
        }
        EG_GUIHelper.FEG_EndV();
    }

    void _DrawOneEffectAttrs(EA_Effect effect)
    {
        if (effect.isChanged)
        {
            m_curAni.ResetEvent(effect, effect.trsfParent);
        }

        EG_GUIHelper.FEG_BeginH();
        {
            GUILayout.Label("特效文件:", GUILayout.Width(80));
            effect.gobjFab = EditorGUILayout.ObjectField(effect.gobjFab, typeof(GameObject), false) as GameObject;
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        {
            GUILayout.Label("触发时间:", GUILayout.Width(80));
            effect.time = EditorGUILayout.Slider(effect.time, 0, 1);
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);

        _DrawOneEffectJoinPos(effect);
    }

    void _DrawOneEffectJoinPos(EA_Effect effect)
    {
        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FEG_BeginToggleGroup("手动位置??", ref isEffectJoinSelf);
            effect.trsfParent = EditorGUILayout.ObjectField("位置:", effect.trsfParent, typeof(Transform), isEffectJoinSelf) as Transform;
            EG_GUIHelper.FEG_EndToggleGroup();
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);
        GUIStyle style = EditorStyles.label;
        style.alignment = TextAnchor.MiddleLeft;

        EG_GUIHelper.FEG_BeginH();
        {
            effect.v3LocPos = EditorGUILayout.Vector3Field("偏移:", effect.v3LocPos);
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        {
            effect.v3LocEulerAngle = EditorGUILayout.Vector3Field("旋转:", effect.v3LocEulerAngle);
        }
        EG_GUIHelper.FEG_EndH();

        EG_GUIHelper.FG_Space(5);

        EG_GUIHelper.FEG_BeginH();
        {
            EG_GUIHelper.FG_Label("缩放:");
            effect.scale = EditorGUILayout.FloatField(effect.scale);
        }
        EG_GUIHelper.FEG_EndH();
    }

    void _DrawOptBtns()
    {
        EG_GUIHelper.FEG_BeginH();
        {
            if (GUILayout.Button("Play"))
            {
                DoPlay();
            }

            if (GUILayout.Button(isPauseing ? "ReGo" : "Pause"))
            {
                isPauseing = !isPauseing;
                if (isPauseing)
                {
                    DoPause();
                }
                else
                {
                    DoResume();
                }
            }

            if (GUILayout.Button("Stop"))
            {
                DoStop();
            }
        }
        EG_GUIHelper.FEG_EndH();
    }


    void OnUpdate()
    {
        if (!isRunnging || isPauseing || isCtrlProgress)
            return;

        if (this.m_curAni == null)
            return;

        this.m_curTime.DoUpdateTime();
        this.m_curAni.DoUpdateAnimator(m_curTime.DeltaTime,cur_speed);

        // 设置粒子速度
        EDM_Particle.m_instance.SetSpeed(cur_speed);
        EDM_Particle.m_instance.OnUpdate(m_curTime.DeltaTime,true);

        // 设置位移
        if (isOpenMovPos)
        {
            movPos = Vector3.zero;
            movPos.x = x_curve.Evaluate(this.m_curTime.ProgressTime);
            movPos.y = y_curve.Evaluate(this.m_curTime.ProgressTime);
            movPos.z = z_curve.Evaluate(this.m_curTime.ProgressTime);
            if(m_myCtrl != null && m_myCtrl.enabled)
            {
                m_myCtrl.Move(movPos);
            }else
            {
                movPos = trsfEntity.TransformDirection(movPos);
                trsfEntity.Translate(movPos);
            }
        }
    }

    void DoPlay() {
        this.m_curAni.DoStart();

        isRunnging = true;
        isPauseing = false;

        EDM_Particle.m_instance.DoInit(false);
    }

    void DoPause() {
        isPauseing = true;
    }

    void DoResume() {
        isPauseing = false;
    }

    void DoStop() {
        isRunnging = false;
        EDM_Particle.m_instance.DoClear();
    }
}
