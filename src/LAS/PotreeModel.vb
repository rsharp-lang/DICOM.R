Imports Microsoft.VisualBasic.Serialization.JSON
Imports SMRUCC.DICOM.LASer.Model
Imports SMRUCC.DICOM.LASer.Potree

''' <summary>
''' Potree http point cloud request helper
''' </summary>
Public Module PotreeModel

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="las"></param>
    ''' <param name="dir">the model saved dir path</param>
    ''' <returns></returns>
    Public Function ExportModel(las As LasReader, dir As String) As Boolean
        Dim lasfile As String = las.sourcefile
        Dim cloud As New Cloud With {
            .hierarchy = {New String() {"r", "5202"}},
            .scale = {las.XScale, las.YScale, las.ZScale}.Average,
            .octreeDir = "data",
            .pointAttributes = "LAS",
            .spacing = 0.05,
            .version = $"{las.VersionMajor}.{las.VersionMinor}",
            .boundingBox = New boundingBox,
            .tightBoundingBox = New boundingBox
        }

        Call las.Dispose()
        Call lasfile.FileCopy($"{dir}/data/r.las")
        Call cloud.GetJson.SaveTo($"{dir}/cloud.js")

        Return True
    End Function

    Public Function ExportModel(las As IEnumerable(Of LasPoint), dir As String) As Boolean
        Dim cloud As New Cloud With {
            .hierarchy = {New String() {"r", "5202"}},
            .scale = 0.01,
            .octreeDir = "data",
            .pointAttributes = "LAS",
            .spacing = 0.05,
            .version = "1.2",
            .boundingBox = New boundingBox,
            .tightBoundingBox = New boundingBox
        }
        Dim laspath As String = $"{dir}/data/r.las"
        Dim lasfile As New LasWriter(laspath)

        For Each p As LasPoint In las
            Call lasfile.WritePoint(p)
        Next

        Call lasfile.Flush()
        Call lasfile.Dispose()
        Call cloud.GetJson.SaveTo($"{dir}/cloud.js")

        Return True
    End Function
End Module
