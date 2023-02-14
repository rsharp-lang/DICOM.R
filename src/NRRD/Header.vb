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

Public Class Metadata

    Public Property type As Types
    Public Property dimension As Integer
    Public Property sizes As Integer()
    Public Property encoding As Encoding
    ''' <summary>
    ''' little/big
    ''' </summary>
    ''' <returns></returns>
    Public Property endian As ByteOrder
    Public Property space_directions As Point3D()
    Public Property space_origin As Point3D

End Class

Public Enum Encoding
    raw
    txt
    text = txt
    ascii = txt
    hex
    gz
    gzip = gz
    bz2
    bzip2 = bz2
End Enum

Public Enum Types
    int
    float
    ''' <summary>
    ''' unsigned char
    ''' </summary>
    uchar
    block
End Enum

Public Enum MagicNumber

    ''' <summary>
    ''' NRRD0001: (and NRRD00.01 for circa 1998 files) original and most basic version
    ''' </summary>
    NRRD0001
    ''' <summary>
    ''' NRRD0002: added key/value pairs
    ''' </summary>
    NRRD0002
    ''' <summary>
    ''' NRRD0003: added "kinds:" field,
    ''' </summary>
    NRRD0003
    ''' <summary>
    ''' NRRD0004: added "thicknesses:" and "sample units" fields, general space 
    ''' and orientation information ("space:", "space dimension:", "space 
    ''' directions:", "space origin:", and "space units:" fields) , and the ability 
    ''' for the "data file:" field to identify multiple data files.
    ''' </summary>
    NRRD0004
    ''' <summary>
    ''' NRRD0005: added "measurement frame:" field (should have been figured out for NRRD0004).
    ''' </summary>
    NRRD0005

End Enum