# BinaryXml
Binary Format Xml Reader &amp; Writer 
---

##### XML 파일 자체가 매우 무겁고 커서 최적화가 필요하지만, 데이터 로딩 로직를 XDocument 사용과 비슷하게 유지하고 싶을 경우에 사용 가능한 라이브러리입니다.

##### 전체 XML파일 내용을 1회 순회하면서 각 노드의 이름을 인덱싱하여 따로 저장하고, 노드 구조는 읽을 때 시퀸스하게 읽기만 해도 각 노드의 부모/자식 관계를 정의할 수 있도록 했습니다. 

##### 아래 함수를 사용하여 .xml 파일로부터 .bxml 파일 생성 가능합니다.
- BXmlDocument.LoadFromXml
- BXmlDocument.LoadFromXmlFile

아이디어가 생각나고 집에서 먼저 구현 시도를 해본 뒤에 회사에 먼저 적용하였고, 기억나는대로 다시 구현해보는 중입니다.
다른 곳에서는 크게 사용할 곳은 없고 회사 내 클라이언트의 XML 로딩 속도를 최적화하는데 사용했습니다 (속도 50% 가량 감소, 임시 메모리 할당을 최소화해 메모리 파편화 감소)

## 주의
* Attribute 와 Element 만 보존하며 주석, 공백, 줄바꿈 등의 요소는 모두 제거됩니다.
* UTF-8 인코딩만 사용됩니다.
* 네임스페이스 등 고급 XML 기능은 지원하지 않습니다.

```

BenchmarkDotNet v0.13.10, Windows 10 (10.0.19045.3803/22H2/2022Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK 7.0.400
  [Host]     : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 7.0.10 (7.0.1023.36312), X64 RyuJIT AVX2


```
| Method                 | Mean      | Error     | StdDev    | Gen0      | Gen1      | Gen2      | Allocated |
|----------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|
| Benchmark_XDocument    | 73.736 ms | 1.3151 ms | 1.6151 ms | 3571.4286 | 3428.5714 | 1285.7143 |  20.63 MB |
| Benchmark_BXmlDocument |  3.175 ms | 0.0289 ms | 0.0242 ms |   15.6250 |   15.6250 |   15.6250 |   7.82 MB |
