﻿#Region "Microsoft.VisualBasic::1fceeb3bc864fccc89f8ebce01d63f7d, ..\sciBASIC#\Data_science\Mathematical\Plots\csv\SerialData.vb"

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
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.base
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Mathematical.Interpolation
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace csv

    Public Class SerialData

        ''' <summary>
        ''' 系列的名称
        ''' </summary>
        ''' <returns></returns>
        Public Property serial As String
        Public Property X As Single
        Public Property Y As Single
        Public Property value As Double
        Public Property tag As String
        Public Property errPlus As Double
        Public Property errMinus As Double
        Public Property Statics As Double()

        Public Shared Function GetData(csv$, Optional colors As Color() = Nothing, Optional lineWidth! = 2) As IEnumerable(Of ChartPlots.SerialData)
            Return GetData(csv.LoadCsv(Of SerialData), colors, lineWidth)
        End Function

        Public Shared Iterator Function GetData(data As IEnumerable(Of SerialData),
                                                Optional colors As Color() = Nothing,
                                                Optional lineWidth! = 2) As IEnumerable(Of ChartPlots.SerialData)
            Dim gs = From x As SerialData
                     In data
                     Select x
                     Group x By x.serial Into Group

            colors = If(
                colors.IsNullOrEmpty,
                Imaging.ChartColors.Shuffles,
                colors)

            For Each g In gs.SeqIterator

                Yield New ChartPlots.SerialData With {
                    .width = lineWidth,
                    .title = g.value.serial,
                    .color = colors(g.i),
                    .pts = LinqAPI.Exec(Of PointData) <=
                        From x As SerialData
                        In g.value.Group
                        Select New PointData With {
                            .errMinus = x.errMinus,
                            .errPlus = x.errPlus,
                            .pt = New PointF(x.X, x.Y),
                            .Tag = x.tag,
                            .value = x.value,
                            .Statics = x.Statics
                        }
                    }
            Next
        End Function

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

        ''' <summary>
        ''' 请注意这里只对一个系列的数据进行插值处理，即<paramref name="raw"/>里面的所有标签<see cref="SerialData.serial"/>都必须要相同
        ''' </summary>
        ''' <param name="raw"></param>
        ''' <param name="degree!"></param>
        ''' <param name="resolution%"></param>
        ''' <returns></returns>
        Public Shared Function Interpolation(raw As IEnumerable(Of SerialData),
                                             Optional degree! = 2,
                                             Optional resolution% = 10) As SerialData()
            Dim rawData As SerialData() = raw _
                .OrderBy(Function(x) x.X) _
                .ToArray
            Dim pts As List(Of PointF) = B_Spline.Compute(
                rawData.ToArray(Function(x) New PointF(x.X, x.Y)),
                degree,
                resolution)
            Dim result As New List(Of SerialData)

            For Each block In pts.SlideWindows(resolution, offset:=resolution).SeqIterator
                Dim pt As SerialData = rawData(block)

                For Each d As PointF In block.value.Elements
                    result += New SerialData With {
                        .errMinus = pt.errMinus,
                        .errPlus = pt.errPlus,
                        .serial = pt.serial,
                        .Statics = pt.Statics,
                        .tag = pt.tag,
                        .value = pt.value,
                        .X = d.X,
                        .Y = d.Y
                    }
                Next
            Next

            Return result
        End Function
    End Class
End Namespace
