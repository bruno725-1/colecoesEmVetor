using System;
using System.Collections.Generic;
using AED;
class Teste
{
    public static void Rodar()
    {
        int a = Array.MaxLength;
        CListaVet<byte> lista = new CListaVet<byte>(a);
        for(int i = 0; i < a; i++)
            lista.Adiciona(48);
        Console.WriteLine("Aperte qualquer tecla para ver tudo explodir");
        Console.ReadKey();
        lista.Adiciona(48);
        CLista<byte> lista2 = new CLista<byte>();
        for(int i = 0; i < 600; i++)
            lista2.InsereFim(48);
        lista.AdicionaFaixa(lista2);
    }
}