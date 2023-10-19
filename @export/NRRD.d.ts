// export R# package module type define for javascript/typescript language
//
//    imports "NRRD" from "DICOM";
//
// ref=DICOM.nrrdFile@DICOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
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
        * @param env default value Is ``null``.
      */
      function ply(raster: object, file: any, env?: object): any;
   }
}
