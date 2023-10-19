Imports Microsoft.VisualBasic.Data.IO

''' <summary>
''' Overview of the header structure
''' 
''' In order to keep compatibility with the analyze format, 
''' the size of the nifti header was maintained at 348 bytes
''' as in the old format. Some fields were reused, some were 
''' preserved, but ignored, and some were entirely 
''' overwritten. 
''' </summary>
Public Class Headers

    ''' <summary>
    ''' Size of the header. Must be 348 (bytes).
    ''' 
    ''' The field int sizeof_hdr stores the size of the header. 
    ''' It must be 348 for a nifti or analyze format.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' offset 0B size 4B 
    ''' </remarks>
    <Field(0)> Public Property sizeof_hdr As Integer

    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	data_type[10] offset 4B size 10B
    ''' </remarks>
    <Field(1, N:=10)> Public Property data_type As String
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	db_name[18] 14B	18B
    ''' </remarks>
    <Field(2, N:=18)> Public Property db_name As String
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 32B	4B
    ''' </remarks>
    <Field(3)> Public Property extents As Integer
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 36B	2B
    ''' </remarks>
    <Field(4)> Public Property session_error As Short
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 38B	1B
    ''' </remarks>
    <Field(5)> Public Property regular As Char

    ''' <summary>
    ''' Encoding directions (phase, frequency, slice).
    ''' 
    ''' The field char dim_info stores, in just one byte, the frequency 
    ''' encoding direction (1, 2 or 3), the phase encoding direction 
    ''' (1, 2 or 3), and the direction in which the volume was sliced 
    ''' during the acquisition (1, 2 or 3). For spiral sequences, frequency 
    ''' and phase encoding are both set as 0. The reason to collapse all
    ''' this information in just one byte was to save space. See also 
    ''' the fields short slice_start, short slice_end, char slice_code 
    ''' and float slice_duration.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 39B	1B
    ''' </remarks>
    <Field(6)> Public Property dim_info As Char
    ''' <summary>
    ''' Data array dimensions.
    ''' 
    ''' Image dimensions
    ''' 
    ''' The field Short Dim[8] contains the size Of the image array. The first 
    ''' element (Dim[0]) contains the number Of dimensions (1-7). If Dim[0] Is 
    ''' Not In this interval, the data Is assumed To have opposite endianness And 
    ''' so, should be Byte-swapped (the nifti standard does Not specify a 
    ''' specific field For endianness, but encourages the use Of Dim[0] For this 
    ''' purpose). The dimensions 1, 2 And 3 are assumed To refer To space (x, y, z), 
    ''' the 4th dimension Is assumed To refer To time, And the remaining 
    ''' dimensions, 5, 6 And 7, can be anything Else. The value Dim[i] Is a 
    ''' positive Integer representing the length Of the i-th dimension.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' short	dim[8] 40B	16B
    ''' </remarks>
    <Field(7, N:=8)> Public Property [dim] As Short()
    ''' <summary>
    ''' 1st intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 56B	4B
    ''' </remarks>
    <Field(8)> Public Property intent_p1 As Single
    ''' <summary>
    ''' 2nd intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 60B	4B
    ''' </remarks>
    <Field(9)> Public Property intent_p2 As Single
    ''' <summary>
    ''' 3rd intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 64B	4B
    ''' </remarks>
    <Field(10)> Public Property intent_p3 As Single
    ''' <summary>
    ''' nifti intent.
    ''' 
    ''' Intent
    ''' The short intent_code Is an integer that codifies that the data
    ''' Is supposed to contain. Some of these codes require extra-parameters, 
    ''' such as the number of degrees of freedom (df). These extra 
    ''' parameters, when needed, can be stored in the fields ``intent_p*`` 
    ''' when they can be applied to the image as a while, Or in the 
    ''' 5th dimension if voxelwise.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 68B	2B
    ''' </remarks>
    <Field(11)> Public Property intent_code As Short
    ''' <summary>
    ''' Data type.
    ''' 
    ''' The field int datatype indicates the type of the data stored.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 70B	2B
    ''' </remarks>
    <Field(12)> Public Property datatype As DataTypes
    ''' <summary>
    ''' Number of bits per voxel.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 72B	2B
    ''' </remarks>
    <Field(13)> Public Property bitpix As Short
    ''' <summary>
    ''' First slice index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 74B	2B
    ''' </remarks>
    <Field(14)> Public Property slice_start As Short
    ''' <summary>
    ''' Grid spacings (unit per dimension).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	pixdim[8]	76B	32B
    ''' </remarks>
    <Field(15, N:=8)> Public Property pixdim As Single()
    ''' <summary>
    ''' Offset into a .nii file.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 108B	4B
    ''' </remarks>
    <Field(16)> Public Property vox_offset As Single
    ''' <summary>
    ''' Data scaling, slope.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 112B	4B
    ''' </remarks>
    <Field(17)> Public Property scl_slope As Single
    ''' <summary>
    ''' Data scaling, offset.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 116B	4B
    ''' </remarks>
    <Field(18)> Public Property scl_inter As Single
    ''' <summary>
    ''' Last slice index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 120B	2B
    ''' </remarks>
    <Field(19)> Public Property slice_end As Short
    ''' <summary>
    ''' Slice timing order.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 122B	1B
    ''' </remarks>
    <Field(20)> Public Property slice_code As Char
    ''' <summary>
    ''' Units of pixdim[1..4].
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 123B	1B
    ''' </remarks>
    <Field(21)> Public Property xyzt_units As Char
    ''' <summary>
    ''' Maximum display intensity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>124B	4B</remarks>
    <Field(22)> Public Property cal_max As Single
    ''' <summary>
    ''' Minimum display intensity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>128B	4B</remarks>
    <Field(23)> Public Property cal_min As Single
    ''' <summary>
    ''' Time for one slice.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>132B	4B</remarks>
    <Field(24)> Public Property slice_duration As Single
    ''' <summary>
    ''' Time axis shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>136B	4B</remarks>
    <Field(25)> Public Property toffset As Single
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>140B	4B</remarks>
    <Field(26)> Public Property glmax As Integer
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>144B	4B</remarks>
    <Field(27)> Public Property glmin As Integer
    ''' <summary>
    ''' Any text.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	descrip[80]	148B	80B
    ''' </remarks>
    <Field(28, N:=80)> Public Property descrip As String
    ''' <summary>
    ''' Auxiliary filename.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	aux_file[24]	228B	24B
    ''' </remarks>
    <Field(29, N:=24)> Public Property aux_file As String
    ''' <summary>
    ''' Use the quaternion fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 252B	2B
    ''' </remarks>
    <Field(30)> Public Property qform_code As Short
    ''' <summary>
    ''' Use of the affine fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>254B	2B</remarks>
    <Field(31)> Public Property sform_code As Short
    ''' <summary>
    ''' Quaternion b parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 256B	4B
    ''' </remarks>
    <Field(32)> Public Property quatern_b As Single
    ''' <summary>
    ''' Quaternion c parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 260B	4B
    ''' </remarks>
    <Field(33)> Public Property quatern_c As Single
    ''' <summary>
    ''' Quaternion d parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>264B	4B</remarks>
    <Field(34)> Public Property quatern_d As Single
    ''' <summary>
    ''' Quaternion x shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>268B	4B</remarks>
    <Field(35)> Public Property qoffset_x As Single
    ''' <summary>
    ''' Quaternion y shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>272B	4B</remarks>
    <Field(36)> Public Property qoffset_y As Single
    ''' <summary>
    ''' Quaternion z shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>276B	4B</remarks>
    <Field(37)> Public Property qoffset_z As Single
    ''' <summary>
    ''' 1st row affine transform
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_x[4]	280B	16B
    ''' </remarks>
    <Field(38, N:=4)> Public Property srow_x As Single()
    ''' <summary>
    ''' 2nd row affine transform.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_y[4]	296B	16B
    ''' </remarks>
    <Field(39, N:=4)> Public Property srow_y As Single()
    ''' <summary>
    ''' 3rd row affine transform.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_z[4]	312B	16B
    ''' </remarks>
    <Field(40, N:=4)> Public Property srow_z As Single()
    ''' <summary>
    ''' Name or meaning of the data.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	intent_name[16]	328B	16B
    ''' </remarks>
    <Field(41, N:=16)> Public Property intent_name As String
    ''' <summary>
    ''' Magic string.
    ''' 
    ''' The char magic[4] field is a “magic” string that declares 
    ''' the file as conforming with the nifti standard. It was 
    ''' placed at the very end of the header to avoid overwriting 
    ''' fields that are needed for the analyze format. Ideally, 
    ''' however, this string should be checked first. It should be 
    ''' 'ni1' (or '6E 69 31 00' in hexadecimal) for .hdr/.img pair, 
    ''' or 'n+1' (or '6E 2B 31 00') for a .nii single file. In the 
    ''' absence of this string, the file should be treated as 
    ''' analyze. Future versions of the nifti format may increment 
    ''' the string to 'n+2', 'n+3', etc. Indeed, as of 2012, a second 
    ''' version is under preparation.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	magic[4]	344B	4B
    ''' </remarks>
    <Field(42, N:=4)> Public Property magic As String

    Public Function Validate() As String
        If sizeof_hdr <> 348 Then
            Return "size of header is mis-matched!"
        End If
        If magic <> "ni1" OrElse
            magic <> "n+1" OrElse
            magic <> "n+2" OrElse
            magic <> "n+3" Then

            Return "invalid magic header string!"
        End If

        Return Nothing
    End Function
End Class

Public Enum dim_info As Byte
    phase = 1
    frequency
    slice
End Enum