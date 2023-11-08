require(DICOM);
require(graphics2D);

setwd(@dir);

const nrrd = NRRD::nrrdRead("../data/brain1_image.nrrd");
const header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);

bitmap(file = "./raster_image_heatmap.png", size = [1024,1024]);

graphics2D::rasterHeatmap(raster, colorName = "viridis");

dev.off();
