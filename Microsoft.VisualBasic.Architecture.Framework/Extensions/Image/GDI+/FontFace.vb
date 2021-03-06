﻿#Region "Microsoft.VisualBasic::ce919a34e097f9d00f90cc674f0ca2f4, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Extensions\Image\GDI+\FontFace.vb"

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

Imports System.Drawing
Imports System.Drawing.Text

Namespace Imaging

    ''' <summary>
    ''' Font names collection
    ''' </summary>
    Public Module FontFace

        ''' <summary>
        ''' 微软雅黑字体的名称
        ''' </summary>
        Public Const MicrosoftYaHei As String = "Microsoft YaHei"
        Public Const MicrosoftYaHeiUI As String = "Microsoft YaHei UI"
        Public Const Ubuntu As String = "Ubuntu"
        Public Const SegoeUI As String = "Segoe UI"
        Public Const Arial As String = "Arial"
        Public Const BookmanOldStyle As String = "Bookman Old Style"
        Public Const Calibri As String = "Calibri"
        Public Const Cambria As String = "Cambria"
        Public Const CambriaMath As String = "Cambria Math"
        Public Const Consolas As String = "Consolas"
        Public Const CourierNew As String = "Courier New"
        Public Const NSimSun As String = "NSimSun"
        Public Const SimSun As String = "SimSun"
        Public Const Verdana As String = "Verdana"
        Public Const Tahoma As String = "Tahoma"
        Public Const TimesNewRoman As String = "Times New Roman"

        Public ReadOnly Property InstalledFontFamilies As IReadOnlyCollection(Of String)

        ReadOnly __fontFamilies As Dictionary(Of String, String)

        Sub New()
            Dim fontFamilies() As FontFamily
            Dim installedFontCollection As New InstalledFontCollection()

            ' Get the array of FontFamily objects.
            fontFamilies = installedFontCollection.Families
            InstalledFontFamilies = fontFamilies.Select(Function(f) f.Name).ToArray
            __fontFamilies = New Dictionary(Of String, String)

            For Each family$ In InstalledFontFamilies
                __fontFamilies(LCase(family)) = family
            Next
        End Sub

        ''' <summary>
        ''' 由于字体名称的大小写敏感，所以假若是html css之类的渲染的话，由于可能会是小写的字体名称会导致无法正确的加载所需要的字体，所以可以使用这个函数来消除这种由于大小写敏感而带来的bug
        ''' </summary>
        ''' <param name="name$"></param>
        ''' <param name="default">默认使用Windows10的默认字体</param>
        ''' <returns></returns>
        Public Function GetFontName(name$, Optional default$ = FontFace.SegoeUI) As String
            If __fontFamilies.ContainsKey(name) Then
                Return __fontFamilies(name)
            Else
                name = LCase(name)

                If __fontFamilies.ContainsKey(name) Then
                    Return __fontFamilies(name)
                Else
                    Return [default]
                End If
            End If
        End Function
    End Module
End Namespace
