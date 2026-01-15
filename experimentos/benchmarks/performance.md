## Decisão de implementação: wrap-around de índices

Esta fila utiliza a seguinte lógica para avanço de índices:

```csharp
temp = indice + 1;
if(temp == _itens.Length)
    temp = 0;

indice = temp;
```
ao invés de:

```csharp
indice = (indice + 1) % _itens.Length;
```

Justificativa:

Benchmarks realizados com BenchmarkDotNet mostram que, para capacidades
não potência de 2 (ex: 6), o uso de % resulta em custo significativamente maior,
por envolver divisão inteira no hot path da estrutura.
Evidência
Ver resultados completos em:
results/FilaBenchmark-loop-report-github.md

