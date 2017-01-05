using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// 类名 : 删除组件脚本的测试类
/// 作者 : Canyon
/// 日期 : 2017-01-05 13:40
/// 功能 : 
/// </summary>
public class EDW_DeleteComponent : EditorWindow
{
    static bool isOpenWindowView = false;

    static protected EDW_DeleteComponent vwWindow = null;

    [MenuItem("Tools/Windows/DeleteComponent")]
    static void AddWindow()
    {
        if (isOpenWindowView || vwWindow != null)
            return;

        isOpenWindowView = true;
        vwWindow = GetWindow<EDW_DeleteComponent>("技能编辑器");

        int width = 600;
        int height = 400;
        int x = (Screen.width - width) / 2;
        int y = (Screen.height - height) / 2;
        x = 200;
        y = 200;
        vwWindow.position = new Rect(x, y, width, height);
    }    

    string className = "Rigidbody";

    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    Rect windowRect = new Rect(100, 100, 200, 200);

    #region  == EditorWindow Func ===

    void OnEnable()
    {
        Debug.Log("== OnEnable ==");
    }

    void OnDisable()
    {
        Debug.Log("== OnDisable ==");
    }

    void OnGUI()
    {
        Debug.Log("== OnGUI ==");
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


        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();

        BeginWindows();
        windowRect = GUILayout.Window(1, windowRect, DoWindow, "Hi There");
        EndWindows();
    }

    void Update()
    {
        Debug.Log("== Update ==");
    }

    void OnDestroy()
    {
        Debug.Log("== OnDestroy ==");
        isOpenWindowView = false;
        vwWindow = null;
        EditorApplication.update -= OnUpdate;
    }

    void OnInspectorUpdate()
    {
        Debug.Log("== OnInspectorUpdate ==");
    }

    #endregion

    #region  == Self Func ===

    void OnUpdate()
    {

    }

    void DoWindow(int windowid)
    {
        GUILayout.Button("Hi");
        GUI.DragWindow();
    }
    #endregion

}
