using System;
using System.Collections.Generic;
class Teste
{
    public static void Rodar()
    {
        CListaVet<int> lista = new CListaVet<int>();
        ICollection<int> col = lista;
        try
        {
            for (int i = 0; i <= 20; i++)
                col.Add(i);

        }
        catch (System.IndexOutOfRangeException)
        {
            Console.WriteLine($"Será que o erro foi aqui???");
        }
        CListaVet<int> lista2 = new CListaVet<int>(lista);
        Console.WriteLine(string.Join(", ", lista2));
        Console.WriteLine($"Quantidade de itens da lista 1: {col.Count}");
        Console.WriteLine($"Quantidade de itens da lista 2: {lista2.Quantidade}");
        Console.WriteLine($"Capacidade da lista 1: {lista.Capacidade}");
        Console.WriteLine($"Capacidade da lista 2: {lista2.Capacidade}");
        lista.Capacidade = 225;
        Console.WriteLine($"Capacidade da lista 1 após usar o novíssimo setter dela: {lista.Capacidade}");
        lista.Limpar();
        lista.CortarExcessos();
        Console.WriteLine($"Quantidade de itens da lista 1: {col.Count}");
        Console.WriteLine($"Capacidade da lista 1 após cortar excessos: {lista.Capacidade}");
        lista.Adiciona(7555);
        lista.Adiciona(9255);
        lista.Adiciona(1655);
        lista.Adiciona(8700);
        Console.WriteLine(string.Join(", ", lista));
        Console.WriteLine($"Quantidade de itens da lista 1: {col.Count}");
        Console.WriteLine($"Capacidade da lista 1 após adicionar novos itens: {lista.Capacidade}");
    }
}