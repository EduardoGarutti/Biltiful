using static System.Runtime.InteropServices.JavaScript.JSType;

public class Producao
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Produto.dat"));
    public static int Contador = 1;
    public int Id { get; private set; } //99.999 5 dig
    public DateOnly DataProducao { get; private set; } //dd/mm/yyyy
    public string Produto { get; private set; } //13
    public int Quantidade { get; private set; } //999,99 5 dig
    private static List<Producao> Producoes = new List<Producao>();

    public Producao(DateOnly dataProducao, string produto, int quantidade)
    {
        Id = Contador;
        DataProducao = dataProducao;
        Produto = produto;
        Quantidade = quantidade;
        
    }

    public Producao(int id,DateOnly dataProducao, string produto, int quantidade)
    {
        Id = id;
        DataProducao = dataProducao;
        Produto = produto;
        Quantidade = quantidade;
    }

    public Producao(string produto, int quantidade)
    {
        Id = Contador;
        Contador++;
        DataProducao = DateOnly.FromDateTime(DateTime.Now);
        Produto = produto;
        Quantidade = quantidade;
    }

    private bool ValidarCaminho()
    {
        if (true)//vai ser a verificacao do utils se a pasta existe
        {
            if (!File.Exists(CaminhoArquivo))
            {
                File.Create(CaminhoArquivo).Close();
                return true;
            }
            else
            {
                return true;
            }
        }
    }

    private  List<Producao> CarregarProducoes()
    {
        Producoes.Clear();
        try
        {
            if (ValidarCaminho())
                foreach (var producao in File.ReadAllLines(CaminhoArquivo))
                {
                    var parteid = producao.Substring(0, 5);
                    var parteDataProducao = producao.Substring(5, 10);
                    var parteProduto = producao.Substring(15, 13);
                    var parteQuantidade = producao.Substring(28, 5);

                    var id = int.Parse(parteid);
                    var dataProducao = DateOnly.Parse(parteDataProducao);
                    var produto = parteProduto;
                    var quantidade = int.Parse(parteQuantidade);
                    var novaproducao = new Producao(id, dataProducao, produto, quantidade);
                    Producoes.Add(novaproducao);
                }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao carregar as produções: {e.Message}");
        }
        return Producoes;
    }
    private void SalvarArquivo()
    {
        try
        {
            using (var sw = new StreamWriter(CaminhoArquivo))
            {
                foreach (var producao in Producoes)
                {
                    sw.WriteLine(producao.StringParaArquivo());
                }
            }
        }
        catch 
        {
            throw new Exception ($"Ocorreu um erro ao salvar os contatos");
        }
    }

    public  Producao? CadastrarProducao()
    {
       
        CarregarProducoes();
        Producao producao = null;
        try
        {
            string produto = string.Empty;
            int quantidade = 0;

            while (string.IsNullOrEmpty(produto) || produto.Length > 13 )
            {
                Console.Write("Informe o código de barras do produto: ");
                produto = Console.ReadLine();
            }
            bool converteu = false;
            while (!converteu || quantidade <= 0 || quantidade > 99999)
            {
                Console.Write("Informe a quantidade de produto a ser produzido(Max:99,999): ");
                 converteu = int.TryParse((Console.ReadLine()), out quantidade);
            }

            producao = new Producao(produto, quantidade);
            Producoes.Add(producao);
        }
        catch
        {
            throw new Exception("Ocorreu um erro ao criar a Produção.");
        }
        SalvarArquivo();
        return producao;
    }

    public Producao? Localizar(int id)
    {
        CarregarProducoes();
        foreach (Producao producao in Producoes)
        {
            if (producao.Id == id)
            {
                return producao;
            }
        }
        return null;
    }

    public bool Excluir(int id)
    {
        foreach (Producao producao in Producoes)
        {
            if (producao.Id == id)
            {
                try
                {
                    Producoes.Remove(producao);
                    SalvarArquivo();
                    return true;
                }
                catch
                {
                    throw new Exception("Ocorreu um erro ao excluir a produção!");
                }
            }
        }
        throw new Exception("Erro ao procurar produção a ser excluida!");
    }

    //void ImpressaoPorRegistro(List<Producao> producoes)
    //{
    //    if (producoes.Count.Equals(0))
    //    {
    //        Console.WriteLine("Nenhum contato encontrado!");
    //        Console.ReadKey();
    //        return;
    //    }

    //    int indice = 0;
    //    bool mostrando = true;

    //    while (mostrando)
    //    {
    //        var opcoes = new List<string> { "1 - Primeiro ", "2 - Anterior ", "3 - Próximo ", "4 - Último ", "0 - Sair\n" };

    //        Console.Clear();
    //        Console.WriteLine($"Item {indice + 1} de {producoes.Count}");
    //        Console.WriteLine(producoes[indice] + "\n");

    //        switch (Utils.CriarMenu(opcoes))
    //        {
    //            case '1':
    //                indice = 0;
    //                break;

    //            case '2':
    //                if (indice > 0) indice--;
    //                break;

    //            case '3':
    //                if (indice < producoes.Count - 1) indice++;
    //                break;

    //            case '4':
    //                indice = producoes.Count - 1;
    //                break;

    //            case '0':
    //                mostrando = false;
    //                break;

    //            default:
    //                Console.WriteLine("Opção inválida!");
    //                break;
    //        }

    //    }
    //}

    public string StringParaArquivo()
    {
        var id = Id.ToString();
        string dataProducao = DataProducao.ToString();
        string quantidade = Quantidade.ToString();

        return  id.PadRight(5) 
            + dataProducao.PadRight(10) 
            + Produto.PadRight(13) 
            + quantidade.PadRight(5); ;
    }


}