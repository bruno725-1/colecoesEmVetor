using System;
using System.Collections.Generic;
using AED;
class Teste
{
    public static void Rodar()
    {
        CFilaVet<int> fila = new CFilaVet<int>();
        fila.Enfileira(1);
        fila.Enfileira(2);
        fila.Enfileira(3);
        fila.Enfileira(4);
        Console.WriteLine("Desenfileirando:");
        Console.WriteLine(fila.Desenfileira());
        Console.WriteLine(fila.Desenfileira());
        fila.Enfileira(5);
        fila.Enfileira(6);
        fila.Enfileira(7); // aqui força wrap-around
        Console.WriteLine($"Quantidade de itens: {fila.Quantidade}");
        Console.WriteLine($"Capacidade: {fila.Capacidade}");
        fila.Enfileira(8);
        Console.WriteLine($"Quantidade de itens: {fila.Quantidade}");
        Console.WriteLine($"Capacidade: {fila.Capacidade}");
        /*Console.WriteLine("Desenfileirando a porra toda:");
        while(fila.Quantidade > 0)
            Console.Write(fila.Desenfileira() + ", ");*/
        fila.Enfileira(9);
        Console.WriteLine($"Quantidade de itens: {fila.Quantidade}");
        Console.WriteLine($"Capacidade: {fila.Capacidade}");
        Console.WriteLine("Desenfileirando a porra toda:");
        while(fila.Quantidade > 0)
            Console.Write(fila.Desenfileira() + ", ");
    }
}