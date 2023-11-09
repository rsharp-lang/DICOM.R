
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports NRRD
Imports SMRUCC.DICOM.LASer
Imports SMRUCC.DICOM.LASer.Model
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

<Package("LASer")>
Module LASerFile

    ''' <summary>
    ''' Open the file read to the specific las file
    ''' </summary>
    ''' <param name="lasfile"></param>
    ''' <returns></returns>
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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    <Extension>
    Private Function getRaster(raster As RasterPointCloud) As IEnumerable(Of LasPoint)
        Return raster.GetPointCloud(Of LasPoint)(skipZero:=True)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="raster"></param>
    ''' <param name="file">
    ''' the format version of the las file is 1.2, and
    ''' the point data format is 2(RGB) value
    ''' </param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("write.las")>
    Public Function writeLas(<RRawVectorArgument> raster As Object, file As Object, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Write, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        If TypeOf raster Is RasterPointCloud Then
            raster = DirectCast(raster, RasterPointCloud).GetPointCloud(Of LasPoint)(skipZero:=True).ToArray
        End If

        Dim pointcloud As pipeline = pipeline.TryCreatePipeline(Of LasPoint)(raster, env)

        If pointcloud.isError Then
            Return pointcloud.getError
        End If

        Dim save As New LasWriter(buf.TryCast(Of Stream))

        For Each point As LasPoint In pointcloud.populates(Of LasPoint)(env)
            Call save.WritePoint(point)
        Next

        Call save.Flush()
        Call save.Dispose()

        Return True
    End Function
End Module
