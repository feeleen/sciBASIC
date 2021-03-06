﻿#Region "Microsoft.VisualBasic::5584bdf7e82a54369c9affcf59e7222c, ..\sciBASIC#\gr\Microsoft.VisualBasic.Imaging\Drawing3D\Painter.vb"

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
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging.Drawing3D.Device
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Namespace Drawing3D

    ''' <summary>
    ''' ``PAINTERS ALGORITHM`` provider
    ''' </summary>
    Public Module PainterAlgorithm

        ''' <summary>
        ''' 这个函数主要是应用于函数绘图的。请注意，这个并没有rotate，只会利用camera进行project
        ''' </summary>
        ''' <param name="canvas"></param>
        ''' <param name="camera"></param>
        ''' <param name="surfaces"></param>
        <Extension>
        Public Sub SurfacePainter(ByRef canvas As Graphics, camera As Camera, surfaces As IEnumerable(Of Surface), Optional drawPath As Boolean = False)
            Call canvas.BufferPainting(camera.PainterBuffer(surfaces), drawPath)
        End Sub

        <Extension>
        Public Sub BufferPainting(ByRef canvas As Graphics, buf As IEnumerable(Of Polygon),
                                  Optional drawPath As Boolean = False,
                                  Optional illumination As Boolean = False)
            If illumination Then
                buf = buf.Illumination
            End If
            For Each polygon As Polygon In buf
                With polygon
                    If drawPath Then
                        Call canvas.DrawPolygon(Pens.Black, .points)
                    End If
                    Call canvas.FillPolygon(.brush, .points)
                End With
            Next
        End Sub

        ''' <summary>
        ''' 生成三维图形绘图的多边形缓存。请注意，这个并没有rotate，只会利用camera进行project
        ''' </summary>
        ''' <param name="camera"></param>
        ''' <param name="surfaces"></param>
        ''' <returns></returns>
        <Extension>
        Public Function PainterBuffer(camera As Camera, surfaces As IEnumerable(Of Surface)) As IEnumerable(Of Polygon)
            Dim sv As New List(Of Surface)

            For Each s As Surface In surfaces
                Dim v As Point3D() = camera _
                    .Project(s.vertices) _
                    .ToArray

                sv += New Surface With {
                    .vertices = v,
                    .brush = s.brush
                }
            Next

            Dim order As List(Of Integer) = sv.OrderProvider(
                Function(surface) surface _
                    .vertices _
                    .Average(Function(z) z.Z))
            Dim screen As Size = camera.screen
            Dim out As New List(Of Polygon)

            ' Draw the faces using the PAINTERS ALGORITHM (distant faces first, closer faces last).
            For i As Integer = 0 To sv.Count - 1
                Dim index As Integer = order(i)
                Dim s As Surface = sv(index)
                Dim points() As Point = s _
                    .vertices _
                    .Select(Function(p3D) p3D.PointXY(screen)) _
                    .ToArray

                out += New Polygon With {
                    .brush = s.brush,
                    .points = points
                }
            Next

            Return out
        End Function

        ''' <summary>
        ''' The polygon buffer unit after the 3D to 2D projection and the z-order sorts.
        ''' (经过投影和排序操作之后的多边形图形缓存单元)
        ''' </summary>
        Public Structure Polygon
            ''' <summary>
            ''' The 3D projection result buffer
            ''' </summary>
            Dim points As Point()
            ''' <summary>
            ''' Surface fill
            ''' </summary>
            Dim brush As Brush
        End Structure

        ''' <summary>
        ''' ``PAINTERS ALGORITHM`` kernel
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="source"></param>
        ''' <param name="z">计算出z轴的平均数据</param>
        ''' <returns></returns>
        <Extension>
        Public Function OrderProvider(Of T)(source As IEnumerable(Of T), z As Func(Of T, Double)) As List(Of Integer)
            Dim order As New List(Of Integer)
            Dim avgZ As New List(Of Double)

            ' Compute the average Z value of each face.
            For Each i As SeqValue(Of T) In source.SeqIterator
                Call avgZ.Add(z(+i))
                Call order.Add(i)
            Next

            Dim iMax%, tmp#

            ' Next we sort the faces in descending order based on the Z value.
            ' The objective is to draw distant faces first. This is called
            ' the PAINTERS ALGORITHM. So, the visible faces will hide the invisible ones.
            ' The sorting algorithm used is the SELECTION SORT.
            For i% = 0 To avgZ.Count - 1
                iMax = i

                For j = i + 1 To avgZ.Count - 1
                    If avgZ(j) > avgZ(iMax) Then
                        iMax = j
                    End If
                Next

                If iMax <> i Then
                    tmp = avgZ(i)
                    avgZ(i) = avgZ(iMax)
                    avgZ(iMax) = tmp

                    tmp = order(i)
                    order(i) = order(iMax)
                    order(iMax) = tmp
                End If
            Next

            Call order.Reverse()

            Return order
        End Function
    End Module
End Namespace
