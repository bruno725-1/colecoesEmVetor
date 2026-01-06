using System;
using System.Collections.Generic;
using AED;
class Teste
{
    public static void Rodar()
    {
        CListaVet<int> lista = new CListaVet<int>(10);
        for(int i = 1; i <= 10; i++)
            lista.Adiciona(i);
        Console.WriteLine(string.Join(", ", lista));
        lista.RemoveFaixa(2, 8);
        Console.WriteLine("Lista após RemoveFaixa:");
        Console.WriteLine(string.Join(", ", lista));
        Console.WriteLine($"Quantidade de elementos: {lista.Quantidade}");
        int[] vetor = {3, 4, 5, 6, 7, 8, 9, 10};
        lista.AdicionaFaixa(vetor);
        Console.WriteLine($"Quantidade de elementos: {lista.Quantidade}");
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
        Console.WriteLine(string.Join(", ", lista));
        lista.Limpar();
        Console.WriteLine($"Quantidade de elementos após limpar: {lista.Quantidade}");
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
        lista.CortarExcessos();
        Console.WriteLine($"Capacidade da lista após cortar excessos: {lista.Capacidade}");
        lista.Adiciona(2026);
        Console.WriteLine($"Quantidade de elementos após adicionar: {lista.Quantidade}");
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
    }
}