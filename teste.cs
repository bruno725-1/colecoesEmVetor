using System;
using System.Collections.Generic;
using System.Linq;
using AED;
class Teste
{
    public static void Rodar()
    {
        CLista<int> lista = new CLista<int>();
        for(int i = 1; i <= 10; i++)
            lista.InsereFim(i);
        CPilhaVet<int> pilha = new CPilhaVet<int>(lista);
        Console.WriteLine("Imprimindo a pilha original:");
        Console.WriteLine(string.Join(", ", pilha));
        foreach(var item in pilha)
        {
            pilha.Desempilha();
            Console.WriteLine("Escreve um número");
            int num = int.Parse(Console.ReadLine());
            pilha.Empilha(num);
        }
        Console.WriteLine("Imprimindo a pilha modificada:");
        Console.WriteLine(string.Join(", ", pilha));
        Console.WriteLine($"Provavelmente a pilha foi modificada, não tenho certeza, mas se sim:");
        Console.WriteLine("Hahahahahahahahaha! Modifiquei a pilha e o enumerador nem percebeu");
        Console.WriteLine("É por isso que se precisa de atributo de versão, mesmo em coleções como pilhas e filas, que não se pode alterar os itens por índice harbitrário");
        /*int[] vetor = pilha.ToArray();
        Console.WriteLine("Imprimindo o vetor:");
        Console.WriteLine(string.Join(", ", vetor));
        Console.WriteLine($"Capacidade da pilha: {pilha.Capacity}");*/
    }
}