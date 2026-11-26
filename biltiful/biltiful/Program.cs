using System.Globalization;
using System.Text.RegularExpressions;


List<Cliente> clientes = Cliente.CarregarClientes("Clientes.dat");

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
        "[1] - Cadastrar Cliente\n",
        "[2] - Cadastrar Fornecedor\n",
        "[3] - Cadastrar Produto\n",
        "[4] - Cadastrar Matéria Prima\n",
        "[0] - Voltar\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                PreencherInformacoesDoCliente();
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

void PreencherInformacoesDoCliente()
{
    try
    {
        string cpf;
        do
        {
            Console.WriteLine("Informe CPF do Cliente: ");
            cpf = Console.ReadLine();

            cpf = Regex.Replace(cpf, @"[^a-zA-Z0-9]", "");
        } while (cpf.Length != 11);

        Console.WriteLine("Informe o nome do Cliente: ");
        string nome = Console.ReadLine();

        string[] formatosDeData = { "dd/MM/yyyy", "ddMMyyyy" };
        DateTime dataNasc;
        string entrada;

        Console.WriteLine("Informe a data de nascimento (dd/MM/yyyy)");
        entrada = Console.ReadLine();


        while (!DateTime.TryParseExact(entrada, formatosDeData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNasc))
        {
            Console.WriteLine("Data no formato Invalido! informe novamente: ");
            entrada = Console.ReadLine();
        }

        bool sexoValido = false;
        char sexo = '0';

        while (!sexoValido)
        {
            var opcao = Utils.CriarMenu(new List<string> {
            "[1] - Masculino",
            "[2] - Feminimo",
            });

            switch (opcao)
            {
                case '1':
                    sexo = 'M';
                    sexoValido = true;
                    break;
                case '2':
                    sexo = 'F';
                    sexoValido = true;
                    break;
                default:
                    Console.WriteLine("Sexo Invalido! Informe novamente:");
                    break;
            }
        }

         clientes.Add(Cliente.CadastrarCliente(clientes, cpf, nome, dataNasc, sexo));
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        PreencherInformacoesDoCliente();
    }
}

void CadastrarFornecedor()
{

}