using System;
using System.Collections.Generic;
class Teste
{
    public static void Rodar()
    {
        List<int> ints = new List<int>(30);
        for(int i = 1; i <= 30; i++)
            ints.Add(i);
        List<int> ints1 = null;
        CListaVet<int> lista = new CListaVet<int>(ints1);
        Console.WriteLine($"Último item da lista: {lista[lista.Quantidade - 1]}");
    }
}