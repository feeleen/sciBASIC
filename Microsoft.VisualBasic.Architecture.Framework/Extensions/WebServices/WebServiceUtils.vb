﻿#Region "Microsoft.VisualBasic::36c94de2c06da55ad2e84121a05355a3, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Extensions\WebServices\WebServiceUtils.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports System.Collections.Specialized
Imports System.IO
Imports System.Net
Imports System.Net.Security
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq.Extensions
Imports Microsoft.VisualBasic.Net.Http
Imports Microsoft.VisualBasic.Scripting
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Terminal.Utility
Imports Microsoft.VisualBasic.Text
Imports Microsoft.VisualBasic.Text.HtmlParser

''' <summary>
''' The extension module for web services works.
''' </summary>
'''
<PackageNamespace("Utils.WebServices",
                  Description:="The extension module for web services programming in your scripting.",
                  Category:=APICategories.UtilityTools,
                  Publisher:="<a href=""mailto://xie.guigang@gmail.com"">xie.guigang@gmail.com</a>")>
Public Module WebServiceUtils

    ''' <summary>
    ''' Web protocols enumeration
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Protocols As String() = {"http://", "https://", "ftp://", "sftp://"}

    ''' <summary>
    ''' Determine that is this uri string is a network location?
    ''' (判断这个uri字符串是否是一个网络位置)
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    <Extension> Public Function isURL(url As String) As Boolean
        Return url.InStrAny(Protocols) > -1
    End Function

    ''' <summary>
    ''' Build the request parameters for the HTTP POST
    ''' </summary>
    ''' <param name="dict"></param>
    ''' <returns></returns>
    <ExportAPI("Build.Reqparm",
               Info:="Build the request parameters for the HTTP POST")>
    <Extension> Public Function BuildReqparm(dict As Dictionary(Of String, String)) As Specialized.NameValueCollection
        Dim reqparm As New Specialized.NameValueCollection
        For Each Value As KeyValuePair(Of String, String) In dict
            Call reqparm.Add(Value.Key, Value.Value)
        Next
        Return reqparm
    End Function

    ''' <summary>
    ''' Build the request parameters for the HTTP POST
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    <ExportAPI("Build.Reqparm", Info:="Build the request parameters for the HTTP POST")>
    <Extension>
    Public Function BuildReqparm(data As IEnumerable(Of KeyValuePair(Of String, String))) As Specialized.NameValueCollection
        Dim reqparm As New Specialized.NameValueCollection
        For Each Value As KeyValuePair(Of String, String) In data
            Call reqparm.Add(Value.Key, Value.Value)
        Next
        Return reqparm
    End Function

    Const PortOccupied As String = "Only one usage of each socket address (protocol/network address/port) Is normally permitted"

    ''' <summary>
    ''' Only one usage of each socket address (protocol/network address/port) Is normally permitted
    ''' </summary>
    ''' <param name="ex"></param>
    ''' <returns></returns>
    <Extension> Public Function IsSocketPortOccupied(ex As Exception) As Boolean
        If TypeOf ex Is System.Net.Sockets.SocketException AndAlso
            InStr(ex.ToString, PortOccupied, CompareMethod.Text) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Create a parameter dictionary from the request parameter tokens.
    ''' (请注意，字典的key默认为转换为小写的形式)
    ''' </summary>
    ''' <param name="tokens">
    ''' 元素的个数必须要大于1，因为从url里面解析出来的元素之中第一个元素是url本身，则不再对url做字典解析
    ''' </param>
    ''' <returns>
    ''' ###### 2016-11-21
    ''' 因为post可能会传递数组数据进来，则这个时候就会出现重复的键名，则已经不再适合字典类型了，这里改为返回<see cref="NameValueCollection"/>
    ''' </returns>
    <ExportAPI("CreateDirectory", Info:="Create a parameter dictionary from the request parameter tokens.")>
    <Extension>
    Public Function GenerateDictionary(tokens As String(), Optional lowercase As Boolean = True) As NameValueCollection
        Dim out As New NameValueCollection

        If tokens.IsNullOrEmpty Then
            Return out
        End If
        If tokens.Length = 1 Then  ' 只有url，没有附带的参数，则返回一个空的字典集合
            If InStr(tokens(Scan0), "=") = 0 Then
                Return out
            End If
        End If

        Dim LQuery = (From s As String
                      In tokens
                      Let p As Integer = InStr(s, "="c)
                      Let Key As String = Mid(s, 1, p - 1)
                      Let value = Mid(s, p + 1)
                      Select Key,
                          value).ToArray

        For Each x In LQuery
            Dim name As String = If(lowercase,
                x.Key.ToLower,
                x.Key)
            Call out.Add(name, x.value)
        Next

        Return out
    End Function

    ''' <summary>
    ''' 不像<see cref="PostUrlDataParser(String, Boolean)"/>函数，这个函数不会替换掉转义字符，并且所有的Key都已经被默认转换为小写形式的了
    ''' </summary>
    ''' <param name="argsData">URL parameters</param>
    ''' <returns></returns>
    <ExportAPI("Request.Parser")>
    <Extension> Public Function RequestParser(argsData As String, Optional TransLower As Boolean = True) As NameValueCollection
        Dim Tokens As String() = argsData.Split("&"c)
        Return GenerateDictionary(Tokens, TransLower)
    End Function

    ''' <summary>
    ''' 生成URL请求的参数
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="escaping">是否进行对value部分的字符串数据进行转义</param>
    ''' <returns></returns>
    <Extension> Public Function BuildUrlData(data As IEnumerable(Of KeyValuePair(Of String, String)), Optional escaping As Boolean = False) As String
        Dim __get As Func(Of String, String) = If(escaping,
            AddressOf UrlDecode,
            Function(s) s)
        Dim urlData As String = data _
            .Select(Function(x) $"{x.Key}={__get(x.Value)}").JoinBy("&")
        Return urlData
    End Function

    <ExportAPI("Build.Args")>
    Public Function BuildArgs(ParamArray params As String()()) As String
        If params.IsNullOrEmpty Then
            Return ""
        Else
            Dim values = params.ToArray(Function(arg) $"{arg(Scan0)}={arg(1)}")
            Return String.Join("&", values)
        End If
    End Function

    ''' <summary>
    ''' 在服务器端对URL进行解码还原
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="encoding"></param>
    ''' <returns></returns>
    <Extension> <ExportAPI("URL.Decode")>
    Public Function UrlDecode(s As String, Optional encoding As Encoding = Nothing) As String
        If encoding IsNot Nothing Then
            Return HttpUtility.UrlDecode(s, encoding)
        Else
            Return HttpUtility.UrlDecode(s)
        End If
    End Function

    <ExportAPI("URL.Decode")>
    Public Sub UrlDecode(s As String, ByRef output As TextWriter)
        If s IsNot Nothing Then
            output.Write(UrlDecode(s))
        End If
    End Sub

    ''' <summary>
    ''' 进行url编码，将特殊字符进行转码
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="encoding"></param>
    ''' <returns></returns>
    <ExportAPI("URL.Encode")>
    <Extension>
    Public Function UrlEncode(s As String, Optional encoding As Encoding = Nothing) As String
        If encoding IsNot Nothing Then
            Return HttpUtility.UrlEncode(s, encoding)
        Else
            Return HttpUtility.UrlEncode(s)
        End If
    End Function

    <ExportAPI("URL.Encode")>
    Public Sub UrlEncode(s As String, ByRef output As TextWriter)
        If s IsNot Nothing Then
            output.Write(UrlEncode(s))
        End If
    End Sub

    ''' <summary>
    ''' 编码整个URL
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    '''
    <ExportAPI("URL.PathEncode")>
    <Extension>
    Public Function UrlPathEncode(s As String) As String
        If s Is Nothing Then
            Return Nothing
        End If

        Dim idx As Integer = s.IndexOf("?"c)
        Dim s2 As String = Nothing
        If idx <> -1 Then
            s2 = s.Substring(0, idx)
            s2 = HttpUtility.UrlEncode(s2) & s.Substring(idx)
        Else
            s2 = HttpUtility.UrlEncode(s)
        End If

        Return s2
    End Function

    ''' <summary>
    ''' 假若你的数据之中包含有SHA256的加密数据，则非常不推荐使用这个函数进行解析。因为请注意，这个函数会替换掉一些转义字符的，所以会造成一些非常隐蔽的BUG
    ''' </summary>
    ''' <param name="data">转义的时候大小写无关</param>
    ''' <returns></returns>
    '''
    <ExportAPI("PostRequest.Parsing")>
    <Extension> Public Function PostUrlDataParser(data As String, Optional TransLower As Boolean = True) As NameValueCollection
        If String.IsNullOrEmpty(data) Then
            Return New NameValueCollection
        End If

        Dim Tokens As String() = data.UrlDecode.Split("&"c)
        Dim hash = GenerateDictionary(Tokens, TransLower)
        Return hash
    End Function

    ''' <summary>
    ''' Download stream data from the http response.
    ''' </summary>
    ''' <param name="stream">
    ''' Create from <see cref="WebServiceUtils.GetRequestRaw(String, Boolean, String)"/>
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("Stream.Copy", Info:="Download stream data from the http response.")>
    <Extension> Public Function CopyStream(stream As Stream) As Byte()
        If stream Is Nothing Then
            Return New Byte() {}
        End If

        Dim stmMemory As MemoryStream = New MemoryStream()
        Dim buffer As Byte() = New Byte(64 * 1024) {}
        Dim i As New Value(Of Integer)
        Do While i = stream.Read(buffer, 0, buffer.Length) > 0
            Call stmMemory.Write(buffer, 0, +i)
        Loop
        buffer = stmMemory.ToArray()
        Call stmMemory.Close()
        Return buffer
    End Function

    <ExportAPI("GET", Info:="GET http request")>
    <Extension> Public Function GetRequest(strUrl As String, ParamArray args As String()()) As String
        If args.IsNullOrEmpty Then
            Return GetRequest(strUrl)
        Else
            Dim params As String = BuildArgs(args)
            If String.IsNullOrEmpty(params) Then
                Return GetRequest(strUrl)
            Else
                Return GetRequest($"{strUrl}?{params}")
            End If
        End If
    End Function

    ''' <summary>
    ''' GET http request
    ''' </summary>
    ''' <param name="url"></param>
    ''' <returns></returns>
    <ExportAPI("GET", Info:="GET http request")>
    <Extension> Public Function GetRequest(url As String, Optional https As Boolean = False, Optional userAgent As String = "Microsoft.VisualBasic.[HTTP/GET]") As String
        Dim strData As String = ""
        Dim strValue As New List(Of String)
        Dim Reader As New StreamReader(GetRequestRaw(url, https, userAgent), Encoding.UTF8)

        Do While True
            strData = Reader.ReadLine()
            If strData Is Nothing Then
                Exit Do
            Else
                strValue += strData
            End If
        Loop

        strData = String.Join(vbCrLf, strValue.ToArray)
        Return strData
    End Function

    Sub New()
        ServicePointManager.ServerCertificateValidationCallback =
            New RemoteCertificateValidationCallback(AddressOf CheckValidationResult)
    End Sub

    Private Function CheckValidationResult(sender As Object,
                                           certificate As X509Certificate,
                                           chain As X509Chain,
                                           errors As SslPolicyErrors) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="https"></param>
    ''' <param name="userAgent">
    ''' 
    ''' fix a bug for github API:
    ''' 
    ''' Protocol violation using Github api
    ''' 
    ''' You need to set UserAgent like this:
    ''' webRequest.UserAgent = "YourAppName"
    ''' Otherwise it will give The server committed a protocol violation. Section=ResponseStatusLine Error.
    ''' </param>
    ''' <returns></returns>
    <ExportAPI("GET.Raw", Info:="GET http request")>
    <Extension> Public Function GetRequestRaw(url As String,
                                              Optional https As Boolean = False,
                                              Optional userAgent As String = "Microsoft.VisualBasic.[HTTP/GET]") As Stream
        Dim request As HttpWebRequest
        If https Then
            request = WebRequest.CreateDefault(New Uri(url))
        Else
            request = WebRequest.Create(url).As(Of HttpWebRequest)
        End If

        request.Method = "GET"
        request.KeepAlive = False
        request.ServicePoint.Expect100Continue = False
        request.UserAgent = userAgent

        Dim response As HttpWebResponse =
            request.GetResponse.As(Of HttpWebResponse)
        Dim s As Stream = response.GetResponseStream()
        Return s
    End Function

    <ExportAPI("POST", Info:="POST http request")>
    Public Function PostRequest(url As String, Optional params As IEnumerable(Of KeyValuePair(Of String, String)) = Nothing) As String
        Return url.POST(params.BuildReqparm)
    End Function

    <ExportAPI("POST", Info:="POST http request")>
    Public Function PostRequest(url As String, ParamArray params As String()()) As String
        Dim post As KeyValuePair(Of String, String)()
        If params Is Nothing Then
            post = Nothing
        Else
            post = params.ToArray(Function(value) New KeyValuePair(Of String, String)(value(0), value(1)))
        End If
        Return PostRequest(url, post)
    End Function

    ''' <summary>
    ''' POST http request for get html.
    ''' (请注意，假若<paramref name="params"/>之中含有字符串数组的话，则会出错，这个时候需要使用
    ''' <see cref="Post(String, Dictionary(Of String, String()), String, String, String)"/>方法)
    ''' </summary>
    ''' <param name="url$"></param>
    ''' <param name="params"></param>
    ''' <param name="Referer$"></param>
    ''' <returns></returns>
    <ExportAPI("POST", Info:="POST http request")>
    <Extension> Public Function POST(url$, params As NameValueCollection, Optional Referer$ = "", Optional proxy$ = Nothing) As String
        Using request As New WebClient

            Call request.Headers.Add("User-Agent", UserAgent.GoogleChrome)
            Call request.Headers.Add(NameOf(Referer), Referer)

            If String.IsNullOrEmpty(proxy) Then
                proxy = WebServiceUtils.Proxy
            End If
            If Not String.IsNullOrEmpty(proxy) Then
                Call request.SetProxy(proxy)
            End If

            Call $"[POST] {url}....".__DEBUG_ECHO

            Dim response As Byte() = request.UploadValues(url, "POST", params)
            Dim strData As String = Encoding.UTF8.GetString(response)

            Call $"[GET] {response.Length} bytes...".__DEBUG_ECHO

            Return strData
        End Using
    End Function

    ''' <summary>
    ''' POST http request for get html
    ''' </summary>
    ''' <param name="url$"></param>
    ''' <param name="data"></param>
    ''' <param name="Referer$"></param>
    ''' <returns></returns>
    <ExportAPI("POST", Info:="POST http request")>
    <Extension> Public Function POST(url$, data As Dictionary(Of String, String()),
                                     Optional Referer$ = "",
                                     Optional proxy$ = Nothing,
                                     Optional ua As String = UserAgent.GoogleChrome) As String

        Dim postString As New List(Of String)

        For Each postValue As KeyValuePair(Of String, String()) In data
            postString += postValue.Value _
                .Select(Function(v) postValue.Key & "=" & HttpUtility.UrlEncode(v))
        Next

        Dim postData As String = postString.JoinBy("&")
        Dim request As HttpWebRequest = WebRequest.Create(url).As(Of HttpWebRequest)

        request.Method = "POST"
        request.Accept = "application/json"
        request.ContentLength = postData.Length
        request.ContentType = "application/x-www-form-urlencoded; charset=utf-8"
        request.UserAgent = ua
        request.Referer = Referer

        If Not String.IsNullOrEmpty(proxy) Then
            Call request.SetProxy(proxy)
        End If

        Call $"[POST] {url}....".__DEBUG_ECHO

        ' post data Is sent as a stream
        Using sender As New StreamWriter(request.GetRequestStream())
            sender.Write(postData)
        End Using

        ' returned values are returned as a stream, then read into a string
        Dim response = request.GetResponse().As(Of HttpWebResponse)
        Using responseStream As New StreamReader(response.GetResponseStream())
            Dim html As New StringBuilder
            Dim s As New Value(Of String)

            Do While Not (s = responseStream.ReadLine) Is Nothing
                Call html.AppendLine(+s)
            Loop

            Call $"[GET] {html.Length} bytes...".__DEBUG_ECHO

            Return html.ToString
        End Using
    End Function

#If FRAMEWORD_CORE Then
    ''' <summary>
    ''' Get the html page content from a website request or a html file on the local filesystem.(同时支持http位置或者本地文件，失败或者错误会返回空字符串)
    ''' </summary>
    ''' <param name="url">web http request url or a file path handle</param>
    ''' <param name="retry">发生错误的时候的重试的次数</param>
    ''' <returns>失败或者错误会返回空字符串</returns>
    ''' <remarks></remarks>
    '''
    <ExportAPI("Webpage.Request", Info:="Get the html page content from a website request Or a html file on the local filesystem.")>
    <Extension> Public Function [GET](url As String,
                                      <Parameter("Request.TimeOut")>
                                      Optional retry As UInt16 = 10,
                                      <Parameter("FileSystem.Works?", "Is this a local html document on your filesystem?")>
                                      Optional isFileUrl As Boolean = False,
                                      Optional headers As Dictionary(Of String, String) = Nothing,
                                      Optional proxy As String = Nothing,
                                      Optional doNotRetry404 As Boolean = False,
                                      Optional UA$ = UserAgent.GoogleChrome) As String
#Else
    ''' <summary>
    ''' Get the html page content from a website request or a html file on the local filesystem.
    ''' </summary>
    ''' <param name="url">web http request url or a file path handle</param>
    ''' <param name="RequestTimeOut">发生错误的时候的重试的次数</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''
    <Extension> Public Function Get_PageContent(url As String, Optional RequestTimeOut As UInteger = 20, Optional FileSystemUrl As Boolean = False) As String
#End If
        ' Call $"Request data from: {If(isFileUrl, url.ToFileURL, url)}".__DEBUG_ECHO
        Call $"GET {If(isFileUrl, url.ToFileURL, url)}".__DEBUG_ECHO

        If FileIO.FileSystem.FileExists(url) Then
            Call "[Job DONE!]".__DEBUG_ECHO
            Return FileIO.FileSystem.ReadAllText(url)
        Else
            If isFileUrl Then
                Call $"URL {url.ToFileURL} can not solved on your filesystem!".Warning
                Return ""
            End If
        End If

        Return url.__httpRequest(retry, headers, proxy, doNotRetry404, UA)
    End Function

    ''' <summary>
    ''' Example for xx-net tool:
    ''' 
    ''' ```
    ''' http://127.0.0.1:8087/
    ''' ```
    ''' </summary>
    ''' <returns></returns>
    Public Property Proxy As String

    <Extension>
    Private Function __httpRequest(url$,
                                   retries%,
                                   headers As Dictionary(Of String, String),
                                   proxy As String,
                                   DoNotRetry404 As Boolean,
                                   UA$) As String

        Dim retryTime As Integer = 0

        If String.IsNullOrEmpty(proxy) Then
            proxy = WebServiceUtils.Proxy
        End If

        Try
RETRY:      Return __get(url, headers, proxy, UA)
        Catch ex As Exception
            Dim is404 As Boolean =
                InStr(ex.Message, "(404) Not Found") > 0

            ex = New Exception(url, ex)
            ex.PrintException

            If retryTime < retries Then
                If is404 AndAlso DoNotRetry404 Then
                    Return LogException(url, ex)
                End If

                retryTime += 1
                Call "Data download error, retry connect to the server!".PrintException
                GoTo RETRY
            Else
                Return LogException(url, ex)
            End If
        End Try
    End Function

    Private Function LogException(url As String, ex As Exception) As String
        Dim exMessage As String = String.Format("Unable to get the http request!" & vbCrLf &
                                                "  Url:=[{0}]" & vbCrLf &
                                                "  EXCEPTION ===>" & vbCrLf & ex.ToString, url)
        Call App.LogException(exMessage, NameOf([GET]) & "::HTTP_REQUEST_EXCEPTION")
        Return ""
    End Function

    <Extension>
    Public Sub SetProxy(ByRef request As HttpWebRequest, proxy As String)
        request.Proxy = proxy.GetProxy
    End Sub

    <Extension>
    Public Sub SetProxy(ByRef request As WebClient, proxy As String)
        request.Proxy = proxy.GetProxy
    End Sub

    <Extension>
    Public Function GetProxy(proxy As String) As WebProxy
        Return New WebProxy With {
            .Address = New Uri(proxy),
            .Credentials = New NetworkCredential()
        }
    End Function

    Private Function __get(url$, headers As Dictionary(Of String, String), proxy$, UA$) As String
        Dim timer As Stopwatch = Stopwatch.StartNew
        Dim webRequest As HttpWebRequest = HttpWebRequest.Create(url)

        webRequest.Headers.Add("Accept-Language", "en-US,en;q=0.8,zh-Hans-CN;q=0.5,zh-Hans;q=0.3")
        webRequest.UserAgent = UserAgent.GoogleChrome

        If Not headers.IsNullOrEmpty Then
            For Each x In headers
                webRequest.Headers(x.Key) = x.Value
            Next
        End If
        If Not String.IsNullOrEmpty(proxy) Then
            Call webRequest.SetProxy(proxy)
        End If

        Using respStream As Stream = webRequest.GetResponse.GetResponseStream,
            reader As New StreamReader(respStream)

            Dim html As String = reader.ReadToEnd
            Dim title As String = html.HTMLTitle

            If InStr(html, "http://www.doctorcom.com") > 0 Then
                Return ""
            End If

            Call $"[{title}  {url}] --> sizeOf:={Len(html)} chars; response_time:={timer.ElapsedMilliseconds} ms.".__DEBUG_ECHO
#If DEBUG Then
            Call html.SaveTo($"{App.AppSystemTemp}/{App.PID}/{url.NormalizePathString}.html")
#End If
            Return html
        End Using
    End Function

#If FRAMEWORD_CORE Then
    ''' <summary>
    ''' download the file from <paramref name="strUrl"></paramref> to <paramref name="save">local file</paramref>.
    ''' </summary>
    ''' <param name="strUrl"></param>
    ''' <param name="save">The file path of the file saved</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <ExportAPI("wget", Info:="Download data from the specific URL location.")>
    <Extension> Public Function DownloadFile(<Parameter("url")> strUrl As String,
                                             <Parameter("Path.Save", "The saved location of the downloaded file data.")>
                                             save As String,
                                             Optional proxy As String = Nothing,
                                             Optional ua As String = UserAgent.FireFox,
                                             Optional retry As Integer = 10) As Boolean
#Else
    ''' <summary>
    ''' download the file from <paramref name="strUrl"></paramref> to <paramref name="SavedPath">local file</paramref>.
    ''' </summary>
    ''' <param name="strUrl"></param>
    ''' <param name="SavedPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension> Public Function DownloadFile(strUrl As String, SavedPath As String) As Boolean
#End If
RE0:
        Try
            Using dwl As New WebClient()
                If Not String.IsNullOrEmpty(proxy) Then
                    Call dwl.SetProxy(proxy)
                End If

                Call dwl.Headers.Add(UserAgent.UAheader, ua)
                Call save.ParentPath.MkDIR
                Call $"{strUrl} --> {save}".__DEBUG_ECHO
                Call dwl.DownloadFile(strUrl, save)
            End Using
            Return True
        Catch ex As Exception
            Dim trace As String = MethodBase.GetCurrentMethod.GetFullName

            Call App.LogException(
                New Exception(strUrl, ex),
                trace)
            Call ex.PrintException

            If retry > 0 Then
                retry -= 1
                GoTo RE0
            Else

            End If

            Return False
        Finally
            If save.FileExists Then
                Call $"[{FileIO.FileSystem.GetFileInfo(save).Length} Bytes]".__DEBUG_ECHO
            Else
                Call $"Download failure!".__DEBUG_ECHO
            End If
        End Try
    End Function

    ''' <summary>
    ''' 使用GET方法下载文件
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="savePath"></param>
    ''' <returns></returns>
    '''
    <ExportAPI("GET.Download", Info:="Download file from http request and save to a specific location.")>
    <Extension> Public Function GetDownload(url As String, savePath As String) As Boolean
        Try
            Dim responseStream As Stream = GetRequestRaw(url)
            Dim buffer As Byte() = responseStream.CopyStream
            Call $"[{buffer.Length} Bytes]".__DEBUG_ECHO
            Return buffer.FlushStream(savePath)
        Catch ex As Exception
            ex = New Exception(url, ex)
            Call ex.PrintException
            Call App.LogException(ex)
            Return False
        End Try
    End Function

    Public Const IPAddress As String = "http://ipaddress.com/"
    ''' <summary>
    ''' Microsoft DNS Server
    ''' </summary>
    Public Const MicrosoftDNS As String = "4.2.2.1"

    ''' <summary>
    ''' 获取我的公网IP地址，假若没有连接互联网的话则会返回局域网IP地址
    ''' </summary>
    ''' <returns></returns>
    Public Function GetMyIPAddress() As String
        Dim hasInternet As Boolean

        Try
            hasInternet = Not Net.PingUtility.Ping(System.Net.IPAddress.Parse(MicrosoftDNS)) > Integer.MaxValue
        Catch ex As Exception
            hasInternet = False
        End Try

        If hasInternet Then
            Return __getMyIPAddress()   'IPAddress on Internet
        Else
            Return Net.AsynInvoke.LocalIPAddress  'IPAddress in LAN
        End If
    End Function

    Public Const RegexIPAddress As String = "\d{1,3}(\.\d{1,3}){3}"

    Private Function __getMyIPAddress() As String
        Dim page As String = IPAddress.GET
        Dim ipResult As String = Regex.Match(page, $"IP[:] {RegexIPAddress}<br><img", RegexOptions.IgnoreCase).Value
        ipResult = Regex.Match(ipResult, RegexIPAddress).Value
        Return ipResult
    End Function
End Module
