using System;
using System.Collections.Generic;
class Teste
{
    public static void Rodar()
    {
        int a = Array.MaxLength;
        List<byte> lista = new List<byte>(a);
        for(int i = 0; i < a; i++)
            lista.Add(48);
        byte[] vetor = new byte[600000000];
        for(int i = 0; i < 600000000; i++)
            vetor[i] = 48;
        lista.AddRange(vetor);
    }
}