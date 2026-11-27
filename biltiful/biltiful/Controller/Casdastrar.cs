using System.Globalization;
using System.Text.RegularExpressions;

public class Cadastrar()
{

    public static void CadastrarCliente(List<Cliente> clientes)
    {
        try
        {
            string cpf;
            Console.WriteLine("Informe CPF do Cliente: ");
            cpf = Console.ReadLine();

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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarCliente(clientes);
            }
        }
    }
    public static void CadastrarFornecedor(List<Fornecedor> fornecedores)
    {
        try
        {

            Console.WriteLine("Informe CNPJ do Fornecedor: ");
            string cnpj = Console.ReadLine();

            Console.WriteLine("Informe Razão Social do Fornecedor: ");
            string razaoSocial = Console.ReadLine();

            Console.WriteLine("Informe a data de Abertura da empresa: (dd/MM/yyyy)");
            string data = Console.ReadLine();

            fornecedores.Add(Fornecedor.CadastrarFornecedor(fornecedores, cnpj, razaoSocial, data));
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarFornecedor(fornecedores);
            }
        }
    }
    public static void CadastrarProduto(List<Produto> produtos)
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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarProduto(produtos);
            }
        }

    }
    public static void CadastrarMateriaPrima(List<MateriaPrima> materiasPrimas)
    {
        try
        {
            Console.WriteLine("Informe o nome da matéria prima: ");
            string nomeMateriaPrima = Console.ReadLine();

            materiasPrimas.Add(MateriaPrima.CadastrarMateriaPrima(materiasPrimas, nomeMateriaPrima));
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarMateriaPrima(materiasPrimas);
            }
        }
    }
    public static void CadastrarInadimplente(List<string> inadimplentes, List<Cliente> clientes)
    {
        try
        {
            Console.WriteLine("Informe CPF do Inadimplente: ");
            string cpf = Console.ReadLine();

            bool existeCliente = false;
            foreach (Cliente cliente in clientes)
            {
                if (cliente.CPF == cpf)
                {
                    existeCliente = true;
                }
            }
            if (existeCliente)
                Banidos.Cadastrar(inadimplentes, cpf, "risco.dat");
            else
                throw new Exception("Esse cpf não existe no nosso cadastro!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarInadimplente(inadimplentes, clientes);
            }
        }
    }
    public static void CadastrarBloqueado(List<string> bloqueados, List<Fornecedor> fornecedores)
    {
        try
        {
            string cnpj;
            do
            {
                Console.WriteLine("Informe CNPJ do Inadimplente: ");
                cnpj = Console.ReadLine();

                int aux = int.Parse(Regex.Replace(cnpj, @"[^a-zA-Z0-9]", "");
            } while (cnpj.Length != 18);

            bool existeFornecedor = false;
            foreach (Fornecedor fornecedor in fornecedores)
            {
                if (fornecedor.CNPJ == cnpj)
                {
                    existeFornecedor = true;
                }
            }
            if (existeFornecedor)
                Banidos.Cadastrar(bloqueados, cnpj, "bloqueados.dat");
            else
                throw new Exception("Esse cnpj não existe no nosso cadastro!");


        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarBloqueado(bloqueados, fornecedores);
            }
        }
    }
}


