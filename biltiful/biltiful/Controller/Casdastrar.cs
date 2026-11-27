using System.Globalization;
using System.Text.RegularExpressions;

public class Cadastrar()
{

    void CadastrarCliente<T>(List<T> clientes)
    {
        try
        {
            string cpf;
            do
            {
                Console.WriteLine("Informe CPF do Cliente: ");
                cpf = Console.ReadLine();
            } while (cpf.Length != 14);

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

    void CadastrarFornecedor<T>(List<T> fornecedores)
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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarFornecedor(fornecedores);
            }
        }
    }

    void CadastrarProduto<T>(List<T> produtos)
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

    void CadastrarMateriaPrima<T>(List<T> materiasPrimas)
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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarMateriaPrima(materiasPrimas);
            }
        }
    }

    void CadastrarInadimplente<T>(List<T> inadimplentes, List<T> clientes)
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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarInadimplente(inadimplentes, clientes);
            }
        }
    }

    void CadastrarBloqueado<T>(List<T> bloqueados, List<T> fornecedores)
    {
        try
        {
            string cnpj;
            do
            {
                Console.WriteLine("Informe CNPJ do Inadimplente (00.000.000/0001-00): ");
                cnpj = Console.ReadLine();

                int aux = int.Parse(Regex.Replace(cnpj, @"[^a-zA-Z0-9]", "");
            } while (cnpj.Length != 18);

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
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarInadimplente(bloqueados, fornecedores);
            }
        }
    }
    void ExisteCPF<T>(string cpf, Cliente aux, List<T> clientes)
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
}


