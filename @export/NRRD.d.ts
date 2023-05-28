// export R# package module type define for javascript/typescript language
//
//    imports "NRRD" from "NRRD";
//
// ref=NRRD.Rscript@NRRD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

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
   */
   function metadata(nrrd: object): object;
   /**
     * @param env default value Is ``null``.
   */
   function nrrdRead(file: any, env?: object): object;
   module write {
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
