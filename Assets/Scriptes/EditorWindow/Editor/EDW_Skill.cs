using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 技能编辑器窗口Class
/// 作者 : Canyon
/// 日期 : 2017-01-05 17:10
/// 功能 : 
/// </summary>
public class EDW_Skill : EditorWindow
{
    static bool isOpenWindowView = false;

    static protected EDW_Skill vwWindow = null;
    
    [MenuItem("Tools/Windows/EDSkill")]
    static void AddWindow()
    {
        if (isOpenWindowView || vwWindow != null)
            return;

        try
        {
            isOpenWindowView = true;
            vwWindow = GetWindow<EDW_Skill>("SkillEditor");

            int width = 900;
            int height = 400;
            float x = 460;
            float y = 220;
            vwWindow.position = new Rect(x, y, width, height);

            vwWindow.Show();
        }
        catch (System.Exception)
        {
            OnClearSWindow();
            throw;
        }
    }

    static void OnClearSWindow()
    {
        isOpenWindowView = false;
        vwWindow = null;
    }

    #region  == Member Attribute ===

    // delegate 更新
    System.Action call4OnUpdate;

    // 模型的Prefab
    GameObject gobjFab;
    bool m_isChangeFab;

    // 实例对象
    public GameObject gobjEntity;
    public ED_Ani_YGame me_ani;

    PS_MidLeft m_midLeft;
    PS_MidRight m_midRight;

    #endregion

    #region  == EditorWindow Func ===

    void Awake()
    {
        DoInit();
    }

    void OnEnable()
    {
        EditorApplication.update += OnUpdate;
    }

    void OnDisable()
    {
        EditorApplication.update -= OnUpdate;
    }

    void OnGUI()
    {
        EG_GUIHelper.FEG_BeginV();
        // 中体结构分三层(上，中，下)
        {
            // 上
            EG_GUIHelper.FEG_BeginH();

            GUIStyle style = EditorStyles.label;
            style.alignment = TextAnchor.MiddleCenter;
            string txtDecs = "类名 : 技能编辑器窗口\n"
                + "作者 : Canyon\n"
                + "日期 : 2017 - 01 - 05 17:10\n"
                + "描述 : 结构分上中下三层，中分左右两块，左边提供给美术对动作-特效；右边提供策划导出数据表。\n";
            GUILayout.Label(txtDecs, style);
            
            EG_GUIHelper.FEG_EndH();

            GameObject go = EditorGUILayout.ObjectField("Model Prefab : ", gobjFab, typeof(GameObject), false) as GameObject;
            if(go == null)
            {
                OnClearFab();
                Debug.Log("== 1 ==");
                DoClearEntity();

                EditorGUILayout.HelpBox("Model不能为空,请选择模型 !!!", MessageType.Error);
            }else
            {
                m_isChangeFab = go != gobjFab;
                if (m_isChangeFab)
                {
                    gobjFab = go;

                    OnInitEntity();
                }
            }
            EG_GUIHelper.FG_Space(3);
        }

        {
            // 中 : 分(左,右)
            EG_GUIHelper.FEG_BeginH();
            {
                // 左边
                DrawLeft();
                DrawRight();
            }
            EG_GUIHelper.FEG_EndH();
        }
        {
            // 下
        }

        EG_GUIHelper.FEG_EndV();

        m_isChangeFab = false;
    }

    // 在给定检视面板每秒10帧更新
    void OnInspectorUpdate()
    {
        Repaint();
    }

    void OnDestroy()
    {
        OnClearSWindow();
        EditorApplication.update -= OnUpdate;

        DoClear();
    }

    #endregion

    #region  == Self Func ===

    public void AddCall4Update(System.Action callFunc)
    {
        if(this.call4OnUpdate == null)
        {
            this.call4OnUpdate = callFunc;
        }else
        {
            this.call4OnUpdate += callFunc;
        }
    }

    public void RemoveCall4Update(System.Action callFunc)
    {
        if (this.call4OnUpdate != null)
        {
            this.call4OnUpdate -= callFunc;
        }
    }

    void DoInit()
    {
        OnInitMidLeft();
        OnInitMidRight();
    }

    void OnInitMidLeft()
    {
        if(m_midLeft == null)
        {
            m_midLeft = new PS_MidLeft();
            m_midLeft.DoInit(this);
        }
    }

    void OnInitMidRight()
    {
        if (m_midRight == null)
        {
            m_midRight = new PS_MidRight();
            m_midRight.DoInit(this);
        }
    }

    void OnInitEntity()
    {
        Debug.Log("== 2 ==");
        DoClearEntity();
        
        gobjEntity = GameObject.Instantiate<GameObject>(gobjFab);
        // gobjEntity = GameObject.Instantiate(gobjFab, Vector3.zero, Quaternion.identity) as GameObject;

        OnInitEnAni();
    }

    public void OnInitEnAni()
    {
        if (gobjEntity == null)
            return;

        if(me_ani == null)
            me_ani = new ED_Ani_YGame();

        me_ani.DoReInit(gobjEntity);

        m_midLeft.DoReset();
    }

    void OnUpdate()
    {
        if (this.call4OnUpdate != null)
        {
            this.call4OnUpdate();
        }
    }

    void DrawLeft()
    {
        OnInitMidLeft();
        m_midLeft.DrawShow();
    }

    void DrawRight()
    {
        OnInitMidRight();
        m_midRight.DrawShow();
    }

    void DoClear()
    {
        OnClearFab();

        Debug.Log("== 3 ==");

        DoClearEntity();

        m_midLeft = null;
        m_midRight = null;
        call4OnUpdate = null;
    }

    void OnClearFab()
    {
        m_isChangeFab = false;
        gobjFab = null;
    }

    void DoClearEntity()
    {
        if (gobjEntity)
            GameObject.DestroyImmediate(gobjEntity);
        gobjEntity = null;

        if (me_ani != null)
        {
            me_ani.DoClear();
            me_ani = null;
        }
    }
    #endregion
}
