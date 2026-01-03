using System;
using System.Collections.Generic;
class Teste
{
    public static void Rodar()
    {
        CListaVet<int> lista = new CListaVet<int>();
        lista.Adiciona(3);
        lista.Adiciona(48);
        ICollection<int> collection = lista;
        Console.WriteLine(string.Join(", ", collection));
        collection.Clear();
        Console.WriteLine("Lista após Clear executado via ICollection:");
        Console.WriteLine(string.Join(", ", collection));
    }
}