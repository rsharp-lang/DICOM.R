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
    <Field(0)> Public Property extents As Integer
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 36B	2B
    ''' </remarks>
    <Field(0)> Public Property session_error As Short
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 38B	1B
    ''' </remarks>
    <Field(0)> Public Property regular As Char
    ''' <summary>
    ''' Encoding directions (phase, frequency, slice).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 39B	1B
    ''' </remarks>
    <Field(0)> Public Property dim_info As dim_info
    ''' <summary>
    ''' Data array dimensions.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' short	dim[8] 40B	16B
    ''' </remarks>
    <Field(0)> Public Property [dim] As Short()
    ''' <summary>
    ''' 1st intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 56B	4B
    ''' </remarks>
    <Field(0)> Public Property intent_p1 As Single
    ''' <summary>
    ''' 2nd intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 60B	4B
    ''' </remarks>
    <Field(0)> Public Property intent_p2 As Single
    ''' <summary>
    ''' 3rd intent parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 64B	4B
    ''' </remarks>
    <Field(0)> Public Property intent_p3 As Single
    ''' <summary>
    ''' nifti intent.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 68B	2B
    ''' </remarks>
    <Field(0)> Public Property intent_code As Short
    ''' <summary>
    ''' Data type.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 70B	2B
    ''' </remarks>
    <Field(0)> Public Property datatype As Short
    ''' <summary>
    ''' Number of bits per voxel.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 72B	2B
    ''' </remarks>
    <Field(0)> Public Property bitpix As Short
    ''' <summary>
    ''' First slice index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 74B	2B
    ''' </remarks>
    <Field(0)> Public Property slice_start As Short
    ''' <summary>
    ''' Grid spacings (unit per dimension).
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	pixdim[8]	76B	32B
    ''' </remarks>
    <Field(0)> Public Property pixdim As Single()
    ''' <summary>
    ''' Offset into a .nii file.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 108B	4B
    ''' </remarks>
    <Field(0)> Public Property vox_offset As Single
    ''' <summary>
    ''' Data scaling, slope.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 112B	4B
    ''' </remarks>
    <Field(0)> Public Property scl_slope As Single
    ''' <summary>
    ''' Data scaling, offset.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 116B	4B
    ''' </remarks>
    <Field(0)> Public Property scl_inter As Single
    ''' <summary>
    ''' Last slice index.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 120B	2B
    ''' </remarks>
    <Field(0)> Public Property slice_end As Short
    ''' <summary>
    ''' Slice timing order.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 122B	1B
    ''' </remarks>
    <Field(0)> Public Property slice_code As Char
    ''' <summary>
    ''' Units of pixdim[1..4].
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 123B	1B
    ''' </remarks>
    <Field(0)> Public Property xyzt_units As Char
    ''' <summary>
    ''' Maximum display intensity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>124B	4B</remarks>
    <Field(0)> Public Property cal_max As Single
    ''' <summary>
    ''' Minimum display intensity.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>128B	4B</remarks>
    <Field(0)> Public Property cal_min As Single
    ''' <summary>
    ''' Time for one slice.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>132B	4B</remarks>
    <Field(0)> Public Property slice_duration As Single
    ''' <summary>
    ''' Time axis shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>136B	4B</remarks>
    <Field(0)> Public Property toffset As Single
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>140B	4B</remarks>
    <Field(0)> Public Property glmax As Integer
    ''' <summary>
    ''' Not used; compatibility with analyze.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>144B	4B</remarks>
    <Field(0)> Public Property glmin As Integer
    ''' <summary>
    ''' Any text.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	descrip[80]	148B	80B
    ''' </remarks>
    <Field(0)> Public Property descrip As String
    ''' <summary>
    ''' Auxiliary filename.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	aux_file[24]	228B	24B
    ''' </remarks>
    <Field(0)> Public Property aux_file As String
    ''' <summary>
    ''' Use the quaternion fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 252B	2B
    ''' </remarks>
    <Field(0)> Public Property qform_code As Short
    ''' <summary>
    ''' Use of the affine fields.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>254B	2B</remarks>
    <Field(0)> Public Property sform_code As Short
    ''' <summary>
    ''' Quaternion b parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 256B	4B
    ''' </remarks>
    <Field(0)> Public Property quatern_b As Single
    ''' <summary>
    ''' Quaternion c parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' 260B	4B
    ''' </remarks>
    <Field(0)> Public Property quatern_c As Single
    ''' <summary>
    ''' Quaternion d parameter.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>264B	4B</remarks>
    <Field(0)> Public Property quatern_d As Single
    ''' <summary>
    ''' Quaternion x shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>268B	4B</remarks>
    <Field(0)> Public Property qoffset_x As Single
    ''' <summary>
    ''' Quaternion y shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>272B	4B</remarks>
    <Field(0)> Public Property qoffset_y As Single
    ''' <summary>
    ''' Quaternion z shift.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>276B	4B</remarks>
    <Field(0)> Public Property qoffset_z As Single
    ''' <summary>
    ''' 1st row affine transform
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_x[4]	280B	16B
    ''' </remarks>
    <Field(0)> Public Property srow_x As Single()
    ''' <summary>
    ''' 2nd row affine transform.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_y[4]	296B	16B
    ''' </remarks>
    <Field(0)> Public Property srow_y As Single()
    ''' <summary>
    ''' 3rd row affine transform.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' float	srow_z[4]	312B	16B
    ''' </remarks>
    <Field(0)> Public Property srow_z As Single()
    ''' <summary>
    ''' Name or meaning of the data.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	intent_name[16]	328B	16B
    ''' </remarks>
    <Field(0)> Public Property intent_name As String
    ''' <summary>
    ''' Magic string.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>
    ''' char	magic[4]	344B	4B
    ''' </remarks>
    <Field(0)> Public Property magic As String

End Class

Public Enum dim_info As Byte
    phase
    frequency
    slice
End Enum