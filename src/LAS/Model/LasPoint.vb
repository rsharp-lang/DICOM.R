Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Model

    ''' <summary>
    ''' the LASer POINT DATA RECORD
    ''' </summary>
    Public Structure LasPoint : Implements PointF3D, IPointCloud

        Public Property X As Double Implements Layout2D.X
        Public Property Y As Double Implements Layout2D.Y
        Public Property Z As Double Implements PointF3D.Z
        Public Property intensity As UShort
        ''' <summary>
        ''' 8 bits
        ''' 
        ''' + Return Number 3 bits (bits 0, 1, 2) 3 bits * 
        ''' + Number of Returns (given pulse) 3 bits (bits 3, 4, 5) 3 bits *
        ''' + Scan Direction Flag 1 bit (bit 6) 1 bit *
        ''' + Edge of Flight Line 1 bit (bit 7) 1 bit *
        ''' </summary>
        ''' <returns></returns>
        Public Property scanFlag As Byte
        Public Property [class] As Byte
        Public Property scanAngleRank As Byte
        Public Property userdata As Byte
        Public Property pointSourceID As UShort
        Public Property GPSTime As Double
        ''' <summary>
        ''' format 2 and 3
        ''' </summary>
        ''' <returns></returns>
        Public Property rgb As RGBColor

        Private Property IPointCloud_intensity As Double Implements IPointCloud.intensity
            Get
                Return CDbl(intensity)
            End Get
            Set(value As Double)
                intensity = CUShort(value)
            End Set
        End Property

        Public Overrides Function ToString() As String
            Dim metadata As New Dictionary(Of String, Double) From {
                {NameOf(scanFlag), scanFlag},
                {NameOf([class]), [class]},
                {NameOf(scanAngleRank), scanAngleRank},
                {NameOf(userdata), userdata},
                {NameOf(pointSourceID), pointSourceID},
                {NameOf(GPSTime), GPSTime}
            }

            Return $"[{X},{Y},{Z}] {intensity} {rgb.ToString} {metadata.GetJson}"
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(lp As LasPoint) As Point3D
            Return New Point3D(lp)
        End Operator

    End Structure

End Namespace
