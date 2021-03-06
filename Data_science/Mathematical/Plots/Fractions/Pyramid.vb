﻿#Region "Microsoft.VisualBasic::69909920a03a2652b2d302f2115c5f21, ..\sciBASIC#\Data_science\Mathematical\Plots\Fractions\Pyramid.vb"

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
Imports System.Drawing.Drawing2D
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.g
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Vector.Shapes
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis

Public Module Pyramid

    ''' <summary>
    ''' 绘制金字塔图，用来表示占比的数据可视化
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="size"></param>
    ''' <param name="padding$"></param>
    ''' <param name="bg$"></param>
    ''' <param name="legendBorder"></param>
    ''' <param name="wp#"></param>
    ''' <returns></returns>
    Public Function Plot(data As IEnumerable(Of Fractions),
                         Optional size As Size = Nothing,
                         Optional padding$ = g.DefaultPadding,
                         Optional bg$ = "white",
                         Optional legendBorder As Border = Nothing,
                         Optional wp# = 0.8) As Bitmap

        Dim array As Fractions() =
            data _
            .OrderByDescending(Function(x) x.Percentage) _
            .ToArray
        Dim margin As Padding = padding

        If size.IsEmpty Then
            size = New Size(3000, 2000)
        End If

        Return GraphicsPlots(size, margin, bg,
                Sub(ByRef g, region)
                    Dim height% = region.PlotRegion.Height
                    Dim width% = region.PlotRegion.Width * wp
                    Dim left! = (region.PlotRegion.Width - width) / 2 + margin.Left
                    Dim tan_ab = height / (width / 2) ' tan(a)
                    Dim right! = (left + width)
                    Dim bottom! = region.PlotRegion.Bottom

                    For Each l As Fractions In array
                        Dim dh! = height * l.Percentage
                        Dim dw! = dh / tan_ab
                        ' b/| dh |\c
                        ' ---    ---
                        ' a        d
                        Dim a As New Point(left, bottom)
                        Dim b As New Point(left + dw, a.Y - dh)
                        Dim c As New Point(right - dw, b.Y)
                        Dim d As New Point(right, a.Y)

                        Dim path As New GraphicsPath
                        path.AddLine(a, b)
                        path.AddLine(b, c)
                        path.AddLine(c, d)
                        path.AddLine(d, a)
                        path.CloseAllFigures()

                        Call g.FillPath(New SolidBrush(l.Color), path)

                        left += dw
                        bottom -= dh
                        width -= dw * 2
                        right -= dw
                    Next

                    Dim font As New Font(FontFace.MicrosoftYaHei, 32)
                    Dim gr As Graphics = g
                    Dim maxL = data.Select(Function(x) gr.MeasureString(x.Name, font).Width).Max
                    left = size.Width - (margin.Horizontal) - maxL
                    Dim top = margin.Top
                    Dim legends As New List(Of Legend)

                    For Each x As Fractions In data
                        legends += New Legend With {
                           .color = x.Color.RGBExpression,
                           .style = LegendStyles.Rectangle,
                           .title = x.Name,
                           .fontstyle = CSSFont.GetFontStyle(font.Name, font.Style, font.Size)
                        }
                    Next

                    Call g.DrawLegends(New Point(left, top), legends, ,, legendBorder)
                End Sub)
    End Function
End Module
