﻿#Region "Microsoft.VisualBasic::8fbe5e27bcc6f3a9d04425788a7e6e33, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Extensions\IO\Extensions.vb"

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

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.Text

Namespace FileIO

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' <see cref="Encoding"/>会和<see cref="Encodings"/>产生冲突，
    ''' 使用这个单独的拓展模块，但是位于不同的命名空间来解决这个问题。
    ''' </remarks>
    Public Module Extensions

        ''' <summary>
        ''' Write all object into a text file by using its <see cref="Object.ToString"/> method.
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="data"></param>
        ''' <param name="saveTo"></param>
        ''' <param name="encoding"></param>
        ''' <returns></returns>
        <Extension> Public Function FlushAllLines(Of T)(data As IEnumerable(Of T), saveTo$, Optional encoding As Encoding = Nothing) As Boolean
            Dim strings As IEnumerable(Of String) =
                data.Select(AddressOf Scripting.ToString)
            Dim parent$ = FileSystem.GetParentPath(saveTo)

            Call parent.MkDIR

            If encoding Is Nothing Then
                encoding = Encoding.Default
            End If

            Try
                Using writer As StreamWriter = saveTo.OpenWriter(encoding,)
                    For Each line As String In strings
                        Call writer.WriteLine(line)
                    Next
                End Using
            Catch ex As Exception
                Call App.LogException(New Exception(saveTo, ex))
                Return False
            End Try

            Return True
        End Function

        ''' <summary>
        ''' Open text file writer, this function will auto handle all things.
        ''' </summary>
        ''' <param name="path">假若路径是指向一个已经存在的文件，则原有的文件数据将会被清空</param>
        ''' <param name="encoding"></param>
        ''' <returns></returns>
        <Extension>
        Public Function OpenWriter(path As String, Optional encoding As Encoding = Nothing, Optional newLine As String = vbLf) As StreamWriter
            Call "".SaveTo(path)

            Dim file As New FileStream(path, FileMode.OpenOrCreate)
            Dim writer As New StreamWriter(file, encoding) With {
                .NewLine =
                If(newLine Is Nothing OrElse newLine.Length = 0,
                vbLf,
                newLine)
            }

            Return writer
        End Function
    End Module
End Namespace
