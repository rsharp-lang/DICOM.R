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
        headers = New ReaderProvider(file).LoadObject(Of Headers)()
        Return Me
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: �ͷ��й�״̬(�йܶ���)
                Call file.Dispose()
            End If

            ' TODO: �ͷ�δ�йܵ���Դ(δ�йܵĶ���)����д�ս���
            ' TODO: �������ֶ�����Ϊ null
            disposedValue = True
        End If
    End Sub

    ' ' TODO: ������Dispose(disposing As Boolean)��ӵ�������ͷ�δ�й���Դ�Ĵ���ʱ������ս���
    ' Protected Overrides Sub Finalize()
    '     ' ��Ҫ���Ĵ˴��롣�뽫���������롰Dispose(disposing As Boolean)��������
    '     Dispose(disposing:=False)
    '     MyBase.Finalize()
    ' End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' ��Ҫ���Ĵ˴��롣�뽫���������롰Dispose(disposing As Boolean)��������
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
