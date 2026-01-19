using System;
class ListaVet
{
    public static void Rodar()
    {
        // Criando uma lista de inteiros
        CListaVet<int> numeros = new CListaVet<int>();

        // Adiciona - adiciona elementos à lista
        numeros.Adiciona(10);
        numeros.Adiciona(50);
        numeros.Adiciona(30);
        numeros.Adiciona(40);
        numeros.Adiciona(20);
        Console.WriteLine("Lista após Adiciona: " + string.Join(", ", numeros));

        // Limpar - remove todos os elementos
        CListaVet<int> outraLista = new CListaVet<int>(new int[]{1, 2, 3});
        outraLista.Limpar();
        Console.WriteLine("Outra lista após Limpar: " + string.Join(", ", outraLista));
        // Capacidade - propriedade que retorna o número de elementos que podem ser adicionados antes que o vetor interno precise ser redimensionado
        // também podemos usar esta propriedade para redimensionar o vetor interno, quando sabemos de quanto espaço a lista vai precisar
        Console.WriteLine("Capacidade de outra lista após Limpar: " + outraLista.Capacidade);

        // Contem - verifica se um elemento existe
        Console.WriteLine("Contém 20? " + numeros.Contem(20));

        // Quantidade - propriedade que retorna o número de elementos
        Console.WriteLine("Quantidade de elementos: " + numeros.Quantidade);

        // PrimeiraOcorrenciaDe - retorna a primeira ocorrência de um elemento
        Console.WriteLine("Índice do 30: " + numeros.PrimeiraOcorrenciaDe(30));

        // InsereIndice - insere um elemento em um índice específico
        numeros.InsereIndice(25, 2); // insere 25 na posição 2
        Console.WriteLine("Lista após InsereIndice: " + string.Join(", ", numeros));

        // UltimaOcorrenciaDe - última ocorrência
        numeros.Adiciona(30);
        Console.WriteLine("Último índice do 30: " + numeros.UltimaOcorrenciaDe(30));

        // Remove - remove a primeira ocorrência do valor
        numeros.Remove(30);
        Console.WriteLine("Lista após Remove(30): " + string.Join(", ", numeros));

        // RemoveIndice - remove pelo índice
        numeros.RemoveIndice(0); // remove o primeiro elemento
        Console.WriteLine("Lista após RemoveIndice(0): " + string.Join(", ", numeros));

        // Remove faixa - remove um intervalo de elementos
        numeros.RemoveFaixa(0, 2); // remove os dois primeiros elementos
        Console.WriteLine("Lista após RemoveFaixa(0, 2): " + string.Join(", ", numeros));

        // Inverte - inverte a ordem da lista
        numeros.Inverte();
        Console.WriteLine("Lista após Inverte: " + string.Join(", ", numeros));

        // Ordenar - ordena a lista
        numeros.Ordenar();
        Console.WriteLine("Lista após Ordenar: " + string.Join(", ", numeros));

        // ParaVetor - converte para array
        int[] array = numeros.ParaVetor();
        Console.WriteLine("Array convertido da lista: " + string.Join(", ", array));

        // CortarExcessos - ajusta capacidade da lista para a quantidade real de elementos
        CListaVet<int> grandeLista = new CListaVet<int>(1000);
        // AdicionaFaixa - adiciona uma coleção de elementos no fim da lista
        grandeLista.AdicionaFaixa(new int[]{1, 2, 3});
        Console.WriteLine($"Capacidade antes do CortarExcessos: {grandeLista.Capacidade}");
        grandeLista.CortarExcessos();
        Console.WriteLine($"Capacidade após CortarExcessos: {grandeLista.Capacidade}");
        Console.WriteLine("Lista números: " + string.Join(", ", numeros));
        Console.WriteLine($"Capacidade da lista números: {numeros.Capacidade}");
        Console.WriteLine("Quantidade de itens da lista números: " + numeros.Quantidade);
        numeros.CortarExcessos();
        Console.WriteLine("Capacidade após cortar excessos: " + numeros.Capacidade);
        numeros.AdicionaFaixa(new int[]{95, 98, 2001});
        Console.WriteLine("Lista números após AdicionaFaixa: " + string.Join(", ", numeros));
        Console.WriteLine("Capacidade da lista números: " + numeros.Capacidade);
    }
}