require(DICOM);

imports "NRRD" from "DICOM";

let file = nrrdRead("G:\demo\3D\single_ions\413.0958.nrrd");

str(as.list(NRRD::metadata(file)));

