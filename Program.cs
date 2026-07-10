using System;
class Program
{
    public static void Main(string[] args)
    {
        CDequeVet<int> deque = new CDequeVet<int>();
        for(int i = 1; i <= 6; i++)
            deque.AdicionaEsquerdo(i);

        Console.WriteLine($"Deque antes de limpar: {string.Join(", ", deque)}");
        deque.Limpar();
        Console.WriteLine($"Deque após limpar: {string.Join(", ", deque)}");
        int[] vetor = deque.ParaVetor();
        Console.WriteLine($"Vetor: {string.Join(", ", vetor)}");
        Console.WriteLine($"Comprimento do vetor: {vetor.Length}");
        Console.WriteLine($"Iteração reversa no deque: {string.Join(", ", deque.EnumerarReverso())}");
    }
}