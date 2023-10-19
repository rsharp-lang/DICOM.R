// export R# package module type define for javascript/typescript language
//
//    imports "NIFTI" from "DICOM";
//
// ref=DICOM.niftiFile@DICOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace NIFTI {
   module open {
      /**
        * @param env default value Is ``null``.
      */
      function nifti(file: any, env?: object): object;
   }
}
