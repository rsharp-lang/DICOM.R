setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\stent.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);