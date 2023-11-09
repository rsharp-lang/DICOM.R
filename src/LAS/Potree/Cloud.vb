Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Potree

    ''' <summary>
    ''' cloud.js
    ''' </summary>
    Public Class Cloud

        Public Property version As String = "1.1"
        Public Property octreeDir As String = "data"
        Public Property boundingBox As boundingBox
        Public Property tightBoundingBox As boundingBox
        Public Property pointAttributes As String = "LAS"
        Public Property spacing As Double
        Public Property scale As Double
        Public Property hierarchy As String()()

        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function

    End Class

    Public Class boundingBox
        Public Property lx As Double
        Public Property ly As Double
        Public Property lz As Double
        Public Property ux As Double
        Public Property uy As Double
        Public Property uz As Double
    End Class
End Namespace