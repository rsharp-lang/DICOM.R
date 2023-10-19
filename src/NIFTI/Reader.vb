Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Class Reader : Implements IDisposable

    ReadOnly file As BinaryDataReader

    Public ReadOnly Property headers As Headers

    Dim disposedValue As Boolean

    Sub New(file As BinaryDataReader)
        Me.file = file
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(file As Stream)
        Call Me.New(New BinaryDataReader(file))
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(path As String)
        Call Me.New(path.Open(FileMode.Open, doClear:=False, [readOnly]:=True))
    End Sub

    Public Function LoadHeaders() As Reader
        _headers = New ReaderProvider(file).LoadObject(Of Headers)(offset:=0)
        Return Me
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then              
                Call file.Dispose()
            End If

            disposedValue = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
