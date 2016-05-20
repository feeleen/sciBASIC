#Easy Document in VisualBasic

##INI
There are two Win32 API was used for ini profile file data read and write:

    ''' <summary>
    ''' Write a string value into a specific section in a specifc ini profile.(在初始化文件指定小节内设置一个字串)
    ''' </summary>
    ''' <param name="section">
    ''' <see cref="String"/>，要在其中写入新字串的小节名称。这个字串不区分大小写
    ''' </param>
    ''' <param name="key">
    ''' <see cref="String"/>，要设置的项名或条目名。这个字串不区分大小写。
    ''' 用<see cref="vbNullString"/>可删除这个小节的所有设置项
    ''' </param>
    ''' <param name="val">
    ''' <see cref="String"/>，指定为这个项写入的字串值。用<see cref="vbNullString"/>表示删除这个项现有的字串
    ''' </param>
    ''' <param name="filePath">
    ''' <see cref="String"/>，初始化文件的名字。如果没有指定完整路径名，则windows会在windows目录查找文件。
    ''' 如果文件没有找到，则函数会创建它</param>
    ''' <returns>Long，非零表示成功，零表示失败。会设置<see cref="GetLastErrorAPI.GetLastError()"/></returns>
    <DllImport("kernel32")>
    Public Shared Function WritePrivateProfileString(section As String,
                                                     key As String,
                                                     val As String,
                                                     filePath As String) As Long
    End Function

    ''' <summary>
    ''' 为初始化文件中指定的条目取得字串
    ''' </summary>
    ''' <param name="section">
    ''' String，欲在其中查找条目的小节名称。这个字串不区分大小写。如设为vbNullString，就在lpReturnedString
    ''' 缓冲区内装载这个ini文件所有小节的列表。
    ''' </param>
    ''' <param name="key">
    ''' String，欲获取的项名或条目名。这个字串不区分大小写。如设为vbNullString，就在lpReturnedString
    ''' 缓冲区内装载指定小节所有项的列表
    ''' </param>
    ''' <param name="def">String，指定的条目没有找到时返回的默认值。可设为空（""）</param>
    ''' <param name="retVal">String，指定一个字串缓冲区，长度至少为nSize</param>
    ''' <param name="size">Long，指定装载到lpReturnedString缓冲区的最大字符数量</param>
    ''' <param name="filePath">
    ''' String，初始化文件的名字。如没有指定一个完整路径名，windows就在Windows目录中查找文件
    ''' </param>
    ''' <returns>
    ''' Long，复制到lpReturnedString缓冲区的字节数量，其中不包括那些NULL中止字符。如lpReturnedString
    ''' 缓冲区不够大，不能容下全部信息，就返回nSize-1（若lpApplicationName或lpKeyName为NULL，则返回nSize-2）
    ''' </returns>
    <DllImport("kernel32")>
    Public Shared Function GetPrivateProfileString(section As String,
                                                   key As String,
                                                   def As String,
                                                   retVal As StringBuilder,
                                                   size As Integer,
                                                   filePath As String) As Integer
    End Function

And the wrapper for the ini data serialization and deserialization is already been developed for the Class object in the VisualBasic. let's see how simple it is:



##Structure Binary

##JSON



##XML
##Csv


All of the example test code can be download from [here](https://github.com/xieguigang/VisualBasic_AppFramework/tree/master/Example/EasyDocument)