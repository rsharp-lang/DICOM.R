require(NRRD);
require(graphics2D);

setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\foolf.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);

bitmap(file = "./raster_image_heatmap.png", size = [1024,1024]);

graphics2D::rasterHeatmap(raster, colorName = "jet");

dev.off();
