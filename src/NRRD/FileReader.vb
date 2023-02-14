Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Language

Public Class FileReader : Implements IDisposable

    ReadOnly file As Stream
    ReadOnly header As Header

    Public ReadOnly Property NrddHeader As Metadata
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Get
            Return header.ToMetadata
        End Get
    End Property

    Dim disposedValue As Boolean
    Dim scan0 As Long

    Sub New(file As Stream)
        Me.file = file
        Me.header = loadNrrdHeader()
    End Sub

    Private Function loadNrrdHeader() As Header
        Dim read As New StreamReader(file)
        Dim magic As String = read.ReadLine
        Dim line As Value(Of String) = ""
        Dim metadata As NamedValue(Of String)
        Dim header As New Header With {
            .magicNumber = [Enum].Parse(GetType(MagicNumber), magic),
            .metadata = New Dictionary(Of String, String)
        }

        Do While Not (line = read.ReadLine).StringEmpty
            metadata = line.Value.GetTagValue(":", trim:=True)
            header.metadata(metadata.Name) = metadata.Value
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
