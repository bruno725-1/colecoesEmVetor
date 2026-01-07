using System;
using System.Collections.Generic;
using System.Linq;
using AED;
class Teste
{
    public static void Rodar()
    {
        CPilhaVet<int> p1 = new CPilhaVet<int>();
        CPilhaVet<int> p2 = new CPilhaVet<int>();
        for (int i = 1; i <= 10; i++)
            p1.Empilha(i);
        for (int i = 11; i <= 20; i++)
            p2.Empilha(i);
        Console.WriteLine("P1:");
        Console.WriteLine(string.Join(", ", p1));
        Console.WriteLine("P2:");
        Console.WriteLine(string.Join(", ", p2));
        Console.WriteLine($"P1 contém 5? {p1.Contem(5)}");
        Console.WriteLine($"P2 contém 12? {p2.Contem(12)}");
        Console.WriteLine($"P1 contém 25? {p1.Contem(25)}");
        Console.WriteLine($"P2 contém 32? {p2.Contem(32)}");
        p1 = null;
        CPilhaVet<int> concatenada = CPilhaVet<int>.ConcatenaPilha(p1, p2);
    }
}