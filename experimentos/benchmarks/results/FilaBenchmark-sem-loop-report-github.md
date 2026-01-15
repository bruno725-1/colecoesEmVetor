```

BenchmarkDotNet v0.15.8, Windows 10 (10.0.19045.6466/22H2/2022Update)
Intel Core i5-4300M CPU 2.60GHz (Haswell), 1 CPU, 4 logical and 2 physical cores
.NET SDK 9.0.307
  [Host]     : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3


```
| Method | Mean      | Error     | StdDev    |
|------- |----------:|----------:|----------:|
| Modulo | 7.3466 ns | 0.0228 ns | 0.0202 ns |
| IfWrap | 0.2824 ns | 0.0076 ns | 0.0067 ns |
