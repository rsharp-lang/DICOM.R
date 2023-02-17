# NRRD

## Nearly Raw Raster Data
 
 Nrrd is a library and file format designed to support scientific 
 visualization and image processing involving N-dimensional raster 
 data. Nrrd stands for "nearly raw raster data". Besides dimensional
 generality, nrrd is flexible with respect to type (8 integral 
 types, 2 floating point types), encoding of written files (raw, 
 ascii, hex, or gzip or bzip2 compression), and endianness (the byte 
 order of data is explicitly recorded when the type or encoding expose
 it). Besides the NRRD format, the library can read and write PNG, PPM, 
 and PGM images, as well as some VTK "STRUCTURED_POINTS" datasets. 
 
 About two dozen operations on raster data are implemented, including
 simple things like quantizing, slicing, and cropping, as well as 
 fancier things like projection, histogram equalization, and filtered
 resampling (up and down) with arbitrary seperable kernels.

+ [nrrdRead](NRRD/nrrdRead.1) 
+ [metadata](NRRD/metadata.1) 
+ [getRaster](NRRD/getRaster.1) 
+ [getRasterLayer](NRRD/getRasterLayer.1) 
+ [as.pointCloud](NRRD/as.pointCloud.1) 
+ [as.pointMatrix](NRRD/as.pointMatrix.1) 
+ [write.ply](NRRD/write.ply.1) write the NRRD raster data to PLY point cloud model
