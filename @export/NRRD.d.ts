// export R# package module type define for javascript/typescript language
//
//    imports "NRRD" from "DICOM";
//
// ref=DICOM.nrrdFile@DICOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * ## Nearly Raw Raster Data
 *  
 *  Nrrd is a library and file format designed to support scientific 
 *  visualization and image processing involving N-dimensional raster 
 *  data. Nrrd stands for "nearly raw raster data". Besides dimensional
 *  generality, nrrd is flexible with respect to type (8 integral 
 *  types, 2 floating point types), encoding of written files (raw, 
 *  ascii, hex, or gzip or bzip2 compression), and endianness (the byte 
 *  order of data is explicitly recorded when the type or encoding expose
 *  it). Besides the NRRD format, the library can read and write PNG, PPM, 
 *  and PGM images, as well as some VTK "STRUCTURED_POINTS" datasets. 
 *  
 *  About two dozen operations on raster data are implemented, including
 *  simple things like quantizing, slicing, and cropping, as well as 
 *  fancier things like projection, histogram equalization, and filtered
 *  resampling (up and down) with arbitrary seperable kernels.
 * 
*/
declare namespace NRRD {
   module as {
      /**
        * @param skip_zero default value Is ``true``.
      */
      function pointCloud(raster: object, skip_zero?: boolean): object;
      /**
        * @param skip_zero default value Is ``true``.
      */
      function pointMatrix(raster: object, skip_zero?: boolean): object;
   }
   /**
   */
   function getRaster(nrrd: object): object;
   /**
   */
   function getRasterLayer(raster: object, layer: object): object;
   /**
    * get nrrd header metadata
    * 
    * 
     * @param nrrd -
   */
   function metadata(nrrd: object): object;
   /**
    * open a nrrd @``T:SMRUCC.DICOM.NRRD.FileReader``
    * 
    * 
     * @param file -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function nrrdRead(file: any, env?: object): object;
   module write {
      /**
       * create a nrrd file based on a given collection of the image data objects.
       * 
       * > the required **`rasters`** data collection element could be one of the:
       * >  
       * >  1. the gdi+ @``T:System.Drawing.Image`` data object
       * >  2. the @``T:Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.RasterMatrix`` for do heatmap rendering
       * >  
       * >  for a collection with only one ratser object inside, 2d nrrd object will be generates,
       * >  for a collection with multiple raster object inside, 3d nrrd object will be generates.
       * 
        * @param file -
        * @param rasters a collection of the image data objects. all of the raster object inside
        *  this given collection should be in the same dimension size!
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function nrrd(file: any, rasters: any, env?: object): any;
      /**
       * create a file writer session for save large raster data collection
       * 
       * 
        * @param file -
        * @param z 
        * + default value Is ``1``.
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function nrrd_session(file: any, dims: any, z?: object, env?: object): object;
      /**
       * write the NRRD raster data to PLY point cloud model
       * 
       * 
        * @param raster A 3 space dimension NRRD raster object
        * @param file file to the ply file target
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function ply(raster: object, file: any, env?: object): any;
   }
}
