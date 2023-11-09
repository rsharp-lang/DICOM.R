
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging

Namespace Model

    Public Structure RGBColor

        Public Property Red As UShort
        Public Property Green As UShort
        Public Property Blue As UShort

        Public Overrides Function ToString() As String
            Return CType(Me, Color).ToHtmlColor
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Function FromArgb(r As UShort, g As UShort, b As UShort) As RGBColor
            Return New RGBColor With {.Red = r, .Blue = b, .Green = g}
        End Function

        Public Shared Narrowing Operator CType(c As RGBColor) As Color
            Dim total As Double = c.Red + c.Blue + c.Green
            Dim r As Byte = 255 * c.Red / total
            Dim g As Byte = 255 * c.Green / total
            Dim b As Byte = 255 * c.Blue / total

            Return Color.FromArgb(r, g, b)
        End Operator

    End Structure
End Namespace