// export R# package module type define for javascript/typescript language
//
//    imports "LASer" from "DICOM";
//
// ref=DICOM.LASerFile@DICOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace LASer {
   module as {
      /**
       * try to cast the raw raster object to las point cloud data
       * 
       * 
        * @param raster -
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function las(raster: any, env?: object): object;
   }
   /**
    * Open the file read to the specific las file
    * 
    * 
     * @param lasfile -
   */
   function open(lasfile: string): any;
   /**
    * load all point cloud model data
    * 
    * 
     * @param las -
   */
   function points(las: object): object;
   module write {
      /**
       * 
       * 
        * @param raster -
        * @param file the format version of the las file is 1.2, and
        *  the point data format is 2(RGB) value
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function las(raster: any, file: any, env?: object): any;
   }
}
