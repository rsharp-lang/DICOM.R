Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Class Reader : Implements IDisposable

    ReadOnly file As BinaryDataReader

    Dim headers As Headers

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
        headers = New ReaderProvider(file).LoadObject(Of Headers)(offset:=0)
        Return Me
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
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
