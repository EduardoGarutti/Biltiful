public class Venda
{
    public int Id { get; private set; }
    public DateOnly Data { get; private set; }
    public string Cliente { get; private set; }
    public decimal ValorTotal { get; private set; }

    public Venda(int id, string cliente)
    {
        if (id < 1 || id > 99999)
        {
            throw new ArgumentException("O ID maximo da venda é 99.999.");
        }
        if (string.IsNullOrWhiteSpace(cliente) || cliente.Length != 11 || !cliente.All(char.IsDigit))
        {
            throw new ArgumentException("O Cpf deve ter exatamente 11 caracteres numéricos.");
        }


        Id = id;
        Data = DateOnly.FromDateTime(DateTime.Now);
        Cliente = cliente;
        ValorTotal = 0.0m;
    }

    public void CalcularValorTotal(List<ItemVenda> itensVenda)
    {
        if (itensVenda == null || itensVenda.Count < 1 || itensVenda.Count > 3)
        {
            throw new ArgumentException("A venda deve ter de 1 a 3 itens.");
        }
        decimal total = 0;
        foreach (var item in itensVenda)
        {
            total += item.TotalItem;
        }
        if (total <= 0 || total > 99999.99m)
        {
            throw new ArgumentException("O valor total da venda deve estar entre 0,01 e 99.999,99.");
        }
        ValorTotal = total;
    }

    public Venda(int id, DateOnly data, string cliente, decimal valorTotal)
    {
        if (id < 1 || id > 99999)
        {
            throw new ArgumentException("O ID maximo da venda é 99.999.");
        }
        if (string.IsNullOrWhiteSpace(cliente) || cliente.Length != 11 || !cliente.All(char.IsDigit))
        {
            throw new ArgumentException("O Cpf deve ter exatamente 11 caracteres numéricos.");
        }
        if (valorTotal <= 0 || valorTotal > 99999.99m)
        {
            throw new ArgumentException("O valor maximo da venda é 99.999,99.");
        }
        Id = id;
        Data = data;
        Cliente = cliente;
        ValorTotal = valorTotal;
    }

}