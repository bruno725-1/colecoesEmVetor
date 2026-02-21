using System;
using System.Collections;
using System.Collections.Generic;
///<summary>
/// Classe CPilhaVet - implementa uma lista LIFO: last-in first-out
/// Internamente, a classe utiliza um vetor para armazenar os itens, então empilhar pode ser o(n), enquanto desempilhar sempre será o(1)
/// </summary>
public class CPilhaVet<T> : IEnumerable<T>, ICollection<T>
{
    private T[] _itens; // vetor que armazena os itens da pilha
    private int _quantidade; // número de itens que a pilha contém
    private uint _versao; // atributo para impedir modificações durante loops foreach
    private static readonly T[] s_vetorVazio = new T[0]; // o campo itens de pilhas vazias sempre apontará para este vetor

    /// <summary>
    /// Constrói uma pilha inicialmente vazia com capacidade para 0 elementos.
    /// Ao adicionar o primeiro elemento, a capacidade da pilha aumenta para 6, e nas próximas realocações, dobra
    /// </summary>
    public CPilhaVet()
    {
        _itens = s_vetorVazio;
    }

    /// <summary>
    /// Constrói uma pilha com uma capacidade definida.
    /// A pilha tem espaço para armazenar o número de elementos especificado antes que qualquer realocação seja necessária.
    /// </summary>
    /// <param name="tamanho"></param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o tamanho é negativo.
    /// </exception>
    public CPilhaVet(int tamanho)
    {
        if (tamanho < 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "A capacidade da pilha não pode ser um número negativo.");

        if (tamanho == 0)
            _itens = s_vetorVazio;
        else
            _itens = new T[tamanho];
    }

    /// <summary>
    /// Constrói uma pilha, copiando o conteúdo de uma coleção fornecida.
    /// A capacidade da nova pilha será igual a quantidade de itens da coleção.
    /// </summary>
    /// <param name="colecao"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CPilhaVet(IEnumerable<T> colecao)
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
                Empilha(item);
        }
    }

    private int CalcularCapacidade(int capacidade)
    {
        int novaCapacidade = _quantidade == 0 ? 6 : _quantidade * 2;
        // Permite que a pilha cresça o máximo possível, antes de ocorrer overflow.
        // Esta checagem funciona mesmo quando a nova capacidade sofreu overflow, graças ao casting para uint.
        if ((uint)novaCapacidade > Array.MaxLength) novaCapacidade = Array.MaxLength;
        // se a capacidade calculada for menor que o necessário, seta o parâmetro original como nova capacidade.
        // Se a capacidade exceder Array.MaxLength, ocorrerá OutOfMemoryException.
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

    public void Empilha(T elemento)
    {
        if (_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));

        _itens[_quantidade] = elemento;
        _quantidade++;
        _versao++;
    }

    public T Desempilha()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Pilha vazia.");

        _quantidade--;
        T item = _itens[_quantidade];
        _itens[_quantidade] = default!;
        _versao++;
        return item;
    }

    public T Peek()
    {
        if (_quantidade == 0)
            throw new InvalidOperationException("Pilha vazia.");

        return _itens[_quantidade - 1];
    }

    public bool Contem(T elemento)
    {
        bool achou = false;
        for (int i = 0; i < _quantidade && !achou; i++)
            achou = EqualityComparer<T>.Default.Equals(_itens[i], elemento);

        return achou;
    }

    public void CortarExcessos() => Redimensiona(_quantidade);

    // Copia a pilha para um vetor, na mesma ordem que os itens seriam desempilhados.
    public T[] ParaVetor()
    {
        T[] vetor = new T[_quantidade];
        for (int i = 0; i < _quantidade; i++)
            vetor[i] = _itens[_quantidade - i - 1];

        return vetor;
    }

    /// <summary>
    /// Copia os itens das duas pilhas recebidas como parâmetro para uma única pilha.
    /// Dentro da pilha resultante, a ordem de cada pilha original é invertida, mas a sequência p1 seguida de p2 é mantida.
    /// </summary>
    public static CPilhaVet<T> ConcatenaPilha(CPilhaVet<T> p1, CPilhaVet<T> p2)
    {
        if (p1 == null)
            throw new ArgumentNullException(nameof(p1), "Nenhuma das pilhas a concatenar pode ser nula.");
        if (p2 == null)
            throw new ArgumentNullException(nameof(p2), "Nenhuma das pilhas a concatenar pode ser nula.");

        CPilhaVet<T> concatenada = new CPilhaVet<T>(checked(p1._quantidade + p2._quantidade));
        foreach (T item in p1)
            concatenada.Empilha(item);
        foreach (T item in p2)
            concatenada.Empilha(item);

        return concatenada;
    }

    public void Limpar()
    {
        for (int i = 0; i < _quantidade; i++)
            _itens[i] = default!;

        _quantidade = 0;
        _versao++;
    }

    public int Capacidade => _itens.Length;

    public int Quantidade => _quantidade;

    public bool EstaVazia => _quantidade == 0;

    public IEnumerator<T> GetEnumerator()
    {
        uint versao = _versao;
        for (int i = _quantidade - 1; i >= 0; i--)
        {
            if (versao != _versao)
                throw new InvalidOperationException("A pilha foi modificada. Operação de enumeração cancelada.");

            yield return _itens[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    // Implementação explícita da interface ICollection (métodos são acessíveis apenas via interface)
    void ICollection<T>.Add(T item) => Empilha(item); // insere o item no topo da pilha

    void ICollection<T>.Clear() => Limpar();

    bool ICollection<T>.Contains(T item) => Contem(item);

    void ICollection<T>.CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array), "O array de destino não pode ser nulo.");
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), "O índice de destino não pode ser negativo.");
        if (array.Length - arrayIndex < _quantidade)
            throw new ArgumentException("O array de destino não possui espaço suficiente.");

        for (int i = 0; i < _quantidade; i++)
            array[arrayIndex + i] = _itens[_quantidade - i - 1];
    }

    int ICollection<T>.Count => _quantidade;

    bool ICollection<T>.IsReadOnly => false;

    bool ICollection<T>.Remove(T item) => throw new NotSupportedException("Não é possível remover itens de forma harbitrária.");
}