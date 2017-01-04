1 主要针对xlsx，就是2007版本以后的excel
需要设置
PlayerSettings 中 的Optimization 设置 Api Compatibility Level 为.Net2.0
不然就会导致该库文件引用问题如下
Unhandled Exception: System.Reflection.ReflectionTypeLoadException: The classes in the module cannot be loaded.
  at (wrapper managed-to-native) System.Reflection.Assembly:GetTypes (bool)
  at System.Reflection.Assembly.GetTypes () [0x00000] in <filename unknown>:0 
  at Mono.CSharp.RootNamespace.ComputeNamespaces (System.Reflection.Assembly assembly, System.Type extensionType) [0x00000] in <filename unknown>:0 
  at Mono.CSharp.RootNamespace.ComputeNamespace (Mono.CSharp.CompilerContext ctx, System.Type extensionType) [0x00000] in <filename unknown>:0 
  at Mono.CSharp.GlobalRootNamespace.ComputeNamespaces (Mono.CSharp.CompilerContext ctx) [0x00000] in <filename unknown>:0 
  at Mono.CSharp.Driver.LoadReferences () [0x00000] in <filename unknown>:0 
  at Mono.CSharp.Driver.Compile () [0x00000] in <filename unknown>:0 
  at Mono.CSharp.Driver.Main (System.String[] args) [0x00000] in <filename unknown>:0 

The following assembly referenced from E:\_workspaces\u3d5_4_2\SkillEditorCollect\Assets\Scriptes\_Excel\EPPlus\EPPlus\EPPlus.dll could not be loaded:
     Assembly:   System.Drawing    (assemblyref_index=3)
     Version:    2.0.0.0
     Public Key: b03f5f7f11d50a3a
The assembly was not found in the Global Assembly Cache, a path listed in the MONO_PATH environment variable, or in the location of the executing assembly (E:\_workspaces\u3d5_4_2\SkillEditorCollect\Assets\Scriptes\_Excel\EPPlus\EPPlus\).

Could not load file or assembly 'System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.
Could not load file or assembly 'System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.
The class OfficeOpenXml.Style.Dxf.ExcelDxfColor could not be loaded, used in EPPlus, Version=4.0.4.0, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
The class OfficeOpenXml.Style.Dxf.ExcelDxfColor could not be loaded, used in EPPlus, Version=4.0.4.0, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
The class OfficeOpenXml.Style.Dxf.ExcelDxfFontBase could not be loaded, used in EPPlus, Version=4.0.4.0, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
The following assembly referenced from E:\_workspaces\u3d5_4_2\SkillEditorCollect\Assets\Scriptes\_Excel\EPPlus\EPPlus\EPPlus.dll could not be loaded:
     Assembly:   System.Security    (assemblyref_index=6)
     Version:    2.0.0.0
     Public Key: b03f5f7f11d50a3a
The assembly was not found in the Global Assembly Cache, a path listed in the MONO_PATH environment variable, or in the location of the executing assembly (E:\_workspaces\u3d5_4_2\SkillEditorCollect\Assets\Scriptes\_Excel\EPPlus\EPPlus\).

Could not load file or assembly 'System.Security, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a' or one of its dependencies.

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