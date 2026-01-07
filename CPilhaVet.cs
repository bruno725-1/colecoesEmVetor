using System;
using System.Collections;
using System.Collections.Generic;
///<summary>
/// Classe CPilhaVet - implementa uma lista LIFO: last-in first-out
/// Internamente, a classe utiliza um vetor para armazenar os itens, então empilhar pode ser o(n), enquanto desempilhar sempre será o(1)
/// </summary>
public class CPilhaVet<T> : IEnumerable<T>
{
    private T[] _itens; // vetor que armazena os itens da pilha
    private int _quantidade; // número de itens que a pilha contém
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
        if(tamanho < 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "A capacidade da pilha não pode ser um número negativo.");

        if(tamanho == 0)
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
        if(colecao == null)
            throw new ArgumentNullException(nameof(colecao), "A coleção a copiar não pode ser nula.");

        if(colecao is ICollection<T> c)
        {
            int tamanho = c.Count;
            if(tamanho > 0)
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
            foreach(T item in colecao)
                Empilha(item);
        }
    }

    private int CalcularCapacidade(int capacidade)
    {
        int novaCapacidade = _quantidade == 0 ? 6 : _quantidade * 2;
        if ((uint) novaCapacidade > Array.MaxLength) novaCapacidade = Array.MaxLength;
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
        if(_quantidade == _itens.Length)
            Redimensiona(CalcularCapacidade(_quantidade + 1));

        _itens[_quantidade] = elemento;
        _quantidade++;
    }

    public T Desempilha()
    {
        _quantidade--;
        T item = _itens[_quantidade];
        _itens[_quantidade] = default!;
        return item;
    }

    public int Capacidade => _itens.Length;

    public int Quantidade => _quantidade;

    public bool Vazia => _quantidade == 0;

    public IEnumerator<T> GetEnumerator()
    {
        int quantidade = _quantidade;
        for (int i = _quantidade - 1; i >= 0; i--)
        {
            if (quantidade != _quantidade)
                throw new InvalidOperationException("A pilha foi modificada. Enumeração cancelada.");

            yield return _itens[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}