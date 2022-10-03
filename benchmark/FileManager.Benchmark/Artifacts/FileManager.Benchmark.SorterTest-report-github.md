``` ini

BenchmarkDotNet=v0.13.2, OS=macOS Monterey 12.6 (21G115) [Darwin 21.6.0]
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT AVX2
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT AVX2


```
|         Method |  Size |     Mean |    Error |   StdDev |        Gen0 |        Gen1 |       Gen2 |  Allocated |
|--------------- |------ |---------:|---------:|---------:|------------:|------------:|-----------:|-----------:|
| **ExternalSorter** | **100MB** |  **3.694 s** | **0.0737 s** | **0.1081 s** |  **92000.0000** |  **22000.0000** |  **7000.0000** |  **753.84 MB** |
| **ExternalSorter** |   **1GB** | **49.044 s** | **0.7675 s** | **0.6409 s** | **988000.0000** | **287000.0000** | **61000.0000** | **8095.44 MB** |
