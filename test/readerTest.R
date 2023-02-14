

nrrd = NRRD::nrrdRead("E:\NRRD\data\stent.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);
