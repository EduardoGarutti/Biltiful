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
        List<Venda> vendas = new List<Venda>();
        
        try
        {
            if (Utils.ValidarCaminho(caminhoArquivo))

                foreach (var venda in File.ReadAllLines(caminhoArquivo))
                {
                    Venda  v = new Venda();

                    v.Id = int.Parse(venda.Substring(0,5));
                    v.Data = DateTime.Parse(venda.Substring(5, 13));
                    v.ClienteId = venda.Substring(13, 24);
                    v.ValorTotal = decimal.Parse(venda.Substring(24, 31));
                   
                    vendas.Add(v);                    
                }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao carregar a venda:{ex.Message}");
        }
        return vendas;

    }
   
    public static Venda CadastrarVenda(List<Cliente> clientes, string cpf, decimal valor)
    {
       foreach(var cliente in clientes)
        {
            if(cliente.CPF == cpf)
            {
               if(Cliente.EhMenorDeIdade(cliente))
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
                using (var sw = new StreamWriter(caminhoArquivo))
                {
                    foreach(var venda in vendas)
                    {
                        sw.WriteLine($"{venda.Id}, {venda.Data}, {venda.ClienteId}, {venda.ValorTotal}");
                    }
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gravar o arquivo: {ex.Message}");
        }
    }

    public string ToString(Venda v)
    {
        return $" Venda Id {v.Id}, Data Da venda{v.Data}, Cliente{v.ClienteId}, Valor Total {v.ValorTotal}";
    }
}
