using System;
using System.Collections.Generic;
using AED;
class Teste
{
    public static void Rodar()
    {
        CPilhaVet<int> pilha = new CPilhaVet<int>();
        for (int i = 1; i <= 10; i++)
            pilha.Empilha(i);

        CListaVet<int> lista = new CListaVet<int>(pilha);
        Console.WriteLine("Imprimindo a pilha:");
        Console.WriteLine(string.Join(", ", pilha));
        Console.WriteLine("Imprimindo a lista:");
        Console.WriteLine(string.Join(", ", lista));
        Console.WriteLine($"Capacidade da pilha: {pilha.Capacidade}");
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
    }
}