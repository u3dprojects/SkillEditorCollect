using UnityEngine;
using System.IO;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class EditorExcelWindow : EditorWindow
{
    private Excel mExcel;
    private ExcelTable mTable;
    private int selectIndex;

    static string showPath = "";

    static public void ShowWindow(string path)
    {
        showPath = path;

        bool isExit = File.Exists(showPath);
        if (isExit)
        {
            EditorExcelWindow window = EditorWindow.GetWindowWithRect<EditorExcelWindow>(new Rect(0, 0, 800, 400));
            window.Show();

            Excel xls = ExcelHelper.LoadExcel(showPath);
            xls.ShowLog();

            window.Show(xls);
        }
        else
        {
            EditorUtility.DisplayDialog("提示","excel文件不存在,path = " + showPath,"确定");
        }
    }


    public void Show(Excel xls)
    {
        mExcel = xls;

        // ChangeType(mExcel);
    }

    public void ChangeType(Excel mExcel)
    {
        for (int i = 0; i < mExcel.Tables.Count; i++)
        {
            mExcel.Tables[i].SetCellTypeColumn(1, ExcelTableCellType.Label);
            mExcel.Tables[i].SetCellTypeColumn(3, ExcelTableCellType.Popup, new List<string>() { "1", "2", "3", "4", "5" });
            mExcel.Tables[i].SetCellTypeRow(1, ExcelTableCellType.Label);
            mExcel.Tables[i].SetCellTypeRow(2, ExcelTableCellType.Label);
        }
    }

    void OnGUI()
    {
        if (mExcel != null)
        {
            EditorDrawHelper.DrawTableTab(mExcel, ref selectIndex);
            mTable = mExcel.Tables[selectIndex];
            EditorDrawHelper.DrawTable(mTable);
            DrawButton();
        }
    }

    public void DrawButton()
    {
        EditorGUILayout.BeginHorizontal();
        EditorDrawHelper.DrawButton("+Row", delegate ()
        {
            mTable.NumberOfRows++;
            Show(mExcel);
        });

        EditorDrawHelper.DrawButton("-Row", delegate ()
        {
            mTable.NumberOfRows--;
            Show(mExcel);
        });

        EditorDrawHelper.DrawButton("+Col", delegate ()
        {
            mTable.NumberOfColumns++;
        });

        EditorDrawHelper.DrawButton("-Col", delegate ()
        {
            mTable.NumberOfColumns--;
        });

        EditorDrawHelper.DrawButton("Save", delegate ()
        {
            string path = showPath;
            ExcelHelper.SaveExcel(mExcel, path);
            EditorUtility.DisplayDialog("Save Success", path, "ok");
        });
        EditorGUILayout.EndHorizontal();
    }
}
