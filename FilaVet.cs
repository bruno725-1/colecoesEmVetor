using System;
class FilaVet
{
    public static void Rodar()
    {
        // Criando uma fila de strings
        CFilaVet<string> fila = new CFilaVet<string>();
        // Enfileira - adiciona elementos ao final da fila
        fila.Enfileira("João");
        fila.Enfileira("Maria");
        fila.Enfileira("Carlos");
        Console.WriteLine("Fila após Enfileira: " + String.Join(", ", fila));

        // Peek - retorna o elemento do início sem removê-lo
        string primeiro = fila.Peek();
        Console.WriteLine($"Primeiro da fila (Peek): {primeiro}");

        // Contem - verifica se um elemento está na fila
        Console.WriteLine("A fila contém 'Maria'? " + fila.Contem("Maria"));
        Console.WriteLine("A fila contém 'Ana'? " + fila.Contem("Ana"));

        // Quantidade - quantidade de elementos na fila
        Console.WriteLine("Quantidade de elementos na fila: " + fila.Quantidade);

        // Desenfileira - remove e retorna o primeiro elemento da fila
        string removido = fila.Desenfileira();
        Console.WriteLine($"Elemento removido (Desenfileira): {removido}");
        Console.WriteLine("Fila após Desenfileira: " + string.Join(", ", fila));
        // Limpar - remove todos os elementos da fila
        fila.Limpar();
        Console.WriteLine("Fila após Limpar: " + string.Join(", ", fila));
        Console.WriteLine("Quantidade após Limpar: " + fila.Quantidade);
        // Capacidade - propriedade que retorna o tamanho do vetor interno
        Console.WriteLine("Capacidade do vetor interno da fila: " + fila.Capacidade);
        // CortarExcessos - reduz o tamanho do vetor interno para o número real de elementos
        fila.CortarExcessos();
        Console.WriteLine($"Nova capacidade: {fila.Capacidade}");
        fila.Enfileira("lava");
        Console.WriteLine($"Capacidade da fila após primeiro Enfileira: {fila.Capacidade} (capacidade padrão)");
        fila.Enfileira("fumaça");
        fila.Enfileira("cinzas");
        fila.Enfileira("explosões");
        fila.Enfileira("terremoto");
        fila.Enfileira("Tsunami");
        fila.Enfileira("fonte termal");
        Console.WriteLine("Quantidade de elementos: " + fila.Quantidade);
        Console.WriteLine($"Capacidade do vetor interno: {fila.Capacidade}");
        Console.WriteLine($"Fila após vários Enfileiras: {string.Join(", ", fila)}");
    }
}