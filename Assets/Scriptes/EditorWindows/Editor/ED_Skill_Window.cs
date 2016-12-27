using UnityEngine;
using System.Collections;
using UnityEditor;

public class ED_Skill_Window : EditorWindow
{
    static bool isOpenWindowView = false;

    static protected ED_Skill_Window vwWindow = null;

    [MenuItem("Tools/Edtior_Skill")]
    static void AddWindow()
    {
        if (isOpenWindowView || vwWindow != null)
            return;

        isOpenWindowView = true;
        vwWindow = GetWindow<ED_Skill_Window>("技能编辑器");
    }

    //DBU3D_Ani m_db_ani = new DBU3D_Ani();

    //DBU3D_GUI m_gui_draw = new DBU3D_GUI();

    //DBOpt_Time m_db_time = new DBOpt_Time();

    string className = "Rigidbody";
    void OnGUI()
    {
        className = EditorGUILayout.TextField("Component:", className);
        if (GUILayout.Button("Delete!"))
        {
            var destroyedCount = 0;

			foreach(GameObject obj in Selection.GetFiltered(
            typeof(GameObject),
            SelectionMode.Editable | SelectionMode.Deep))
            {
                var component = obj.GetComponent(className);
                if (component)
                {
                    DestroyImmediate(component);
                    destroyedCount++;
                }
            }
            this.Close();
            EditorUtility.DisplayDialog("Deletion Report",
                string.Format("Deleted {0} components of type \"{1}\"", destroyedCount, className),
                "Close");
        }
    }

    void OnDestroy()
    {
        isOpenWindowView = false;
        vwWindow = null;
    }

    // 在所有可见的窗口每秒调用100次。
    void Update()
    {

    }
}
