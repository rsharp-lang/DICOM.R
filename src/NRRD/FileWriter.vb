Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Math.LinearAlgebra

Public Module FileWriter

    Public Sub WriteFile(file As BinaryDataWriter, dims As Size, rasters As Image())
        Call WriteAsciiHeaderCommon(file, dims, len:=rasters.Length)

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

    Private Sub WriteAsciiHeaderCommon(file As BinaryDataWriter, dims As Size, len As Integer)
        file.ByteOrder = ByteOrder.LittleEndian

        Call file.WriteString(FileReader.MagicBytes)
        Call file.WriteString("# This is an NRRD file: http://teem.sourceforge.net/nrrd/format.html")
        Call file.WriteString("# This data is a rawdata file generated from the R# language DICOM image library.")
        Call file.WriteString($"type: float")
        Call file.WriteString($"dimension: 3")
        Call file.WriteString($"sizes: {dims.Width} {dims.Height} {len}")
        Call file.WriteString($"encoding: raw")
        Call file.WriteString($"endian: little")
        Call file.WriteString($"space directions: (1,0,0) (0,1,0) (0,0,1)")
        Call file.WriteString($"space origin: (0, 0, 0)")
        Call file.WriteString("")
        Call file.Seek(5, SeekOrigin.Current)
    End Sub

    Public Sub WriteFile(file As BinaryDataWriter, dims As Size, rasters As RasterMatrix())
        Dim intensity As Single()

        Call WriteAsciiHeaderCommon(file, dims, len:=rasters.Length)

        For Each matrix As RasterMatrix In rasters
            For Each row As Vector In matrix.GetRowScans
                intensity = row.AsSingle
                file.Write(intensity)
            Next
        Next
    End Sub

End Module
