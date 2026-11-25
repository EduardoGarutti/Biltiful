 void InicializarPrograma()
{
    while (true)
    {
        var escolha = Utils.CriarMenu(new List<string>(){
        "============BIUTIFUL============\n",
        "[1] - Cadastrar\n",
        "[2] - Consultar\n",
        "[3] - Vender\n",
        "[4] - Comprar\n",
        "[5] - Produzir\n",
        "[0] - Sair\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                MenuCadastro();
                break;
            case '2':
                MenuConsultas();
                break;
            case '3':
                MenuVendas();
                break;
            case '4':
                MenuCompras();
                break;
            case '5':
                MenuProducao();
                break;
            case '0':
                Console.WriteLine("Encerrando programa...");
                Environment.Exit(0);
            default:
                Console.WriteLine("Opção invalida!");
                break;
        }
    }
}

void MenuCadastro()
{
    while (true)
    {
        var escolha = Utils.CriarMenu(new List<string>(){
        "============BIUTIFUL============\n",
        "[1] - Cadastrar\n",
        "[2] - Consultar\n",
        "[3] - Vender\n",
        "[4] - Comprar\n",
        "[5] - Produzir\n",
        "[0] - Sair\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                CadastrarCliente();
                break;
            case '2':
                CadastrarFornecedor();
                break;
            case '3':
                CadastrarProduto();
                break;
            case '4':
                CadastrarMateriaPrima();
                break;
            case '0':
                return;
            default:
                Console.WriteLine("Opção Invalida!");
                break;
        }
    }
}

void CadastrarCliente()
{
    Console.WriteLine("Informe Nome do Cliente: ");
    string nome = Console.ReadLine();

    int idade;
}