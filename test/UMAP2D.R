imports ["dataset", "umap"] from "MLkit";

setwd(@dir);

const filename as string = "./raster.csv";
const MNIST_LabelledVectorArray = `./${filename}`
|> read.csv(row.names = 1, check.names = FALSE)
;
const tags as string = rownames(MNIST_LabelledVectorArray);
const kdtree_metric as boolean = FALSE;

bitmap(file = `./MNIST-LabelledVectorArray-50000x100.umap_scatter${ifelse(kdtree_metric, "_kdtree_KNN", "")}.png`, size = [6000,4000]) {
	const t1 = now();
	const manifold = umap(MNIST_LabelledVectorArray,
		dimension         = 2, 
		numberOfNeighbors = 60,
		localConnectivity = 1,
		KnnIter           = 64,
		bandwidth         = 1,
		debug             = TRUE,
		KDsearch          = kdtree_metric
	)
	;
	const t2 = now();
	
	print(t2 - t1);
	
	manifold$umap
	|> as.data.frame
	|> write.csv( 
		file      = `./raster-umap2d.csv`, 
		row.names = tags
	);
	
	plot(manifold$umap,
		show_bubble = FALSE,
		point_size  = 50,
		legendlabel = "font-style: normal; font-size: 24; font-family: Bookman Old Style;",
		padding     = "padding:150px 150px 350px 350px;"
	);
}
