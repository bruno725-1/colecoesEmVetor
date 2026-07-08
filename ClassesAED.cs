#nullable disable
// Código baseado em encadeamento com célula cabeça.
// Referências nulas fazem parte do modelo.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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

        public CCelula() { }

        public CCelula(T valorItem)
        {
            Item = valorItem;
        }

        public CCelula(T valorItem, CCelula<T> proxCelula) : this(valorItem)
        {
            Prox = proxCelula;
        }
    }
    #endregion

    #region Classe CFila - Fila (ou lista FIFO: first-in first-out)
    public class CFila<T> : IEnumerable<T>
    {
        private CCelula<T> _frente;
        private CCelula<T> _tras;
        private int _quantidade;
        private uint _versao; // Impede modificações durante loops foreach

        public CFila()
        {
            _frente = new CCelula<T>();
            _tras = _frente;
        }

        // Copia o conteúdo de uma coleção recebida como parâmetro.
        // Antes de copiar a coleção, o construtor inicializa a célula cabeça através do encadeamento.
        // Para que o compilador gere warnings corretamente em caso de coleções possivelmente nulas, reativei os avisos de nulabilidade.
#nullable restore
        public CFila(IEnumerable<T> colecao) : this()
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (T item in colecao)
                Enfileira(item);
        }
#nullable disable

        public bool EstaVazia => _frente == _tras;

        public void Enfileira(T valorItem)
        {
            _tras.Prox = new CCelula<T>(valorItem);
            _tras = _tras.Prox;
            _quantidade++;
            _versao++;
        }

        public T Desenfileira()
        {
            if (_frente == _tras)
                ThrowHelper.ColecaoVazia("Fila");

            _frente = _frente.Prox;
            T item = _frente.Item;
            _quantidade--;
            _versao++;
            return item;
        }

        public T Peek()
        {
            if (_frente == _tras)
                ThrowHelper.ColecaoVazia("Fila");

            return _frente.Prox.Item;
        }

        public bool Contem(T valorItem)
        {
            for (CCelula<T> aux = _frente.Prox; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public int Quantidade => _quantidade;

#nullable restore
        public static CFila<T> ConcatenaFila(CFila<T> f1, CFila<T> f2)
        {
            if (f1 == null)
                ThrowHelper.ColecaoNula(nameof(f1));
            if (f2 == null)
                ThrowHelper.ColecaoNula(nameof(f2));

            CFila<T> concatenada = new CFila<T>();
            //percorrer as duas filas até o final
            for (CCelula<T> aux = f1._frente.Prox; aux != null; aux = aux.Prox)
            {
                concatenada._tras.Prox = new CCelula<T>(aux.Item);
                concatenada._tras = concatenada._tras.Prox;
                concatenada._quantidade++;
            }
            for (CCelula<T> aux = f2._frente.Prox; aux != null; aux = aux.Prox)
            {
                concatenada._tras.Prox = new CCelula<T>(aux.Item);
                concatenada._tras = concatenada._tras.Prox;
                concatenada._quantidade++;
            }
            return concatenada;
        }
#nullable disable

        public void Limpar()
        {
            while (_frente.Prox != null)
                _frente.Prox = _frente.Prox.Prox;

            _quantidade = 0;
            _tras = _frente;
            _versao++;
        }

        public T[] ParaVetor()
        {
            T[] vetor = new T[_quantidade];
            CCelula<T> aux = _frente.Prox;
            for(int i = 0; i < _quantidade; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        public IEnumerator<T> GetEnumerator()
        {
            uint versao = _versao;
            for (var aux = _frente.Prox; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    #region Classe CPilha - CPilha (ou lista LIFO: last-in first-out)
    public class CPilha<T> : IEnumerable<T>
    {
        private CCelula<T> _topo;
        private int _quantidade;
        private uint _versao;

        public CPilha() { }

#nullable restore
        public CPilha(IEnumerable<T> colecao)
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (T item in colecao)
                Empilha(item);
        }
#nullable disable

        public bool EstaVazia => _topo == null;

        public void Empilha(T valorItem)
        {
            _topo = new CCelula<T>(valorItem, _topo);
            _quantidade++;
            _versao++;
        }

        public T Desempilha()
        {
            if (_topo == null)
                ThrowHelper.ColecaoVazia("Pilha");

            T item = _topo.Item;
            _topo = _topo.Prox;
            _quantidade--;
            _versao++;
            return item;
        }

        public T Peek()
        {
            if (_topo == null)
                ThrowHelper.ColecaoVazia("Pilha");

            return _topo.Item;
        }

        public bool Contem(T valorItem)
        {
            for (var aux = _topo; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public int Quantidade => _quantidade;

#nullable restore
        public static CPilha<T> ConcatenaPilha(CPilha<T> p1, CPilha<T> p2)
        {
            if (p1 == null)
                ThrowHelper.ColecaoNula(nameof(p1));
            if (p2 == null)
                ThrowHelper.ColecaoNula(nameof(p2));

            CPilha<T> concatenada = new CPilha<T>();
            //percorrer as duas pilhas até o final
            for (CCelula<T> aux = p1._topo; aux != null; aux = aux.Prox)
            {
                concatenada._topo = new CCelula<T>(aux.Item, concatenada._topo);
                concatenada._quantidade++;
            }
            for (CCelula<T> aux = p2._topo; aux != null; aux = aux.Prox)
            {
                concatenada._topo = new CCelula<T>(aux.Item, concatenada._topo);
                concatenada._quantidade++;
            }
            return concatenada;
        }
#nullable disable

        public void Limpar()
        {
            while (_topo != null)
                _topo = _topo.Prox;

            _quantidade = 0;
            _versao++;
        }

        public T[] ParaVetor()
        {
            T[] vetor = new T[_quantidade];
            CCelula<T> aux = _topo;
            for(int i = 0; i < _quantidade; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        public IEnumerator<T> GetEnumerator()
        {
            uint versao = _versao;
            for (var aux = _topo; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    #endregion

    #region Classe CLista - Lista encadeada (simples) com célula cabeça
    public class CLista<T> : IEnumerable<T>, ICollection<T>
    {
        private CCelula<T> _primeira;
        private CCelula<T> _ultima;
        private int _quantidade;
        private uint _versao;

        public CLista()
        {
            _primeira = new CCelula<T>();
            _ultima = _primeira;
        }

#nullable restore
        public CLista(IEnumerable<T> colecao) : this()
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (T item in colecao)
                Adiciona(item);
        }
#nullable disable

        public bool EstaVazia => _primeira == _ultima;

        public void Adiciona(T valorItem)
        {
            _ultima.Prox = new CCelula<T>(valorItem);
            _ultima = _ultima.Prox;
            _quantidade++;
            _versao++;
        }

        public void InsereComeco(T valorItem)
        {
            _primeira.Prox = new CCelula<T>(valorItem, _primeira.Prox);
            if (_primeira.Prox.Prox == null)
                _ultima = _primeira.Prox;

            _quantidade++;
            _versao++;
        }

        public void InsereIndice(T valorItem, int posicao)
        {
            if (posicao < 1 || posicao > _quantidade + 1)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens + 1");

            CCelula<T> aux = _primeira;
            for (int i = 0; i < posicao - 1; i++)
                aux = aux.Prox;

            aux.Prox = new CCelula<T>(valorItem, aux.Prox);
            if (aux.Prox.Prox == null)
                _ultima = aux.Prox;

            _quantidade++;
            _versao++;
        }

        public void RemoveIndice(int posicao)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            CCelula<T> aux = _primeira;
            for (int i = 0; i < posicao - 1; i++)
                aux = aux.Prox;
            aux.Prox = aux.Prox.Prox;
            if (aux.Prox == null)
                _ultima = aux;

            _quantidade--;
            _versao++;
        }

        public void InsereAntesDe(T elementoAInserir, T referencia)
        {
            for (CCelula<T> aux = _primeira; aux.Prox != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Prox.Item, referencia))
                {
                    aux.Prox = new CCelula<T>(elementoAInserir, aux.Prox);
                    _quantidade++;
                    _versao++;
                    return;
                }
            }
            //se percorreu todo o loop, é porque o elemento antes do qual o item seria adicionado não foi encontrado
            ThrowHelper.ReferenciaNaoEncontrada(nameof(referencia));
        }

        public void InsereDepoisDe(T elementoAInserir, T referencia)
        {
            for (CCelula<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, referencia))
                {
                    aux.Prox = new CCelula<T>(elementoAInserir, aux.Prox);
                    _quantidade++;
                    _versao++;
                    return;
                }
            }
            ThrowHelper.ReferenciaNaoEncontrada(nameof(referencia));
        }

        public T[] ParaVetor()
        {
            T[] vetor = new T[_quantidade];
            CCelula<T> aux = _primeira.Prox;
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
            for (CCelula<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
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
            for (CCelula<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
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

        public int OcorrenciasDe(T elemento)
        {
            int ocorrencias = 0;
            for (CCelula<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                    ocorrencias++;
            }
            return ocorrencias;
        }

        public void Ordenar()
        {
            if (_quantidade < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                CCelula<T> atual = _primeira.Prox;
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
            _versao++;
        }

        public void Imprime()
        {
            CCelula<T> aux = _primeira.Prox;
            while (aux != null)
            {
                Console.WriteLine(aux.Item);
                aux = aux.Prox;
            }
        }

        public void ImprimeInv()
        {
            if (_primeira.Prox != null)//se a lista não estiver vazia
                ImprimeInv(_primeira.Prox);
        }

        ///<summary>
        /// Implementação de um método recursivo que imprime a lista de forma inversa.
        /// O método é chamado, verifica se aux é diferente de null.
        /// Se for, chama o método novamente e após isso, o item será impresso na tela.
        /// Como consequência, a impressão dos elementos ocorrerá no desmanche da pilha de registros de ativação.
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
            for (var aux = _primeira.Prox; aux != null; aux = aux.Prox)
                if (EqualityComparer<T>.Default.Equals(aux.Item, valorItem))
                    return true;
            return false;
        }

        public T RetornaIndice(int posicao)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            var aux = _primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;

            return aux.Item;
        }

        public void AlteraIndice(int posicao, T elemento)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            var aux = _primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;

            aux.Item = elemento;
            _versao++;
        }

        public T RetornaPrimeiro()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            return _primeira.Prox.Item;
        }

        public T RetornaUltimo()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            return _ultima.Item;
        }

        public T RemoveRetornaComeco()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            _primeira = _primeira.Prox;
            _quantidade--;
            _versao++;
            return _primeira.Item;
        }

        public T RemoveRetornaFim()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            var aux = _primeira;
            while (aux.Prox != _ultima)
                aux = aux.Prox;

            var removida = _ultima.Item;
            _ultima = aux;
            _ultima.Prox = null;
            _quantidade--;
            _versao++;
            return removida;
        }

        public bool Remove(T valorItem)
        {
            var aux = _primeira;
            while (aux.Prox != null)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Prox.Item, valorItem))
                {
                    aux.Prox = aux.Prox.Prox;
                    if (aux.Prox == null)
                        _ultima = aux;
                    _quantidade--;
                    _versao++;
                    return true;
                }
                aux = aux.Prox;
            }
            return false;
        }

        public void Limpar()
        {
            while (_primeira.Prox != null)
                _primeira.Prox = _primeira.Prox.Prox;

            _quantidade = 0;
            _ultima = _primeira;
            _versao++;
        }

        public int Quantidade => _quantidade;

        public void Inverte()
        {
            if (_quantidade < 2) return;
            T[] vet = new T[_quantidade];
            CCelula<T> aux = _primeira.Prox;
            for (int i = 0; i < _quantidade; i++)
            {
                vet[i] = aux.Item;
                aux = aux.Prox;
            }
            aux = _primeira.Prox;
            for (int i = _quantidade - 1; i >= 0; i--)
            {
                aux.Item = vet[i];
                aux = aux.Prox;
            }
            _versao++;
        }

#nullable restore
        public static CLista<T> ConcatenaLista(CLista<T> l1, CLista<T> l2)
        {
            if (l1 == null)
                ThrowHelper.ColecaoNula(nameof(l1));
            if (l2 == null)
                ThrowHelper.ColecaoNula(nameof(l2));

            CLista<T> concatenada = new CLista<T>();
            foreach (T item in l1)
                concatenada.Adiciona(item);
            foreach (T item in l2)
                concatenada.Adiciona(item);

            return concatenada;
        }
#nullable disable

        public IEnumerator<T> GetEnumerator()
        {
            uint versao = _versao;
            for (var aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Implementação explícita da interface ICollection (métodos são acessíveis apenas via interface)
        void ICollection<T>.Add(T item) => Adiciona(item);

        void ICollection<T>.Clear() => Limpar();

        bool ICollection<T>.Contains(T item) => Contem(item);

#nullable restore
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                ThrowHelper.DestinoNulo(nameof(array));
            if (arrayIndex < 0)
                ThrowHelper.IndiceNegativo(nameof(arrayIndex));
            if (array.Length - arrayIndex < _quantidade)
                ThrowHelper.EspacoInsuficiente();
            if (_quantidade == 0)
                return;

            CCelula<T> aux = _primeira.Prox;
            for (int i = 0; i < _quantidade; i++)
            {
                array[arrayIndex + i] = aux.Item;
                aux = aux.Prox;
            }
        }
#nullable disable

        int ICollection<T>.Count => _quantidade;

        bool ICollection<T>.IsReadOnly => false;
    }
    #endregion

    #region Classe CCelulaDup<T> - representa a célula utilizada pelas classes CListaDup<T> e CDeque<T>
    /// <summary>
    /// Classe utilizada pelas classes CListaDup<T> e CDeque<T>
    /// </summary>
    public class CCelulaDup<T>
    {
        public T Item; // O Item armazenado pela célula
        public CCelulaDup<T> Ant; // Referencia a célula anterior
        public CCelulaDup<T> Prox; // Referencia a próxima célula

        public CCelulaDup() { }

        public CCelulaDup(T valorItem)
        {
            Item = valorItem;
        }

        public CCelulaDup(T valorItem, CCelulaDup<T> celulaAnt, CCelulaDup<T> proxCelula) : this(valorItem)
        {
            Ant = celulaAnt;
            Prox = proxCelula;
        }
    }
    #endregion

    #region Classe CListaDup<T> - Lista duplamente encadeada com célula cabeça
    /// <summary>
    /// Implementa uma lista duplamente encadeada genérica.
    /// </summary>
    public class CListaDup<T> : IEnumerable<T>, ICollection<T>
    {
        private CCelulaDup<T> _primeira; // Referencia a primeira célula da lista (célula cabeça)
        private CCelulaDup<T> _ultima; // Referencia a última célula da lista 
        private int _quantidade;
        private uint _versao;

        public CListaDup()
        {
            _primeira = new CCelulaDup<T>();
            _ultima = _primeira;
        }

#nullable restore
        public CListaDup(IEnumerable<T> colecao) : this()
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (T item in colecao)
                Adiciona(item);
        }
#nullable disable

        public bool EstaVazia => _primeira == _ultima;

        public T[] ParaVetor()
        {
            T[] vetor = new T[_quantidade];
            CCelulaDup<T> aux = _primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        public void Ordenar()
        {
            if (_quantidade < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                CCelulaDup<T> atual = _primeira.Prox;
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
            _versao++;
        }

        public void Adiciona(T valorItem)
        {
            _ultima.Prox = new CCelulaDup<T>(valorItem, _ultima, null);
            _ultima = _ultima.Prox;
            _quantidade++;
            _versao++;
        }

        public void InsereComeco(T valorItem)
        {
            if (_primeira == _ultima) // Se a lista estiver vazia insere no fim
            {
                _ultima.Prox = new CCelulaDup<T>(valorItem, _ultima, null);
                _ultima = _ultima.Prox;
            }
            else // senão insere no começo
            {
                _primeira.Prox = new CCelulaDup<T>(valorItem, _primeira, _primeira.Prox);
                _primeira.Prox.Prox.Ant = _primeira.Prox;
            }
            _quantidade++;
            _versao++;
        }

        public void RemoveComecoSemRetorno()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            _primeira = _primeira.Prox;
            _primeira.Ant = null;
            _quantidade--;
            _versao++;
        }

        public void InsereIndice(T valorItem, int posicao)
        {
            if (posicao < 1 || posicao > _quantidade + 1)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens + 1");

            CCelulaDup<T> aux = _primeira;
            for(int i = 0; i < posicao - 1; i++)
                aux = aux.Prox;

            CCelulaDup<T> nova = new CCelulaDup<T>(valorItem);
            // Encadeamento para frente
            nova.Prox = aux.Prox;
            nova.Ant = aux;
            if(aux.Prox != null) // Se não for inserção no fim
                aux.Prox.Ant = nova;
            else
                _ultima = nova;

            aux.Prox = nova;
            _quantidade++;
            _versao++;
        }

        public void RemoveIndice(int posicao)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            CCelulaDup<T> aux = _primeira;
            for (int i = 0; i < posicao - 1; i++)
            {
                aux = aux.Prox;
            }
            aux.Prox = aux.Prox.Prox;
            if (aux.Prox != null) //se a célula removida não era a última
                aux.Prox.Ant = aux;
            else
                _ultima = aux; //atualizar ponteiro Ultima
            _quantidade--;
            _versao++;
        }

        public void Imprime()
        {
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
                Console.WriteLine(aux.Item);
        }

        public void ImprimeInv()
        {
            for (CCelulaDup<T> aux = _ultima; aux.Ant != null; aux = aux.Ant)
                Console.WriteLine(aux.Item);
        }

        public bool Contem(T elemento)
        {
            bool achou = false;
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null && !achou; aux = aux.Prox)
                achou = EqualityComparer<T>.Default.Equals(aux.Item, elemento);
            return achou;
        }

        public T RetornaPrimeiro()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            return _primeira.Prox.Item;
        }

        public T RetornaIndice(int posicao)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            CCelulaDup<T> aux = _primeira.Prox;
            for (int i = 1; i < posicao; i++, aux = aux.Prox) ;
            return aux.Item;
        }

        public T RetornaUltimo()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            return _ultima.Item;
        }

        public void RemoveFimSemRetorno()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            _ultima = _ultima.Ant;
            _ultima.Prox = null;
            _quantidade--;
            _versao++;
        }

        public bool Remove(T valorItem)
        {
            if (_primeira != _ultima)
            {
                CCelulaDup<T> aux = _primeira.Prox;
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
                        _ultima = anterior;
                    _quantidade--;
                    _versao++;
                    return true;
                }
            }
            return false;
        }

        public T RemoveRetornaComeco()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            CCelulaDup<T> aux = _primeira.Prox;
            _primeira = _primeira.Prox;
            _primeira.Ant = null;
            _quantidade--;
            _versao++;
            return aux.Item;
        }

        public T RemoveRetornaFim()
        {
            if (_primeira == _ultima)
                ThrowHelper.ColecaoVazia("Lista");

            CCelulaDup<T> aux = _ultima;
            _ultima = _ultima.Ant;
            _ultima.Prox = null;
            _quantidade--;
            _versao++;
            return aux.Item;
        }

        public int Quantidade => _quantidade;

        public int PrimeiraOcorrenciaDe(T elemento)
        {
            int indice = 1;
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
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
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
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

        public int OcorrenciasDe(T elemento)
        {
            int ocorrencias = 0;
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (EqualityComparer<T>.Default.Equals(aux.Item, elemento))
                    ocorrencias++;
            }
            return ocorrencias;
        }

        public void AlteraIndice(int posicao, T elemento)
        {
            if (posicao < 1 || posicao > _quantidade)
                ThrowHelper.IndiceInvalido(nameof(posicao), "maior que 0 e menor ou igual a quantidade de itens");

            var aux = _primeira.Prox;
            for (int i = 1; i < posicao; i++)
                aux = aux.Prox;
            aux.Item = elemento;
            _versao++;
        }

        public void Limpar()
        {
            if (_primeira == _ultima) return;
            CCelulaDup<T> atual = _primeira.Prox;
            while (atual != null)
            {
                CCelulaDup<T> proxima = atual.Prox;
                atual.Ant = null;
                atual.Prox = null;
                atual = proxima;
            }
            _quantidade = 0;
            _primeira.Prox = null;
            _ultima = _primeira;
            _versao++;
        }

        public void Inverte()
        {
            int inicio = 1;
            int fim = _quantidade;
            CCelulaDup<T> inicial = _primeira.Prox;
            CCelulaDup<T> final = _ultima;
            while (inicio < fim)
            {
                T temp = inicial.Item;
                inicial.Item = final.Item;
                final.Item = temp;
                inicial = inicial.Prox;
                final = final.Ant;
                inicio++;
                fim--;
            }
            _versao++;
        }

#nullable restore
        public static CListaDup<T> ConcatenaLista(CListaDup<T> l1, CListaDup<T> l2)
        {
            if (l1 == null)
                ThrowHelper.ColecaoNula(nameof(l1));
            if (l2 == null)
                ThrowHelper.ColecaoNula(nameof(l2));

            CListaDup<T> concatenada = new CListaDup<T>();
            foreach (T item in l1)
                concatenada.Adiciona(item);
            foreach (T item in l2)
                concatenada.Adiciona(item);

            return concatenada;
        }
#nullable disable

        public IEnumerator<T> GetEnumerator()
        {
            uint versao = _versao;
            for (CCelulaDup<T> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<T>.Add(T item) => Adiciona(item);

        void ICollection<T>.Clear() => Limpar();

        bool ICollection<T>.Contains(T item) => Contem(item);

#nullable restore
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                ThrowHelper.DestinoNulo(nameof(array));
            if (arrayIndex < 0)
                ThrowHelper.IndiceNegativo(nameof(arrayIndex));
            if (array.Length - arrayIndex < _quantidade)
                ThrowHelper.EspacoInsuficiente();
            if (_quantidade == 0)
                return;

            CCelulaDup<T> aux = _primeira.Prox;
            for (int i = 0; i < _quantidade; i++)
            {
                array[arrayIndex + i] = aux.Item;
                aux = aux.Prox;
            }
        }
#nullable disable

        int ICollection<T>.Count => _quantidade;

        bool ICollection<T>.IsReadOnly => false;
    }
    #endregion

    #region Classe CDeque<T> - implementa uma coleção que funciona como uma fila e como uma pilha
    ///<summary>
    /// Implementa uma classe que funciona como fila e pilha ao mesmo tempo, permitindo que itens sejam inseridos e removidos em ambas as extremidades.
    /// </summary>
    public class CDeque<T> : IEnumerable<T>
    {
        private CCelulaDup<T> _esq; // Referencia o lado esquerdo do deque
        private CCelulaDup<T> _dir; // Referencia o lado direito do deque
        private int _quantidade;
        private uint _versao;

        public CDeque() { }

#nullable restore
        public CDeque(IEnumerable<T> colecao)
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (T item in colecao)
                AdicionaDireito(item);
        }
#nullable disable

        public void AdicionaDireito(T elemento)
        {
            CCelulaDup<T> nova = new CCelulaDup<T>(elemento, _dir, null);
            if (_dir != null)
                _dir.Prox = nova; // liga a célula antiga à nova
            else
                _esq = nova; // deque estava vazio

            _dir = nova; // atualiza o ponteiro do deque
            _quantidade++;
            _versao++;
        }

        public void AdicionaEsquerdo(T elemento)
        {
            CCelulaDup<T> nova = new CCelulaDup<T>(elemento, null, _esq);
            if (_esq != null)
                _esq.Ant = nova;
            else
                _dir = nova;

            _esq = nova;
            _quantidade++;
            _versao++;
        }

        public T RemoveRetornaEsquerdo()
        {
            if (_esq == null)
                ThrowHelper.ColecaoVazia("Coleção");

            T item = _esq.Item;
            if (_esq.Prox != null)
            {
                _esq = _esq.Prox;
                _esq.Ant = null;
            }
            else
            {
                _esq = null;
                _dir = null;
            }
            _quantidade--;
            _versao++;
            return item;
        }

        public T RemoveRetornaDireito()
        {
            if (_dir == null)
                ThrowHelper.ColecaoVazia("Coleção");

            T item = _dir.Item;
            if (_dir.Ant != null)
            {
                _dir = _dir.Ant;
                _dir.Prox = null;
            }
            else
            {
                _dir = null;
                _esq = null;
            }
            _quantidade--;
            _versao++;
            return item;
        }

        public T RetornaEsquerdo()
        {
            if (_esq == null)
                ThrowHelper.ColecaoVazia("Coleção");

            return _esq.Item;
        }

        public T RetornaDireito()
        {
            if (_dir == null)
                ThrowHelper.ColecaoVazia("Coleção");

            return _dir.Item;
        }

        // Copia o conteúdo de um deque para um vetor e o retorna.
        // Os itens são copiados da esquerda para a direita.
        public T[] ParaVetor()
        {
            T[] vetor = new T[_quantidade];
            CCelulaDup<T> aux = _esq;
            for (int i = 0; i < _quantidade; i++)
            {
                vetor[i] = aux.Item;
                aux = aux.Prox;
            }
            return vetor;
        }

        // Copia o conteúdo de dois deques recebidos por parâmetro para um único e o retorna.
        // A ordem de cópia e adição dos itens ao novo deque é da esquerda para a direita, bem como a do enumerador desta classe.
#nullable restore
        public static CDeque<T> ConcatenaDeque(CDeque<T> d1, CDeque<T> d2)
        {
            if (d1 == null)
                ThrowHelper.ColecaoNula(nameof(d1));
            if (d2 == null)
                ThrowHelper.ColecaoNula(nameof(d2));

            CDeque<T> concatenado = new CDeque<T>();
            foreach (T item in d1)
                concatenado.AdicionaDireito(item);
            foreach (T item in d2)
                concatenado.AdicionaDireito(item);

            return concatenado;
        }
#nullable disable

        public bool Contem(T elemento)
        {
            bool achou = false;
            for (CCelulaDup<T> aux = _esq; aux != null && !achou; aux = aux.Prox)
                achou = EqualityComparer<T>.Default.Equals(aux.Item, elemento);
            return achou;
        }

        public void Limpar()
        {
            if (_esq == null) return;
            CCelulaDup<T> atual = _esq;
            while (atual != null)
            {
                CCelulaDup<T> proxima = atual.Prox;
                atual.Ant = null;
                atual.Prox = null;
                atual = proxima;
            }
            _esq = null;
            _dir = null;
            _quantidade = 0;
            _versao++;
        }

        public bool EstaVazio => _esq == null;

        public int Quantidade => _quantidade;

        public IEnumerator<T> GetEnumerator()
        {
            uint versao = _versao;
            for (CCelulaDup<T> aux = _esq; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Possibilita iteração no deque de forma reversa (da direita para a esquerda)
        public IEnumerable<T> EnumerarReverso()
        {
            uint versao = _versao;
            for (CCelulaDup<T> aux = _dir; aux != null; aux = aux.Ant)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return aux.Item;
            }
        }
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
        public CCelulaDic() { }

        public CCelulaDic(TChave key, TValor value)
        {
            Chave = key;
            Valor = value;
        }
    }
    #endregion

    #region Classe CDicionario - implementa um dicionário chave e valor
    public class CDicionario<TChave, TValor> : IEnumerable<CPar<TChave, TValor>>, IEnumerable<KeyValuePair<TChave, TValor>>
    {
        private CCelulaDic<TChave, TValor> _primeira;
        private CCelulaDic<TChave, TValor> _ultima;
        private int _quantidade;
        private uint _versao;

        public CDicionario()
        {
            _primeira = new CCelulaDic<TChave, TValor>();
            _ultima = _primeira;
        }

#nullable restore
        public CDicionario(IEnumerable<KeyValuePair<TChave, TValor>> colecao) : this()
        {
            if (colecao == null)
                ThrowHelper.ColecaoNula(nameof(colecao));

            foreach (KeyValuePair<TChave, TValor> kvp in colecao)
                Adiciona(kvp.Key, kvp.Value);
        }
#nullable disable

        public bool EstaVazio => _primeira == _ultima;

        public void Adiciona(TChave chave, TValor valor)
        {
            //percorrer todo o dicionário para verificar se a chave passada como argumento já existe
            var aux = _primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, chave))
                    ThrowHelper.ChaveDuplicada(chave);

                aux = aux.Prox;
            }
            // Se chegou aqui, é porque a chave não existe
            _ultima.Prox = new CCelulaDic<TChave, TValor>(chave, valor);
            _ultima = _ultima.Prox;
            _quantidade++;
            _versao++;
        }

        public bool Remove(TChave chave)
        {
            CCelulaDic<TChave, TValor> aux = _primeira;
            while (aux.Prox != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Prox.Chave, chave))
                {
                    aux.Prox = aux.Prox.Prox;
                    if (aux.Prox == null)
                        _ultima = aux;
                    _quantidade--;
                    _versao++;
                    return true;
                }
                aux = aux.Prox;
            }
            return false;
        }

        public TValor RetornaValor(TChave chave)
        {
            CCelulaDic<TChave, TValor> aux = _primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, chave))
                    return aux.Valor;
                aux = aux.Prox;
            }
            //se percorreu todo o loop, é porque a chave passada por parâmetro não foi encontrada
            ThrowHelper.ChaveNaoEncontrada(chave);
            return default;
        }

        // Altera o valor associado à uma chave. Se a chave não existir, ela será criada.
        public void InsereValor(TChave chave, TValor valor)
        {
            var aux = _primeira.Prox;
            while (aux != null)
            {
                if (EqualityComparer<TChave>.Default.Equals(aux.Chave, chave))
                {
                    aux.Valor = valor;
                    _versao++;
                    return;
                }
                aux = aux.Prox;
            }
            _ultima.Prox = new CCelulaDic<TChave, TValor>(chave, valor);
            _ultima = _ultima.Prox;
            _quantidade++;
            _versao++;
        }

        public TChave[] Chaves()
        {
            TChave[] vetor = new TChave[_quantidade];
            var aux = _primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Chave;
                aux = aux.Prox;
            }
            return vetor;
        }

        public TValor[] Valores()
        {
            TValor[] vetor = new TValor[_quantidade];
            var aux = _primeira.Prox;
            for (int i = 0; i < vetor.Length; i++)
            {
                vetor[i] = aux.Valor;
                aux = aux.Prox;
            }
            return vetor;
        }

        public int Quantidade => _quantidade;

        public bool ContemChave(TChave chave)
        {
            CCelulaDic<TChave, TValor> aux = _primeira.Prox;
            bool achou = false;
            while (aux != null && !achou)
            {
                achou = EqualityComparer<TChave>.Default.Equals(aux.Chave, chave);
                aux = aux.Prox;
            }
            return achou;
        }

        public bool ContemValor(TValor valor)
        {
            CCelulaDic<TChave, TValor> aux = _primeira.Prox;
            bool achou = false;
            while (aux != null && !achou)
            {
                achou = EqualityComparer<TValor>.Default.Equals(aux.Valor, valor);
                aux = aux.Prox;
            }
            return achou;
        }

        public void Ordenar() // Implementa um método de ordenação por chaves
        {
            if (_quantidade < 2) return;
            bool houveTroca = true;
            while (houveTroca)
            {
                houveTroca = false;
                for (var atual = _primeira.Prox; atual != null && atual.Prox != null; atual = atual.Prox)
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
            _versao++;
        }

        public void Limpar()
        {
            while (_primeira.Prox != null)
                _primeira.Prox = _primeira.Prox.Prox;

            _quantidade = 0;
            _ultima = _primeira;
            _versao++;
        }

#nullable restore
        public static CDicionario<TChave, TValor> ConcatenaDicionario(CDicionario<TChave, TValor> d1, CDicionario<TChave, TValor> d2)
        {
            if (d1 == null)
                ThrowHelper.ColecaoNula(nameof(d1));
            if (d2 == null)
                ThrowHelper.ColecaoNula(nameof(d2));

            CDicionario<TChave, TValor> concatenado = new CDicionario<TChave, TValor>();
            foreach (CPar<TChave, TValor> par in d1)
                concatenado.Adiciona(par.Chave, par.Valor);
            foreach (CPar<TChave, TValor> par in d2)
                concatenado.Adiciona(par.Chave, par.Valor);

            return concatenado;
        }
#nullable disable

        public IEnumerator<CPar<TChave, TValor>> GetEnumerator()
        {
            uint versao = _versao;
            for (CCelulaDic<TChave, TValor> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return new CPar<TChave, TValor>(aux.Chave, aux.Valor);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Implementação explícita da interface IEnumerable com KeyValuePair para fins de interoperabilidade.
        // A implementação desta interface permite copiar o CDicionario para outras classes através de construtoras que aceitem IEnumerable<KeyValuePair<TChave, TValor>>.
        IEnumerator<KeyValuePair<TChave, TValor>> IEnumerable<KeyValuePair<TChave, TValor>>.GetEnumerator()
        {
            uint versao = _versao;
            for (CCelulaDic<TChave, TValor> aux = _primeira.Prox; aux != null; aux = aux.Prox)
            {
                if (versao != _versao)
                    ThrowHelper.ColecaoModificada();

                yield return new KeyValuePair<TChave, TValor>(aux.Chave, aux.Valor);
            }
        }
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

    #region Classe ThrowHelper - classe auxiliar para lançamento de exceções quando necessário
    internal static class ThrowHelper
    {
        ///<summary>Lança uma exceção quando se tenta remover um item de uma coleção vazia.</summary>
        [DoesNotReturn]
        internal static void ColecaoVazia(string nomeColecao) =>
            throw new InvalidOperationException($"{nomeColecao} vazia.");

        ///<summary>Lança uma exceção quando se tenta acessar um índice inválido nas listas.</summary>
        internal static void IndiceInvalido(string nomeParametro, string intervalo) =>
            throw new ArgumentOutOfRangeException(nomeParametro, $"O índice especificado estava fora do intervalo válido. Deve ser {intervalo}.");

        ///<summary>Lança uma exceção quando se tenta adicionar um elemento antes ou depois de um elemento que não existe nas listas.</summary>
        internal static void ReferenciaNaoEncontrada(string nomeParametro) =>
            throw new ArgumentException("O elemento de referência especificado não foi encontrado na lista.", nomeParametro);

        ///<summary>Lança uma exceção para tentativas de inserção de chave duplicada no dicionário.</summary>
        internal static void ChaveDuplicada<TChave>(TChave chave) =>
            throw new ArgumentException($"Um item com a mesma chave já foi adicionado. Chave: {chave}");

        ///<summary>Lança uma exceção para chaves não encontradas no dicionário.</summary>
        internal static void ChaveNaoEncontrada<TChave>(TChave chave) =>
            throw new KeyNotFoundException($"A chave '{chave}' não foi encontrada no dicionário.");

        ///<summary>Lança uma exceção para coleções modificadas durante loops foreach</summary>
        internal static void ColecaoModificada() =>
            throw new InvalidOperationException("A coleção foi modificada. Operação de enumeração cancelada.");

        ///<summary>Lança uma exceção quando se tenta copiar coleções nulas.</summary>
        [DoesNotReturn]
        internal static void ColecaoNula(string nomeParametro) =>
            throw new ArgumentNullException(nomeParametro, "A coleção a copiar não pode ser nula.");

        /// <summary>
        /// <summary>Lança uma exceção quando o array de destino é nulo na interface ICollection das listas.</summary>
        [DoesNotReturn]
        internal static void DestinoNulo(string nomeParametro) =>
            throw new ArgumentNullException(nomeParametro, "O array de destino não pode ser nulo.");

        /// <summary>Lança uma exceção quando o índice de destino é negativo na interface ICollection das listas./// </summary>
        internal static void IndiceNegativo(string nomeParametro) =>
            throw new ArgumentOutOfRangeException(nomeParametro, "O índice de destino não pode ser negativo.");

        ///<summary>Lança uma exceção se o array de destino não possuir espaço suficiente para copiar a lista através da interface ICollection.</summary>
        internal static void EspacoInsuficiente() =>
            throw new ArgumentException("O array de destino não possui espaço suficiente.");
    }
    #endregion
}
#nullable restore