Imports System.Drawing
Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Imaging.Landscape.Ply
Imports Microsoft.VisualBasic.Imaging.Math2D
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports Microsoft.VisualBasic.Text
Imports SMRUCC.DICOM.NRRD
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Internal.[Object]
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' ## Nearly Raw Raster Data
''' 
''' Nrrd is a library and file format designed to support scientific 
''' visualization and image processing involving N-dimensional raster 
''' data. Nrrd stands for "nearly raw raster data". Besides dimensional
''' generality, nrrd is flexible with respect to type (8 integral 
''' types, 2 floating point types), encoding of written files (raw, 
''' ascii, hex, or gzip or bzip2 compression), and endianness (the byte 
''' order of data is explicitly recorded when the type or encoding expose
''' it). Besides the NRRD format, the library can read and write PNG, PPM, 
''' and PGM images, as well as some VTK "STRUCTURED_POINTS" datasets. 
''' 
''' About two dozen operations on raster data are implemented, including
''' simple things like quantizing, slicing, and cropping, as well as 
''' fancier things like projection, histogram equalization, and filtered
''' resampling (up and down) with arbitrary seperable kernels.
''' </summary>
<Package("NRRD")>
Public Module nrrdFile

    ''' <summary>
    ''' open a nrrd <see cref="FileReader"/>
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <example>
    ''' imports "NRRD" from "DICOM";
    ''' 
    ''' let nrrd = NRRD::nrrdRead(file = "./test.nrrd");
    ''' </example>
    <ExportAPI("nrrdRead")>
    <RApiReturn(GetType(FileReader))>
    Public Function nrrdRead(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As Object
        Dim filedata = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If filedata Like GetType(Message) Then
            Return filedata.TryCast(Of Message)
        End If

        Dim reader As New FileReader(filedata.TryCast(Of Stream))
        Return reader
    End Function

    ''' <summary>
    ''' get nrrd header metadata
    ''' </summary>
    ''' <param name="nrrd"></param>
    ''' <returns></returns>
    ''' <example>
    ''' imports "NRRD" from "DICOM";
    ''' 
    ''' let nrrd = NRRD::nrrdRead(file = "./test.nrrd");
    ''' let meta = NRRD::metadata(nrrd);
    ''' 
    ''' str(as.list(meta));
    ''' </example>
    <ExportAPI("metadata")>
    Public Function GetMetadata(nrrd As FileReader) As Metadata
        Return nrrd.NrddHeader
    End Function

    <ExportAPI("getRaster")>
    Public Function GetRaster(nrrd As FileReader) As RasterObject
        Return nrrd.LoadRaster
    End Function

    <ExportAPI("getRasterLayer")>
    Public Function GetRasterLayer(raster As RasterPointCloud, layer As Integer) As RasterObject
        Return raster.GetRasterImage(layer)
    End Function

    <ExportAPI("as.pointCloud")>
    Public Function toPointCloud(raster As RasterPointCloud, Optional skip_zero As Boolean = True) As PointCloud()
        Return raster.GetPointCloud(Of PointCloud)(skip_zero).ToArray
    End Function

    <ExportAPI("as.pointMatrix")>
    Public Function toPointMatrix(raster As RasterPointCloud, Optional skip_zero As Boolean = True) As dataframe
        Dim pointCloud = raster.GetPointCloud(Of PointCloud)(skip_zero).ToArray
        Dim labels = pointCloud.Select(Function(p) $"[{p.x},{p.y},{p.z}]").ToArray
        Dim x = pointCloud.Select(Function(p) p.x).ToArray
        Dim y = pointCloud.Select(Function(p) p.y).ToArray
        Dim z = pointCloud.Select(Function(p) p.z).ToArray
        Dim intensity = pointCloud.Select(Function(p) p.intensity).ToArray

        Return New dataframe With {
            .rownames = labels,
            .columns = New Dictionary(Of String, Array) From {
                {"x", x},
                {"y", y},
                {"z", z},
                {"scale", intensity}
            }
        }
    End Function

    ''' <summary>
    ''' write the NRRD raster data to PLY point cloud model 
    ''' </summary>
    ''' <param name="raster">A 3 space dimension NRRD raster object</param>
    ''' <param name="file">file to the ply file target</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("write.ply")>
    Public Function writePly(raster As RasterPointCloud, file As Object, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Write, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        Return SimplePlyWriter.WriteAsciiText(
            pointCloud:=raster.GetPointCloud(Of PointCloud)(skipZero:=True),
            buffer:=buf.TryCast(Of Stream)
        )
    End Function

    ''' <summary>
    ''' create a nrrd file based on a given collection of the image data objects.
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="rasters">a collection of the image data objects. all of the raster object inside
    ''' this given collection should be in the same dimension size!</param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' the required <paramref name="rasters"/> data collection element could be one of the:
    ''' 
    ''' 1. the gdi+ <see cref="Image"/> data object
    ''' 2. the <see cref="RasterMatrix"/> for do heatmap rendering
    ''' 
    ''' for a collection with only one ratser object inside, 2d nrrd object will be generates,
    ''' for a collection with multiple raster object inside, 3d nrrd object will be generates.
    ''' </remarks>
    <ExportAPI("write.nrrd")>
    Public Function writeNrrd(file As Object, <RRawVectorArgument> rasters As Object, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Write, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If
#Disable Warning
        Dim imgs = pipeline.TryCreatePipeline(Of Image)(rasters, env)

        If imgs.isError Then
            imgs = pipeline.TryCreatePipeline(Of RasterMatrix)(rasters, env)

            If imgs.isError Then
                Return imgs.getError
            End If

            ' save rasters matrix as nrrd file
            Dim imgList As RasterMatrix() = imgs.populates(Of RasterMatrix)(env).ToArray
            Dim dimSize As Size = imgList(0).Size

            ' matrix needs reverse for keeps the right order
            dimSize = New Size(dimSize.Height, dimSize.Width)

            Using s As Stream = buf.TryCast(Of Stream)
                FileWriter.WriteFile(New BinaryDataWriter(s, Encodings.ASCII), dimSize, imgList)
            End Using
        Else
            Dim imgList As Image() = imgs.populates(Of Image)(env).ToArray
            Dim dimSize As Size = imgList(0).Size

            ' matrix needs reverse for keeps the right order
            dimSize = New Size(dimSize.Height, dimSize.Width)

#Enable Warning
            Using s As Stream = buf.TryCast(Of Stream)
                FileWriter.WriteFile(New BinaryDataWriter(s, Encodings.ASCII), dimSize, imgList)
            End Using
        End If

        Return True
    End Function

    ''' <summary>
    ''' create a file writer session for save large raster data collection
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="env"></param>
    ''' <returns></returns>
    <ExportAPI("write.nrrd_session")>
    <RApiReturn(GetType(FileWriterSession))>
    Public Function writerSession(file As Object, <RRawVectorArgument> dims As Object, Optional z As Integer = 1, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Write, env)
        Dim dim_size As Size = InteropArgumentHelper.getSize(dims, env, [default]:="0,0").SizeParser

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If
        If dim_size.Area = 0 Then
            Return Internal.debug.stop("invalid dimension size value!", env)
        End If

        Dim writer As New FileWriterSession(buf.TryCast(Of Stream))
        writer.WriteHeader(dim_size, z)
        Return New WriteNrrdFunction(session:=writer)
    End Function
End Module
