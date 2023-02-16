
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 2d grayscale image
''' </summary>
Public Class RasterImage : Inherits RasterObject

    ''' <summary>
    ''' A grayscale image pixels
    ''' </summary>
    ''' <returns></returns>
    Public Property grayscale As Double()()

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

End Class
