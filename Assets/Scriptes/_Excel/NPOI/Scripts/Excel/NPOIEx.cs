using UnityEngine;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

/// <summary>
/// 类名 : NPOI_HSSF 工具
/// 作者 : Canyon
/// 日期 : 2017-01-04 16:10
/// 功能 : 
/// </summary>
public class NPOIEx
{

    static public float Round2F(float org, int acc)
    {
        float pow = Mathf.Pow(10, acc);
        float temp = org * pow;
        return Mathf.RoundToInt(temp) / pow;
    }

    static public double Round2D(float org, int acc)
    {
        double pow = Mathf.Pow(10, acc);
        double temp = org * pow;
        return Mathf.RoundToInt((float)temp) / pow;
    }

    static public Stream ToStream(string FileName)
    {
        FileInfo fi = new FileInfo(FileName);
        if (fi.Exists)
        {
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            return fs;
        }
        return null;
    }

    static public Stream ToStream(HSSFWorkbook InWorkbook)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            InWorkbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }
    }

    static public void ToFile(byte[] data, string FileName)
    {
        FileStream fs = new FileStream(FileName, FileMode.Create, FileAccess.Write);
        fs.Write(data, 0, data.Length);
        fs.Flush();
        fs.Close();
        data = null;
        fs = null;
    }

    static public void ToFile(MemoryStream ms, string FileName)
    {
        ToFile(ms.ToArray(), FileName);
        ms = null;
    }

    static public void ToFile(HSSFWorkbook workbook, string FileName)
    {
        MemoryStream ms = ToStream(workbook) as MemoryStream;
        ToFile(ms, FileName);
        ms = null;
    }
    
    static public HSSFWorkbook ToWorkBook(Stream InStream)
    {
        return new HSSFWorkbook(InStream);
    }
    
    static public HSSFWorkbook ToWorkBook(string FileName)
    {
        Stream _stream = ToStream(FileName);
        if (_stream == null)
            return null;
        return ToWorkBook(_stream);
    }

    // 取得数量
    static public int SheetNum(HSSFWorkbook wb)
    {
        return wb.NumberOfSheets;
    }

    static public ISheet GetISheet(HSSFWorkbook wb,string sheetName)
    {
        return wb.GetSheet(sheetName);
    }

    static public ISheet GetISheet(HSSFWorkbook wb, int sheetIndex)
    {
        return wb.GetSheetAt(sheetIndex);
    }

    static public IRow GetIRow(ISheet sheet,int rowIndex)
    {
        return sheet.GetRow(rowIndex);
    }

    static public IRow GetIRow(HSSFWorkbook wb, int sheetIndex, int rowIndex)
    {
        return GetISheet(wb,sheetIndex).GetRow(rowIndex);
    }

    static  public ICell GetICell(IRow row,int columnIndex)
    {
        return row.GetCell(columnIndex);
    }

    static public ICell GetICell(HSSFWorkbook wb, int sheetIndex, int rowIndex, int columnIndex)
    {
        return GetIRow(wb, sheetIndex,rowIndex).GetCell(rowIndex);
    }

    static public int GetColumnWidth(ISheet sheet,int columnIndex)
    {
        
        return sheet.GetColumnWidth(columnIndex);
    }

    // 备注
    static public IComment GetIComment(ISheet  sheet,int rowIndex, int columnIndex)
    {
        return sheet.GetCellComment(rowIndex, columnIndex);
    }

    #region === 创建 ===

    static public ISheet CreateISheet(HSSFWorkbook wb)
    {
        return wb.CreateSheet();
    }

    static public ISheet CreateISheet(HSSFWorkbook wb, string sheetName)
    {
        return wb.CreateSheet(sheetName);
    }

    static public IRow CreateIRow(ISheet sheet, int rowIndex)
    {
        return sheet.CreateRow(rowIndex);
    }

    static public ICell CreateICell(IRow row, int columnIndex)
    {
        return row.CreateCell(columnIndex);
    }
    
    #endregion
}
