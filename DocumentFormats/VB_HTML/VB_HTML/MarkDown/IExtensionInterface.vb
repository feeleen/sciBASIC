'*
' * This file is part of the MarkdownSharp package
' * For the full copyright and license information,
' * view the LICENSE file that was distributed with this source code.
' 

Namespace MarkDown

    Public Interface IExtensionInterface

        ''' <summary>
        ''' Replace inline element
        ''' </summary>
        ''' <param name="text"></param>
        ''' <returns></returns>
        Function Transform(text As String) As String
    End Interface
End Namespace
