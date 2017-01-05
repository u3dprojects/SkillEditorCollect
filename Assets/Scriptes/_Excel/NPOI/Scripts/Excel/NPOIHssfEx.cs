using UnityEngine;
using System.Collections;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

/// <summary>
/// 类名 : NPOI_HSSF 工具
/// 作者 : Canyon
/// 日期 : 2017-01-04 15:10
/// 功能 : 
/// </summary>
public class NPOIHssfEx : NPOIEx
{
    static public HSSFSheet GetSheet(HSSFWorkbook wb, string sheetName)
    {
        return GetISheet(wb, sheetName) as HSSFSheet;
    }

    static public HSSFSheet GetSheet(HSSFWorkbook wb, int sheetIndex)
    {
        return GetISheet(wb, sheetIndex) as HSSFSheet;
    }

    static public HSSFRow GetRow(HSSFSheet sheet, int rowIndex)
    {
        return GetIRow(sheet, rowIndex) as HSSFRow;
    }

    static public HSSFCell GetCell(HSSFRow row, int columnIndex)
    {
        return GetICell(row,columnIndex) as HSSFCell;
    }

    static public HSSFCell GetCell(HSSFSheet sheet, int rowIndex,int columnIndex)
    {
        return GetCell(GetRow(sheet,rowIndex),columnIndex);
    }
}
