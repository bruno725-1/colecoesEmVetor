/*using System;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// <summary>
/// Implementa uma lista FIFO (first-in first-out)
/// Internamente, a classe utiliza um vetor circular para armazenar os itens. Portanto, enfileirar e desenfileirar são tipicamente O(1), amenos que o vetor interno precise de redimensionamento.
/// </summary>
/// <typeparam name="T"></typeparam>
class CFilaVet<T>
{
    private T[] _itens; // vetor que armazena os itens da fila
    private int _frente; // Índice por onde os itens serão desenfileirados.
    private int _tras; // Índice por onde os itens serão enfileirados.
    private int _quantidade; // número de itens que a fila contém
    private uint _versao; // atributo para impedir modificações durante loops foreach
    private static readonly T[] s_vetorVazio = new T[0]; // o campo itens de filas vazias sempre apontará para este vetor

    ///<summary>
    /// Constrói uma pilha inicialmente vazia com capacidade para 0 elementos.
    /// Ao adicionar o primeiro elemento, a capacidade da pilha aumenta para 6, e nas próximas realocações, dobra
    /// </summary>
    public CFilaVet()
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
    public CFilaVet(int tamanho)
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
    public CFilaVet(IEnumerable<T> colecao)
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
            foreach(T item in colecao)
                Enfileira(item);
        }
    }

    private void Avancar(ref int indice)
    {
        // Wrap-around feito com comparação ao invés de módulo (%) por desempenho.
        // Benchmarks disponíveis em experimentos/benchmarks/performance.md
        int temp = indice + 1;
        if(temp == _itens.Length)
            temp = 0;

        indice = temp;
    }

    private int CalcularCapacidade(int capacidade)
    {
        int novaCapacidade = _quantidade == 0 ? 6 : _quantidade * 2;
        // Permite que a fila cresça o máximo possível, antes de ocorrer overflow.
        // Esta checagem funciona mesmo quando a nova capacidade sofreu overflow, graças ao casting para uint.
        if ((uint) novaCapacidade > Array.MaxLength) novaCapacidade = Array.MaxLength;
        // se a capacidade calculada for menor que o necessário, seta o parâmetro original como nova capacidade.
        // Se a capacidade exceder Array.MaxLength, ocorrerá OutOfMemoryException.
        if (novaCapacidade < capacidade) novaCapacidade = capacidade;
        return novaCapacidade;
    }

    private void Copiar(int indiceFonte, T[] vetorDestino, int indiceDestino, int quantidade)
    {
        // implemento esse trem depois
    }
}*/