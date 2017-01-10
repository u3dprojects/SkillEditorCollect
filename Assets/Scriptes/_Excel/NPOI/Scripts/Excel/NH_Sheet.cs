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

    // 排序对象
    static NH_Sort_Cell _sort = new NH_Sort_Cell();

    public HSSFWorkbook m_wb;
    public HSSFSheet m_sheet;
    public string sheetName;
    public int sheetIndex;
    public int maxCol;
    public int maxRow;

    protected HSSFWorkbook m_preWb;

    // 文件路径
    public string pathFile;

    // 表数据
    public List<NH_SheetCell> m_tableList = new List<NH_SheetCell>();
    
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
            
            NH_SheetCell tmpCell = null;
            for (int i = 0; i < this.maxRow; i++)
            {
                for(int j = 0; j < this.maxCol; j++)
                {
                    tmpCell = NH_SheetCell.NewCell(this, i, j);
                    m_tableList.Add(tmpCell);
                }
            }

            m_tableList.Sort(_sort);
        }
    }

    public HSSFRow GetRow(int rowIndex,bool isNew = false)
    {
        HSSFRow ret = NPOIHssfEx.GetRow(m_sheet, rowIndex);
        if (isNew && ret == null)
        {
            ret = NPOIHssfEx.CreateRow(m_sheet, rowIndex);
        }
        return ret;
    }

    public HSSFCell GetCell(int rowIndex,int columnIndex,bool isNew = false)
    {
        HSSFCell ret = null;
        try
        {
            ret = NPOIHssfEx.GetCell(m_sheet, rowIndex, columnIndex);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("rIndex = " + rowIndex + ",cIndex = " + columnIndex + "\n" + ex);
        }

        if (isNew && ret == null)
        {
            HSSFRow row = GetRow(rowIndex, isNew);
            ret = NPOIHssfEx.CreateCell(row, columnIndex);
        }
        return ret;
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

    public string GetString(int rowIndex, int columnIndex)
    {
        object obj = GetObject(rowIndex, columnIndex);
        if (obj != null)
            return obj.ToString();
        return "";
    }

    public int GetInt(int rowIndex, int columnIndex)
    {
        object obj = GetObject(rowIndex, columnIndex);
        try
        {
            if (obj != null)
                return int.Parse(obj.ToString());
        }
        catch (System.Exception)
        {
        }
        return 0;
    }

    public float GetFloat(int rowIndex, int columnIndex)
    {
        object obj = GetObject(rowIndex, columnIndex);
        try
        {
            if (obj != null)
                return float.Parse(obj.ToString());
        }
        catch (System.Exception)
        {
        }
        return 0.0f;
    }

    public double GetDouble(int rowIndex, int columnIndex)
    {
        object obj = GetObject(rowIndex, columnIndex);
        try
        {
            if (obj != null)
                return double.Parse(obj.ToString());
        }
        catch (System.Exception)
        {
        }
        return 0.0d;
    }

    #region === 保存 ===

    public void SaveValue(int rowIndex, int columnIndex, string val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(val);
        cell.SetAsActiveCell();
    }

    public void SaveValue(int rowIndex, int columnIndex, int val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(val);
        cell.SetAsActiveCell();
    }

    public void SaveValue(int rowIndex, int columnIndex, float val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(NPOIEx.Round2D(val,2));
        cell.SetAsActiveCell();
    }

    public void SaveValue(int rowIndex, int columnIndex, double val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(val);
        cell.SetAsActiveCell();
    }

    public void SaveValue(int rowIndex,int columnIndex,bool val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(val);
        cell.SetAsActiveCell();
    }

    public void SaveValue(int rowIndex, int columnIndex, System.DateTime val)
    {
        HSSFCell cell = GetCell(rowIndex, columnIndex, true);
        cell.SetCellValue(val);
        cell.SetAsActiveCell();
    }

    public void SaveValueToExcel(int rowIndex, int columnIndex,object val)
    {
        if (val == null)
            return;
        if(val is bool)
        {
            SaveValue(rowIndex, columnIndex, (bool)val);
        }else if (val is int)
        {
            SaveValue(rowIndex, columnIndex, (int)val);
        }else if (val is float)
        {
            SaveValue(rowIndex, columnIndex, (float)val);
        }else if (val is double)
        {
            SaveValue(rowIndex, columnIndex, (double)val);
        }else if (val is System.DateTime)
        {
            SaveValue(rowIndex, columnIndex, (System.DateTime)val);
        }
        else
        {
            SaveValue(rowIndex, columnIndex,val.ToString());
        }
    }

    public void SaveValueToExcel(NH_SheetCell one)
    {
        if (one == null)
            return;
        SaveValueToExcel(one.rowIndex, one.columnIndex, one.val);
    }

    public void SaveValueToCache(int rowIndex, int columnIndex, object val)
    {
        if (val == null)
            return;
        
        NH_SheetCell ncell = GetNCell(rowIndex, columnIndex);
        if(ncell == null)
        {
            ncell = NH_SheetCell.NewCell(this, rowIndex, columnIndex);
            m_tableList.Add(ncell);
        }
        ncell.val = val;
    }

    #endregion

    public HSSFWorkbook ToWorkbook(bool isNew = false)
    {
        this.m_preWb = this.m_wb;
        if (isNew)
        {
            this.m_wb = new HSSFWorkbook();
            this.m_sheet = NPOIHssfEx.CreateSheet(this.m_wb,this.sheetName);
        }

        int lens = m_tableList.Count;
        for (int i = 0; i < lens; i++)
        {
            SaveValueToExcel(m_tableList[i]);
        }
        
        return this.m_wb;
    }

    // L - Local:表示当前Class中的变量
    public string ToLString()
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
        m_tableList.Sort(_sort);

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append("list = [");
        builder.Append("\n\t");
        int lens = m_tableList.Count;
        for (int i = 0; i < lens; i++)
        {
            builder.Append((m_tableList[i]).ToRCVString());
            builder.Append("\n\t");
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
        if (m_tableList.Count <= 0)
            return null;
        int lens = m_tableList.Count;
        List<NH_SheetCell> ret = new List<NH_SheetCell>();
        NH_SheetCell tmpCell = null;
        for (int i = 0; i < lens; i++)
        {
            tmpCell = m_tableList[i];
            if (tmpCell.rowIndex == rowIndex)
            {
                    ret.Add(tmpCell);
            }
        }
        return ret;
    }

    public NH_SheetCell GetNCell(int rowIndex,int columnIndex)
    {
        if (m_tableList.Count <= 0)
            return null;

        int lens = m_tableList.Count;
        NH_SheetCell tmpCell = null;
        for (int i = 0; i < lens; i++)
        {
            tmpCell = m_tableList[i];
            if (rowIndex == tmpCell.rowIndex && columnIndex == tmpCell.columnIndex)
            {
                return tmpCell;
            }
        }
        return null;
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

        m_tableList.Clear();

        this.m_preWb = null;
    }
}
