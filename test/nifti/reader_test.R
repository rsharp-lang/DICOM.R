require(DICOM);

const demo_file = "F:\Downloads\DS_10283_2861\TS23_EMA83_reference.nii";

let file = NIFTI::open.nifti(file = demo_file);

str(as.list([file]::headers));