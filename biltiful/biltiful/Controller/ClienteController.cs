using System.Globalization;

public static class ClienteController
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

    public static void ExcluirCliente(List<Cliente> clientes)
    {
        try
        {
            Console.WriteLine("Informe CPF do cliente que deseja excluir: ");
            string cpf = Console.ReadLine();

            Cliente.ValidarCPF(cpf);

            int id = LocalizarCliente(clientes, cpf);

            clientes[id].AlterarSituacao();
            Console.WriteLine("Cliente Excluido com sucesso!");
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Exclusão...");
                CadastrarCliente(clientes);
            }
        }
    }

    public static void EditarCliente(List<Cliente> clientes)
    {
        try
        {
            Console.WriteLine("Informe CPF do cliente: ");
            string cpf = Console.ReadLine();

            int id = LocalizarCliente(clientes, cpf);
            Console.WriteLine("Cliente localizado com sucesso!");

            Console.WriteLine("Informe o Novo nome do Cliente: ");
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
            clientes[id] = Cliente.EditarCliente(clientes, cpf, nome, dataNasc, sexo);
        } 
        catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Edição...");
                EditarCliente(clientes);
            }
        }   
    }

    public static void ImprimirClientes(List<Cliente> clientes)
    {
        Consultar.Consulta(clientes);
    }

    private static int LocalizarCliente(List<Cliente> c, string cpf)
    {
        int id = Cliente.LocalizarCliente(c, cpf);

        if (id == -1)
            throw new Exception("Cliente não encontrado no nosso Cadastro!");
        return id;
    }

    public static void CadastrarInadimplente(List<string> inadimplentes, List<Cliente> clientes)
    {
        try
        {
            Console.WriteLine("Informe CPF do Cliente: ");
            string cpf = Console.ReadLine();

            int id = LocalizarCliente(clientes, cpf);

            Banidos.Cadastrar(inadimplentes, cpf, "risco.dat");
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
}
