Public Class RasterObject

    Public Property metadata As Metadata
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
