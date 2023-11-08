
Imports Microsoft.VisualBasic.Imaging.Landscape.Ply
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 3d scan object model
''' </summary>
Public Class RasterPointCloud : Inherits RasterObject

    Public Property volumn As Double()()()

    ''' <summary>
    ''' Convert the NRRD raster data to PLY point cloud model 
    ''' </summary>
    ''' <returns></returns>
    Public Iterator Function GetPointCloud(skipZero As Boolean) As IEnumerable(Of PointCloud)
        Dim scale As Double

        For z As Integer = 1 To volumn.Length
            Dim layer As RasterImage = GetRasterImage(z)

            For i As Integer = 0 To layer.dimensionSize(1) - 1
                For j As Integer = 0 To layer.dimensionSize(0) - 1
                    scale = layer.grayscale(j)(i)

                    If skipZero AndAlso scale = 0.0 Then
                        Continue For
                    End If

                    Yield New PointCloud With {
                        .intensity = scale,
                        .x = i,
                        .y = j,
                        .z = z
                    }
                Next
            Next
        Next
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i">1 based offset value</param>
    ''' <returns></returns>
    Public Function GetRasterImage(i As Integer) As RasterImage
        Return New RasterImage With {
            .dimensionSize = dimensionSize.Take(2).ToArray,
            .grayscale = volumn(i - 1),
            .metadata = metadata
        }
    End Function

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
