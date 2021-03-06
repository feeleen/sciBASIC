﻿Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.Data
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Data.csv
Imports Microsoft.VisualBasic.DataMining.FuzzyCMeans
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Mathematical
Imports Microsoft.VisualBasic.Mathematical.Correlations
Imports Microsoft.VisualBasic.MIME.Markup.HTML.CSS
Imports Microsoft.VisualBasic.Serialization.JSON

Module FuzzyCMeansVisualize

    ''' <summary>
    ''' Assign random points within given range
    ''' </summary>
    ''' <param name="raw"></param>
    ''' <param name="rnd"></param>
    ''' <param name="n%"></param>
    ''' <param name="up%"></param>
    <Extension>
    Private Sub AddPoints(ByRef raw As List(Of Entity), rnd As Random, n%, up%)
        For i As Integer = 0 To n
            raw += New Entity With {
                .uid = i,
                .Properties = {
                    rnd.Next(0, up),
                    rnd.Next(0, up)
                }
            }
        Next
    End Sub

    Private Function CMeans() As (raw As Entity(), n%, trace As Dictionary(Of Integer, List(Of Entity)))
        Dim raw As New List(Of Entity)
        Dim rnd As New Random(Now.Millisecond)
        Dim up% = 1500

        ' initizlize of the points
        For i As Integer = 0 To 25
            Call raw.AddPoints(rnd, 50, up)
            up -= 50
        Next

        Dim n% = 10  ' required of 10 clusters
        Dim trace As New Dictionary(Of Integer, List(Of Entity))

        ' invoke cmeans cluster and gets the centra points
        Dim centras = raw.FuzzyCMeans(n, 2, trace:=trace)

#Region "DEBUG INFO OUTPUTS"
        For Each x In centras
            Call $"centra {x.uid} =>  {x.Properties.GetJson}".PrintException
        Next

        For Each x In raw
            Call ($"{x.uid}: {x.Properties.GetJson} => " & x.Memberships.GetJson).__DEBUG_ECHO
        Next
#End Region

        Return (raw, n, trace)
    End Function

    Private Sub CMeansVisualize()
        Call CMeans.Visualize
    End Sub

    Sub Main()
        Call CMeansVisualize()
    End Sub

    <Extension>
    Private Sub Visualize(data As (raw As Entity(), n%, trace As Dictionary(Of Integer, List(Of Entity))))
        Dim trace As Dictionary(Of Integer, List(Of Entity)) = data.trace

        ' data plots visualize
        Dim plotData As New List(Of SerialData)
        ' using ColorBrewer color patterns from the visualbasic internal color designer
        Dim colors As Color() = Designer.GetColors("Paired:c10", data.n)

        ' generates serial data for each point in the raw inputs
        For Each x As Entity In data.raw
            Dim r = colors(x.ProbablyMembership).R
            Dim g = colors(x.ProbablyMembership).G
            Dim b = colors(x.ProbablyMembership).B
            Dim c As Color = Color.FromArgb(CInt(r), CInt(g), CInt(b))

            plotData += Scatter.FromPoints(
                {New PointF(x(0), x(1))},
                c.RGBExpression,
                ptSize:=30,
                title:="Point " & x.uid)
        Next

        Dim traceSerials As New List(Of List(Of Entity))

        For i As Integer = 0 To data.n - 1
            traceSerials += New List(Of Entity)
        Next

        For Each k In trace.Keys.OrderBy(Function(x) x)
            For i As Integer = 0 To data.n - 1
                traceSerials(i) += trace(k)(i)
            Next
        Next

        ' generates the serial data for each centra points
        For i = 0 To data.n - 1
            Dim points As IEnumerable(Of PointF) =
                traceSerials(i) _
                .Select(Function(x) New PointF(x(0), x(1)))
            plotData += Scatter.FromPoints(
                points,
                colors(i).RGBExpression,
                ptSize:=10,
                title:="Cluster " & i)
            plotData.Last.AddMarker(
                plotData.Last.pts.Last.pt.X,
                "Cluster " & i,
                "red",
                style:=LegendStyles.Triangle)
        Next

        Call Scatter.Plot(plotData, New Size(5000, 3000), fillPie:=True, showLegend:=False) _
            .SaveAs("./CMeans.png")
    End Sub
End Module
