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
