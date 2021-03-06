﻿#Region "Microsoft.VisualBasic::3bc4dea7eaec7d8bdc9118b593afa400, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Serialization\JSON\Formatter\Strategies\OpenBracketStrategy.vb"

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

Namespace Serialization.JSON.Formatter.Internals.Strategies
    Friend NotInheritable Class OpenBracketStrategy
        Implements ICharacterStrategy
        Public Sub Execute(context As JsonFormatterStrategyContext) Implements ICharacterStrategy.Execute
            If context.IsProcessingString Then
                context.AppendCurrentChar()
                Return
            End If

            context.AppendCurrentChar()
            context.EnterObjectScope()

            If Not IsBeginningOfNewLineAndIndentionLevel(context) Then
                Return
            End If

            context.BuildContextIndents()
        End Sub

        Private Shared Function IsBeginningOfNewLineAndIndentionLevel(context As JsonFormatterStrategyContext) As Boolean
            Return context.IsProcessingVariableAssignment OrElse (Not context.IsStart AndAlso Not context.IsInArrayScope)
        End Function

        Public ReadOnly Property ForWhichCharacter() As Char Implements ICharacterStrategy.ForWhichCharacter
            Get
                Return "{"c
            End Get
        End Property
    End Class
End Namespace
