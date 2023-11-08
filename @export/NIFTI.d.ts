// export R# package module type define for javascript/typescript language
//
//    imports "NIFTI" from "DICOM";
//
// ref=DICOM.niftiFile@DICOM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * The Neuroimaging Informatics Technology Initiative (nifti)
 *  file format was envisioned about a decade ago as a replacement 
 *  to the then widespread, yet problematic, analyze 7.5 file 
 *  format.
 * 
*/
declare namespace NIFTI {
   module open {
      /**
        * @param env default value Is ``null``.
      */
      function nifti(file: any, env?: object): object;
   }
}
