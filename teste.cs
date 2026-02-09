using System;
class Teste
{
    public static void Rodar()
    {
        CFilaVet<int> fila = new CFilaVet<int>();
        for (int i = 1; i <= 10; i++)
            fila.Enfileira(i);

        for (int i = 0; i < 5; i++)
            fila.Desenfileira();

        CListaVet<int> lista = new CListaVet<int>(2);
        lista.Adiciona(1);
        lista.Adiciona(2);
        lista.AdicionaFaixa(fila);
        Console.WriteLine($"Capacidade da fila: {fila.Capacidade}");
        Console.WriteLine($"Quantidade de itens: {fila.Quantidade}");
        Console.WriteLine($"Capacidade da lista: {lista.Capacidade}");
        Console.WriteLine($"Quantidade de itens: {lista.Quantidade}");
        Console.WriteLine("Imprimindo a lista:");
        Console.WriteLine(string.Join(", ", lista));
        Console.WriteLine($"Imprimindo a fila:");
        Console.WriteLine(string.Join(", ", fila));
    }
}