using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Implementa uma classe que pode funcionar como fila e pilha ao mesmo tempo, permitindo que operações como adição e remoção de itens sejam feitas em ambas as extremidades.
/// Internamente, a classe utiliza um vetor circular para armazenar os itens.
/// </summary>
public class CDequeVet<T> : ICollection<T>
{
    private T[] _itens; // vetor que armazena os itens do deque
    private int _esq; // Ponta esquerda do deque
    private int _dir; // Ponta direita
    private int _quantidade; // Número de itens que o deque contém
    private uint _versao; // Atributo para impedir modificações durante loops foreach
    private static readonly T[] s_vetorVazio = new T[0]; // o campo itens de deques vazios sempre apontará para este vetor

    ///<summary>
    /// Constrói um deque inicialmente vazio com capacidade para 0 elementos.
    /// Ao adicionar o primeiro elemento, a capacidade do deque aumenta para 6, e nas próximas realocações, dobra.
    /// </summary>
    public CDequeVet()
    {
        _itens = s_vetorVazio;
    }

    /// <summary>
    /// Constrói um deque com uma capacidade definida.
    /// O deque tem espaço para armazenar o número de elementos especificado antes que o vetor interno precise ser redimensionado.
    /// </summary>
    public CDequeVet(int tamanho)
    {
        if (tamanho < 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "A capacidade do deque não pode ser um número negativo.");

        if (tamanho == 0)
            _itens = s_vetorVazio;
        else
            _itens = new T[tamanho];
    }

    /// <summary>
    /// Constrói um deque, copiando o conteúdo de uma coleção fornecida.
    /// A capacidade do novo deque será igual a quantidade de itens da coleção.
    /// </summary>
    /// <param name="colecao"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CDequeVet(IEnumerable<T> colecao)
    {
        if (colecao == null)
            throw new ArgumentNullException(nameof(colecao), "A coleção a copiar não pode ser nula.");

        if (colecao is ICollection<T> c)
        {
            int tamanho = c.Count;
            if (tamanho > 0)
            {
                _itens = new T[tamanho];
                c.CopyTo(_itens, 0);
                _quantidade = tamanho;
            }
            else
                _itens = s_vetorVazio;
        }
        else
        {
            _itens = s_vetorVazio;
            foreach (T item in colecao)
                AdicionaDireito(item);
        }
    }

    private void Avancar(ref int indice)
    {
        // Wrap-around feito com comparação ao invés de módulo (%) por desempenho.
        // Benchmarks disponíveis em experimentos/benchmarks/performance.md
        int temp = indice + 1;
        if (temp == _itens.Length)
            temp = 0;

        indice = temp;
    }

    private void Retroceder(ref int indice)
    {
        int temp = indice - 1;
        if (temp < 0)
            temp = _itens.Length - 1;

        indice = temp;
    }

    private int CalcularCapacidade(int capacidade)
    {
        int novaCapacidade = _quantidade == 0 ? 6 : _itens.Length * 2;
        // Permite que o deque cresça o máximo possível, antes de ocorrer overflow.
        // Esta checagem funciona mesmo quando a nova capacidade sofreu overflow, graças ao casting para uint.
        if ((uint)novaCapacidade > Array.MaxLength) novaCapacidade = Array.MaxLength;
        // se a capacidade calculada for menor que o necessário, seta o parâmetro original como nova capacidade.
        // Se a capacidade exceder Array.MaxLength, ocorrerá OutOfMemoryException.
        if (novaCapacidade < capacidade) novaCapacidade = capacidade;
        return novaCapacidade;
    }

    private void Copiar(int indiceFonte, T[] vetorDestino, int indiceDestino, int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
            vetorDestino[indiceDestino + i] = _itens[indiceFonte + i];
    }

    private void Redimensiona(int tamanho)
    {
        if (tamanho != _itens.Length)
        {
            if (tamanho > 0)
            {
                T[] novoItens = new T[tamanho];
                if (_esq < _dir)
                    Copiar(_esq, novoItens, 0, _quantidade);
                else
                {
                    int tamanhoBloco1 = _itens.Length - _esq;
                    Copiar(_esq, novoItens, 0, tamanhoBloco1);
                    Copiar(0, novoItens, tamanhoBloco1, _dir);
                }
                _itens = novoItens;
            }
            else
                _itens = s_vetorVazio;

            // Reconfigurar esq e dir para ficarem compatíveis com o novo vetor
            _esq = 0;
            _dir = _quantidade;
        }
    }

    public void AdicionaDireito(T elemento)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));

        _itens[_dir] = elemento;
        Avancar(ref _dir);
        _quantidade++;
        _versao++;
    }

    public void AdicionaEsquerdo(T elemento)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));

        Retroceder(ref _esq);
        _itens[_esq] = elemento;
        _quantidade++;
        _versao++;
    }

    public T RemoveRetornaEsquerdo()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Deque vazio.");

        T item = _itens[_esq];
        _itens[_esq] = default!;
        Avancar(ref _esq);
        _quantidade--;
        _versao++;
        return item;
    }

    public T RemoveRetornaDireito()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Deque vazio.");

        Retroceder(ref _dir);
        T item = _itens[_dir];
        _itens[_dir] = default!;
        _quantidade--;
        _versao++;
        return item;
    }

    public T RetornaEsquerdo()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Deque vazio.");

        return _itens[_esq];
    }

    public T RetornaDireito()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Deque vazio.");

        int i = _dir;
        Retroceder(ref i);
        return _itens[i];
    }

    // Copia o conteúdo de um deque para um vetor e o retorna.
    // Os itens são copiados da esquerda para a direita.
    public T[] ParaVetor()
    {
        T[] vetor = new T[_quantidade];
        if (_quantidade > 0)
        {
            if (_esq < _dir)
                Copiar(_esq, vetor, 0, _quantidade);
            else
            {
                int tamanhoBloco1 = _itens.Length - _esq;
                Copiar(_esq, vetor, 0, tamanhoBloco1);
                Copiar(0, vetor, tamanhoBloco1, _dir);
            }
        }
        return vetor;
    }

    public bool Contem(T elemento)
    {
        // Evita buscas desnecessárias caso o deque esteja vazio
        if (_quantidade == 0)
            return false;

        bool achou = false;
        if (_esq < _dir)
        {
            for (int i = _esq; i < _dir && !achou; i++)
                achou = EqualityComparer<T>.Default.Equals(_itens[i], elemento);
        }
        else
        {
            for (int i = _esq; i < _itens.Length && !achou; i++)
                achou = EqualityComparer<T>.Default.Equals(_itens[i], elemento);
            for (int i = 0; i < _dir && !achou; i++)
                achou = EqualityComparer<T>.Default.Equals(_itens[i], elemento);
        }
        return achou;
    }

    public void Limpar()
    {
        if (_quantidade > 0)
        {
            if (_esq < _dir)
            {
                for (int i = _esq; i < _dir; i++)
                    _itens[i] = default!;
            }
            else
            {
                for (int i = _esq; i < _itens.Length; i++)
                    _itens[i] = default!;
                for (int i = 0; i < _dir; i++)
                    _itens[i] = default!;
            }
            _quantidade = 0;
        }
        _esq = 0;
        _dir = 0;
        _versao++;
    }

    public static CDequeVet<T> ConcatenaDeque(CDequeVet<T> d1, CDequeVet<T> d2)
    {
        if (d1 == null)
            throw new ArgumentNullException(nameof(d1), "Nenhum dos deques a concatenar pode ser nulo.");
        if (d2 == null)
            throw new ArgumentNullException(nameof(d2), "Nenhum dos deques a concatenar pode ser nulo.");

        CDequeVet<T> concatenado = new CDequeVet<T>(checked(d1._quantidade + d2._quantidade));
        foreach (T item in d1)
            concatenado.AdicionaDireito(item);
        foreach (T item in d2)
            concatenado.AdicionaDireito(item);

        return concatenado;
    }

    public void CortarExcessos() => Redimensiona(_quantidade);

    public int Capacidade => _itens.Length;

    public bool EstaVazio => _quantidade == 0;

    public int Quantidade => _quantidade;

    public IEnumerator<T> GetEnumerator()
    {
        if (_quantidade == 0)
            yield break;

        uint versao = _versao;
        if (_esq < _dir)
        {
            for (int i = _esq; i < _dir; i++)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
        }
        else
        {
            for (int i = _esq; i < _itens.Length; i++)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
            for (int i = 0; i < _dir; i++)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Possibilita iteração no deque de forma reversa (da direita para a esquerda)
    public IEnumerable<T> EnumerarReverso()
    {
        if(_quantidade == 0)
            yield break;

        uint versao = _versao;
        if (_esq < _dir)
        {
            for (int i = _dir - 1; i >= _esq; i--)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
        }
        else
        {
            for (int i = _dir - 1; i >= 0; i--)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
            for (int i = _itens.Length - 1; i >= _esq; i--)
            {
                if (versao != _versao)
                    throw new InvalidOperationException("O deque foi modificado. Operação de enumeração cancelada.");

                yield return _itens[i];
            }
        }
    }

    // Implementação explícita da interface ICollection (métodos são acessíveis apenas via interface)
    void ICollection<T>.Add(T item) => AdicionaDireito(item); // Insere o item na ponta direita do deque

    void ICollection<T>.Clear() => Limpar();

    bool ICollection<T>.Contains(T item) => Contem(item);

    int ICollection<T>.Count => _quantidade;

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array), "O array de destino não pode ser nulo.");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "O índice de destino não pode ser negativo.");
        if (array.Length - arrayIndex < _quantidade)
            throw new ArgumentException("O array de destino não possui espaço suficiente.");

        if (_quantidade > 0)
        {
            if (_esq < _dir)
                Copiar(_esq, array, arrayIndex, _quantidade);
            else
            {
                int tamanhoBloco1 = _itens.Length - _esq;
                Copiar(_esq, array, arrayIndex, tamanhoBloco1);
                Copiar(0, array, tamanhoBloco1 + arrayIndex, _dir);
            }
        }
    }

    bool ICollection<T>.IsReadOnly => false;

    bool ICollection<T>.Remove(T item) => throw new NotSupportedException("Não é possível remover itens de forma harbitrária.");
}