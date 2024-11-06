require(DICOM);
require(graphics2D);

imports "NRRD" from "DICOM";

let file = nrrdRead("G:\demo\3D\single_ions\413.0958.nrrd");

str(as.list(NRRD::metadata(file)));

let model_3d = getRaster(file);

let image = getRasterLayer(model_3d, layer = 250);

bitmap(file = `${@dir}/raster__.png`, size = [image]::dimensionSize, fill ="black");
# draw heatmap
graphics2D::rasterHeatmap(
    x = image, 
    colorName = "viridis:turbo"
);
dev.off();