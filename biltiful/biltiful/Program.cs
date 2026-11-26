using System.Globalization;
using System.Linq.Expressions;
using System.Text.RegularExpressions;


List<Cliente> clientes = Cliente.CarregarClientes("Clientes.dat");
List<Fornecedor> fornecedores = Fornecedor.CarregarFornecedores("Fornecedor.dat");
List<Produto> produtos = Produto.CarregarProdutos("Produto.dat");
List<MateriaPrima> materiasPrimas = MateriaPrima.CarregarMateriasPrimas("Materia");
List<Inadimplentes> inadimplentes = Inadimplentes.CarregarArquivo();
List<Bloqueados> bloqueados = Bloqueados.CarregarArquivo();

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
        "[5] - Cadastrar Cliente Inadimplente\n",
        "[6] - Cadastrar Fornecedor Bloqueado\n",
        "[0] - Voltar\n",
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
            case '5':
                CadastrarInadimplente();
                break;
            case '6':
                CadastrarBloqueado();
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
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarCliente();
        }
    }
}

void CadastrarFornecedor()
{
    try
    {
        string cnpj;
        do
        {
            Console.WriteLine("Informe CNPJ do Fornecedor: ");
            cnpj = Console.ReadLine();

            cnpj = Regex.Replace(cnpj, @"[^a-zA-Z0-9]", "");
        } while (cnpj.Length != 14);

        Console.WriteLine("Informe Razão Social do Fornecedor: ");
        string razaoSocial = Console.ReadLine();

        string[] formatosDeData = { "dd/MM/yyyy", "ddMMyyyy" };
        DateTime dataAbertura;
        string entrada;

        Console.WriteLine("Informe a data de Abertura da empresa: (dd/MM/yyyy)");
        entrada = Console.ReadLine();


        while (!DateTime.TryParseExact(entrada, formatosDeData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataAbertura))
        {
            Console.WriteLine("Data no formato Invalido! informe novamente: ");
            entrada = Console.ReadLine();
        }



        fornecedores.Add(Fornecedor.CadastrarFornecedor(fornecedores, cnpj, razaoSocial, dataAbertura));
    }
    catch (Exception e)
    {
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarFornecedor();
        }
    }
}

void CadastrarProduto()
{
    try
    {
        string codigoDeBarras;
        string comecoValido;
        do
        {
            Console.WriteLine("Informe Codigo de Barras do produto(Começando obrigatóriamente com 789): ");
            codigoDeBarras = Console.ReadLine();

            codigoDeBarras = Regex.Replace(codigoDeBarras, @"[^a-zA-Z0-9]", "");
            comecoValido = codigoDeBarras.Substring(0, 3);
        } while (codigoDeBarras.Length != 13 || comecoValido.Equals("789"));

        Console.WriteLine("Informe nome do Produto: ");
        string nomeProduto = Console.ReadLine();

        Console.WriteLine("Informe Valor da venda:");
        decimal valorVenda = decimal.Parse(Console.ReadLine());

        while (valorVenda is > (decimal)999.99 or <= 0)
        {
            Console.WriteLine("Valor de venda Invalido! Informe novamente: ");
            valorVenda = decimal.Parse(Console.ReadLine());
        }

        produtos.Add(Produto.CadastrarProduto(produtos, codigoDeBarras, nomeProduto, valorVenda));
    }
    catch (Exception e)
    {
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarProduto();
        }
    }

}

void CadastrarMateriaPrima()
{
    try
    {
        Console.WriteLine("Informe o nome da matéria prima: ");
        string nomeMateriaPrima = Console.ReadLine();

        materiasPrimas.Add(MateriaPrima.CadastrarMateriaPrima(nomeMateriaPrima));
    }
    catch (Exception e)
    {
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarMateriaPrima();
        }
    }
}

void CadastrarInadimplente()
{
    try
    {
        string cpf;
        do
        {
            Console.WriteLine("Informe CPF do Inadimplente: ");
            cpf = Console.ReadLine();

            cpf = Regex.Replace(cpf, @"[^a-zA-Z0-9]", "");
        } while (cpf.Length != 11);

        bool existeCliente = false;
        foreach (var cliente in clientes)
        {
            if (cliente.CPF == cpf)
            {
                existeCliente = true;
            }
        }
        if (existeCliente)
            Inadimplentes.Cadastrar(inadimplentes, cpf);
        else
            throw new Exception("Esse cpf não existe no nosso cadastro!");


    }
    catch (Exception e)
    {
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarInadimplente();
        }
    }
}

void CadastrarBloqueado()
{
    try
    {
        string cnpj;
        do
        {
            Console.WriteLine("Informe CPF do Inadimplente: ");
            cnpj = Console.ReadLine();

            cnpj = Regex.Replace(cnpj, @"[^a-zA-Z0-9]", "");
        } while (cnpj.Length != 14);

        bool existeFornecedor = false;
        foreach (var fornecedor in fornecedores)
        {
            if (fornecedor.cnpj == cnpj)
            {
                existeFornecedor = true;
            }
        }
        if (existeFornecedor)
            bloqueados.Cadastrar(bloqueados, cnpj);
        else
            throw new Exception("Esse cnpj não existe no nosso cadastro!");


    }
    catch (Exception e)
    {
        Console.WriteLine("Erro: " + e.Message);
        if (TentarNovamente())
        {
            Console.WriteLine("Reiniciando Processo de Cadastro...");
            CadastrarInadimplente();
        }
    }
}
bool TentarNovamente()
{
    while (true)
    {
        var op = Utils.CriarMenu(new List<string>(){
            "Deseja tentar o cadastro novamente?\n",
            "[1] - Sim\n",
            "[2] - Não"
    });

        switch (op)
        {
            case '1':
                return true;
            case '2':
                return false;
            default:
                Console.WriteLine("Opção invalida");
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
        "[0] - Voltar\n",
        "================================"
        });

        switch (escolha)
        {
            case '1':
                Consultar(clientes);
                break;
            case '2':
                Consultar(fornecedores);
                break;
            case '3':
                Consultar(produtos);
                break;
            case '4':
                Consultar(materiasPrimas);
                break;
            case '5':
                Consultar(inadimplentes);
                break;
            case '6':
                Consultar(bloqueados);
                break;
            case '0':
                return;
            default:
                Console.WriteLine("Opção Invalida!");
                break;
        }
    }
}
void Consultar<T>(List<T> lista)
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
void MenuVendas()
{
    try
    {
        Console.WriteLine("Informe o CPF do cliente que deseja realizar uma venda: ");
        string cpf = Console.ReadLine();

        Cliente aux;
        ExisteCPF(cpf, aux);

        isMenorIdade(aux);

    }
    


}

void ExisteCPF(string cpf, Cliente aux)
{
    foreach (var cliente in clientes)
    {
        if (cliente.CPF.Equals(cpf))
        {
            aux = cliente;
            return;
        }
    }
    throw new Exception("Cpf não encontrado na lista!");
}
void isMenorIdade(Cliente aux)
{
    DateTime hoje = DateTime.Now;

    int idade = hoje.Year - aux.dataNasc.Year;

    if (aux.dataNasc.Date > hoje.AddYears(-idade).Date) //verifica se o cliente fez aniversário nesse ano ou não
    {
        idade--;
    }

    if (idade < 18)
    {
        throw new Exception("Cliente menor de idade!");
    }
}