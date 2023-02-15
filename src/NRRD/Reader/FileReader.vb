Imports System.IO
Imports System.IO.Compression
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Values
Imports Microsoft.VisualBasic.Net.Http

Public Class FileReader : Implements IDisposable

    ReadOnly file As Stream
    ReadOnly header As Header
    ReadOnly comments As New List(Of String)

    Public ReadOnly Property NrddHeader As Metadata
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return header.toMetadata
        End Get
    End Property

    Dim disposedValue As Boolean
    Dim scan0 As Long

    Sub New(file As Stream)
        Me.file = file
        Me.header = loadNrrdHeader()
    End Sub

    Public Function LoadRaster()
        Dim options As Metadata = Nothing
        Dim bytes As New BinaryDataReader(loadNrrdRawBuffer(options))
        Dim data As Array
        Dim deltaSize As Integer = BytesBuffer.checkBufferSize(bytes.BaseStream, options)

        If deltaSize <> 0 Then
            Throw New InvalidDataException($"The required size of the raster data is not matched(delta_size {deltaSize} bytes) with the nrdd sub-stream size!")
        End If

        ' the source stream is loaded from
        ' the nrdd file substream
        ' the actually scan0 is ZERO
        bytes.Seek(0, SeekOrigin.Begin)
        data = BytesBuffer.parseNRRDRawData(bytes, options)

        Return data
    End Function

    Private Function loadNrrdRawBuffer(ByRef metadata As Metadata) As MemoryStream
        Dim size As Integer = file.Length - scan0
        Dim bytes As Byte() = New Byte(size - 1) {}

        ' get raster data reader options
        metadata = header.toMetadata

        file.Seek(scan0, SeekOrigin.Begin)
        file.Read(bytes, 0, bytes.Length)

        Select Case metadata.encoding
            Case Encoding.raw : Return New MemoryStream(bytes)
            Case Encoding.gzip, Encoding.gz
                ' 创建一个GZip解压流
                Dim gz As New GZipStream(New MemoryStream(bytes), CompressionMode.Decompress)
                ' 用一个临时内存流来保存解压数据
                Dim ms As New MemoryStream
                ' 缓冲数据
                Dim buf(99) As Byte, i As Integer = 0
                ' 不断从流中解压数据
                While True
                    i = gz.Read(buf, 0, 100)

                    If i = 0 Then
                        Exit While
                    Else
                        ms.Write(buf, 0, i)
                    End If
                End While

                ' 关闭所有的流
                Call gz.Close()

                Return ms
            Case Else
                Throw New NotImplementedException(metadata.encoding)
        End Select
    End Function

    Private Function loadNrrdHeader() As Header
        Dim read As New StreamReader(file, System.Text.Encoding.ASCII)
        Dim magic As String = read.ReadLine
        Dim line As Value(Of String) = ""
        Dim metadata As NamedValue(Of String)
        Dim header As New Header With {
            .magicNumber = [Enum].Parse(GetType(MagicNumber), magic),
            .metadata = New Dictionary(Of String, String)
        }

        Do While Not (line = read.ReadLine).StringEmpty
            If line.First = "#"c Then
                comments.Add(line)
            Else
                metadata = line.GetTagValue(":", trim:=True)
                header.add(metadata.Name, metadata.Value)
            End If
        Loop

        scan0 = file.Position

        Return header
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects)
                Call file.Close()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
            ' TODO: set large fields to null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
    ' Protected Overrides Sub Finalize()
    '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
