Public Enum DataTypes As Short
    <BitPix(0)> unknown = 0
    <BitPix(1)> bool = 1
    <BitPix(8)> unsigned_char = 2
    <BitPix(16)> signed_short = 4
    <BitPix(32)> signed_int = 8
    <BitPix(32)> float = 16
    <BitPix(64)> complex = 32
    <BitPix(64)> [double] = 64
    <BitPix(24)> rgb = 128
    <BitPix(0)> all = 255
    <BitPix(8)> signed_char = 256
    <BitPix(16)> unsigned_short = 512
    <BitPix(32)> unsigned_int = 768
    <BitPix(64)> long_long = 1024
    <BitPix(64)> unsigned_long_long = 1280
    <BitPix(128)> long_double = 1536
    <BitPix(128)> double_pair = 1792
    <BitPix(256)> long_double_pair = 2048
    <BitPix(32)> rgba = 2304
End Enum

<AttributeUsage(AttributeTargets.Field, AllowMultiple:=False)>
Public Class BitPixAttribute : Inherits Attribute

    Public ReadOnly Property Bits As Integer

    Sub New(bits As Integer)
        Me.Bits = bits
    End Sub

    Public Overrides Function ToString() As String
        Return $"{Bits} bits"
    End Function
End Class

