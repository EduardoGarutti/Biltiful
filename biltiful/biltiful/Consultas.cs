public class Consultas() 
{
    public static void Consultar<T>(List<T> lista)
    {
        int controle = 0;

        if (lista.Count == 0)
        {
            Console.WriteLine("Não existem cadastros!");
            return;
        }

        while (true)
        {
            var escolha = Utils.CriarMenu(new List<string>(){
            "============CONSULTAS============\n",
            "[1] - Primeiro Cadastro\n",
            "[2] - Próximo Cadastro\n",
            "[3] - Cadastro Anterior\n",
            "[4] - Último Cadastro\n",
            "[0] - Sair\n",
            "================================"
        });

            switch (escolha)
            {
                case '1':
                    controle = 0;
                    break;
                case '2':
                    if (controle < lista.Count - 1)
                        controle++;
                    else
                        Console.WriteLine("Você já está no último cadastro!");
                    break;
                case '3':
                    if (controle > 0)
                        controle--;
                    else
                        Console.WriteLine("Você já está no primeiro cadastro!");
                    break;
                case '4':
                    controle = lista.Count - 1;
                    break;
                case '0':
                    return;
                default:
                    Console.WriteLine("Opção inválida!");
                    continue;
            }
            Console.WriteLine(lista[controle]);
        }
    }

    public static void ImprimirLista(List<Produto> lista) { 
    
    }
}

