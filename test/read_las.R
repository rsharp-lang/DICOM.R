require(DICOM);

setwd(@dir);

let las = LASer::open("F:\Downloads\Potree\Potree\pointclouds\lion_takanawa_las\data\r16.las"); #= LASer::open("./brain1_image.las");
let pointCloud #= las |> points();

 = las |> points();