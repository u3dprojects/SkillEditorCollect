1 主要针对xlsx，就是2007版本以后的excel

// 创建
string outputPath = pathXls;
FileEx.createFolder(outputPath);

Excel xls = new Excel();
ExcelTable table = new ExcelTable();
table.TableName = "test";
xls.Tables.Add(table);
xls.Tables[0].SetValue(1, 1, "1");
xls.Tables[0].SetValue(1, 2, "2");
xls.Tables[0].SetValue(2, 1, "3");
xls.Tables[0].SetValue(2, 2, "4");
xls.ShowLog();
ExcelHelper.SaveExcel(xls, outputPath);

// 读取
Excel xls = ExcelHelper.LoadExcel(showPath);

// 显示
EditorExcelWindow.ShowWindow(pathXlsx);