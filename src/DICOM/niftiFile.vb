
Imports System.IO
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData
Imports SMRUCC.DICOM.NIFTI
Imports SMRUCC.Rsharp.Runtime
Imports SMRUCC.Rsharp.Runtime.Components
Imports SMRUCC.Rsharp.Runtime.Interop

''' <summary>
''' The Neuroimaging Informatics Technology Initiative (nifti)
''' file format was envisioned about a decade ago as a replacement 
''' to the then widespread, yet problematic, analyze 7.5 file 
''' format.
''' </summary>
<Package("NIFTI")>
Module niftiFile

    <ExportAPI("open.nifti")>
    <RApiReturn(GetType(Reader))>
    Public Function open(<RRawVectorArgument> file As Object, Optional env As Environment = Nothing) As Object
        Dim buf = SMRUCC.Rsharp.GetFileStream(file, FileAccess.Read, env)

        If buf Like GetType(Message) Then
            Return buf.TryCast(Of Message)
        End If

        Return New Reader(buf.TryCast(Of Stream)).LoadHeaders
    End Function
End Module
