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

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function toMetadata() As Metadata
        Return New Metadata With {
            .type = [Enum].Parse(GetType(Types), metadata.TryGetValue("type", [default]:="float")),
            .dimension = Integer.Parse(metadata.TryGetValue("dimension", [default]:=3)),
            .encoding = [Enum].Parse(GetType(Encoding), metadata.TryGetValue("encoding", [default]:="gzip")),
            .endian = If(metadata.TryGetValue("endian", [default]:="little") = "little", ByteOrder.LittleEndian, ByteOrder.BigEndian),
            .sizes = metadata _
                .TryGetValue("sizes", [default]:="0 0 0") _
                .Split(" "c) _
                .Select(AddressOf Integer.Parse) _
                .ToArray,
            .space_origin = Point3D.Parse(metadata.TryGetValue("space origin")),
            .space_directions = metadata _
                .TryGetValue("space directions") _
                .Split _
                .Select(AddressOf Point3D.Parse) _
                .ToArray
        }
    End Function

End Class
