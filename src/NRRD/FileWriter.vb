Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Math.LinearAlgebra
Imports Microsoft.VisualBasic.Text

Public Module FileWriter

    Public Sub WriteFile(file As BinaryDataWriter, dims As Size, rasters As Image())
        Call WriteAsciiHeaderCommon(file, dims, len:=rasters.Length)

        For Each image As Image In rasters
            Dim buffer As BitmapBuffer = BitmapBuffer.FromImage(image)
            Dim colors As Color() = buffer.GetPixelsAll.ToArray
            Dim intensity As Single() = colors.Select(Function(c) CSng(c.GrayScale)).ToArray

            Call file.Write(intensity)
        Next
    End Sub

    <Extension>
    Private Sub WriteString(file As BinaryDataWriter, str As String)
        Call file.Write(str & vbLf, BinaryStringFormat.NoPrefixOrTermination)
    End Sub

    ''' <summary>
    ''' data endian is always little
    ''' </summary>
    ''' <param name="file"></param>
    ''' <param name="dims"></param>
    ''' <param name="len"></param>
    Friend Sub WriteAsciiHeaderCommon(ByRef file As BinaryDataWriter, dims As Size, len As Integer)
        Dim w = dims.Width
        Dim h = dims.Height

        file.ByteOrder = ByteOrder.LittleEndian

        Call file.WriteString(FileReader.MagicBytes)
        Call file.WriteString("# This is an NRRD file: http://teem.sourceforge.net/nrrd/format.html")
        Call file.WriteString("# This data is a rawdata file generated from the R# language DICOM image library.")
        Call file.WriteString($"type: float")
        Call file.WriteString($"dimension: 3")
        Call file.WriteString($"sizes: {w} {h} {len}")
        Call file.WriteString($"encoding: raw")
        Call file.WriteString($"endian: little")
        Call file.WriteString($"space directions: (1,0,0) (0,1,0) (0,0,1)")
        Call file.WriteString($"space origin: (0, 0, 0)")
        Call file.WriteString("")
        Call file.Seek(5, SeekOrigin.Current)
    End Sub

    Public Sub WriteFile(file As BinaryDataWriter, dims As Size, rasters As RasterMatrix())
        Dim session As New FileWriterSession(file)
        Call session.WriteHeader(dims, z:=rasters.Length)
        Call rasters.DoEach(AddressOf session.WriterRasterLayer)
        Call file.Flush()
    End Sub

End Module

''' <summary>
''' File writer session for write large scale raster data model
''' </summary>
Public Class FileWriterSession : Implements IDisposable

    ReadOnly file As BinaryDataWriter

    Dim disposedValue As Boolean
    Dim z_len As Integer
    Dim dimension As Size

    Sub New(file As Stream)
        Me.file = New BinaryDataWriter(file, Encodings.ASCII)
    End Sub

    Sub New(file As BinaryDataWriter)
        Me.file = file
    End Sub

    Public Sub WriteHeader(dims As Size, z As Integer)
        dimension = dims
        z_len = z

        Call FileWriter.WriteAsciiHeaderCommon(file, dims, len:=z)
        Call file.Flush()
    End Sub

    Public Sub WriterRasterLayer(matrix As RasterMatrix)
        Dim intensity As Single() = Nothing

        For Each row As Vector In matrix.GetRowScans
            intensity = row.AsSingle
            file.Write(intensity)
        Next
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call file.Flush()
                Call file.Dispose()
            End If

            ' TODO: 释放未托管的资源(未托管的对象)并重写终结器
            ' TODO: 将大型字段设置为 null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: 仅当“Dispose(disposing As Boolean)”拥有用于释放未托管资源的代码时才替代终结器
    ' Protected Overrides Sub Finalize()
    '     ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class