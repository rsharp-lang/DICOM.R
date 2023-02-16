Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Values
Imports Microsoft.VisualBasic.Net.Http

Public Class FileReader : Implements IDisposable

    ReadOnly file As Stream
    ReadOnly header As Header
    ReadOnly comments As New List(Of String)
    ReadOnly filePath As String

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

        If TypeOf file Is FileStream Then
            filePath = DirectCast(file, FileStream).Name
        End If
    End Sub

    Public Function LoadRaster() As RasterObject
        Dim options As Metadata = Nothing
        Dim rawdata = loadNrrdRawBuffer(options)
        Dim bytes As New BinaryDataReader(BytesBuffer.checkBufferSize(rawdata, options))
        Dim data As Array

        ' the source stream is loaded from
        ' the nrdd file substream
        ' the actually scan0 is ZERO
        bytes.Seek(0, SeekOrigin.Begin)
        data = BytesBuffer.parseNRRDRawData(bytes, options)

        Return RasterObject.CreateRasterObject(options, data)
    End Function

    Private Function loadNrrdRawBuffer(ByRef metadata As Metadata) As MemoryStream
        Dim size As Integer = file.Length - scan0 - 1
        Dim bytes As Byte() = New Byte(size - 1) {}

        ' get raster data reader options
        metadata = header.toMetadata

        file.Seek(scan0 + 1, SeekOrigin.Begin)
        file.Read(bytes, 0, bytes.Length)

        Select Case metadata.encoding
            Case Encoding.raw : Return New MemoryStream(bytes)
            Case Encoding.gzip, Encoding.gz
                Return bytes.UnGzipStream
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
        Dim readSize As New List(Of String)

        Do While Not (line = read.ReadLine).StringEmpty
            Call readSize.Add(line)

            If line.First = "#"c Then
                comments.Add(line)
            Else
                metadata = line.GetTagValue(":", trim:=True)
                header.add(metadata.Name, metadata.Value)
            End If
        Loop

        ' 20230216
        ' evaluate the actual text offset
        ' the streamReader has a bug about buffer size
        ' it could cause the incorrect stream position
        scan0 = Aggregate str As String
                In readSize
                Let chars = str.Length + 2
                Into Sum(chars)

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
