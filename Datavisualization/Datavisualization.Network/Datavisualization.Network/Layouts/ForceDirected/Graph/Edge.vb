'! 
'@file Edge.cs
'@author Woong Gyu La a.k.a Chris. <juhgiyo@gmail.com>
'		<http://github.com/juhgiyo/epForceDirectedGraph.cs>
'@date August 08, 2013
'@brief Edge Interface
'@version 1.0
'
'@section LICENSE
'
'The MIT License (MIT)
'
'Copyright (c) 2013 Woong Gyu La <juhgiyo@gmail.com>
'
'Permission is hereby granted, free of charge, to any person obtaining a copy
'of this software and associated documentation files (the "Software"), to deal
'in the Software without restriction, including without limitation the rights
'to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
'copies of the Software, and to permit persons to whom the Software is
'furnished to do so, subject to the following conditions:
'
'The above copyright notice and this permission notice shall be included in
'all copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
'FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
'AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
'LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
'OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
'THE SOFTWARE.
'
'@section DESCRIPTION
'
'An Interface for the Graph Class.
'
'

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Layouts.Graph

    Public Class Edge
        Public Sub New(iId As String, iSource As Node, iTarget As Node, iData As EdgeData)
            ID = iId
            Source = iSource
            Target = iTarget
            Data = If((iData IsNot Nothing), iData, New EdgeData())
            Directed = False
        End Sub

        Public Property ID() As String
            Get
                Return m_ID
            End Get
            Private Set
                m_ID = Value
            End Set
        End Property
        Private m_ID As String
        Public Property Data() As EdgeData
            Get
                Return m_Data
            End Get
            Private Set
                m_Data = Value
            End Set
        End Property
        Private m_Data As EdgeData

        Public Property Source() As Node
            Get
                Return m_Source
            End Get
            Private Set
                m_Source = Value
            End Set
        End Property
        Private m_Source As Node

        Public Property Target() As Node
            Get
                Return m_Target
            End Get
            Private Set
                m_Target = Value
            End Set
        End Property
        Private m_Target As Node

        Public Property Directed() As Boolean
            Get
                Return m_Directed
            End Get
            Set
                m_Directed = Value
            End Set
        End Property
        Private m_Directed As Boolean

        Public Overrides Function GetHashCode() As Integer
            Return ID.GetHashCode()
        End Function
        Public Overrides Function Equals(obj As System.Object) As Boolean
            ' If parameter is null return false.
            If obj Is Nothing Then
                Return False
            End If

            ' If parameter cannot be cast to Point return false.
            Dim p As Edge = TryCast(obj, Edge)
            If DirectCast(p, System.Object) Is Nothing Then
                Return False
            End If

            ' Return true if the fields match:
            Return (ID = p.ID)
        End Function

        Public Overloads Function Equals(p As Edge) As Boolean
            ' If parameter is null return false:
            If DirectCast(p, Object) Is Nothing Then
                Return False
            End If

            ' Return true if the fields match:
            Return (ID = p.ID)
        End Function

        Public Shared Operator =(a As Edge, b As Edge) As Boolean
            ' If both are null, or both are same instance, return true.
            If System.[Object].ReferenceEquals(a, b) Then
                Return True
            End If

            ' If one is null, but not both, return false.
            If (DirectCast(a, Object) Is Nothing) OrElse (DirectCast(b, Object) Is Nothing) Then
                Return False
            End If

            ' Return true if the fields match:
            Return a.ID = b.ID
        End Operator

        Public Shared Operator <>(a As Edge, b As Edge) As Boolean
            Return Not (a = b)
        End Operator
    End Class
End Namespace