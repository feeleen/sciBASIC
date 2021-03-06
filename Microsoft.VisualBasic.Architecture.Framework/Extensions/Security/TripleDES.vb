﻿#Region "Microsoft.VisualBasic::d65684dc5d5f8fecadfeac41e2e710a2, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Extensions\Security\TripleDES.vb"

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
Imports System.Text
Imports System.Security.Cryptography

Namespace SecurityString

    Public Class TripleDES : Inherits SecurityString.SecurityStringModel

        Private ReadOnly _des As New TripleDESCryptoServiceProvider
        Private ReadOnly _uni As New UnicodeEncoding
        Private ReadOnly _key() As Byte
        Private ReadOnly _iv() As Byte

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="key">24byte</param>
        ''' <param name="iv">8byte</param>
        ''' <remarks></remarks>
        Public Sub New(key() As Byte, iv() As Byte)
            _key = key
            _iv = iv
        End Sub

        Sub New(Optional password As String = "")
            If Not String.IsNullOrEmpty(password) Then
                strPassphrase = password
            End If

            Dim md5 = SecurityString.GetMd5Hash(strPassphrase)
            md5 = md5 & md5 & md5 & md5
            Dim bytes = _uni.GetBytes(md5)
            If bytes.Length < 24 Then
                bytes = {bytes, New Byte() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}}.ToVector
            End If
            _key = bytes.Take(24).ToArray

            Dim n As Integer = bytes.Length / 3
            _iv = _key.Take(8).Reverse.ToArray
        End Sub

        Public Overrides Function ToString() As String
            Return strPassphrase
        End Function

        Public Overrides Function Encrypt(input() As Byte) As Byte()
            Return InternalCryptoTransform(input, _des.CreateEncryptor(_key, _iv))
        End Function

        Public Overrides Function EncryptData(text As String) As String
            Dim input() = _uni.GetBytes(text)
            Dim output() = InternalCryptoTransform(input, _des.CreateEncryptor(_key, _iv))
            Return Convert.ToBase64String(output)
        End Function

        Public Overrides Function Decrypt(input() As Byte) As Byte()
            Return InternalCryptoTransform(input, _des.CreateDecryptor(_key, _iv))
        End Function

        Public Overrides Function DecryptString(text As String) As String
            Dim input() = Convert.FromBase64String(text)
            Dim output() = InternalCryptoTransform(input, _des.CreateDecryptor(_key, _iv))
            Return _uni.GetString(output)
        End Function

        Private Function InternalCryptoTransform(input() As Byte, transform As ICryptoTransform) As Byte()
            Dim result As Byte() = Nothing

            Using ms As New MemoryStream
                Using cs As New CryptoStream(ms, transform, CryptoStreamMode.Write)
                    cs.Write(input, 0, input.Length)
                    Try
                        cs.FlushFinalBlock()
                    Catch ex As Exception
                        Call Console.WriteLine(ex.ToString)
                    End Try
                    ms.Position = 0
                    Try
                        result = ms.ToArray()
                    Catch ex As Exception
                        Call Console.WriteLine(ex.ToString)
                    End Try

                End Using
            End Using

            Return result
        End Function
    End Class
End Namespace
