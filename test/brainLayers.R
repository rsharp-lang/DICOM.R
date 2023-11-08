require(DICOM);
require(graphics2D);

setwd(@dir);

nrrd = NRRD::nrrdRead("../data/brain1_image.nrrd");
header = as.list(NRRD::metadata(nrrd));
raster = NRRD::getRaster(nrrd);

for(i in 1:25) {
    bitmap(file = `./brain/raster__${i}.png`, size = [256,256], fill ="black");
    # draw heatmap
    graphics2D::rasterHeatmap(
        x = raster → NRRD::getRasterLayer(i), 
        colorName = "viridis:turbo"
    );
    dev.off();
}


