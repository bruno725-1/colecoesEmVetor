using System;
using System.Collections;
using System.Collections.Generic;

namespace AED
{
    #region Classe CCelula - representa a célula utilizada pelas classes CLista, CFila e CPilha
    /// <summary>
    /// Classe utilizada pelas classes CLista, CFila e CPilha
    /// </summary>
    public class CCelula<T>
    {
        public T Item;
        public CCelula<T> Prox;

        public CCelula()
        {
            Item = default(T);
            Prox = null;
        }

        public CCelula(T valorItem)
        {
            Item = valorItem;
            Prox = null;
        }

        public CCelula(T valorItem, CCelula<T> proxCelula)
        {
            Item = valorItem;
            Prox = proxCelula;
        }
    }
    #endregion

    #region Classe CFila - Fila (ou lista FIFO: first-in first-out)
    public class CFila<T> : IEnumerable<T>
    {
        private CCelula<T> Frente;
        private CCelula<T> Tras;
        private int Qtde = 0;

        public CFila()
        {
            Frente = new CCelula<T>();
            Tras = Frente;
        }

        public CFila(T[] vetor)
        {
            Frente = new CCelula<T>();
            Tras = Frente;
            for (int i = 0; i < vetor.Length; i++)
            {
                Tras.Prox = new CCelula<T>(vetor[i]);
                Tras = Tras.Prox;
                Qtde++;
            }
        }

        public CFila(CFila<T> f)
        {
            Frente = new CCelula<T>();
            Tras = Frente;
            for (CCelula<T> aux = f.Frente.Prox; aux != null; aux = aux.Prox)
            {
                Tras.Prox = new CCelula<T>(aux.Item);
                Tras = Tras.Prox;
                Qtde++;
            }
        }

        public CFila(CPilha<T> p)
        {
            Frente = new CCelula<T>();
            Tras = Frente;
            foreach (T item in p)
                Enfileira(item);
        }

        public bool EstaVazia() => Frente == Tras;

        public void Enfileira(T valorItem)
        {
            Tras.Prox = new CCelula<T>(valorItem);
            Tras = Tras.Prox;
            Qtde++;
        }

        public T Desenfileira()
        {
            if (Frente != Tras)
            {
                Frente = Frente.Prox;
                T item = Frente.Item;
                Qtde--;
                return item;
            }
            return default(T);
        }

        public T Peek() => Frente != Tras ? Frente.Prox.Item : default(T);

        public bool Contem(T valorItem)
        {
            for (CCelula<T> aux = Frente.Prox; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public int Quantidade() => Qtde;

        public static CFila<T> ConcatenaFila(CFila<T> F1, CFila<T> F2)
        {
            CFila<T> concatenada = new CFila<T>();
            //percorrer as duas filas até o final
            for (CCelula<T> aux = F1.Frente.Prox; aux != null; aux = aux.Prox)
            {
                concatenada.Tras.Prox = new CCelula<T>(aux.Item);
                concatenada.Tras = concatenada.Tras.Prox;
                concatenada.Qtde++;
            }
            for (CCelula<T> aux = F2.Frente.Prox; aux != null; aux = aux.Prox)
            {
                concatenada.Tras.Prox = new CCelula<T>(aux.Item);
                concatenada.Tras = concatenada.Tras.Prox;
                concatenada.Qtde++;
            }
            return concatenada;
        }

        public int OcorrenciasDe(T elemento)
        {
            int ocorrencias = 0;
            for (CCelula<T> aux = Frente.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                    ocorrencias++;
            }
            return ocorrencias;
        }

        public void Limpar()
        {
            if (Frente == Tras) return;
            while (Frente.Prox != null)
            {
                Frente.Prox = Frente.Prox.Prox;
                Qtde--;
            }
            Tras = Frente;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var aux = Frente.Prox; aux != null; aux = aux.Prox)
                yield return aux.Item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    #region Classe CPilha - CPilha (ou lista LIFO: last-in first-out)
    public class CPilha<T> : IEnumerable<T>
    {
        private CCelula<T> Topo = null;
        private int Qtde = 0;

        public bool EstaVazia() => Topo == null;

        public void Empilha(T valorItem)
        {
            Topo = new CCelula<T>(valorItem, Topo);
            Qtde++;
        }

        public T Desempilha()
        {
            if (Topo != null)
            {
                T item = Topo.Item;
                Topo = Topo.Prox;
                Qtde--;
                return item;
            }
            return default(T);
        }

        public T Peek() => Topo != null ? Topo.Item : default(T);

        public bool Contem(T valorItem)
        {
            for (var aux = Topo; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public int Quantidade() => Qtde;

        public static CPilha<T> ConcatenaPilha(CPilha<T> P1, CPilha<T> P2)
        {
            CPilha<T> concatenada = new CPilha<T>();
            //percorrer as duas pilhas até o final
            for (CCelula<T> aux = P1.Topo; aux != null; aux = aux.Prox)
            {
                concatenada.Topo = new CCelula<T>(aux.Item, concatenada.Topo);
                concatenada.Qtde++;
            }
            for (CCelula<T> aux = P2.Topo; aux != null; aux = aux.Prox)
            {
                concatenada.Topo = new CCelula<T>(aux.Item, concatenada.Topo);
                concatenada.Qtde++;
            }
            return concatenada;
        }

        public void Limpar()
        {
            if (Topo == null) return;
            while (Topo != null)
            {
                Topo = Topo.Prox;
                Qtde--;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var aux = Topo; aux != null; aux = aux.Prox)
                yield return aux.Item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    #region Classe CLista - Lista encadeada (simples) com célula cabeça
    public class CLista<T> : IEnumerable<T>
    {
        private CCelula<T> Primeira;
        private CCelula<T> Ultima;
        private int Qtde = 0;

        public CLista()
        {
            Primeira = new CCelula<T>();
            Ultima = Primeira;
        }

        public CLista(T[] vetor)
        {
            Primeira = new CCelula<T>();
            Ultima = Primeira;
            for (int i = 0; i < vetor.Length; i++)
            {
                Ultima.Prox = new CCelula<T>(vetor[i]);
                Ultima = Ultima.Prox;
                Qtde++;
            }
        }

        public CLista(CLista<T> l)
        {
            Primeira = new CCelula<T>();
            Ultima = Primeira;
            for (CCelula<T> aux = l.Primeira.Prox; aux != null; aux = aux.Prox)
            {
                Ultima.Prox = new CCelula<T>(aux.Item);
                Ultima = Ultima.Prox;
                Qtde++;
            }
        }

        public bool Vazia() => Primeira == Ultima;

        public void InsereFim(T valorItem)
        {
            Ultima.Prox = new CCelula<T>(valorItem);
            Ultima = Ultima.Prox;
            Qtde++;
        }

        public void InsereComeco(T valorItem)
        {
            Primeira.Prox = new CCelula<T>(valorItem, Primeira.Prox);
            if (Primeira.Prox.Prox == null)
                Ultima = Primeira.Prox;
            Qtde++;
        }

        public bool InsereIndice(T valorItem, int posicao)
        {
            if (posicao < 1 || posicao > Qtde + 1) return false;

            CCelula<T> aux = Primeira;
            for (int i = 0; i < posicao - 1; i++)
                aux = aux.Prox;

            aux.Prox = new CCelula<T>(valorItem, aux.Prox);
            if (aux.Prox.Prox == null)
                Ultima = aux.Prox;
            Qtde++;
            return true;
        }

        public void RemovePos(int n)
        {
            if (n < 1 || n > Qtde)
                throw new ArgumentException("Índice inválido ou inexistente");
            CCelula<T> aux = Primeira;
            for (int i = 0; i < n - 1; i++)
                aux = aux.Prox;
            aux.Prox = aux.Prox.Prox;
            if (aux.Prox == null)
                Ultima = aux;
            Qtde--;
        }

        public void InsereAntesDe(T valorItem, T elemento)
        {
            for (CCelula<T> aux = Primeira; aux.Prox != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Prox.Item, elemento))
                {
                    aux.Prox = new CCelula<T>(valorItem, aux.Prox);
                    Qtde++;
                    return;
                }
            }
            //se percorreu todo o loop, é porque o elemento antes do qual o item seria adicionado não foi encontrado
            throw new ArgumentException("Elemento não encontrado");
        }

        public void InsereDepoisDe(T valorItem, T elemento)
        {
            for (CCelula<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                {
                    aux.Prox = new CCelula<T>(valorItem, aux.Prox);
                    Qtde++;
                    return;
                }
            }
            throw new ArgumentException("Elemento não encontrado");
        }

        public T[] ParaVetor()
        {
            T[] vetor = new T[Qtde];
            CCelula<T> aux = Primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        public int PrimeiraOcorrenciaDe(T elemento)
        {
            int indice = 1;
            for (CCelula<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento)) return indice;
                indice++;
            }
            return -1;
        }

        public int UltimaOcorrenciaDe(T elemento)
        {
            int indice = 1, ocorrencia = 0;
            bool achou = false;
            for (CCelula<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                {
                    achou = true;
                    ocorrencia = indice; //armazena a posição do elemento se for encontrado
                }
                indice++;
            }
            if (!achou) return -1;
            return ocorrencia;
        }

        public void Ordenar()
        {
            if (Qtde < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                CCelula<T> atual = Primeira.Prox;
                while (atual != null && atual.Prox != null)
                {
                    if (Comparer<T>.Default.Compare(atual.Item, atual.Prox.Item) > 0)
                    {
                        T temp = atual.Item;
                        atual.Item = atual.Prox.Item;
                        atual.Prox.Item = temp;
                        houveTroca = true;
                    }
                    atual = atual.Prox;
                }
            }
        }

        public void Imprime()
        {
            CCelula<T> aux = Primeira.Prox;
            while (aux != null)
            {
                Console.WriteLine(aux.Item);
                aux = aux.Prox;
            }
        }

        public void ImprimeInv()
        {
            if(Primeira.Prox != null)//se a lista não estiver vazia
                ImprimeInv(Primeira.Prox);
        }

        ///<summary>
        /// Implementação de um método recursivo que imprime a lista de forma inversa. O método é chamado, verifica se aux é diferente de null. Se for, chama o método novamente e após isso, o item será impresso na tela. Como consequência, a impressão dos elementos ocorrerá no desmanche da pilha de registros de ativação
        ///</summary>
        public void ImprimeInv(CCelula<T> aux)
        {
            if (aux != null)
            {
                ImprimeInv(aux.Prox);
                Console.WriteLine(aux.Item);
            }
        }

        public bool Contem(T valorItem)
        {
            for (var aux = Primeira.Prox; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public T RetornaIndice(int posicao)
        {
            if (posicao < 1 || posicao > Qtde) return default(T);

            var aux = Primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;
            return aux.Item;
        }

        public void AlteraIndice(int posicao, T elemento)
        {
            if (posicao < 1 || posicao > Qtde)
                throw new ArgumentException("Índice inválido ou inexistente");
            var aux = Primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;
            aux.Item = elemento;
        }

        public T RetornaPrimeiro() => Primeira != Ultima ? Primeira.Prox.Item : default(T);

        public T RetornaUltimo() => Primeira != Ultima ? Ultima.Item : default(T);

        public T RemoveRetornaComecoSimples()
        {
            if (Primeira != Ultima)
            {
                Primeira = Primeira.Prox;
                Qtde--;
                return Primeira.Item;
            }
            return default(T);
        }

        public T RemoveRetornaFim()
        {
            if (Primeira == Ultima) return default(T);

            var aux = Primeira;
            while (aux.Prox != Ultima)
                aux = aux.Prox;

            var removida = Ultima.Item;
            Ultima = aux;
            Ultima.Prox = null;
            Qtde--;
            return removida;
        }

        public void Remove(T valorItem)
        {
            var aux = Primeira;
            while (aux.Prox != null)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Prox.Item, valorItem))
                {
                    aux.Prox = aux.Prox.Prox;
                    if (aux.Prox == null)
                        Ultima = aux;
                    Qtde--;
                    return;
                }
                aux = aux.Prox;
            }
        }

        public void Limpar()
        {
            if (Primeira == Ultima) return;
            while (Primeira.Prox != null)
            {
                Primeira.Prox = Primeira.Prox.Prox;
                Qtde--;
            }
            Ultima = Primeira;
        }

        public int Quantidade() => Qtde;

        public void Inverte()
        {
            T[] vet = new T[Qtde];
            CCelula<T> aux = Primeira.Prox;
            for (int i = 0; i < Qtde; i++)
            {
                vet[i] = aux.Item;
                aux = aux.Prox;
            }
            aux = Primeira.Prox;
            for (int i = Qtde - 1; i >= 0; i--)
            {
                aux.Item = vet[i];
                aux = aux.Prox;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var aux = Primeira.Prox; aux != null; aux = aux.Prox)
                yield return aux.Item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    #endregion

    #region Classe CCelulaDup<T> - representa a célula utilizada pela classe CListaDup<T>
    /// <summary>
    /// Classe utilizada pela classe CListaDup<T>
    /// </summary>
    public class CCelulaDup<T>
    {
        public T Item; // O Item armazenado pela célula
        public CCelulaDup<T> Ant; // Referencia a célula anterior
        public CCelulaDup<T> Prox; // Referencia a próxima célula

        public CCelulaDup()
        {
            Item = default(T);
            Ant = null;
            Prox = null;
        }

        public CCelulaDup(T valorItem)
        {
            Item = valorItem;
            Ant = null;
            Prox = null;
        }

        public CCelulaDup(T valorItem, CCelulaDup<T> celulaAnt, CCelulaDup<T> proxCelula)
        {
            Item = valorItem;
            Ant = celulaAnt;
            Prox = proxCelula;
        }
    }
    #endregion

    #region Classe CListaDup<T> - Lista duplamente encadeada com célula cabeça
    /// <summary>
    /// Implementa uma lista duplamente encadeada genérica.
    /// </summary>
    public class CListaDup<T> : IEnumerable<T>
    {
        private CCelulaDup<T> Primeira; // Referencia a primeira célula da lista (célula cabeça)
        private CCelulaDup<T> Ultima; // Referencia a última célula da lista 
        private int Qtde = 0;

        public CListaDup()
        {
            Primeira = new CCelulaDup<T>();
            Ultima = Primeira;
        }

        public CListaDup(CListaDup<T> l)
        {
            Primeira = new CCelulaDup<T>();
            Ultima = Primeira;
            for (CCelulaDup<T> aux = l.Primeira.Prox; aux != null; aux = aux.Prox)
            {
                Ultima.Prox = new CCelulaDup<T>(aux.Item, Ultima, null);
                Ultima = Ultima.Prox;
                Qtde++;
            }
        }

        public CListaDup(T[] vetor)
        {
            Primeira = new CCelulaDup<T>();
            Ultima = Primeira;
            for (int i = 0; i < vetor.Length; i++)
            {
                Ultima.Prox = new CCelulaDup<T>(vetor[i], Ultima, null);
                Ultima = Ultima.Prox;
                Qtde++;
            }
        }

        public bool Vazia()
        {
            return Primeira == Ultima;
        }

        public T[] ParaVetor()
        {
            T[] vetor = new T[Qtde];
            CCelulaDup<T> aux = Primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        public void Ordenar()
        {
            if (Qtde < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                CCelulaDup<T> atual = Primeira.Prox;
                while (atual != null && atual.Prox != null)
                {
                    if (Comparer<T>.Default.Compare(atual.Item, atual.Prox.Item) > 0)
                    {
                        T temp = atual.Item;
                        atual.Item = atual.Prox.Item;
                        atual.Prox.Item = temp;
                        houveTroca = true;
                    }
                    atual = atual.Prox;
                }
            }
        }

        public void InsereFim(T valorItem)
        {
            Ultima.Prox = new CCelulaDup<T>(valorItem, Ultima, null);
            Ultima = Ultima.Prox;
            Qtde++;
        }

        public void InsereComeco(T valorItem)
        {
            if (Primeira == Ultima) // Se a lista estiver vazia insere no fim
            {
                Ultima.Prox = new CCelulaDup<T>(valorItem, Ultima, null);
                Ultima = Ultima.Prox;
            }
            else // senão insere no começo
            {
                Primeira.Prox = new CCelulaDup<T>(valorItem, Primeira, Primeira.Prox);
                Primeira.Prox.Prox.Ant = Primeira.Prox;
            }
            Qtde++;
        }

        public void RemoveComecoSemRetorno()
        {
            if (Primeira != Ultima)
            {
                Primeira = Primeira.Prox;
                Primeira.Ant = null;
                Qtde--;
            }
        }

        public void RemovePos(int n)
        {
            if (n < 1 || n > Qtde)
                throw new ArgumentException("Índice inválido ou inexistente");
            CCelulaDup<T> aux = Primeira;
            for (int i = 0; i < n - 1; i++)
            {
                aux = aux.Prox;
            }
            aux.Prox = aux.Prox.Prox;
            if (aux.Prox != null) //se a célula removida não era a última
                aux.Prox.Ant = aux;
            else
                Ultima = aux; //atualizar ponteiro Ultima
            Qtde--;
        }

        public void Imprime()
        {
            for (CCelulaDup<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
                Console.WriteLine(aux.Item);
        }

        public void ImprimeInv()
        {
            for (CCelulaDup<T> aux = Ultima; aux.Ant != null; aux = aux.Ant)
                Console.WriteLine(aux.Item);
        }

        public bool Contem(T elemento)
        {
            bool achou = false;
            for (CCelulaDup<T> aux = Primeira.Prox; aux != null && !achou; aux = aux.Prox)
                achou = EqualityComparer<T>.Default.Equals(aux.Item, elemento);
            return achou;
        }

        public T RetornaPrimeiro()
        {
            if (Primeira != Ultima)
                return Primeira.Prox.Item;
            return default(T);
        }

        public T RetornaIndice(int Posicao)
        {
            if ((Posicao >= 1) && (Posicao <= Qtde) && (Primeira != Ultima))
            {
                CCelulaDup<T> aux = Primeira.Prox;
                for (int i = 1; i < Posicao; i++, aux = aux.Prox) ;
                if (aux != null)
                    return aux.Item;
            }
            return default(T);
        }

        public T RetornaUltimo()
        {
            if (Primeira != Ultima)
                return Ultima.Item;
            return default(T);
        }

        public void RemoveFimSemRetorno()
        {
            if (Primeira != Ultima)
            {
                Ultima = Ultima.Ant;
                Ultima.Prox = null;
                Qtde--;
            }
        }

        public void Remove(T valorItem)
        {
            if (Primeira != Ultima)
            {
                CCelulaDup<T> aux = Primeira.Prox;
                bool achou = false;
                while (aux != null && !achou)
                {
                    achou = EqualityComparer<T>.Default.Equals(aux.Item, valorItem);
                    if (!achou)
                        aux = aux.Prox;
                }
                if (achou) // achou o elemento
                {
                    CCelulaDup<T> anterior = aux.Ant;
                    CCelulaDup<T> proximo = aux.Prox;
                    anterior.Prox = proximo;
                    if (proximo != null)
                        proximo.Ant = anterior;
                    else
                        Ultima = anterior;
                    Qtde--;
                }
            }
        }

        public T RemoveRetornaComeco()
        {
            if (Primeira != Ultima)
            {
                CCelulaDup<T> aux = Primeira.Prox;
                Primeira = Primeira.Prox;
                Primeira.Ant = null;
                Qtde--;
                return aux.Item;
            }
            else
                return default(T);
        }

        public T RemoveRetornaFim()
        {
            if (Primeira != Ultima)
            {
                CCelulaDup<T> aux = Ultima;
                Ultima = Ultima.Ant;
                Ultima.Prox = null;
                Qtde--;
                return aux.Item;
            }
            else
                return default(T);
        }

        public int Quantidade()
        {
            return Qtde;
        }

        public int PrimeiraOcorrenciaDe(T elemento)
        {
            int indice = 1;
            for (CCelulaDup<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento)) return indice;
                indice++;
            }
            return -1;
        }

        public int UltimaOcorrenciaDe(T elemento)
        {
            int indice = 1, ocorrencia = 0;
            bool achou = false;
            for (CCelulaDup<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                {
                    achou = true;
                    ocorrencia = indice; //armazena a posição do elemento se for encontrado
                }
                indice++;
            }
            if (!achou) return -1;
            return ocorrencia;
        }

        public void AlteraIndice(int posicao, T elemento)
        {
            if (posicao < 1 || posicao > Qtde)
                throw new ArgumentException("Índice inválido ou inexistente");
            var aux = Primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;
            aux.Item = elemento;
        }

        public void Limpar()
        {
            if (Primeira == Ultima) return;
            CCelulaDup<T> atual = Primeira.Prox;
            while (atual != null)
            {
                CCelulaDup<T> proxima = atual.Prox;
                atual.Ant = null;
                atual.Prox = null;
                atual = proxima;
                Qtde--;
            }
            Primeira.Prox = null;
            Ultima = Primeira;
        }

        public void Inverte()
        {
            int inicio = 1;
            int fim = Qtde;
            CCelulaDup<T> inicial = Primeira.Prox;
            CCelulaDup<T> final = Ultima;
            while(inicio<fim)
            {
                T temp = inicial.Item;
                inicial.Item = final.Item;
                final.Item = temp;
                inicial = inicial.Prox;
                final = final.Ant;
                inicio++;
                fim--;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (CCelulaDup<T> aux = Primeira.Prox; aux != null; aux = aux.Prox)
                yield return aux.Item;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
    #endregion

    #region Classe CCelulaDic - Célula que será utilizada pelo CDicionario
    /// < summary>
    /// Classe utilizada pela classe CDicionario
    /// </summary>
    public class CCelulaDic<TChave, TValor>
    {
        public TChave Chave;
        public TValor Valor;
        public CCelulaDic<TChave, TValor> Prox;
        public CCelulaDic()
        {
            Chave = default(TChave);
            Valor = default(TValor);
            Prox = null;
        }

        public CCelulaDic(TChave key, TValor value)
        {
            Chave = key;
            Valor = value;
            Prox = null;
        }

        public CCelulaDic(TChave key, TValor value, CCelulaDic<TChave, TValor> proxCelula)
        {
            Chave = key;
            Valor = value;
            Prox = proxCelula;
        }
    }
    #endregion

    #region Classe CDicionario - implementa um dicionário chave e valor
    public class CDicionario<TChave, TValor> : IEnumerable<CPar<TChave, TValor>>
    {
        private CCelulaDic<TChave, TValor> Primeira;
        private CCelulaDic<TChave, TValor> Ultima;
        private int Qtde = 0;
        public CDicionario()
        {
            Primeira = new CCelulaDic<TChave, TValor>();
            Ultima = Primeira;
        }

        public CDicionario(CDicionario<TChave, TValor> d)
        {
            Primeira = new CCelulaDic<TChave, TValor>();
            Ultima = Primeira;
            for (var aux = d.Primeira.Prox; aux != null; aux = aux.Prox)
            {
                Ultima.Prox = new CCelulaDic<TChave, TValor>(aux.Chave, aux.Valor);
                Ultima = Ultima.Prox;
                Qtde++;
            }
        }

        public bool Vazio() => Primeira == Ultima;

        public void Adiciona(TChave key, TValor value)
        {
            //percorrer todo o dicionário para verificar se a chave passada como argumento já existe
            var aux = Primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, key))
                    throw new ArgumentException("A chave passada por parâmetro já existe");
                aux = aux.Prox;
            }
            //se chegou aqui, é porque a chave não existe
            Ultima.Prox = new CCelulaDic<TChave, TValor>(key, value);
            Ultima = Ultima.Prox;
            Qtde++;
        }

        public void Remove(TChave key)
        {
            CCelulaDic<TChave, TValor> aux = Primeira;
            while (aux.Prox != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Prox.Chave, key))
                {
                    aux.Prox = aux.Prox.Prox;
                    if (aux.Prox == null)
                        Ultima = aux;
                    Qtde--;
                    return;
                }
                aux = aux.Prox;
            }
        }

        public TValor RetornaValor(TChave key)
        {
            CCelulaDic<TChave, TValor> aux = Primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, key))
                    return aux.Valor;
                aux = aux.Prox;
            }
            return default(TValor);
        }

        public void InsereValor(TChave key, TValor value)
        {
            var aux = Primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, key))
                {
                    aux.Valor = value;
                    return;
                }
                aux = aux.Prox;
            }
            //se percorreu todo o loop, é porque a chave passada por parâmetro não foi encontrada
            throw new ArgumentException("A chave passada por parâmetro não foi encontrada");
        }

        public TChave[] Chaves()
        {
            TChave[] vetor = new TChave[Qtde];
            var aux = Primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Chave;
                aux = aux.Prox;
            }
            return vetor;
        }

        public TValor[] Valores()
        {
            TValor[] vetor = new TValor[Qtde];
            var aux = Primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Valor;
                aux = aux.Prox;
            }
            return vetor;
        }

        public int Quantidade() => Qtde;

        public bool ContemChave(TChave key)
        {
            CCelulaDic<TChave, TValor> aux = Primeira.Prox;
            bool achou = false;
            while (aux != null && !achou)
            {
                achou = EqualityComparer<TChave>.Default.Equals(aux.Chave, key);
                aux = aux.Prox;
            }
            return achou;
        }

        public bool ContemValor(TValor value)
        {
            CCelulaDic<TChave, TValor> aux = Primeira.Prox;
            bool achou = false;
            while (aux != null && !achou)
            {
                achou = EqualityComparer<TValor>.Default.Equals(aux.Valor, value);
                aux = aux.Prox;
            }
            return achou;
        }

        public void Ordenar() //implementa um método de ordenação por chaves
        {
            if (Qtde < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                for (var atual = Primeira.Prox; atual != null && atual.Prox != null; atual = atual.Prox)
                {
                    if (Comparer<TChave>.Default.Compare(atual.Chave, atual.Prox.Chave) > 0)
                    {
                        TChave cTemp = atual.Chave;
                        TValor vTemp = atual.Valor;
                        atual.Chave = atual.Prox.Chave;
                        atual.Valor = atual.Prox.Valor;
                        atual.Prox.Chave = cTemp;
                        atual.Prox.Valor = vTemp;
                        houveTroca = true;
                    }
                }
            }
        }

        public void Limpar()
        {
            if (Primeira == Ultima) return;
            while (Primeira.Prox != null)
            {
                Primeira.Prox = Primeira.Prox.Prox;
                Qtde--;
            }
            Ultima = Primeira;
        }

        public IEnumerator<CPar<TChave, TValor>> GetEnumerator()
        {
            for (CCelulaDic<TChave, TValor> aux = Primeira.Prox; aux != null; aux = aux.Prox)
                yield return new CPar<TChave, TValor>(aux.Chave, aux.Valor);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    #region Classe CPar - possibilita a iteração em um dicionário com foreach
    public class CPar<C, V>
    {
        public C Chave { get; }
        public V Valor { get; }

        public CPar(C chave, V valor)
        {
            Chave = chave;
            Valor = valor;
        }
    }
    #endregion
}