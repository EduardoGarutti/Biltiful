using biltiful;
using System.Globalization;
using System.Text.RegularExpressions;


List<Cliente> clientes = Cliente.CarregarClientes();
List<Fornecedor> fornecedores = Fornecedor.CarregarFornecedores();
List<Produto> produtos = Produto.CarregarProdutos();
List<MateriaPrima> materiasPrimas = MateriaPrima.CarregarMateriaPrima();
List<string> inadimplentes = Banidos.CarregarArquivo("risco.dat");
List<string> bloqueados = Banidos.CarregarArquivo("banidos.dat");
//--------------------------------------------------------------------------------
List<Venda> vendas = CarregarVendas("Vendas.dat");
List<Producao> producoes = Producao.CarregarProducoes();
List<ItemProducao> itemproducoes = ItemProducao.CarregarItemProducoes();

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
                //MenuCompras();
                break;
            case '5':
                // MenuProducao();
                break;
            case '0':
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
                // CadastrarCliente();
                break;
            case '2':
                // CadastrarFornecedor();
                break;
            case '3':
                // CadastrarProduto();
                break;
            case '4':
                //CadastrarMateriaPrima();
                break;
            case '5':
                //CadastrarInadimplente();
                break;
            case '6':
                //CadastrarBloqueado();
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
                //Consultar(clientes);
                break;
            case '2':
                // Consultar(fornecedores);
                break;
            case '3':
                //Consultar(produtos);
                break;
            case '4':
                //Consultar(materiasPrimas);
                break;
            case '5':
                //Consultar(inadimplentes);
                break;
            case '6':
                //Consultar(bloqueados);
                break;
            case '7':
                //Consultar(vendas);
                break;
            case '8':
                //Consultar(compras);
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
            VendasMenu.Salvar(vendas, "Vendas.dat");
            break;
        case '0':
            return;
        default:
            Console.WriteLine("Opção Invalida!");
            break;
    }
}
void MenuCompras()
{
    Comprar.RealizarCompra(fornecedores, compras, bloqueados, materiasPrimas);
}
void MenuProducao()
{
    Produzir.RealizarProducao(producoes);
}
