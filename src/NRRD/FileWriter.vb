Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Imaging.BitmapImage

Public Module FileWriter

    Public Sub WriteFile(file As BinaryDataWriter, dims As Size, rasters As Image())
        file.ByteOrder = ByteOrder.LittleEndian

        Call file.WriteString(FileReader.MagicBytes)
        Call file.WriteString("# This is an NRRD file: http://teem.sourceforge.net/nrrd/format.html")
        Call file.WriteString("# This data is a rawdata file generated from the R# language DICOM image library.")
        Call file.WriteString($"type: float")
        Call file.WriteString($"dimension: 3")
        Call file.WriteString($"sizes: {dims.Width} {dims.Height} {rasters.Length}")
        Call file.WriteString($"encoding: raw")
        Call file.WriteString($"endian: little")
        Call file.WriteString($"space directions: (1,0,0) (0,1,0) (0,0,1)")
        Call file.WriteString($"space origin: (0, 0, 0)")
        Call file.WriteString("")
        Call file.Seek(5, SeekOrigin.Current)

        For Each image As Image In rasters
            Dim buffer As BitmapBuffer = BitmapBuffer.FromImage(image)
            Dim colors As Color() = buffer.GetPixelsAll.ToArray
            Dim intensity As Single() = colors.Select(Function(c) CSng(c.GrayScale)).ToArray

            Call file.Write(intensity)
        Next
    End Sub

    <Extension>
    Private Sub WriteString(file As BinaryDataWriter, str As String)
        Call file.Write(str & vbLf, BinaryStringFormat.NoPrefixOrTermination)
    End Sub

End Module
