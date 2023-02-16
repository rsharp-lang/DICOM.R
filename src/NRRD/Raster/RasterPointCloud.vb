
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 3d scan object model
''' </summary>
Public Class RasterPointCloud : Inherits RasterObject

    Public Property volumn As Double()()()

    Public Shared Function Create(metadata As Metadata, rawdata As Array) As RasterPointCloud
        Dim dimensionSize As Integer() = metadata.sizes
        Dim rawDbls As Double() = CLRVector.asNumeric(rawdata)
        Dim fold1 = rawDbls.Split(dimensionSize(0))
        Dim fold2 = fold1.Split(dimensionSize(1))

        If fold2.Length <> dimensionSize(2) Then
            Throw New InvalidOperationException
        End If

        Return New RasterPointCloud With {
            .dimensionSize = dimensionSize,
            .volumn = fold2,
            .metadata = metadata
        }
    End Function
End Class
