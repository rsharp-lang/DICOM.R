Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports SMRUCC.DICOM.NRRD
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

Public Class WriteNrrdFunction : Inherits RDefaultFunction
    Implements IDisposable

    ReadOnly session As FileWriterSession

    Private disposedValue As Boolean

    Sub New(session As FileWriterSession)
        Me.session = session
    End Sub

    <RDefaultFunction>
    Public Function save(raster As Object, Optional env As Environment = Nothing) As Object
        If raster Is Nothing Then
            Return Nothing
        End If

        If TypeOf raster Is RasterMatrix Then
            Call session.WriterRasterLayer(DirectCast(raster, RasterMatrix))
        Else
            Return Message.InCompatibleType(GetType(RasterMatrix), raster.GetType, env)
        End If

        Return Nothing
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call session.Dispose()
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
