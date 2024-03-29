﻿
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Imaging.Landscape.Ply
Imports SMRUCC.Rsharp.Runtime.Vectorization

''' <summary>
''' 3d scan object model
''' </summary>
Public Class RasterPointCloud : Inherits RasterObject

    Public Property volumn As Double()()()

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function GetPlyData(skipZero As Boolean) As IEnumerable(Of PointCloud)
        Return GetPointCloud(Of PointCloud)(skipZero)
    End Function

    ''' <summary>
    ''' Convert the NRRD raster data to PLY point cloud model 
    ''' </summary>
    ''' <returns></returns>
    Public Iterator Function GetPointCloud(Of T As {New, IPointCloud})(skipZero As Boolean) As IEnumerable(Of T)
        Dim scale As Double

        For z As Integer = 1 To volumn.Length
            Dim layer As RasterImage = GetRasterImage(z - 1)

            For i As Integer = 0 To layer.dimensionSize(1) - 1
                For j As Integer = 0 To layer.dimensionSize(0) - 1
                    scale = layer.grayscale(j)(i)

                    If skipZero AndAlso scale = 0.0 Then
                        Continue For
                    End If

                    Yield New T With {
                        .intensity = scale,
                        .X = i,
                        .Y = j,
                        .Z = z
                    }
                Next
            Next
        Next
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i">0 based offset value</param>
    ''' <returns></returns>
    Public Function GetRasterImage(i As Integer) As RasterImage
        Dim dims = dimensionSize _
            .Take(2) _
            .ToArray

        Return New RasterImage With {
            .dimensionSize = dims,
            .grayscale = volumn(i),
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
