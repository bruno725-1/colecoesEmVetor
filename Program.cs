using System;
using System.Collections.Generic;
class Program
{
    public static void Main(string[] args)
    {
        CListaVet<int> lista = new CListaVet<int>();
        for(int i = 1; i <= 400; i++)
            lista.Adiciona(i);

        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
        Console.WriteLine($"Quantidade de itens: {lista.Quantidade}");
        lista.Capacidade = 2000;
        CListaVet<int> outraLista = new CListaVet<int>(lista);
        Console.WriteLine($"Capacidade da outra lista: {outraLista.Capacidade}");
        Console.WriteLine($"Quantidade de itens: {outraLista.Quantidade}");
        lista.CortarExcessos();
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
        Console.WriteLine($"Quantidade de itens: {lista.Quantidade}");
    }
}