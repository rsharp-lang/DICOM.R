
''' <summary>
''' an abstract raw raster object for <see cref="RasterImage"/> and <see cref="RasterPointCloud"/>
''' </summary>
''' <remarks>
''' the <see cref="metadata"/> contains the necessary information for 
''' decode the raster object from file.
''' </remarks>
Public Class RasterObject

    Public Property metadata As Metadata
    ''' <summary>
    ''' the dimension size value: [w,h,z]
    ''' </summary>
    ''' <returns></returns>
    Public Property dimensionSize As Integer()

    Public Shared Function CreateRasterObject(metadata As Metadata, rawdata As Array) As RasterObject
        If metadata.sizes.Length = 2 Then
            Return RasterImage.Create(metadata, rawdata)
        ElseIf metadata.sizes.Length = 3 Then
            Return RasterPointCloud.Create(metadata, rawdata)
        Else
            Throw New NotImplementedException
        End If
    End Function

    Public Shared Function CastPointCloud(img As RasterImage) As RasterObject

    End Function
End Class
