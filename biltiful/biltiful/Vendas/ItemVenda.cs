public class ItemVenda
{
    public int Id { get; private set; }
    public string Produto { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }
    public decimal TotalItem { get; private set; }



    public ItemVenda(int id, string produto, int quantidade, decimal valorUnitario)
    {
        if (id < 1 || id > 99999)
        {
            throw new ArgumentException("O ID do item de venda deve estar entre 1 e 99.999.");

        }
        if (string.IsNullOrWhiteSpace(produto) || produto.Length != 13 || !produto.StartsWith("789"))
        {
            throw new ArgumentException("O nome do produto não pode ser vazio e deve ter exatamente 13 caracteres numéricos.");
        }
        if (quantidade < 1 || quantidade > 999)
        {
            throw new ArgumentException("A quantidade do item de venda deve estar entre 1 e 999.");
        }
        if (valorUnitario <= 0 || valorUnitario > 999.99m)
        {
            throw new ArgumentException("O valor maximo é 999,99.");
        }
        decimal totalItem = quantidade * valorUnitario;
        if (totalItem > 9999.99m)
        {
            throw new ArgumentException("O valor maximo é 9.999,99.");
        }
        Id = id;
        Produto = produto;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
        TotalItem = totalItem;
    }

}