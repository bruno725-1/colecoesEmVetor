using System;
using System.Collections.Generic;
class Teste
{
    public static void Rodar()
    {
        int a = int.MaxValue;
        Console.WriteLine($"Valor de a: {a}");
        int b = a + 600000000;
        Console.WriteLine($"Valor de b: {b}");
        Console.WriteLine($"Valor do uint b: {(uint)b}");
        Console.WriteLine($"b é maior que a? {b > a}");
        Console.WriteLine($"O uint b é maior que a? {(uint)b > a}");
    }
}