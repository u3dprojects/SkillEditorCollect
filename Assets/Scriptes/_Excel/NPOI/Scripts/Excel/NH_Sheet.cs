using UnityEngine;
using System.Collections;
using NPOI.HSSF.UserModel;
using System.Collections.Generic;

/// <summary>
/// 类名 : NPOI_Excel_Sheet 实体类
/// 作者 : Canyon
/// 日期 : 2017-01-04 19:40
/// 功能 : 
/// </summary>
[System.Serializable]
public class NH_Sheet {
    public HSSFWorkbook m_wb { get; set; }
    public HSSFSheet m_sheet { get; set; }
    public string sheetName { get; set; }
    public int sheetIndex { get; set; }
    public int maxCol { get; set; }
    public int maxRow { get; set; }

    // 文件路径
    public string pathFile { get; set; }

    // 行数据
    public Dictionary<int, List<NH_SheetCell>> m_dic_row = new Dictionary<int, List<NH_SheetCell>>();
    
    public NH_Sheet(string path, int sheetIndex)
    {
        HSSFWorkbook wb = NPOIHssfEx.ToWorkBook(path);
        HSSFSheet hsheet = NPOIHssfEx.GetSheet(wb, sheetIndex);
        DoInit(wb, hsheet);
        this.pathFile = path;
    }

    public NH_Sheet(string path, string sheetName)
    {
        HSSFWorkbook wb = NPOIHssfEx.ToWorkBook(path);
        HSSFSheet hsheet = NPOIHssfEx.GetSheet(wb, sheetName);
        DoInit(wb, hsheet);
        this.pathFile = path;
    }

    public NH_Sheet(HSSFWorkbook wb, int sheetIndex)
    {
        HSSFSheet hsheet = NPOIHssfEx.GetSheet(wb, sheetIndex);
        DoInit(wb, hsheet);
    }

    public NH_Sheet(HSSFWorkbook wb, string sheetName)
    {
        HSSFSheet hsheet = NPOIHssfEx.GetSheet(wb, sheetName);
        DoInit(wb, hsheet);
    }

    public NH_Sheet(HSSFWorkbook wb, HSSFSheet sheet)
    {
        DoInit(wb, sheet);
    }

    public void DoInit(HSSFWorkbook wb, HSSFSheet sheet)
    {
        DoClear();

        this.m_wb = wb;
        this.m_sheet = sheet;
        this.sheetName = this.m_sheet.SheetName;
        this.sheetIndex = this.m_wb.GetSheetIndex(this.m_sheet);

        OnInit();
    }

    void OnInit()
    {
        if (this.m_sheet.IsActive) {
            this.maxRow = this.m_sheet.LastRowNum + 1;
            this.maxCol = this.m_sheet.GetRow(0).LastCellNum;
            List<NH_SheetCell> tmpList = null;
            NH_SheetCell tmpCell = null;
            bool isHasList = false;
            for (int i = 0; i < this.maxRow; i++)
            {
                isHasList = m_dic_row.ContainsKey(i);
                if (isHasList)
                {
                    tmpList = m_dic_row[i];
                }else
                {
                    tmpList = new List<NH_SheetCell>();
                }

                for(int j = 0; j < this.maxCol; j++)
                {
                    tmpCell = NH_SheetCell.NewCell(this, i, j);
                    tmpList.Add(tmpCell);
                }

                if (!isHasList)
                {
                    m_dic_row.Add(i, tmpList);
                }
            }
        }
    }

    public HSSFRow GetRow(int rowIndex)
    {
        return NPOIHssfEx.GetRow(m_sheet, rowIndex);
    }

    public HSSFCell GetCell(int rowIndex,int columnIndex)
    {
        try
        {
            return NPOIHssfEx.GetCell(m_sheet, rowIndex, columnIndex);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("rIndex = " + rowIndex + ",cIndex = " + columnIndex + "\n" + ex);
        }
        return null;
    }

    public object GetObject(int rowIndex, int columnIndex)
    {
        if (rowIndex < 0 || (rowIndex + 1) > this.maxRow || columnIndex < 0 || (columnIndex + 1) > this.maxCol)
            return null;
        
        HSSFCell cell = GetCell(rowIndex, columnIndex);
        if (cell == null)
            return null;

        NPOI.SS.UserModel.CellType c_type = cell.CellType;
        switch (c_type)
        {
            case NPOI.SS.UserModel.CellType.Boolean:
                return cell.BooleanCellValue;
            case NPOI.SS.UserModel.CellType.Error:
                return cell.ErrorCellValue;
            case NPOI.SS.UserModel.CellType.Formula:
                return cell.CellFormula;
            case NPOI.SS.UserModel.CellType.Numeric:
                return cell.NumericCellValue;
        }
        return cell.StringCellValue;
    }

    public object GetValue(int row, int column)
    {
        return GetObject(row - 1, column - 1);
    }

    public string GetValString(int row,int column)
    {
        object obj = GetValue(row, column);
        if (obj != null)
            return obj.ToString();
        return "";
    }

    // L - Local:表示当前Class中的变量
    public  string ToLString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("NH_Sheet ,sheetName = [").Append(this.sheetName).Append("]");
        builder.Append(",index = ").Append(this.sheetIndex);
        builder.Append(",[maxRow,maxcolumnIndex] = [").Append(this.maxRow).Append( ",").Append(this.maxCol).Append( "]");
        builder.Append(",FilePaht = [").Append(this.pathFile).Append("]");
        return builder.ToString();
    }

    public string ToNString()
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("list = [");
        foreach (KeyValuePair<int, List<NH_SheetCell>> kv in m_dic_row)
        {
            builder.Append("\n\t");
            builder.Append("row_").Append(kv.Key).Append(" = ");
            builder.Append("[");
            foreach (NH_SheetCell cell in kv.Value)
            {
                builder.Append("\n\t\t").Append(cell.ToCVString()).Append("\n"); ;
            }
            builder.Append("\t]").Append("\n");
        }

        builder.Append("]");
        return builder.ToString();
    }

    public override string ToString()
    {
        return ToLString() + "\n" + ToNString();
    }

    public List<NH_SheetCell> GetNRows(int rowIndex)
    {
        if (!m_dic_row.ContainsKey(rowIndex))
            return null;
        return (m_dic_row[rowIndex]);
    }

    public NH_SheetCell GetNCell(int rowIndex,int columnIndex)
    {
        List<NH_SheetCell> rows = GetNRows(rowIndex);
        if (rows == null || rows.Count <= 0)
            return null;
        return rows[columnIndex];
    }
    
    public void DoClear()
    {
        this.m_wb = null;
        this.m_sheet = null;
        this.sheetName = "";
        this.pathFile = "";
        this.sheetIndex = -1;
        this.maxRow = -1;
        this.maxCol = -1;

        m_dic_row.Clear();
    }
}
