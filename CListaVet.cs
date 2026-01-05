using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Implementa uma lista de tamanho variável que usa um vetor interno para armazenar os itens.
/// A lista tem uma capacidade, que é o comprimento do vetor interno.
/// A medida que elementos são adicionados à lista, a capacidade dela aumenta automaticamente conforme necessário, realocando o vetor interno.
/// </summary>
public class CListaVet<T> : IEnumerable<T>, ICollection<T>
{
    private T[] _itens; // vetor que armazena os itens da lista
    private int _quantidade; // número de elementos que a lista contém
    private uint _versao; // atributo para impedir modificações durante loops foreach
    private static readonly T[] s_vetorVazio = new T[0]; // o campo itens de listas vazias sempre apontará para este vetor

    /// <summary>
    /// Constrói uma lista inicialmente vazia com capacidade para 0 elementos.
    /// Ao adicionar o primeiro elemento, a capacidade da lista aumenta para 6, e nas próximas realocações, dobra
    /// </summary>
    public CListaVet()
    {
        _itens = s_vetorVazio;
    }

    /// <summary>
    /// Constrói uma lista com uma capacidade definida.
    /// A lista tem espaço para armazenar o número de elementos especificado antes que qualquer realocação seja necessária.
    /// </summary>
    /// <param name="tamanho"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o tamanho é negativo.
    /// </exception>
    public CListaVet(int tamanho)
    {
        if (tamanho < 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "A capacidade da lista não pode ser um número negativo.");

        if (tamanho == 0)
            _itens = s_vetorVazio;
        else
            _itens = new T[tamanho];
    }

    /// <summary>
    /// Constrói uma lista, copiando o conteúdo de uma coleção fornecida.
    /// A capacidade da nova lista será igual a quantidade de itens da coleção.
    /// </summary>
    /// <param name="colecao"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CListaVet(IEnumerable<T> colecao)
    {
        if (colecao == null)
            throw new ArgumentNullException(nameof(colecao), "A coleção a copiar não pode ser nula.");

        if (colecao is ICollection<T> c)
        {
            int tamanho = c.Count;
            if (tamanho == 0)
                _itens = s_vetorVazio;
            else
            {
                _itens = new T[tamanho];
                c.CopyTo(_itens, 0);
                _quantidade = tamanho;
            }
        }
        else
        {
            _itens = s_vetorVazio;
            foreach (T item in colecao)
                Adiciona(item);
        }
    }

    private int CalcularCapacidade(int capacidade)
    {
        int novaCapacidade = _quantidade == 0 ? 6 : _quantidade * 2;
        if (novaCapacidade > Array.MaxLength) novaCapacidade = Array.MaxLength;
        if (novaCapacidade < capacidade) novaCapacidade = capacidade;
        return novaCapacidade;
    }

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

    // altera ou retorna o valor de um índice especificado
    public T this[int posicao]
    {
        get
        {
            if (posicao < 0 || posicao >= _quantidade)
                throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");
            return _itens[posicao];
        }
        set
        {
            if (posicao < 0 || posicao >= _quantidade)
                throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");
            _itens[posicao] = value;
            _versao++;
        }
    }

    public bool Vazia() => _quantidade == 0;

    public void Adiciona(T elemento)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));

        _itens[_quantidade] = elemento;
        _quantidade++;
        _versao++;
    }

    public void CortarExcessos() => Redimensiona(_quantidade);

    public bool Remove(T elemento)
    {
        for (int i = 0; i < _quantidade; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_itens[i], elemento))
            {
                for (int j = i; j < _quantidade - 1; j++)
                    _itens[j] = _itens[j + 1]; //desloca os elementos para a esquerda a partir da posição removida.
                //libera a referência da cópia do último item para o GC (muito útil para objetos do tipo referência)
                _itens[_quantidade - 1] = default!;
                _quantidade--;
                _versao++;
                return true;
            }
        }
        return false;
    }

    public void RemoveIndice(int posicao)
    {
        if (posicao < 0 || posicao >= _quantidade)
            throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");

        for (int i = posicao; i < _quantidade - 1; i++)
            _itens[i] = _itens[i + 1]; //desloca os elementos para a esquerda a partir da posição removida.
        _itens[_quantidade - 1] = default!;
        _quantidade--;
        _versao++;
    }

    public void InsereIndice(T elemento, int posicao)
    {
        if (posicao < 0 || posicao > _quantidade)
            throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor ou igual a quantidade de itens da lista.");

        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));
        for (int i = _quantidade - 1; i >= posicao; i--)
            _itens[i + 1] = _itens[i];
        _itens[posicao] = elemento;
        _quantidade++;
        _versao++;
    }

    public void Inverte()
    {
        int inicio = 0;
        int fim = _quantidade - 1;
        while (inicio < fim)
        {
            // Troca os elementos das posições 'inicio' e 'fim'
            T temp = _itens[inicio];
            _itens[inicio] = _itens[fim];
            _itens[fim] = temp;
            inicio++;
            fim--;
        }
        _versao++;
    }

    public T[] ParaVetor()
    {
        T[] vetor = new T[_quantidade];
        for (int i = 0; i < _quantidade; i++)
            vetor[i] = _itens[i];
        return vetor;
    }

    public void InsereAntesDe(T elementoAInserir, T referencia)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));
        for (int i = 0; i < _quantidade; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_itens[i], referencia))
            {
                // desloca os elementos a partir de i uma posição à direita
                for (int j = _quantidade - 1; j >= i; j--)
                    _itens[j + 1] = _itens[j];
                _itens[i] = elementoAInserir; // insere exatamente na posição do elemento de referência
                _quantidade++;
                _versao++;
                return;
            }
        }
        throw new ArgumentException("O elemento de referência especificado não foi encontrado na lista.", nameof(referencia));
    }

    public void InsereDepoisDe(T elementoAInserir, T referencia)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));
        for (int i = 0; i < _quantidade; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_itens[i], referencia))
            {
                // desloca todos os elementos a partir de i+1
                for (int j = _quantidade - 1; j > i; j--)
                    _itens[j + 1] = _itens[j];
                _itens[i + 1] = elementoAInserir;
                _quantidade++;
                _versao++;
                return;
            }
        }
        throw new ArgumentException("O elemento de referência especificado não foi encontrado na lista.", nameof(referencia));
    }

    public void Ordenar()
    {
        if (_quantidade > 1)
            QuickSort(0, _quantidade - 1);

        _versao++;
    }

    private void QuickSort(int esq, int dir)
    {
        if (esq < dir)
        {
            int p = ParticionaHoare(esq, dir); // índice de separação dos subconjuntos do array
            QuickSort(esq, p); // lado esquerdo
            QuickSort(p + 1, dir); // lado direito
        }
    }

    private int ParticionaHoare(int esq, int dir)
    {
        T pivot = _itens[(esq + dir) / 2];
        int i = esq - 1;
        int j = dir + 1;
        while (true)
        {
            do { i++; } while (Comparer<T>.Default.Compare(_itens[i], pivot) < 0);
            do { j--; } while (Comparer<T>.Default.Compare(_itens[j], pivot) > 0);

            if (i >= j)
                return j;

            // swap
            T temp = _itens[i];
            _itens[i] = _itens[j];
            _itens[j] = temp;
        }
    }

    public int PrimeiraOcorrenciaDe(T elemento)
    {
        for (int i = 0; i < _quantidade; i++)
            if (EqualityComparer<T>.Default.Equals(_itens[i], elemento)) return i;

        return -1;
    }

    public int UltimaOcorrenciaDe(T elemento)
    {
        int ocorrencia = 0;
        bool achou = false;
        for (int i = 0; i < _quantidade; i++)
        {
            if (EqualityComparer<T>.Default.Equals(_itens[i], elemento))
            {
                achou = true;
                ocorrencia = i; //armazena a posição do elemento se for encontrado
            }
        }
        if (!achou) return -1;
        return ocorrencia;
    }

    public bool Contem(T elemento)
    {
        bool achou = false;
        for (int i = 0; i < _quantidade && !achou; i++)
            achou = EqualityComparer<T>.Default.Equals(_itens[i], elemento);
        return achou;
    }

    public void Limpar()
    {
        int limite = _quantidade;
        for (int i = 0; i < limite; i++)
        {
            _itens[i] = default!;
            _quantidade--;
        }
        _versao++;
    }

    public void AdicionaFaixa(IEnumerable<T> colecao)
    {
        if (colecao == null)
            throw new ArgumentNullException(nameof(colecao), "A coleção a adicionar não pode ser nula.");

        if (colecao is ICollection<T> c)
        {
            int tamanho = c.Count;
            if (_quantidade + tamanho > _itens.Length)
                Redimensiona(CalcularCapacidade(_quantidade + tamanho));
            c.CopyTo(_itens, _quantidade);
            _quantidade += tamanho;
            _versao++;
        }
        else
        {
            foreach (T item in colecao)
                Adiciona(item);
        }
    }

    public int Quantidade => _quantidade;

    public int Capacidade
    {
        get => _itens.Length;
        set
        {
            if (value < _quantidade)
                throw new ArgumentOutOfRangeException(nameof(value), "A nova capacidade não pode ser menor que a quantidade de itens.");

            Redimensiona(value);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        uint versao = _versao;
        for (int i = 0; i < _quantidade; i++)
        {
            if (versao != _versao)
                throw new InvalidOperationException("A lista foi modificada. Enumeração cancelada.");

            yield return _itens[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Implementação explícita da interface ICollection (métodos são acessíveis apenas via interface)
    void ICollection<T>.Add(T item) => Adiciona(item);

    void ICollection<T>.Clear() => Limpar();

    bool ICollection<T>.Contains(T item) => Contem(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        for (int i = 0; i < _quantidade; i++)
            array[arrayIndex + i] = _itens[i];
    }

    int ICollection<T>.Count => _quantidade;

    bool ICollection<T>.IsReadOnly => false;
}