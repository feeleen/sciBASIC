﻿#Region "Microsoft.VisualBasic::762dc6c06301b0524048e96e6a0141df, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Language\Value\Value.vb"

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

Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel

Namespace Language

    ''' <summary>
    ''' You can applying this data type into a dictionary object to makes the mathematics calculation more easily.
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class Value(Of T) : Implements IValueOf

        Public Interface IValueOf
            Property value As T
        End Interface

        ''' <summary>
        ''' The object value with a specific type define.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Property value As T Implements IValueOf.value

        ''' <summary>
        ''' Creates an reference value object with the specific object value
        ''' </summary>
        ''' <param name="value"></param>
        Sub New(value As T)
            Me.value = value
        End Sub

        ''' <summary>
        ''' Value is Nothing
        ''' </summary>
        Sub New()
            value = Nothing
        End Sub

        ''' <summary>
        ''' Is the value is nothing.
        ''' </summary>
        ''' <returns></returns>
        Public Function IsNothing() As Boolean
            Return value Is Nothing
        End Function

        ''' <summary>
        ''' Display <see cref="value"/> ``ToString()`` function value.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return Scripting.InputHandler.ToString(value)
        End Function

        Public Overloads Shared Operator +(list As Generic.List(Of Value(Of T)), x As Value(Of T)) As Generic.List(Of Value(Of T))
            Call list.Add(x)
            Return list
        End Operator

        Public Overloads Shared Operator +(x As Value(Of T), list As IEnumerable(Of T)) As List(Of T)
            Return (+x).Join(list)
        End Operator

        Public Overloads Shared Operator -(list As Generic.List(Of Value(Of T)), x As Value(Of T)) As Generic.List(Of Value(Of T))
            Call list.Remove(x)
            Return list
        End Operator

        Public Shared Operator <=(value As Value(Of T), o As T) As T
            value.value = o
            Return o
        End Operator

        Public Shared Narrowing Operator CType(x As Value(Of T)) As T
            Return x.value
        End Operator

        Public Shared Widening Operator CType(x As T) As Value(Of T)
            Return New Value(Of T)(x)
        End Operator

        ''' <summary>
        ''' Gets the <see cref="Value"/> property value.
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        Public Shared Operator +(x As Value(Of T)) As T
            Return x.value
        End Operator

        ''' <summary>
        ''' Inline value assignment: ``Dim s As String = Value(Of String) = var``
        ''' </summary>
        ''' <param name="value"></param>
        ''' <param name="o"></param>
        ''' <returns></returns>
        Public Shared Operator =(value As Value(Of T), o As T) As T
            value.value = o
            Return o
        End Operator

        Public Shared Operator <>(value As Value(Of T), o As T) As T
            Throw New NotSupportedException
        End Operator

        Public Shared Operator >=(value As Value(Of T), o As T) As T
            Throw New NotSupportedException
        End Operator

        'Public Shared Operator &(o As Value(Of T)) As T
        '    Return o.value
        'End Operator
    End Class
End Namespace
