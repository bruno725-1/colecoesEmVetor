using System;
using System.Collections.Generic;
using AED;
class Teste
{
    public static void Rodar()
    {
        CFilaVet<int> fila = new CFilaVet<int>();
        for(int i = 1; i <= 6; i++)
            fila.Enfileira(i);

        for(int i = 0; i < 3; i++)
            fila.Desenfileira();

        Console.WriteLine($"Frente da fila: {fila._frente}");
        Console.WriteLine($"Trás da fila: {fila._tras}");
        int[] vetor = fila.ParaVetor();

        Console.WriteLine("Imprimindo o vetor:");
        Console.WriteLine(string.Join(", ", vetor));
        Console.WriteLine($"Imprimindo a fila:");
        while(fila.Quantidade > 0)
        Console.Write(fila.Desenfileira() + ", ");
    }
}