Imports System.Collections.Generic
Imports System.Text
Imports System.Text.RegularExpressions

Namespace HTML.Render

    ''' <summary>
    ''' Collection of regular expressions used when parsing
    ''' </summary>
    Friend NotInheritable Class Parser
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Extracts properties and values from a Css property block; e.g. property:value;
        ''' </summary>
        Public Const CssProperties As String = ";?[^;\s]*:[^\{\}:;]*(\}|;)?"

        ''' <summary>
        ''' Extracts CSS style comments; e.g. /* comment */
        ''' </summary>
        Public Const CssComments As String = "/\*[^*/]*\*/"

        ''' <summary>
        ''' Extracts CSS at-rules; e.g. @media print { block1{} block2{} }
        ''' </summary>
        Public Const CssAtRules As String = "@.*\{\s*(\s*" + CssBlocks + "\s*)*\s*\}"

        ''' <summary>
        ''' Extracts the media types from a media at-rule; e.g. @media print, 3d, screen {
        ''' </summary>
        Public Const CssMediaTypes As String = "@media[^\{\}]*\{"

        ''' <summary>
        ''' Extracts defined blocks in CSS. 
        ''' WARNING: Blocks will include blocks inside at-rules.
        ''' </summary>
        Public Const CssBlocks As String = "[^\{\}]*\{[^\{\}]*\}"

        ''' <summary>
        ''' Extracts a number; e.g.  5, 6, 7.5, 0.9
        ''' </summary>
        Public Const CssNumber As String = "{[0-9]+|[0-9]*\.[0-9]+}"

        ''' <summary>
        ''' Extracts css percentages from the string; e.g. 100% .5% 5.4%
        ''' </summary>
        Public Const CssPercentage As String = "([0-9]+|[0-9]*\.[0-9]+)\%"
        'TODO: Check if works fine
        ''' <summary>
        ''' Extracts CSS lengths; e.g. 9px 3pt .89em
        ''' </summary>
        Public Const CssLength As String = "([0-9]+|[0-9]*\.[0-9]+)(em|ex|px|in|cm|mm|pt|pc)"

        ''' <summary>
        ''' Extracts CSS colors; e.g. black white #fff #fe98cd rgb(5,5,5) rgb(45%, 0, 0)
        ''' </summary>
        Public Const CssColors As String = "(#\S{6}|#\S{3}|rgb\(\s*[0-9]{1,3}\%?\s*\,\s*[0-9]{1,3}\%?\s*\,\s*[0-9]{1,3}\%?\s*\)|maroon|red|orange|yellow|olive|purple|fuchsia|white|lime|green|navy|blue|aqua|teal|black|silver|gray)"

        ''' <summary>
        ''' Extracts line-height values (normal, numbers, lengths, percentages)
        ''' </summary>
        Public Const CssLineHeight As String = "(normal|" + CssNumber + "|" + CssLength + "|" + CssPercentage + ")"

        ''' <summary>
        ''' Extracts CSS border styles; e.g. solid none dotted
        ''' </summary>
        Public Const CssBorderStyle As String = "(none|hidden|dotted|dashed|solid|double|groove|ridge|inset|outset)"

        ''' <summary>
        ''' Extracts CSS border widthe; e.g. 1px thin 3em
        ''' </summary>
        Public Const CssBorderWidth As String = "(" + CssLength + "|thin|medium|thick)"

        ''' <summary>
        ''' Extracts font-family values
        ''' </summary>
        Public Const CssFontFamily As String = "(""[^""]*""|'[^']*'|\S+\s*)(\s*\,\s*(""[^""]*""|'[^']*'|\S+))*"

        ''' <summary>
        ''' Extracts CSS font-styles; e.g. normal italic oblique
        ''' </summary>
        Public Const CssFontStyle As String = "(normal|italic|oblique)"

        ''' <summary>
        ''' Extracts CSS font-variant values; e.g. normal, small-caps
        ''' </summary>
        Public Const CssFontVariant As String = "(normal|small-caps)"

        ''' <summary>
        ''' Extracts font-weight values; e.g. normal, bold, bolder...
        ''' </summary>
        Public Const CssFontWeight As String = "(normal|bold|bolder|lighter|100|200|300|400|500|600|700|800|900)"

        ''' <summary>
        ''' Exracts font sizes: xx-small, larger, small, 34pt, 30%, 2em
        ''' </summary>
        Public Const CssFontSize As String = "(" + CssLength + "|" + CssPercentage + "|xx-small|x-small|small|medium|large|x-large|xx-large|larger|smaller)"

        ''' <summary>
        ''' Gets the font-size[/line-height]? on the font shorthand property.
        ''' Check http://www.w3.org/TR/CSS21/fonts.html#font-shorthand
        ''' </summary>
        Public Const CssFontSizeAndLineHeight As String = CssFontSize + "(\/" + CssLineHeight + ")?(\s|$)"

        ''' <summary>
        ''' Extracts HTML tags
        ''' </summary>
        Public Const HtmlTag As String = "<[^<>]*>"

        ''' <summary>
        ''' Extracts attributes from a HTML tag; e.g. att=value, att="value"
        ''' </summary>
        Public Const HmlTagAttributes As String = "[^\s]*\s*=\s*(""[^""]*""|[^\s]*)"

#Region "Methods"

        ''' <summary>
        ''' Extracts matches from the specified source
        ''' </summary>
        ''' <param name="regex">Regular expression to extract matches</param>
        ''' <param name="source">Source to extract matches</param>
        ''' <returns>Collection of matches</returns>
        Public Shared Function Match(regex As String, source As String) As MatchCollection
            Dim r As New Regex(regex, RegexOptions.IgnoreCase Or RegexOptions.Singleline)
            Return r.Matches(source)
        End Function

        ''' <summary>
        ''' Searches the specified regex on the source
        ''' </summary>
        ''' <param name="regex"></param>
        ''' <param name="source"></param>
        ''' <returns></returns>
        Public Shared Function Search(regex As String, source As String) As String
            Dim position As Integer
            Return Search(regex, source, position)
        End Function

        ''' <summary>
        ''' Searches the specified regex on the source
        ''' </summary>
        ''' <param name="regex"></param>
        ''' <param name="source"></param>
        ''' <returns></returns>
        Public Shared Function Search(regex As String, source As String, ByRef position As Integer) As String
            Dim matches As MatchCollection = Match(regex, source)

            If matches.Count > 0 Then
                position = matches(0).Index
                Return matches(0).Value
            Else
                position = -1
            End If

            Return Nothing
        End Function
#End Region
    End Class
End Namespace