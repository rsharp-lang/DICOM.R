require(NRRD);
require(graphics2D);

nrrd   = NRRD::nrrdRead("..\\data\\stent.nrrd");
header = as.list(NRRD::metadata(nrrd));
raster = NRRD::getRaster(nrrd);

for(i in 1:256) {
    bitmap(file = `./stent/raster__${i}.png`, size = [256,256], fill ="black");
    # draw heatmap
    graphics2D::rasterHeatmap(
        x = raster → NRRD::getRasterLayer(i), 
        colorName = "viridis:turbo"
    );
    dev.off();
}


