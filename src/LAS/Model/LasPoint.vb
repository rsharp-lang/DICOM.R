Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing3D
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Model

    Public Structure LasPoint : Implements PointF3D, IPointCloud

        Public Property X As Double Implements Layout2D.X
        Public Property Y As Double Implements Layout2D.Y
        Public Property Z As Double Implements PointF3D.Z
        Public Property intensity As UShort
        Public Property scanFlag As Byte
        Public Property [class] As Byte
        Public Property scanAngleRank As Byte
        Public Property userdata As Byte
        Public Property pointSourceID As UShort
        Public Property GPSTime As Double

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

            Return $"[{X},{Y},{Z}] {intensity} {metadata.GetJson}"
        End Function

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Shared Narrowing Operator CType(lp As LasPoint) As Point3D
            Return New Point3D(lp)
        End Operator

    End Structure

End Namespace
