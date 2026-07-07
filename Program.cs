using System;
class Program
{
    public static void Main(string[] args)
    {
        CFilaVet<int> fila = new CFilaVet<int>();
        CListaVet<int> lista = new CListaVet<int>();
        CPilhaVet<int> pilha = new CPilhaVet<int>();
        for (int i = 1; i <= 50; i++)
        {
            fila.Enfileira(i);
            lista.Adiciona(i);
            pilha.Empilha(i);

            Console.WriteLine($"Quantidade de elementos: {fila.Quantidade}");
            Console.WriteLine($"Quantidade de elementos: {lista.Quantidade}");
            Console.WriteLine($"Quantidade de elementos: {pilha.Quantidade}");
        }
    }
}