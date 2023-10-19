Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO

Public Class Reader : Implements IDisposable

    ReadOnly file As BinaryDataReader

    Public ReadOnly Property headers As Headers

    Public ReadOnly Property ext As String
        Get
            If headers Is Nothing Then
                Return Nothing
            ElseIf headers.magic = "ni1" Then
                Return ".hdr"
            Else
                Return ".nii"
            End If
        End Get
    End Property

    Dim _msg As String

    ''' <summary>
    ''' Storing extra-information
    ''' 
    ''' Extra information can be included in the nifti format in a number 
    ''' of ways as allowed by the standard. At the end of the header, the 
    ''' next 4 bytes (i.e., from byte 349 to 352, inclusive) may or may 
    ''' not be present in a .hdr file. However, these bytes will always 
    ''' be present in a .nii file. They should be interpreted as a character 
    ''' array, i.e. char extension[4]. In principle, these 4 bytes should 
    ''' be all set to zero. If the first, extension[0], is non-zero, this 
    ''' indicates the presence of extended information beginning at the 
    ''' byte number 353. Such extended information needs to have size multiple
    ''' of 16. The first 8 bytes of this extension should be interpreted 
    ''' as two integers, int esize and int ecode. The field esize indicates
    ''' the size of the extent, including the first 8 bytes that are the 
    ''' esize and ecode themselves. The field ecode indicates the format 
    ''' used for the remaining of the extension. At the time of this 
    ''' writing, three codes have been defined:
    ''' 
    ''' + 0	Unknown. This code should be avoided.
    ''' + 2	dicom extensions
    ''' + 4	xml extensions used by the afni software package.
    ''' 
    ''' More than one extension can be present in the same file, each one 
    ''' always starting with the pair esize and ecode, and with its first 
    ''' byte immediately past the last byte of the previous extension. In 
    ''' a single .nii file, the float vox_offset must be set properly so 
    ''' that the imaging data begins only after the end of the last 
    ''' extension.
    ''' </summary>
    Dim _extra As Byte()

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
        _msg = _headers.Validate

        If ext = ".nii" Then
            _extra = file.ReadBytes(4)
        End If

        Return Me
    End Function

    Public Overrides Function ToString() As String
        Return $"{file.BaseStream}: {_msg}"
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
