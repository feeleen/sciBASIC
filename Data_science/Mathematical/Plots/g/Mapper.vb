﻿Imports System.Drawing
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data.ChartPlots.BarPlot.Histogram
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Mathematical.LinearAlgebra
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS

Namespace Graphic

    ''' <summary>
    ''' 将数据坐标转换为绘图坐标
    ''' </summary>
    Public Class Mapper

        ''' <summary>
        ''' 坐标轴的数据
        ''' </summary>
        Public ReadOnly xAxis, yAxis As Vector

        ReadOnly serials As SerialData()
        ReadOnly hist As HistogramGroup

        ''' <summary>
        ''' x,y轴分别的最大值和最小值的差值
        ''' </summary>
        Public ReadOnly dx#, dy#
        Public ReadOnly xmin, ymin As Single

        Sub New(range As Scaling, Optional parts% = 10)
            Call Me.New(range.xrange, range.yrange, parts)

            serials = range.serials
            hist = range.hist
        End Sub

        Sub New(xrange As DoubleRange, yrange As DoubleRange, Optional parts% = 10)
            xAxis = New Vector(xrange.GetAxisValues(parts))
            yAxis = New Vector(yrange.GetAxisValues(parts))
            dx = xAxis.Max - xAxis.Min
            dy = yAxis.Max - yAxis.Min
            xmin = xAxis.Min
            ymin = yAxis.Min
        End Sub

        Public Function ScallingWidth(x As Double, width%) As Single
            Return width * (x - xmin) / dx
        End Function

        ''' <summary>
        ''' 返回的系列是已经被转换过的，直接使用来进行画图
        ''' </summary>
        ''' <returns></returns>
        Public Iterator Function ForEach(size As Size, margin As Padding) As IEnumerable(Of SerialData)
            Dim bottom As Integer = size.Height - margin.Bottom
            Dim width As Integer = size.Width - margin.Horizontal
            Dim height As Integer = size.Height - margin.Vertical

            For Each s As SerialData In serials
                Dim pts = LinqAPI.Exec(Of PointData) <=
 _
                From p As PointData
                In s.pts
                Let px As Single = margin.Left + width * (p.pt.X - xmin) / dx
                Let yh As Single = If(dy = 0R, height / 2, height * (p.pt.Y - ymin) / dy) ' 如果y没有变化，则是一条居中的水平直线
                Let py As Single = bottom - yh
                Select New PointData(px, py) With {
                    .errMinus = p.errMinus,
                    .errPlus = p.errPlus,
                    .Tag = p.Tag,
                    .value = p.value,
                    .Statics = p.Statics
                }

                Yield New SerialData With {
                    .color = s.color,
                    .lineType = s.lineType,
                    .PointSize = s.PointSize,
                    .pts = pts,
                    .title = s.title,
                    .width = s.width,
                    .DataAnnotations = s.DataAnnotations
                }
            Next
        End Function

        ''' <summary>
        ''' 返回的系列是已经被转换过的，直接使用来进行画图
        ''' </summary>
        ''' <returns></returns>
        Public Iterator Function ForEach_histSample(size As Size, margin As Padding) As IEnumerable(Of HistProfile)
            Dim bottom As Integer = size.Height - margin.Bottom
            Dim width As Integer = size.Width - margin.Horizontal
            Dim height As Integer = size.Height - margin.Vertical

            For Each histData As HistProfile In hist.Samples
                Dim pts = LinqAPI.Exec(Of HistogramData) <=
 _
                From p As HistogramData
                In histData.data
                Let px1 As Single = margin.Left + width * (p.x1 - xmin) / dx
                Let px2 As Single = margin.Left + width * (p.x2 - xmin) / dx
                Let py As Single = bottom - height * (p.y - ymin) / dy
                Select New HistogramData With {
                    .x1 = px1,
                    .x2 = px2,
                    .y = py
                }

                Yield New HistProfile With {
                    .legend = histData.legend,
                    .data = pts
                }
            Next
        End Function

        Public Function PointScaler(size As Size, padding As Padding) As Func(Of PointF, PointF)
            Dim bottom As Integer = size.Height - padding.Bottom
            Dim width As Integer = size.Width - padding.Horizontal
            Dim height As Integer = size.Height - padding.Vertical

            Return Function(pt)
                       Dim px As Single = padding.Left + width * (pt.X - xmin) / dx
                       Dim py As Single = bottom - height * (pt.Y - ymin) / dy

                       Return New PointF(px, py)
                   End Function
        End Function

        Public Function PointScaler(rect As GraphicsRegion) As Func(Of PointF, PointF)
            Return PointScaler(rect.Size, rect.Padding)
        End Function

        Public Function TupleScaler(rect As GraphicsRegion) As Func(Of (x#, y#), PointF)
            Dim point = PointScaler(rect.Size, rect.Padding)
            Return Function(pt) point(New PointF(pt.x, pt.y))
        End Function

        Public Function PointScaler(r As GraphicsRegion, pt As PointF) As PointF
            Dim bottom As Integer = r.Size.Height - r.Padding.Bottom
            Dim width As Integer = r.Size.Width - r.Padding.Horizontal
            Dim height As Integer = r.Size.Height - r.Padding.Vertical
            Dim px As Single = r.Padding.Left + width * (pt.X - xmin) / dx
            Dim py As Single = bottom - height * (pt.Y - ymin) / dy

            Return New PointF(px!, py!)
        End Function

        Public Function XScaler(size As Size, margin As Padding) As Func(Of Single, Single)
            Dim bottom As Integer = size.Height - margin.Bottom
            Dim width As Integer = size.Width - margin.Horizontal
            Dim height As Integer = size.Height - margin.Vertical

            Return Function(x) margin.Left + width * (x - xmin) / dx
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="size"></param>
        ''' <param name="margin"></param>
        ''' <param name="avg">当这个参数值是一个有效的数字的时候，返回的Y将会以这个平均值为零点</param>
        ''' <returns></returns>
        Public Function YScaler(size As Size, margin As Padding, Optional avg# = Double.NaN) As Func(Of Single, Single)
            Dim bottom As Integer = size.Height - margin.Bottom
            Dim height As Integer = size.Height - margin.Vertical    ' 绘图区域的高度

            If Double.IsNaN(avg#) Then
                Return Function(y!) bottom - height * (y - ymin) / dy
            Else
                Dim half As Single = height / 2
                Dim middle As Single = bottom - half

                Return Function(y!) As Single
                           Dim d! = y - avg

                           If d >= 0F Then  ' 在上面
                               Return middle - half * (y - avg) / dy
                           Else
                               Return middle + half * (avg - y) / dy
                           End If
                       End Function
            End If
        End Function
    End Class
End Namespace