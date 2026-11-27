public class ItemProducao
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "ItemProducao.dat"));
    public int Id { get; private set; } //99.999 5 dig
    public DateTime DataProducao { get; private set; } //dd/mm/yyyy
    public string MateriaPrima { get; private set; }// 6 dig
    public int QuantidadeMateriaPrima { get; private set; }// 999,99

    public ItemProducao(int id, DateTime dataProducao, string materiaPrima, int quantidadeMateriaPrima)
    {
        Id = id;
        DataProducao = dataProducao;
        MateriaPrima = materiaPrima;
        QuantidadeMateriaPrima = quantidadeMateriaPrima;
    }
    public ItemProducao(string materiaPrima, int quantidadeMateriaPrima)
    {
        MateriaPrima = materiaPrima;
        QuantidadeMateriaPrima = quantidadeMateriaPrima;
    }
    public ItemProducao(Producao producao, string materiaPrima, int quantidadeMateriaPrima)
    {
        Id = producao.Id;
        DataProducao = producao.DataProducao;
        MateriaPrima = materiaPrima;
        QuantidadeMateriaPrima = quantidadeMateriaPrima;
    }

    public static List<ItemProducao> CarregarItemProducoes() 
    {
        List<ItemProducao> itensProducao = new List<ItemProducao>();
        try
        {
            if (Utils.ValidarCaminho(CaminhoArquivo))
                foreach (var itemProducao in File.ReadAllLines(CaminhoArquivo))
                {
                    var parteid = itemProducao.Substring(0, 5);
                    var parteDataProducao = itemProducao.Substring(5, 10);
                    var parteMateriaPrima = itemProducao.Substring(15, 6);
                    var parteQuantidadeMateriaPrima = itemProducao.Substring(21, 5);

                    var id = int.Parse(parteid);
                    var dataProducao = DateTime.Parse(parteDataProducao);
                    var materiaPrima = parteMateriaPrima;
                    var quantidadeMateriaPrima = int.Parse(parteQuantidadeMateriaPrima);



                    var novoItemProducao = new ItemProducao(id, dataProducao, materiaPrima, quantidadeMateriaPrima);
                    itensProducao.Add(novoItemProducao);
                }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Ocorreu um erro ao carregar os Itens Produções: {e.Message}");
        }
        return itensProducao;
    }

    public static void SalvarArquivoItemProducao(List<ItemProducao> itensProducao)
    {
        try
        {
            using (var sw = new StreamWriter(CaminhoArquivo))
            {
                foreach (var itemProducao in itensProducao)
                {
                    sw.WriteLine(itemProducao.StringParaArquivoItemProducao());
                }
            }
        }
        catch (Exception e)
        {
            throw new Exception($"Ocorreu um erro ao salvar os Itens Produção: {e.Message}");
        }
    }

    public static List<ItemProducao> CadastrarItemProducao(Producao producao,string materiaPrimaId, int quantidadeMateriaPrima,int quantidade)
    {
        try
        {
            List<ItemProducao> itensProducao = new List<ItemProducao>();
            if (quantidadeMateriaPrima <= 0 || quantidadeMateriaPrima > 99999)
            {
                throw new Exception("Quantidade da Materia Prima deve ser maior que zero e menor que 99999");

            }
            for(int i = 0;i < quantidade;i++)
            {
                ItemProducao itemProducao = new ItemProducao(producao,materiaPrimaId, quantidadeMateriaPrima);
                itensProducao.Add(itemProducao);
            }

            return itensProducao;
        }
        catch (Exception e)
        {
            throw new Exception($"Ocorreu um erro ao criar o Item Produção: {e.Message}");
        }

    }

    public static int LocalizarItemProducao(List<ItemProducao> itemProducoes, int id)
    {
        for (int i = 0; i < itemProducoes.Count; i++)
        {
            if (itemProducoes[i].Id == id)
            {
                return i;
            }
        }
        return -1;
    }

    public static bool ExcluirItemProducao(List<ItemProducao> itemProducoes, int id)
    {
        try
        {
            int indice = 0;
            while ((indice = LocalizarItemProducao(itemProducoes,id)) != -1)
            {
                itemProducoes.RemoveAt(indice);
            }
            SalvarArquivoItemProducao(itemProducoes);
            return true;
        }
        catch (Exception e)
        {
            throw new Exception($"Ocorreu um erro ao excluir o Item Produção: {e.Message}");
        }
    }

    public void ImpressaoPorRegistro(List<ItemProducao> itemProducoes)
    {
        if (itemProducoes.Count.Equals(0))
        {
            Console.WriteLine("Nenhum Item Produção encontrado!");
            Console.ReadKey();
            return;
        }

        int indice = 0;
        bool mostrando = true;

        while (mostrando)
        {
            var opcoes = new List<string> { "1 - Primeiro ", "2 - Anterior ", "3 - Próximo ", "4 - Último ", "0 - Sair\n" };

            Console.Clear();
            Console.WriteLine($"Item {indice + 1} de {itemProducoes.Count}");
            Console.WriteLine(itemProducoes[indice] + "\n");

            switch (Utils.CriarMenu(opcoes))
            {
                case '1':
                    indice = 0;
                    break;

                case '2':
                    if (indice > 0) indice--;
                    break;

                case '3':
                    if (indice < itemProducoes.Count - 1) indice++;
                    break;

                case '4':
                    indice = itemProducoes.Count - 1;
                    break;

                case '0':
                    mostrando = false;
                    break;

                default:
                    Console.WriteLine("Opção inválida!");
                    break;
            }

        }
    }

    public string StringParaArquivoItemProducao()
    {
        var id = Id.ToString();
        string dataProducao = DataProducao.ToString("dd/MM/yyyy");
        string quantidadeMateriaPrima = QuantidadeMateriaPrima.ToString();

        return id.PadRight(5)
            + dataProducao.PadRight(10)
            + MateriaPrima.PadRight(6)
            + quantidadeMateriaPrima.PadRight(5);
    }

    public override string ToString()
    {
        return $"Id: {Id}\nData Produção: {DataProducao}\nCódigo Matéria Prima: {MateriaPrima}\nQuantidade Matéria Prima: {QuantidadeMateriaPrima}";
    }
}


