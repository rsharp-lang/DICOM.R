' /*
' ******** NrrdAxisInfo struct
' **
' ** all the information which can sensibly be associated with
' ** one axis of a nrrd.  The only member which MUST be explicitly
' ** set to something meaningful is "size".
' **
' ** If an axis lies conceptually along some direction in an enclosing
' ** space of dimension nrrd->spaceDim, then the first nrrd->spaceDim
' ** entries of spaceDirection[] must be non-NaN, and min, max, spacing,
' ** and units must NOT be set;  thickness, center, and label can still
' ** be used.  The mutual exclusion between axis-aligned and general
' ** direction information is enforced per-axis, not per-array.
' **
' ** The min and max values give the range of positions "represented"
' ** by the samples along this axis.  In node-centering, "min" IS the
' ** position at the lowest index.  In cell-centering, the position at
' ** the lowest index is between min and max (a touch bigger than min,
' ** assuming min < max).
' **
' ** There needs to be a one-to-one correspondence between these variables
' ** and the nrrdAxisInfo* enum (nrrdEnums.h), the per-axis header fields
' ** (see nrrdField* enum in nrrdEnums.h), and the various methods in axis.c
' */
' typedef struct {
' size_t size;              /* number of elements along each axis */
' double spacing;           /* if non-NaN, distance between samples */
' double thickness;         /* if non-NaN, nominal thickness of region
' represented by one sample along the axis. No
' semantics relative to spacing are assumed or
' imposed, and unlike spacing, there is no
' sensible way to alter thickness- it is either
' copied (as with cropping and slicing) or set to
' NaN (when resampled). */
' double min, max;          /* if non-NaN, range of positions spanned by the
' samples on this axis.  Obviously, one can set
' "spacing" to something incompatible with min
' and max: the idea is that only one (min and
' max, or spacing) should be taken to be
' significant at any time. */
' double spaceDirection[NRRD_SPACE_DIM_MAX]; 
' /* the vector, in "space" (as described by
' nrrd->space and/or nrrd->spaceDim), from one
' sample to the next sample along this axis.  It
' is the column vector of the transform from
' index space to "space" space */
' int center;               /* cell vs. node centering (value should be one of
' nrrdCenter{Unknown,Node,Cell} */
' int kind;                 /* what kind of information is along this axis
' (from the nrrdKind* enum) */
' char *label,              /* short info string for each axis */
' *units;                 /* string identifying the unit */
' } NrrdAxisInfo;

' /*
' ******** Nrrd struct
' **
' ** The struct used to wrap around the raw data array
' */
' typedef struct {
' /* 
' ** NECESSARY information describing the main array.  This is
' ** generally set at the same time that either the nrrd is created,
' ** or at the time that the nrrd is wrapped around an existing array 
' */

' void *data;                       /* the data in memory */
' int type;                         /* a value from the nrrdType enum */
' unsigned int dim;                 /* the dimension (rank) of the array */

' /* 
' ** All per-axis specific information
' */
' NrrdAxisInfo axis[NRRD_DIM_MAX];  /* axis[0] is the fastest axis in the scan-
' line ordering, the one who's coordinates
' change the fastest as the elements are
' accessed in the order in which they
' appear in memory */

' /* 
' ** Optional information descriptive of whole array, some of which is
' ** meaningfuly for only some uses of a nrrd
' */
' char *content;                    /* brief account of what this data is */
' char *sampleUnits;                /* units of measurement of the values 
' stored in the array itself (not the 
' array axes and not space coordinates).
' The logical name might be "dataUnits",
' but that's perhaps ambiguous.  Note that
' these units may apply to non-scalar
' kinds (e.g. coefficients of a vector
' have the same units) */
' int space;                        /* from nrrdSpace* enum, and often 
' implies the value of spaceDim */
' unsigned int spaceDim;            /* if non-zero, the dimension of the space
' in which the regular sampling grid
' conceptually lies.  This is a separate
' variable because this dimension can be
' different than the array dimension. 
' The non-zero-ness of this value is in 
' fact the primary indicator that space
' and orientation information is set.
' This identifies the number of entries in
' "origin" and the per-axis "direction"
' vectors that are taken as meaningful */
' char *spaceUnits[NRRD_SPACE_DIM_MAX];
' /* units for coordinates of space */
' double spaceOrigin[NRRD_SPACE_DIM_MAX];
' /* the location of the center the first
' (lowest memory address) array sample,
' regardless of node-vs-cell centering */
' double measurementFrame[NRRD_SPACE_DIM_MAX][NRRD_SPACE_DIM_MAX];
' /* if spaceDim is non-zero, this may store 
' a spaceDim-by-spaceDim matrix which 
' transforms vector/matrix coefficients
' in the "measurement frame" to those in
' the world space described by spaceDim
' (and hopefully space).  Coeff [i][j] is
' *column* i & *row* j, which is probably
' the *transpose* of what you expect.
' There are no semantics linking this to
' the "kind" of any axis, for a variety
' of reasons */
' size_t blockSize;                 /* for nrrdTypeBlock, block byte size */
' double oldMin, oldMax;            /* if non-NaN, and if nrrd is of integral
' type, extremal values for the array
' BEFORE it was quantized */
' void *ptr;                        /* never read or set by nrrd; use/abuse
' as you see fit */

' /* 
' ** Comments.  Read from, and written to, header.
' ** The comment array "cmt" is NOT NULL-terminated.
' ** The number of comments is cmtArr->len.
' */
' char **cmt;
' airArray *cmtArr;

' /*
' ** Key-value pairs.
' */
' char **kvp;
' airArray *kvpArr;
' } Nrrd;

