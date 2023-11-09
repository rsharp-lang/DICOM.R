Imports System.IO
Imports SMRUCC.DICOM.Laser.Model

Public Class LasReader : Implements IDisposable

    Public Property PointDataFormat As Byte

    Public ReadOnly Property HeaderSize As UShort
    Public ReadOnly Property FileCreationYear As UShort
    Public ReadOnly Property FileCreationDayOfYear As UShort
    Public ReadOnly Property GeneratingSoftware As Char()
    Public ReadOnly Property GuidData4 As Char()
    Public ReadOnly Property GuidData3 As UShort
    Public ReadOnly Property GuidData2 As UShort
    Public ReadOnly Property GuidData1 As UInteger

    ''' <summary>
    ''' scan0 of the point data region
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property OffsetToPointData As UInteger
    Public ReadOnly Property PointDataRecordLength As UShort
    Public ReadOnly Property NumberOfPoints As UInteger
    Public ReadOnly Property VersionMajor As Byte
    Public ReadOnly Property VersionMinor As Byte
    Public ReadOnly Property XScale As Double
    Public ReadOnly Property YScale As Double
    Public ReadOnly Property ZScale As Double
    Public ReadOnly Property XOffset As Double
    Public ReadOnly Property YOffset As Double
    Public ReadOnly Property ZOffset As Double
    Public ReadOnly Property MaxX As Double
    Public ReadOnly Property MinX As Double
    Public ReadOnly Property MaxY As Double
    Public ReadOnly Property MinY As Double
    Public ReadOnly Property MaxZ As Double
    Public ReadOnly Property MinZ As Double

    Dim reader As BinaryReader
    Dim currentPointIndex As Integer = 0
    Dim disposedValue As Boolean

    Public ReadOnly Property sourcefile As String

    Public Sub New(lasfile As String)
        reader = New BinaryReader(lasfile.Open(FileMode.Open, doClear:=False, [readOnly]:=True))
        sourcefile = lasfile

        Call MagicValidates()
        Call ParseHeaders()
        Call Reset()
    End Sub

    Private Sub MagicValidates()
        reader.BaseStream.Seek(0, SeekOrigin.Begin)

        If Not reader.ReadBytes(LasWriter.Magic.Length).SequenceEqual(LasWriter.Magic.Select(Function(c) CByte(Asc(c)))) Then
            Throw New InvalidDataException("Invalid magic header of the target laser file!")
        End If

        reader.BaseStream.Seek(8, SeekOrigin.Begin)
    End Sub

    Private Sub ParseHeaders()
        _GuidData1 = reader.ReadUInt32()
        _GuidData2 = reader.ReadUInt16()
        _GuidData3 = reader.ReadUInt16()
        _GuidData4 = reader.ReadChars(8)
        _VersionMajor = reader.ReadByte()
        _VersionMinor = reader.ReadByte()
        Dim systemIdentifier = reader.ReadChars(32)
        _GeneratingSoftware = reader.ReadChars(32)
        _FileCreationDayOfYear = reader.ReadUInt16()
        _FileCreationYear = reader.ReadUInt16()
        _HeaderSize = reader.ReadUInt16()
        _OffsetToPointData = reader.ReadUInt32()
        Dim numberOfVariable = reader.ReadUInt32()
        PointDataFormat = reader.ReadByte()
        _PointDataRecordLength = reader.ReadUInt16()
        _NumberOfPoints = reader.ReadUInt32()
        Dim numberOfReturn = reader.ReadBytes(20)
        _XScale = reader.ReadDouble()
        _YScale = reader.ReadDouble()
        _ZScale = reader.ReadDouble()
        _XOffset = reader.ReadDouble()
        _YOffset = reader.ReadDouble()
        _ZOffset = reader.ReadDouble()
        _MaxX = reader.ReadDouble()
        _MinX = reader.ReadDouble()
        _MaxY = reader.ReadDouble()
        _MinY = reader.ReadDouble()
        _MaxZ = reader.ReadDouble()
        _MinZ = reader.ReadDouble()
    End Sub

    ''' <summary>
    ''' Reset the file pointer for read the point data from start
    ''' </summary>
    Public Sub Reset()
        currentPointIndex = 0
        reader.BaseStream.Seek(OffsetToPointData, 0)
    End Sub

    Public Function ReadNextPoint() As LasPoint
        currentPointIndex += 1

        If currentPointIndex > NumberOfPoints Then
            Return Nothing
        Else
            Return readPoint()
        End If
    End Function

    Private Function readPoint() As LasPoint
        Dim x As Double = reader.ReadInt32() * XScale + XOffset
        Dim y As Double = reader.ReadInt32() * YScale + YOffset
        Dim z As Double = reader.ReadInt32() * ZScale + ZOffset
        Dim i As UShort = reader.ReadUInt16()
        Dim flag As Byte = reader.ReadByte()
        Dim classification As Byte = reader.ReadByte()
        Dim scanAngleRank As Byte = reader.ReadByte()
        Dim userData As Byte = reader.ReadByte()
        Dim pointSourceId As UShort = reader.ReadUInt16()
        Dim gpsTime As Double = 0

        If PointDataFormat = 1 Then
            gpsTime = reader.ReadDouble()
        End If

        Return New LasPoint() With {
            .intensity = i,
            .X = x,
            .Y = y,
            .Z = z,
            .[class] = classification,
            .scanAngleRank = scanAngleRank,
            .userdata = userData,
            .GPSTime = gpsTime,
            .pointSourceID = pointSourceId,
            .scanFlag = flag
        }
    End Function

    Public Iterator Function ReadAll() As IEnumerable(Of LasPoint)
        Call Reset()

        For i As Integer = 1 To NumberOfPoints
            Yield ReadNextPoint()
        Next
    End Function

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                reader.Dispose()
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

    ''' <summary>
    ''' just close the binary data reader
    ''' </summary>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' 不要更改此代码。请将清理代码放入“Dispose(disposing As Boolean)”方法中
        Dispose(disposing:=True)
        GC.SuppressFinalize(Me)
    End Sub
End Class