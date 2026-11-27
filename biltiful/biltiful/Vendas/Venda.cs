public class Venda
{
    public int Id { get; set; } = 0;
    public DateTime Data { get; set; }
    public string ClienteId { get; set; }
    public decimal ValorTotal { get; set; }  
    
    public Venda()
    {

    }

    public Venda(string clienteId, decimal valorTotal)
    {
        Id++;
        Data = DateTime.Now;
        ClienteId = clienteId;
        ValorTotal = valorTotal;
    }
    public static List<Venda> CarregarVendas(string caminhoArquivo)
    {
        var vendas = new List<Venda>();
        try
        {
            if (Utils.ValidarCaminho(caminhoArquivo))

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

    public bool ValidarDatanas()
    {
        return true;
    }

    public static Venda CadastrarVenda(List<Cliente> clientes, string cpf, decimal valor)
    {
       foreach(var cliente in clientes)
        {
            if(cliente.CPF == cpf)
            {
               if(!ValidarDatanas())
                {
                    throw new ArgumentException("Cliente menor de idade");
                }
                return new Venda(cpf, valor);
            }
        }
        throw new ArgumentException("Cliente nao encontrado");
    }

    public static int LocalizarVenda(List<Venda> vendas, int id)
    {
       foreach(var venda in vendas)
        {
            if(venda.Id == id)
            {
                return vendas.IndexOf(venda);
            }
        }
        return -1;
    }

    public void GravarNoArquivo(List<Venda> vendas, string caminhoArquivo)
    {
        try
        {
            if (Utils.ValidarCaminho(caminhoArquivo))
            {
                using (StreamWriter sw = new StreamWriter(caminhoArquivo))
                {
                    foreach(var venda in vendas)
                    {
                        sw.WriteLine($"{venda.Id}, {venda.Data}, {venda.ClienteId}, {venda.ValorTotal}")
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gravar o arquivo: {ex.Message}");
        }
    }
}
