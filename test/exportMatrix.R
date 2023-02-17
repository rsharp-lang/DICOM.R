require(NRRD);

setwd(@dir);

nrrd   = NRRD::nrrdRead("..\\data\\stent.nrrd");
raster = NRRD::getRaster(nrrd);

write.csv(as.pointMatrix(raster), file = "./raster.csv", row.names = TRUE);