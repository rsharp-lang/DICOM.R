
Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 2d grayscale image
''' </summary>
Public Class RasterImage : Inherits RasterObject
    Implements IRasterGrayscaleHeatmap

    ''' <summary>
    ''' A grayscale image pixels
    ''' </summary>
    ''' <returns></returns>
    Public Property grayscale As Double()()

    Public ReadOnly Property RawSize As Size
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return New Size(dimensionSize(0), dimensionSize(1))
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return $"{dimensionSize.GetJson}"
    End Function

    Public Shared Function Create(metadata As Metadata, rawdata As Array) As RasterImage
        Dim dimensionSize As Integer() = metadata.sizes
        Dim rawDbls As Double() = CLRVector.asNumeric(rawdata)
        Dim plant2D As Double()() = rawDbls.Split(dimensionSize(0))

        If plant2D.Length <> dimensionSize(1) Then
            Throw New InvalidProgramException
        End If

        Return New RasterImage With {
            .dimensionSize = dimensionSize,
            .grayscale = plant2D,
            .metadata = metadata
        }
    End Function

    Public Iterator Function GetRasterPixels() As IEnumerable(Of Pixel) Implements IRasterGrayscaleHeatmap.GetRasterPixels
        Dim row As Double()

        For i As Integer = 0 To dimensionSize(1) - 1
            For j As Integer = 0 To dimensionSize(0) - 1
                row = grayscale(j)

                Yield New PixelData With {
                    .Scale = row(i),
                    .X = i,
                    .Y = j
                }
            Next
        Next
    End Function
End Class
