using System.Globalization;

public class FornecedorController
{
    public static void CadastrarFornecedor(List<Fornecedor> fornecedores)
    {
        try
        {

            Console.WriteLine("Informe CNPJ do Fornecedor: ");
            string cnpj = Console.ReadLine();

            Console.WriteLine("Informe Razão Social do Fornecedor: ");
            string razaoSocial = Console.ReadLine();

            string[] formatosDeData = { "dd/MM/yyyy", "ddMMyyyy" };
            DateTime dataAbertura;
            string entrada;

            Console.WriteLine("Informe a data de nascimento (dd/MM/yyyy)");
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

    public static void ExcluirFornecedor(List<Fornecedor> fornecedores)
    {
        try
        {
            Console.WriteLine("Informe cnpj do fornecedor: ");
            string cnpj = Console.ReadLine();

            int id = LocalizarFornecedor(fornecedores, cnpj);

            fornecedores[id].AlterarSituacao();
        }
        catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Exclusão...");
                ExcluirFornecedor(fornecedores);
            }
        }
    }

    public static void EditarFornecedor(List<Fornecedor> fornecedores)
    {
        try
        {
            Console.WriteLine("Informe cnpj do fornecedor: ");
            string cnpj = Console.ReadLine();

            int id = LocalizarFornecedor(fornecedores, cnpj);

            Console.WriteLine("Informe Razão Social do Fornecedor: ");
            string novaRazaoSocial = Console.ReadLine();

            string[] formatosDeData = { "dd/MM/yyyy", "ddMMyyyy" };
            DateTime novaDataAbertura;
            string entrada;

            Console.WriteLine("Informe a data de nascimento (dd/MM/yyyy)");
            entrada = Console.ReadLine();


            while (!DateTime.TryParseExact(entrada, formatosDeData, CultureInfo.InvariantCulture, DateTimeStyles.None, out novaDataAbertura))
            {
                Console.WriteLine("Data no formato Invalido! informe novamente: ");
                entrada = Console.ReadLine();
            }


            fornecedores[id] = Fornecedor.EditarFornecedor(fornecedores, cnpj, novaRazaoSocial, novaDataAbertura);
        } catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Edição...");
                EditarFornecedor(fornecedores);
            }
        }
    }

    public static void ImprimirFornecedores(List<Fornecedor> fornecedores)
    {
        Consultar.Consulta(fornecedores);
    }

    private static int LocalizarFornecedor(List<Fornecedor> fornecedores, string cnpj)
    {
        int id = Fornecedor.LocalizarFornecedor(fornecedores, cnpj);

        if (id == -1)
            throw new Exception("Fornecedor não encontrado no nosso cadastro!");
        return id;
    }

    public static void CadastrarBloqueado(List<string> bloqueados, List<Fornecedor> fornecedores)
    {
        {
            try
            {
                Console.WriteLine("Informe CPF do Cliente: ");
                string cpf = Console.ReadLine();

                int id = LocalizarFornecedor(fornecedores, cpf);

                Banidos.Cadastrar(bloqueados, cpf, Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Bloqueados.dat")));
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
}