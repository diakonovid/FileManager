# External Sorting Algorithm
External sorting algorithm written in C# that can handle massive amounts of data. External sorting is required when the data being sorted do not fit into the main memory of a computing device (usually RAM) and instead they must reside in the slower external memory, usually a disk drive. Thus, external sorting algorithms are external memory algorithms and thus applicable in the external memory model of computation.

Application creates and sorts files with the following format:

    41. at lorem
    5040. posuere nulla enim
    2. at lorem
    32. nulla ut sit quam massa
    2. ipsum aliquam cursus

Sorting result:

    2. at lorem
    41. at lorem
    2. ipsum aliquam cursus
    32. nulla ut sit quam massa
    5040. posuere nulla enim
