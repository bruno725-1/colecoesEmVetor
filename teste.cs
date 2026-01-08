using System;
using System.Collections.Generic;
using System.Linq;
using AED;
class Teste
{
    public static void Rodar()
    {
        CPilhaVet<int> pilha = new CPilhaVet<int>();
        for (int i = 1; i <= 10; i++)
            pilha.Empilha(i);
        for (int i = 1; i <= 15; i++)
        {
            if(!pilha.Vazia)
            {
                Console.WriteLine($"Número desempilhado: {pilha.Desempilha()}");
                Console.WriteLine($"Quantidade de itens: {pilha.Quantidade}");
            }
            else
            {
                Console.WriteLine("Num foi possível desempilhar nada não");
                Console.WriteLine($"Número desempilhado: nenhum é claro");
                Console.WriteLine($"Quantidade de itens: {pilha.Quantidade}");
            }
        }
    }
}