Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports SMRUCC.DICOM.LASer.Model

Public Class LasWriter : Implements IDisposable

    ReadOnly _xScale As Double
    ReadOnly _yScale As Double
    ReadOnly _zScale As Double
    ReadOnly _xOffset As Double
    ReadOnly _yOffset As Double
    ReadOnly _zOffset As Double
    ReadOnly _versionMajor As Byte
    ReadOnly _versionMinor As Byte

    ReadOnly pointDataFormat As Byte

    Dim binaryWriter As BinaryWriter

    ReadOnly x As New DoubleRange With {.Min = Double.MaxValue, .Max = Double.MinValue}
    ReadOnly y As New DoubleRange With {.Min = Double.MaxValue, .Max = Double.MinValue}
    ReadOnly z As New DoubleRange With {.Min = Double.MaxValue, .Max = Double.MinValue}

    Dim numberofpoints As Integer = 0
    Dim disposedValue As Boolean

    Public Const software As String = "DICOM/LASer@SMRUCC             "
    Public Const Magic As String = "LASF"

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Sub New(lasfile As String,
            Optional xScale As Double = 0.0001,
            Optional yScale As Double = 0.0001,
            Optional zScale As Double = 0.0001,
            Optional xOffset As Double = 0,
            Optional yOffset As Double = 0,
            Optional zOffset As Double = 0,
            Optional versionMajor As Byte = 1,
            Optional versionMinor As Byte = 1,
            Optional pointDataFormat As Byte = 1)

        Call Me.New(
            lasfile:=lasfile.Open(FileMode.OpenOrCreate, doClear:=True, [readOnly]:=False),
            xScale:=xScale, yScale:=yScale, zScale:=zScale,
            xOffset:=xOffset, yOffset:=yOffset, zOffset:=zOffset,
            versionMajor:=versionMajor, versionMinor:=versionMinor,
            pointDataFormat:=pointDataFormat
        )
    End Sub

    Public Sub New(lasfile As Stream,
                   Optional xScale As Double = 0.0001,
                   Optional yScale As Double = 0.0001,
                   Optional zScale As Double = 0.0001,
                   Optional xOffset As Double = 0,
                   Optional yOffset As Double = 0,
                   Optional zOffset As Double = 0,
                   Optional versionMajor As Byte = 1,
                   Optional versionMinor As Byte = 1,
                   Optional pointDataFormat As Byte = 1)

        If versionMajor <> 1 Then
            Throw New Exception("given VersionMajor is not supported yet")
        End If
        If versionMinor <> 1 Then
            Throw New Exception("given versionMinor is not supported yet")
        End If
        If pointDataFormat <> 0 AndAlso pointDataFormat <> 1 Then
            Throw New Exception("given pointDataFormat is not supported yet")
        End If

        _xScale = xScale
        _yScale = yScale
        _zScale = zScale
        _xOffset = xOffset
        _yOffset = yOffset
        _zOffset = zOffset
        _versionMajor = versionMajor
        _versionMinor = versionMinor

        Me.pointDataFormat = pointDataFormat

        binaryWriter = New BinaryWriter(lasfile, Encoding.ASCII)
        binaryWriter.Write(Encoding.ASCII.GetBytes(Magic))

        ' write the place holder
        Call HeaderPlaceholder()
    End Sub

    Private Sub HeaderPlaceholder()
        Const fileSourceId As UShort = 0
        Const reserved As UShort = 0
        Const prjidGuid1 As UInteger = 0
        Const prjidGuid2 As UShort = 0
        Const prjidGuid3 As UShort = 0
        ' 8 bytes
        Const prjidGuid4 = "        "
        ' 32 bytes
        Const systemIdentifier = "                                "

        Dim fileCreationDayOfYear As UShort = Date.Now.DayOfYear
        Dim fileCreationYear As UShort = Date.Now.Year

        Const headerSize As UShort = 227
        Const offsetToPointData = 227
        Const numberOfVariable = 0

        binaryWriter.Write(fileSourceId)
        binaryWriter.Write(reserved)
        binaryWriter.Write(prjidGuid1)
        binaryWriter.Write(prjidGuid2)
        binaryWriter.Write(prjidGuid3)
        binaryWriter.Write(prjidGuid4.Select(Function(c) CByte(Asc(c))).ToArray)
        binaryWriter.Write(_versionMajor)
        binaryWriter.Write(_versionMinor)
        binaryWriter.Write(systemIdentifier.MD5.Select(Function(c) CByte(Asc(c))).ToArray)
        binaryWriter.Write(software)
        binaryWriter.Write(fileCreationDayOfYear)
        binaryWriter.Write(fileCreationYear)
        binaryWriter.Write(headerSize)
        binaryWriter.Write(offsetToPointData)
        binaryWriter.Write(numberOfVariable)
        binaryWriter.Write(pointDataFormat)

        Select Case pointDataFormat
            Case 0 : binaryWriter.Write(CUShort(20))
            Case 1 : binaryWriter.Write(CUShort(28))
        End Select

        binaryWriter.Write(numberofpoints)
        binaryWriter.Write(0)
        binaryWriter.Write(0)
        binaryWriter.Write(0)
        binaryWriter.Write(0)
        binaryWriter.Write(0)

        binaryWriter.Write(_xScale)
        binaryWriter.Write(_yScale)
        binaryWriter.Write(_zScale)
        binaryWriter.Write(_xOffset)
        binaryWriter.Write(_yOffset)
        binaryWriter.Write(_zOffset)

        binaryWriter.Write(x.Max)
        binaryWriter.Write(x.Min)
        binaryWriter.Write(y.Max)
        binaryWriter.Write(y.Min)
        binaryWriter.Write(z.Max)
        binaryWriter.Write(z.Min)
    End Sub

    ''' <summary>
    ''' Write a point to the binary data file
    ''' </summary>
    ''' <param name="lasPoint"></param>
    Public Sub WritePoint(lasPoint As LasPoint)
        numberofpoints += 1

        x.Max = Math.Max(lasPoint.X, x.Max)
        x.Min = Math.Min(lasPoint.X, x.Min)
        y.Max = Math.Max(lasPoint.Y, y.Max)
        y.Min = Math.Min(lasPoint.Y, y.Min)
        z.Max = Math.Max(lasPoint.Z, z.Max)
        z.Min = Math.Min(lasPoint.Z, z.Min)

        binaryWriter.Write(Convert.ToInt32((lasPoint.X - _xOffset) / _xScale))
        binaryWriter.Write(Convert.ToInt32((lasPoint.Y - _yOffset) / _yScale))
        binaryWriter.Write(Convert.ToInt32((lasPoint.Z - _zOffset) / _zScale))
        binaryWriter.Write(lasPoint.intensity)

        binaryWriter.Write(lasPoint.scanFlag)
        binaryWriter.Write(lasPoint.class)
        binaryWriter.Write(lasPoint.ScanAngleRank)
        binaryWriter.Write(lasPoint.userdata)
        binaryWriter.Write(lasPoint.pointSourceID)

        If pointDataFormat = 1 Then
            binaryWriter.Write(lasPoint.GPSTime)
        End If
    End Sub

    ''' <summary>
    ''' flush and save the model metadata
    ''' </summary>
    Public Sub Flush()
        Dim s = binaryWriter.BaseStream

        ' move file pointer to the header region
        s.Seek(107, SeekOrigin.Begin)
        s.Write(BitConverter.GetBytes(numberofpoints), 0, 4)
        s.Seek(179, SeekOrigin.Begin)
        s.Write(BitConverter.GetBytes(x.Max), 0, 8)
        s.Write(BitConverter.GetBytes(x.Min), 0, 8)
        s.Write(BitConverter.GetBytes(y.Max), 0, 8)
        s.Write(BitConverter.GetBytes(y.Min), 0, 8)
        s.Write(BitConverter.GetBytes(z.Max), 0, 8)
        s.Write(BitConverter.GetBytes(z.Min), 0, 8)
        s.Flush()
    End Sub

    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: 释放托管状态(托管对象)
                Call Flush()
                Call binaryWriter.Close()
                Call binaryWriter.Dispose()
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