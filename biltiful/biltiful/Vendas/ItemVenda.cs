public class ItemVenda
{
    public int VendaId;
    public string ProdutoId;
    public int Quantidade;
    public decimal ValorUnitario;
    public decimal TotalItem;

    public ItemVenda()
    {

    }

    public ItemVenda(int vendaId, string produtoId, int quantidade, decimal valorunitario)
    {
        VendaId = vendaId;
        ProdutoId = produtoId;
        Quantidade = quantidade;
        ValorUnitario = valorunitario;
        TotalItem = quantidade * valorunitario;

    } 

    public static List<ItemVenda> CarregarItensVenda(string caminhoArquivo)
    {
        var itensVenda = new List<ItemVenda>();
        try
        {
            if (Utils.ValidarCaminho(caminhoArquivo))
            {
                foreach(var item in File.ReadAllLines(caminhoArquivo))
                {
                    var itemData = item.Split(',');
                    if (itemData.Length.Equals(4))
                    {
                        var vendaId = int.Parse(itemData[0]);
                        var produtoId = itemData[1];
                        var quantidade = int.Parse(itemData[2]);
                        var valorunitario = itemData[3];

                        var newItemVenda = new ItemVenda(vendaId, produtoId, quantidade, valorunitario);
                        itensVenda.Add(newItemVenda);

                    }
                }
            }
        }
        catch(Exception ex)
        {
            throw new ArgumentException($"Erro ao carregar o item venda: {ex.Message}");
        }
        return itensVenda;
           

    }

    public static ItemVenda CadastrarItemVenda(List<ItemVenda> itensVenda, List<Venda> vendas, List<Produto> produtos, int vendaId, string produtoId, int quantidade)
    {
        foreach(var venda in vendas)
        {
            if (venda.Id == vendaId)
            {
                foreach(var produto in produtos)
                {
                    if(produto.Id == produtoId)
                    {
                        var newItemVenda = new ItemVenda(vendaId, produtoId, quantidade, valorunitario);
                        itensVenda.Add(newItemVenda);
                        return newItemVenda;
                    }
                }
                throw new ArgumentException("Produto não encontrado");
            }
        }
        throw new ArgumentException("Venda nao encontrada");

    }

    public static List<ItemVenda> LocalizarItemVenda(List<ItemVenda> itemVendas, int vendaID, string produtoId)
    {
        var itensEncontrados = new List<ItemVenda>();
        foreach(var item in itemVendas)
        {
            if(item.VendaId == vendaID && item.ProdutoId == produtoId)
            {
                itensEncontrados.Add(item);
            }
        }
        return itensEncontrados;

    }

    public static List<ItemVenda> ListarItensDaVenda(List<ItemVenda> itemVendas, int vendaId)
    {
        var itensdavenda = new List<ItemVenda>();
        foreach(var item in itensdavenda)
        {
            if(item.VendaId == vendaId)
            {
                itensdavenda.Add(item);
            }
        }
        return itensdavenda;

    }

    public static void GravarNoArquivo(List<ItemVenda> itemVendas, string caminhoArquivo)
    {
        try
        {
            if (Utils.ValidarCaminho(caminhoArquivo))
            {
                using (StreamWriter sw = new StreamWriter(caminhoArquivo))
                {
                    foreach(var item in itemVendas)
                    {
                        sw.WriteLine($"{item.VendaId}, {item.ProdutoId}, {item.Quantidade}, {item.ValorUnitario}");
                    }
                }
            }

        }
        catch(Exception ex)
        {
            throw new ArgumentException($"Erro ao gravar itens: {ex.Message}");
        }
    } 
    

}
