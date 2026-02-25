using System;
class Program {
    public static void Main (string[] args)
    {
        CFilaVet<int> fila = new CFilaVet<int>();
        for(int i = 1; i <= 10; i++)
            fila.Enfileira(i);

        int[] vetor = fila.ParaVetor();
        Console.WriteLine("Imprimindo o vetor resultante da conversão:");
        Console.WriteLine(string.Join(", ", vetor));
        Console.WriteLine($"Comprimento do vetor: {vetor.Length}");
    }
}