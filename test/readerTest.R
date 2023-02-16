setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\stent.nrrd");
# nrrd = NRRD::nrrdRead("F:\Downloads\NRRD.jl-master\test\io\small_time.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);