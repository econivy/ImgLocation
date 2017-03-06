using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("干部任免电子呈阅系统")]
[assembly: AssemblyDescription("干部任免电子呈阅系统 数据转换工具\r\n------\r\n"
    + "1.2.6    修正部分错误。\r\n------\r\n"
    + "1.2.7   修正消息提醒模式，新增无人值守转换模式。\r\n------\r\n"
    + "1.2.8   重构新的重名干部提醒模式，新增手工选择重名干部任免表和考察材料的提醒对话框。\r\n------\r\n"
    + "1.2.9   优化段内搜索模式，跳过部分不含干部的段落，提高搜索效率。\r\n------\r\n"
    + "1.3.1   优化数据库结构，删除DI对象和部分冗余数据，不再生成和存储合成图像。\r\n------\r\n"
    + "1.3.2   优化部分对象属性，减少计算量和数据库操作，提高转换效率。\r\n------\r\n"
    + "1.3.3   实现文档直接替换指定页面功能。\r\n------\r\n"
    + "1.4.1   优化引用库关系，不需要主机配置SQLite环境。\r\n------\r\n"
    + "1.4.2   修正部分错误。\r\n------\r\n"
    //+ "2.0.1   使用XPS替换PDF作为图像的转换机制，优化引用库关系，不需要主机配置Adobe Acrobat环境。\r\n------\r\n"
    //+ "3.0.1   使用OpenXML生成干部任免审批表，大幅提高转换效率。\r\n------\r\n"
    )]
[assembly: AssemblyConfiguration("中共山东省委组织部")]
[assembly: AssemblyCompany("中共山东省委组织部")]
[assembly: AssemblyProduct("数据转换工具 ImgLocation")]
[assembly: AssemblyCopyright("Copyright © 2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]
// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("b0c30308-4c3a-46dc-8824-ddde7a364f49")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("1.0.0.*")]
[assembly: AssemblyFileVersion("1.0.0.*")]
