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

Benchmarks:

``` ini

BenchmarkDotNet=v0.13.2, OS=macOS Monterey 12.6 (21G115) [Darwin 21.6.0]
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT AVX2


```
|           Method |  Size |      Mean |    Error |   StdDev |        Gen0 |        Gen1 |       Gen2 |  Allocated |
|----------------- |------ |----------:|---------:|---------:|------------:|------------:|-----------:|-----------:|
| **ExternalSorter** | **100MB** |   **8.484 s** | **0.1686 s** | **0.3736 s** |  **89000.0000** |  **19000.0000** |  **4000.0000** |   **518.1 MB** |
| **ExternalSorter** |   **1GB** | **107.185 s** | **2.1129 s** | **3.9164 s** | **959000.0000** | **202000.0000** | **47000.0000** | **5463.54 MB** |

