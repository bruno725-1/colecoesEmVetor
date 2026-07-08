using System;
using AED;
class Program
{
    public static void Main(string[] args)
    {
        CDequeVet<int> deque = new CDequeVet<int>();
        for(int i = 1; i <=6; i++)
            deque.AdicionaDireito(i);

        Console.WriteLine(string.Join(", ", deque));
        CDequeVet<int> deque2 = new CDequeVet<int>();
        for(int i = 30; i <= 55; i++)
            deque2.AdicionaDireito(i);

        CDequeVet<int> concatenado = CDequeVet<int>.ConcatenaDeque(deque, deque2);
        for(int i = 0; i < 6; i++)
            concatenado.RemoveRetornaEsquerdo();
        for(int i = 0; i < 6; i++)
            concatenado.AdicionaDireito(i * 4);
        Console.WriteLine(string.Join(", ", concatenado));
        Console.WriteLine($"{concatenado.Quantidade}, {concatenado.Capacidade}");
        Console.WriteLine($"{concatenado._esq}, {concatenado._dir}");
    }
}