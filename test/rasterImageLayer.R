require(NRRD);
require(graphics2D);

setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\stent.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);

bitmap(file = "./raster_image_layer.png", size = [1024,1024], fill ="black");

graphics2D::rasterHeatmap(raster → NRRD::getRasterLayer(100), colorName = "viridis");

dev.off();
