require(NRRD);

setwd(@dir);

nrrd = NRRD::nrrdRead("..\data\foolf.nrrd");
header = as.list(NRRD::metadata(nrrd));

str(header);

raster = NRRD::getRaster(nrrd);

print(raster);

bitmap(file = "./raster_image_heatmap.png");

rasterHeatmap(raster, colorName = "jet");

dev.off();
