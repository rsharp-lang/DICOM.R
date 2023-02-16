require(NRRD);
require(graphics2D);

setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\annotation_25.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);

bitmap(file = "./raster_image_layer.png", size = [1024,1024]);

graphics2D::rasterHeatmap(raster → NRRD::getRasterLayer(200), colorName = "viridis");

dev.off();
