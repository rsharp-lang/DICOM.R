
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports NRRD
Imports SMRUCC.DICOM.LASer
Imports SMRUCC.DICOM.LASer.Model
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("LASer")>
Module LASerFile

    <ExportAPI("open")>
    Public Function open(lasfile As String) As Object
        Return New LasReader(lasfile)
    End Function

    ''' <summary>
    ''' load all point cloud model data
    ''' </summary>
    ''' <param name="las"></param>
    ''' <returns></returns>
    <ExportAPI("points")>
    Public Function loadPoints(las As LasReader) As LasPoint()
        Return las.ReadAll.ToArray
    End Function

    ''' <summary>
    ''' try to cast the raw raster object to las point cloud data
    ''' </summary>
    ''' <param name="raster"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("as.las")>
    <RApiReturn(GetType(LasPoint))>
    Public Function fromRawRaster(<RRawVectorArgument> raster As Object, Optional env As Environment = Nothing) As Object
        If raster Is Nothing Then
            Call env.AddMessage("the given raster object is nothing!")
            Return Nothing
        End If
        If TypeOf raster Is RasterPointCloud Then
            Return DirectCast(raster, RasterPointCloud).getRaster.ToArray
        Else
            Return Message.InCompatibleType(GetType(RasterPointCloud), raster.GetType, env)
        End If
    End Function

    <Extension>
    Private Iterator Function getRaster(raster As RasterPointCloud) As IEnumerable(Of LasPoint)

    End Function
End Module
