using UnityEngine;
using System.Collections;
using UnityEditor;

public class ED_Skill_Window : EditorWindow
{
    static bool isOpenWindowView = false;

    static ED_Skill_Window vwWindow = null;

    [MenuItem("Tools/Edtior_Skill")]
    static void AddWindow()
    {
        if (isOpenWindowView)
            return;

        isOpenWindowView = true;
        vwWindow = GetWindow<ED_Skill_Window>("技能编辑器");
    }

    DBOpt_Ani m_db_ani = new DBOpt_Ani();

    DBOpt_GUI m_gui_draw = new DBOpt_GUI();

    DBOpt_Time m_db_time = new DBOpt_Time();

    void OnGUI()
    {

    }
}
