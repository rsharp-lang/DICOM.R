require(DICOM);

let source = list.files("C:\Users\Administrator\AppData\Local\Allen Institute\Brain Explorer 2\Atlases\Allen Mouse Brain Atlas\Spaces\P56\AtlasSlices", pattern = "*.jpg");
let n = as.integer(basename(source));

source = source[order(n)];

print(source);

let images = sapply(source, path -> readImage(path));
let output = file(`${@dir}/../data/mouseBrain.nrrd`);

write.nrrd(output, images);

close(output);