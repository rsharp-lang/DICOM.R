Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Imaging.Drawing3D

Public Class Header

    Public Property magicNumber As MagicNumber
    Public Property metadata As Dictionary(Of String, String)

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Sub add(name As String, value As String)
        metadata.Add(name, value)
    End Sub

    Public Function toMetadata() As Metadata
        Dim type As Types = BytesBuffer.parseNRRDType(descriptor:=metadata.TryGetValue("type", [default]:="float"))
        Dim sizes As Integer() = metadata _
            .TryGetValue("sizes", [default]:="0 0 0") _
            .Split(" "c) _
            .Select(AddressOf Integer.Parse) _
            .ToArray
        Dim origin = Point3D.Parse(metadata.TryGetValue("space origin"))
        Dim dirs = metadata _
            .TryGetValue("space directions", [default]:="") _
            .Split _
            .Select(AddressOf Point3D.Parse) _
            .ToArray

        Return New Metadata With {
            .type = type,
            .dimension = Integer.Parse(metadata.TryGetValue("dimension", [default]:=3)),
            .encoding = [Enum].Parse(GetType(Encoding), metadata.TryGetValue("encoding", [default]:="gzip")),
            .endian = If(metadata.TryGetValue("endian", [default]:="little") = "little", ByteOrder.LittleEndian, ByteOrder.BigEndian),
            .sizes = sizes,
            .space_origin = origin,
            .space_directions = dirs
        }
    End Function

End Class
