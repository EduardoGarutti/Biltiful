List<Cliente> clientes = Cliente.CarregarClientes();
List<Fornecedor> fornecedores = Fornecedor.CarregarFornecedores();
List<Produto> produtos = Produto.CarregarProdutos();
List<MateriaPrima> materiasPrimas = MateriaPrima.CarregarMateriaPrima();
List<string> inadimplentes = Banidos.CarregarArquivo("risco.dat");
List<string> bloqueados = Banidos.CarregarArquivo("banidos.dat");
//--------------------------------------------------------------------------------
List<Venda> vendas = Vendas.CarregarVendas();
List<Compra> compras = Compra.CarregarCompras();

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
                MenuComprar();
                break;
            case '5':
                MenuProducao();
                break;
            case '0':
                GravarNoArquivo();
                Console.WriteLine("Encerrando programa...");
                Environment.Exit(0);
                break;
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
        "[1] - Cadastrar Cliente\n",
        "[2] - Cadastrar Fornecedor\n",
        "[3] - Cadastrar Produto\n",
        "[4] - Cadastrar Matéria Prima\n",
        "[5] - Cadastrar Cliente Inadimplente\n",
        "[6] - Cadastrar Fornecedor Bloqueado\n",
        "[0] - Voltar\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                Cadastrar.CadastrarCliente(clientes);
                break;
            case '2':
                Cadastrar.CadastrarFornecedor(fornecedores);
                break;
            case '3':
                Cadastrar.CadastrarProduto(produtos);
                break;
            case '4':
                Cadastrar.CadastrarMateriaPrima(materiasPrimas);
                break;
            case '5':
                Cadastrar.CadastrarInadimplente(inadimplentes, clientes);
                break;
            case '6':
                Cadastrar.CadastrarBloqueado(bloqueados, fornecedores);
                break;
            case '0':
                return;
            default:
                Console.WriteLine("Opção Invalida!");
                break;
        }
    }
}
void MenuConsultas()
{
    while (true)
    {
        var escolha = Utils.CriarMenu(new List<string>(){
        "============CONSULTAS============\n",
        "[1] - Consultar Cliente\n",
        "[2] - Consultar Fornecedor\n",
        "[3] - Consultar Produto\n",
        "[4] - Consultar Matéria Prima\n",
        "[5] - Consultar Cliente Inadimplente\n",
        "[6] - Consultar Fornecedor Bloqueado\n",
        "[7] - Consultar Vendas\n",
        "[8] - Consultar Compras\n",
        "[0] - Voltar\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                Consultar.Consulta(clientes);
                break;
            case '2':
                Consultar.Consulta(fornecedores);
                break;
            case '3':
                Consultar.Consulta(produtos);
                break;
            case '4':
                Consultar.Consulta(materiasPrimas);
                break;
            case '5':
                Consultar.Consulta(inadimplentes);
                break;
            case '6':
                Consultar.Consulta(bloqueados);
                break;
            case '7':
                Consultar.Consulta(vendas);
                break;
            case '8':
                Consultar.Consulta(compras);
                break;
            case '0':
                return;
            default:
                Console.WriteLine("Opção Invalida!");
                break;
        }
    }
}
void MenuVendas()
{
    var escolha = Utils.CriarMenu(new List<string>(){
        "============Vendas============\n",
        "[1] - Cadastrar Venda\n",
        "[2] - Consultar Venda\n",
        "[3] - Salvar Venda\n",
        "[0] - Voltar\n",
        "================================"
        });

    switch (escolha)
    {
        case '1':
            VendasMenu.Cadastrar(vendas, clientes, inadimplentes);
            break;
        case '2':
            VendasMenu.Localizar(vendas);
            break;
        case '3':
            VendasMenu.Salvar(vendas);
            break;
        case '0':
            return;
        default:
            Console.WriteLine("Opção Invalida!");
            break;
    }
}

void MenuComprar()
{
    Comprar.RealizarCompra();
}
void MenuProducao()
{
    Produzir.RealizarProducao();
}

void GravarNoArquivo()
{
    try
    {
        Cliente.GravarNoArquivo(clientes);
        Fornecedor.GravarNoArquivo(fornecedores);
        Produto.GravarNoArquivo(produtos);
        MateriaPrima.GravarNoArquivo(materiasPrimas);
        Banidos.GravarNoArquivo(inadimplentes, "risco.dat");
        Banidos.GravarNoArquivo(bloqueados, "banidos.dat");
    } catch(Exception e)
    {
        Console.WriteLine("Erro ao tentar Salvar os arquivos");
    }
}