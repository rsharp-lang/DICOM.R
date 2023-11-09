require(DICOM);

let nrrd = nrrdRead("D:\NRRD\data\brain1_image.nrrd");
let raster = getRaster(nrrd);

setwd(@dir);

write.las(raster, file = "./brain1_image.las");