using UnityEngine;
using System.Collections;

using NPOI.HSSF.UserModel;
using System.IO;

/// <summary>
/// 测试脚本
/// </summary>
public class ER_Test : MonoBehaviour {
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [ContextMenu("Test")]
    void TestFunc()
    {
        string path = Application.dataPath + Path.DirectorySeparatorChar + "Xls" + Path.DirectorySeparatorChar + "Monster.xls";
        NH_Sheet t_sheet = new NH_Sheet(path,0);
        Debug.Log( t_sheet.ToString());
        Debug.Log(t_sheet.GetString(7, 12));
        Debug.Log(t_sheet.GetObject(6, 11));
    }

    [ContextMenu("ChooseFile")]
    void ChooseFile()
    {
        string pathOpen = UnityEditor.EditorUtility.OpenFilePanel("选取excel文件", "", "xls");
        if (string.IsNullOrEmpty(pathOpen))
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "The path is Empty", "Okey");
            return;
        }
        NH_Sheet m_sheet = new NH_Sheet(pathOpen, 0);
        Debug.Log(m_sheet.ToString());
        Debug.Log(m_sheet.GetRow(1000) == null);
    }

    [ContextMenu("SaveFile")]
    void SaveFile()
    {
        string pathOpen = UnityEditor.EditorUtility.OpenFilePanel("选取原excel文件", "", "xls");
        if (string.IsNullOrEmpty(pathOpen))
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "The path is Empty", "Okey");
            return;
        }
        NH_Sheet m_sheet = new NH_Sheet(pathOpen, 0);


        string folder = Path.GetDirectoryName(pathOpen);
        string fileName = Path.GetFileNameWithoutExtension(pathOpen);
        string suffix = Path.GetExtension(pathOpen);
        suffix = suffix.Replace(".", "");

        string savePath = UnityEditor.EditorUtility.SaveFilePanel("保存文件", folder, fileName, suffix);

        if (string.IsNullOrEmpty(savePath))
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "The path is Empty", "Okey");
            return;
        }

        if (m_sheet == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "No Sheet", "Okey");
            return;
        }

        if (m_sheet.m_wb == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "No WorkBook", "Okey");
            return;
        }

        NPOIHssfEx.ToFile(m_sheet.m_wb, savePath);
    }


    [ContextMenu("SaveFileToNew")]
    void SaveFileToNew()
    {

        string pathOpen = UnityEditor.EditorUtility.OpenFilePanel("选取原excel文件", "", "xls");
        if (string.IsNullOrEmpty(pathOpen))
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "The path is Empty", "Okey");
            return;
        }
        NH_Sheet m_sheet = new NH_Sheet(pathOpen, 0);


        string folder = Path.GetDirectoryName(pathOpen);
        string fileName = Path.GetFileNameWithoutExtension(pathOpen);
        string suffix = Path.GetExtension(pathOpen);
        suffix = suffix.Replace(".", "");

        string savePath = UnityEditor.EditorUtility.SaveFilePanel("保存文件", folder, fileName, suffix);

        if (string.IsNullOrEmpty(savePath))
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "The path is Empty", "Okey");
            return;
        }

        if (m_sheet == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "No Sheet", "Okey");
            return;
        }

        if (m_sheet.m_wb == null)
        {
            UnityEditor.EditorUtility.DisplayDialog("Tips", "No WorkBook", "Okey");
            return;
        }

        NPOIHssfEx.ToFile(m_sheet.ToWorkbook(true), savePath);
    }
}
