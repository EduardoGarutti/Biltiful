public class Producao
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Produto.dat"));
    public static int Contador = 1;
    public int Id { get; private set; } //99.999 5 dig
    public DateTime DataProducao { get; private set; } //dd/mm/yyyy
    public string Produto { get; private set; } //13
    public int Quantidade { get; private set; } //999,99 5 dig

    public Producao(DateTime dataProducao, string produto, int quantidade)
    {
        Id = Contador;
        DataProducao = dataProducao;
        Produto = produto;
        Quantidade = quantidade;

    }

    public Producao(int id, DateTime dataProducao, string produto, int quantidade)
    {
        Id = id;
        DataProducao = dataProducao;
        Produto = produto;
        Quantidade = quantidade;
    }

    public Producao(int id, string produto, int quantidade)
    {
        Id = Contador;
        Contador++;
        DataProducao = DateTime.Now;
        Produto = produto;
        Quantidade = quantidade;
    }

    //private bool ValidarCaminho()
    //{
    //    if (true)//vai ser a verificacao do utils se a pasta existe
    //    {
    //        if (!File.Exists(CaminhoArquivo))
    //        {
    //            File.Create(CaminhoArquivo).Close();
    //            return true;
    //        }
    //        else
    //        {
    //            return true;
    //        }
    //    }
    //}

    public static List<Producao> CarregarProducoes(string caminhoArquivo) // pronta
    {
        List<Producao> producoes = new List<Producao>();
        try
        {
            if (Utils.ValidarCaminho(CaminhoArquivo))
                foreach (var producao in File.ReadAllLines(caminhoArquivo))
                {
                    var parteid = producao.Substring(0, 5);
                    var parteDataProducao = producao.Substring(5, 10);
                    var parteProduto = producao.Substring(15, 13);
                    var parteQuantidade = producao.Substring(28, 5);

                    var id = int.Parse(parteid);
                    var dataProducao = DateTime.Parse(parteDataProducao);
                    var produto = parteProduto;
                    var quantidade = int.Parse(parteQuantidade);
                    var novaproducao = new Producao(id, dataProducao, produto, quantidade);
                    producoes.Add(novaproducao);
                }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao carregar as produções: {e.Message}");
        }
        return producoes;
    }
    private void SalvarArquivoProducao(List<Producao> producoes)
    {
        try
        {
            using (var sw = new StreamWriter(CaminhoArquivo))
            {
                foreach (var producao in producoes)
                {
                    sw.WriteLine(producao.StringParaArquivo());
                }
            }
        }
        catch
        {
            throw new Exception($"Ocorreu um erro ao salvar os contatos");
        }
    }

    public Producao? CadastrarProducao(List<Producao> producoes, string produtoId, int quantidade)//pronto
    {
        try
        {
            int id = producoes[-1].Id;
            id++;
            if (id > 99999)
            {
                throw new Exception("Não é possivel gerar mais Produções");
            }
            if (quantidade <= 0 || quantidade > 99999)
            {
                throw new Exception("Quantidade deve ser maior que zero e menor que 99999");

            }

            Producao producao = new Producao(id, produtoId, quantidade);
            // CriarItemProducao(id);
            return producao;
        }
        catch
        {
            throw new Exception("Ocorreu um erro ao criar a Produção.");
        }

    }

    public int Localizar(List<Producao> producoes, int id)//pronta
    {
        for (int i = 0; i < producoes.Count; i++)
        {
            if (producoes[i].Id == id)
            {
                return i;
            }
        }
        return -1;
    }

    public bool Excluir(List<Producao> producoes, int id)
    {
        try
        {
            int indice = Localizar(producoes, id);

            producoes.RemoveAt(indice);
            return true;
        }
        catch
        {
            throw new Exception("Ocorreu um erro ao excluir a produção!");
        }
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

        return id.PadRight(5)
            + dataProducao.PadRight(10)
            + Produto.PadRight(13)
            + quantidade.PadRight(5); ;
    }

}