public class ItemVenda
{
    public int VendaId;
    public string ProdutoId;
    public int Quantidade;
    public double ValorUnitario;
    public double TotalItem;

    public ItemVenda(string vendaId, string produtoId, string quantidade, string valorUnitario, string totalItem)
    {
        VendaId = int.Parse(vendaId);
        ProdutoId = produtoId;
        Quantidade = int.Parse(quantidade);
        ValorUnitario = double.Parse(valorUnitario);
        TotalItem = double.Parse(totalItem);

    } 

    public static List<ItemVenda> CarregarItensVenda(string caminhoArquivo)
    {
        var itens = new List<ItemVenda>();
        if (!File.Exists(caminhoArquivo))
            return itens;

    }

    public static ItemVenda CadastrarItemVenda(List<ItemVenda> itensVenda, List<Venda> vendas, List<Produtos> produtos, int vendaId, string produtoId, int quantidade)
    {

    }

    public static int LocalizarItemVenda(List<ItemVenda> itemVendas, int vendaID, string produtoId)
    {

    }

    public static List<ItemVenda> ListarItensDaVenda(List<ItemVenda> itemVendas, int vendaId)
    {

    }

    public static void GravarNoArquivo(List<ItemVenda> itemVendas, string caminhoArquivo)
    {

    } 
    

}
