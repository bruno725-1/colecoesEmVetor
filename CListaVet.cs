using System;
using System.Collections;
using System.Collections.Generic;
public class CListaVet<T> : IEnumerable<T>
{
    private T[] Itens;
    private int Qtde = 0;

    public CListaVet()
    {
        Itens = new T[6];
    }

    public CListaVet(int tamanho)
    {
        if (tamanho < 0)
            throw new ArgumentException("A capacidade da lista não pode ser um número negativo.", nameof(tamanho));

        Itens = new T[tamanho];
    }

    public CListaVet(T[] vetor)
    {
        Itens = new T[vetor.Length];
        for (int i = 0; i < vetor.Length; i++)
        {
            Itens[i] = vetor[i];
            Qtde++;
        }
    }

    public CListaVet(CListaVet<T> lv)
    {
        Itens = new T[lv.Qtde];
        for (int i = 0; i < Itens.Length; i++)
        {
            Itens[i] = lv.Itens[i];
            Qtde++;
        }
    }

    private void Redimenciona(int tamanho)
    {
        if (tamanho <= 0)
            tamanho = 6; // impede que o novo vetor seja iniciado com comprimento inválido

        T[] novoItens = new T[tamanho];
        for (int i = 0; i < Qtde; i++)
            novoItens[i] = Itens[i];

        Itens = novoItens;
    }

    // altera ou retorna o valor de um índice especificado
    public T this[int posicao]
    {
        get
        {
            if (posicao < 0 || posicao >= Qtde)
                throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");
            return Itens[posicao];
        }
        set
        {
            if (posicao < 0 || posicao >= Qtde)
                throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");
            Itens[posicao] = value;
        }
    }

    public bool Vazia() => Qtde == 0;

    public void Adiciona(T elemento)
    {
        if (Qtde == Itens.Length)
            Redimenciona(Qtde * 2);
        Itens[Qtde] = elemento;
        Qtde++;
    }

    public void CortarExcessos() => Redimenciona(Qtde);

    public void Remove(T elemento)
    {
        bool achou = false;
        for (int i = 0; i < Qtde && !achou; i++)
        {
            achou = EqualityComparer<T>.Default.Equals(Itens[i], elemento);
            if (achou)
            {
                for (int j = i; j < Qtde - 1; j++)
                    Itens[j] = Itens[j + 1]; //desloca os elementos para a esquerda a partir da posição removida.
                //libera a referência da cópia do último item para o GC (muito útil para objetos do tipo referência)
                Itens[Qtde - 1] = default!;
                Qtde--;
            }
        }
        if (!achou)
            throw new ArgumentException("Elemento não encontrado.", nameof(elemento));
    }

    public void RemoveIndice(int posicao)
    {
        if (posicao < 0 || posicao >= Qtde)
            throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor que a quantidade de itens da lista.");
        for (int i = posicao; i < Qtde - 1; i++)
            Itens[i] = Itens[i + 1]; //desloca os elementos para a esquerda a partir da posição removida.
        Itens[Qtde - 1] = default!;
        Qtde--;
    }

    public void InsereIndice(T elemento, int posicao)
    {
        if (posicao < 0 || posicao > Qtde)
            throw new ArgumentOutOfRangeException(nameof(posicao), "O índice especificado estava fora do intervalo válido. Deve ser não-negativo e menor ou igual a quantidade de itens da lista.");
        if (Qtde == Itens.Length)
            Redimenciona(Qtde * 2);
        for (int i = Qtde - 1; i >= posicao; i--)
            Itens[i + 1] = Itens[i];
        Itens[posicao] = elemento;
        Qtde++;
    }

    public void Inverte()
    {
        int inicio = 0;
        int fim = Qtde - 1;
        while (inicio < fim)
        {
            // Troca os elementos das posições 'inicio' e 'fim'
            T temp = Itens[inicio];
            Itens[inicio] = Itens[fim];
            Itens[fim] = temp;
            inicio++;
            fim--;
        }
    }

    public T[] ParaVetor()
    {
        T[] vetor = new T[Qtde];
        for (int i = 0; i < Qtde; i++)
            vetor[i] = Itens[i];
        return vetor;
    }

    public void InsereAntesDe(T elementoAInserir, T referencia)
    {
        if (Qtde == Itens.Length)
            Redimenciona(Qtde * 2);
        for (int i = 0; i < Qtde; i++)
        {
            if (EqualityComparer<T>.Default.Equals(Itens[i], referencia))
            {
                // desloca os elementos a partir de i uma posição à direita
                for (int j = Qtde - 1; j >= i; j--)
                    Itens[j + 1] = Itens[j];
                Itens[i] = elementoAInserir; // insere exatamente na posição do elemento de referência
                Qtde++;
                return;
            }
        }
        throw new ArgumentException("O elemento de referência especificado não foi encontrado na lista.", nameof(referencia));
    }

    public void InsereDepoisDe(T elementoAInserir, T referencia)
    {
        if (Qtde == Itens.Length)
            Redimenciona(Qtde * 2);
        for (int i = 0; i < Qtde; i++)
        {
            if (EqualityComparer<T>.Default.Equals(Itens[i], referencia))
            {
                // desloca todos os elementos a partir de i+1
                for (int j = Qtde - 1; j > i; j--)
                    Itens[j + 1] = Itens[j];
                Itens[i + 1] = elementoAInserir;
                Qtde++;
                return;
            }
        }
        throw new ArgumentException("O elemento de referência especificado não foi encontrado na lista.", nameof(referencia));
    }

    public void Ordenar()
    {
        if (Qtde > 1)
            QuickSort(0, Qtde - 1);
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
        T pivot = Itens[(esq + dir) / 2];
        int i = esq - 1;
        int j = dir + 1;
        while (true)
        {
            do { i++; } while (Comparer<T>.Default.Compare(Itens[i], pivot) < 0);
            do { j--; } while (Comparer<T>.Default.Compare(Itens[j], pivot) > 0);

            if (i >= j)
                return j;

            // swap
            T temp = Itens[i];
            Itens[i] = Itens[j];
            Itens[j] = temp;
        }
    }

    public int PrimeiraOcorrenciaDe(T elemento)
    {
        for (int i = 0; i < Qtde; i++)
            if (EqualityComparer<T>.Default.Equals(Itens[i], elemento)) return i;

        return -1;
    }

    public int UltimaOcorrenciaDe(T elemento)
    {
        int ocorrencia = 0;
        bool achou = false;
        for (int i = 0; i < Qtde; i++)
        {
            if (EqualityComparer<T>.Default.Equals(Itens[i], elemento))
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
        for (int i = 0; i < Qtde && !achou; i++)
            achou = EqualityComparer<T>.Default.Equals(Itens[i], elemento);
        return achou;
    }

    public void Limpar()
    {
        int limite = Qtde;
        for (int i = 0; i < limite; i++)
        {
            Itens[i] = default!;
            Qtde--;
        }
    }

    public int Quantidade => Qtde;

    public int Capacidade => Itens.Length;

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Qtde; i++)
            yield return Itens[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}