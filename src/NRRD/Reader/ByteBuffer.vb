Imports Microsoft.VisualBasic.Data.IO

Public Module BytesBuffer

    Public Function parseNRRDType(descriptor As String) As Types
        Select Case descriptor.ToLower
            Case "signed char", "int8", "int8_t"
                Return Types.int8
            Case "uchar", "unsigned char", "uint8", "uint8_t"
                Return Types.uint8
            Case "short", "short int", "signed short", "signed short int", "int16", "int16_t"
                Return Types.int16
            Case "ushort", "unsigned short", "unsigned short int", "uint16", "uint16_t"
                Return Types.uint16
            Case "int", "signed int", "int32", "int32_t"
                Return Types.int32
            Case "uint", "unsigned int", "uint32", "uint32_t"
                Return Types.uint32
            Case "longlong", "long long", "long long int", "signed long long", "signed long long int", "int64", "int64_t"
                Return Types.int64
            Case "ulonglong", "unsigned long long", "unsigned long long int", "uint64", "uint64_t"
                Return Types.uint64
            Case "float"
                Return Types.float
            Case "double"
                Return Types.double
            Case "block"
                Return Types.block
            Case Else
                Throw New NotImplementedException("Unrecognized NRRD type: " & descriptor)
        End Select
    End Function

    Public Function parseNRRDRawData(buffer As BinaryDataReader, options As Metadata) As Array
        Dim totalLen As Integer = 1
        Dim sizes As Integer() = options.sizes
        Dim type As Types = options.type

        For Each int As Integer In sizes
            totalLen *= int
        Next

        If Type = Types.block Then
            ' Don't do anything special, just return the slice containing all blocks.
            Return buffer.ReadBytes(totalLen * options.blockSize)
        Else
            buffer.ByteOrder = options.endian
        End If

        Select Case Type
            Case Types.int8 : Return buffer.ReadSBytes(totalLen)
            Case Types.uint8 : Return buffer.ReadBytes(totalLen)
            Case Types.int16 : Return buffer.ReadInt16s(totalLen)
            Case Types.uint16 : Return buffer.ReadUInt16s(totalLen)
            Case Types.int32 : Return buffer.ReadInt32s(totalLen)
            Case Types.uint32 : Return buffer.ReadUInt32s(totalLen)
            Case Types.float : Return buffer.ReadSingles(totalLen)
            Case Types.double : Return buffer.ReadDoubles(totalLen)
            Case Else
                Throw New NotImplementedException(Type.Description)
        End Select
    End Function

End Module
