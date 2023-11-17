# BinaryXml
Binary Format Xml Reader &amp; Writer 
---

##### XML 파일 자체가 매우 무겁고 커서 최적화가 필요하지만, 데이터 로딩 로직를 XDocument 사용과 비슷하게 유지하고 싶을 경우에 사용 가능한 라이브러리입니다.

##### 전체 XML파일 내용을 1회 순회하면서 각 노드의 이름을 인덱싱하여 따로 저장하고, 노드 구조는 읽을 때 시퀸스하게 읽기만 해도 각 노드의 부모/자식 관계를 정의할 수 있도록 했습니다. 

##### 아래 함수를 사용하여 .xml 파일로부터 .bxml 파일 생성 가능합니다.
- BXmlDocument.LoadFromXml
- BXmlDocument.LoadFromXmlFile


##### 파일 구조

[Name->Index Table]
[SequenceNode]
    ㄴ flag, childPtr, childCount, attrPtr, attrCount
[SequenceNode]
[SequenceNode]
...
[SequenceNode]

```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3570/22H2/2022Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.400
  [Host]     : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2


```
| Method                 | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2      | Allocated |
|----------------------- |---------:|---------:|---------:|----------:|----------:|----------:|----------:|
| Benchmark_XDocument    | 74.43 ms | 1.482 ms | 1.386 ms | 3571.4286 | 3428.5714 | 1285.7143 |  20.63 MB |
| Benchmark_BXmlDocument | 51.77 ms | 0.995 ms | 1.106 ms | 2700.0000 | 2600.0000 | 1000.0000 |  18.32 MB |



진행 중~
