using UnityEngine;
using System.Collections;

/// <summary>
/// 类名 : NPOI_Excel_Sheet_Cell 实体类
/// 作者 : Canyon
/// 日期 : 2017-01-04 19:30
/// 功能 : 
/// </summary>
[System.Serializable]
public class NH_SheetCell {

    public int rowIndex { get; set; }
    public int columnIndex { get; set; }
    public object val { get; set; }

    public NH_Sheet m_sheet { get; set; }

    public string val2Str{
        get
        {
            if (val != null)
                return val.ToString();
            return "";
        }
    }

    public NH_SheetCell(NH_Sheet nsheet, int rIndex, int cIndex)
    {
        object val = nsheet.GetObject(rIndex, cIndex);
        DoInit(nsheet, rIndex, cIndex, val);
    }
    
    public void DoInit(NH_Sheet nsheet, int rIndex,int cIndex,object val)
    {
        this.m_sheet = nsheet;
        this.rowIndex = rIndex;
        this.columnIndex = cIndex;
        this.val = val;
    }

    // L-Local:本地属性变量
    public string ToLString()
    {
        return "NH_SheetCell,sheetName = [" + this.m_sheet.sheetName + "]," + ToRCVString();
    }

    // R-RowIndex,C-ColumnIndex,V-Value
    public string ToRCVString()
    {
        return "[R,C,V] = [" + this.rowIndex + "," + this.columnIndex + "," + this.val + "]";
    }

    // C-ColumnIndex,V-Value
    public string ToCVString()
    {
        return "[C,V] = " + "[" + this.columnIndex + "," + this.val + "]";
    }

    public override string ToString()
    {
        return ToLString();
    }

    static public NH_SheetCell NewCell(NH_Sheet nsheet, int rIndex, int cIndex)
    {
        return new NH_SheetCell(nsheet, rIndex, cIndex);
    }
}

// 按照时间进度排序
public class NH_Sort_Cell : System.Collections.Generic.IComparer<NH_SheetCell>
{
    public int Compare(NH_SheetCell x, NH_SheetCell y)
    {
        if (x.rowIndex == y.rowIndex)
            return x.columnIndex < y.columnIndex ? -1 : 1;
        return x.rowIndex < y.rowIndex ? -1 : 1;
    }
}