Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
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
Public Module Rscript

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

    <ExportAPI("metadata")>
    Public Function GetMetadata(nrrd As FileReader) As Metadata
        Return nrrd.NrddHeader
    End Function

    <ExportAPI("getRaster")>
    Public Function GetRaster(nrrd As FileReader) As RasterObject
        Return nrrd.LoadRaster
    End Function
End Module
