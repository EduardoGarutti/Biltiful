public class Venda
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string ClienteId { get; set; }
    public double ValorTotal { get; set; }   

    public Venda(int id, string data, string clienteId, string valorTotal)
    {
        Id = id;
        Data = DateTime.Parse(data);
        ClienteId = clienteId;
        ValorTotal = double.Parse(valorTotal);
    }
    public static List<Venda> CarregarVendas(string caminhoArquivo)
    {
        var vendas = new List<Venda>();
        try
        {
            if (Utils.ValidarcaminhoArquivo(caminhoArquivo))

                foreach (var venda in File.ReadAllLines(caminhoArquivo))
                {
                    var vendaData = venda.Split(',');
                    if (vendaData.Length.Equals(4))
                    {
                        var Id = int.Parse(vendaData[0]);
                        var data = vendaData[1];
                        var cliente = vendaData[2];
                        var valortotal = vendaData[3];

                        var newVenda = new Venda(Id, data, cliente, valortotal);
                        vendas.Add(newVenda);
                    }
                }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar a venda:{ex.Message}");
        }
        return vendas;

    }

    public static Venda CadastrarVenda(List<Cliente> clientes, List<Inadimplentes> inadimplentes, string cpf)
    {
        var cliente = clientes.FirstOrDefault(c => c.CPF == cpf);
        if (cliente == null)
        {
            Console.WriteLine("Cliente não encontrado.");            
        }
        if(inadimplentes.Any(i => i.CPF == cpf))
        {
            Console.WriteLine("Cliente inadimplente:");
        } 
        
       
    }

    public static int LocalizarVenda(List<Venda> vendas, int id)
    {
       for (int i = 0; i < vendas.Count; i++)
        {
            if (vendas[i].Id == id)
                return i;
        }
        return -1;
    }

    public void GravarNoArquivo(List<Venda> vendas, string caminhoArquivo)
    {

    }
}
