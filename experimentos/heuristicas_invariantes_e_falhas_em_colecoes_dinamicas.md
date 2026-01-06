# Heurísticas, Invariantes e Falhas em Coleções Dinâmicas em C#

## Resumo

Este mini artigo descreve uma falha real encontrada durante a implementação de uma coleção genérica baseada em vetor (`CListaVet<T>`). A partir de testes práticos, foi possível demonstrar como o uso inadequado de heurísticas de crescimento pode violar invariantes internas da estrutura e resultar em exceções difíceis de diagnosticar, como `IndexOutOfRangeException`, em vez de falhas mais explícitas como `OutOfMemoryException`.

O objetivo é explicar o caminho completo até a descoberta do problema, incluindo o código de teste, as modificações realizadas e as conclusões técnicas obtidas.

---

## 1. Contexto

Durante o desenvolvimento de uma coleção dinâmica própria, surgiu a curiosidade sobre o motivo pelo qual coleções como a `List<T>` da BCL do dotnet sempre chamam métodos internos de cálculo de capacidade e realocação passando a quantidade atual de elementos + 1, em vez de aplicar diretamente uma estratégia de crescimento como a quantidade de elementos * 2.

Essa dúvida levou à experimentação prática e, eventualmente, à descoberta de um bug lógico causado por overflow aritmético e quebra de invariantes.

---

## 2. Código de teste que revelou o problema

O erro foi identificado ao executar o seguinte cenário:

- Criar uma lista baseada em vetor com capacidade máxima (`Array.MaxLength`)
- Preenchê-la completamente
- Tentar adicionar mais elementos, inclusive via adição em lote (`AdicionaFaixa`)
```csharp
int a = Array.MaxLength;
CListaVet<byte> lista = new CListaVet<byte>(a);

for (int i = 0; i < a; i++)
    lista.Adiciona(48);

lista.Adiciona(48);
```
Inicialmente, o erro observado foi:

```
OutOfMemoryException
```

Esse comportamento era esperado, pois o sistema tentava alocar um vetor maior do que o permitido.

---

## 3. Primeira modificação: mudança na estratégia de crescimento

O método Adiciona foi modificado para chamar o redimensionamento da seguinte forma:
```csharp
Redimensiona(CalcularCapacidade(_quantidade * 2));
```

A intenção era aplicar diretamente a heurística clássica de dobrar a capacidade.

Após essa mudança, o erro passou a ser outro:

```
IndexOutOfRangeException
```

Essa mudança de comportamento levantou a suspeita de que o problema não estava apenas na falta de memória, mas na lógica interna da coleção.

---

## 4. Análise do overflow

Quando `_quantidade` atinge valores próximos a `Array.MaxLength`, a expressão:

```csharp
_quantidade * 2
```

sofre **overflow de `int`**, resultando em um valor negativo. Como o C# permite overflow silencioso em contexto `unchecked`, esse valor inválido propagou-se pelo sistema.

---

## 5. O método Redimensiona e a quebra de invariantes

O método de redimensionamento é implementado da seguinte forma:

```csharp
private void Redimensiona(int tamanho)
{
    if (tamanho != _itens.Length)
    {
        if (tamanho > 0)
        {
            T[] novoItens = new T[tamanho];
            for (int i = 0; i < _quantidade; i++)
                novoItens[i] = _itens[i];

            _itens = novoItens;
        }
        else
            _itens = s_vetorVazio;
    }
}
```

Quando `tamanho` tornou-se negativo devido ao overflow:

- O código entrou no `else`
- `_itens` passou a apontar para um vetor vazio
- `_quantidade` permaneceu maior que zero

Isso resultou em um **estado inválido da coleção**.

---

## 6. Invariantes violadas

Uma coleção baseada em vetor deve manter invariantes como:

- `_quantidade >= 0`
- `_quantidade <= _itens.Length`
- Se `_quantidade > 0`, então `_itens.Length > 0`

Após o overflow, a terceira invariante foi quebrada, levando a falhas posteriores ao tentar acessar o vetor interno.

---

## 7. Por que o erro mudou de OutOfMemory para IndexOutOfRange?

- **Antes**: o código tentava alocar um vetor maior que o permitido → falha imediata (`OutOfMemoryException`)
- **Depois**: o overflow gerou um tamanho inválido → realocação incorreta → erro ao acessar índices

Ou seja, a falha passou a ocorrer **depois**, mascarando a causa original.

---

## 8. Abordagem correta

O método que adiciona elementos deve expressar apenas a **necessidade mínima**:

```csharp
Redimensiona(CalcularCapacidade(_quantidade + 1));
```

Cabe ao método `CalcularCapacidade`:

- Aplicar heurísticas de crescimento
- Tratar overflow com segurança
- Respeitar `Array.MaxLength`
- Garantir que invariantes sejam preservadas

Essa é exatamente a abordagem adotada por bibliotecas de coleções padrão nas linguagens de programação.

---

## 9. Conclusão

Este experimento prático mostra que:

- Heurísticas são úteis, mas perigosas se expostas diretamente.
- Invariantes devem ser preservadas a todo custo.
- Overflow aritmético pode gerar falhas silenciosas e estados inválidos.
- A separação entre necessidade mínima e **estratégia de crescimento** é essencial.

A pergunta inicial — “por que em coleções padrão das linguagens de programação passa-se a quantidade de elementos + 1?” — revelou um princípio fundamental de design de coleções robustas.

---

## 10. Frase-chave

> **Nunca passe heurísticas para métodos de contrato. Passe apenas a necessidade mínima.**

