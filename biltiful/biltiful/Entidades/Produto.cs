using System.ComponentModel.DataAnnotations;
using System.Globalization;
public class Produto
{

    public string CodigoBarras { get; private set; }
    public string Nome { get; private set; }
    public double ValorVenda { get; private set; }
    public DateOnly UltimaVenda { get; private set; }
    public DateOnly DataCadastro { get; private set; }
    public char Situacao { get; private set; }
    //public int QtdDisponivel { get; set; } 

    private static readonly CultureInfo ptBR = new CultureInfo("pt-BR"); //pedi pro amigão para poder formatar com 000,00

    public static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dats", "produto.dat"); //sobe 3 niveis de pasta a partir do do bin do projeto

    public Produto()
    {

    }

    public Produto(string nome, double valor)
    {
        CodigoBarras = "789" + Random.Shared.NextInt64(1000000000, 10000000000); // gera um numero aleatorio de 10 digitos e conctena com os outros 3 do comeco
        SetNome(nome);
        SetValor(valor);
        DataCadastro = DateOnly.FromDateTime(DateTime.Now);
        UltimaVenda = DataCadastro;
        Situacao = 'A';
        
    }

    public static List<Produto> CarregarProdutos()
    {

        List<Produto> lista = new List<Produto>();
        if (!File.Exists(filePath))
            return lista;

        foreach(string linha in File.ReadAllLines(filePath))
        {

            Produto p = new Produto();
            p.Nome = linha.Substring(0, 20);
            p.ValorVenda = double.Parse(linha.Substring(20, 6));
            p.CodigoBarras = linha.Substring(26, 13);
            p.DataCadastro = DateOnly.Parse(linha.Substring(39, 10));
            p.UltimaVenda = DateOnly.Parse(linha.Substring(49, 10));
            p.Situacao = char.Parse(linha.Substring(59, 1));

            lista.Add(p);

        }

        return lista;

    }

    public static Produto CadastrarProduto()
    {

        Produto produto = null;
        bool validar;
        double valor;
        string nome;


        Console.Write("Informe o nome do produto: ");
        nome = Console.ReadLine();
        

        Console.WriteLine("informe o valor do produto: ");
        double.TryParse(Console.ReadLine(), out valor);

        produto = new Produto(nome, valor);

        File.AppendAllText(filePath, produto.ToStringFile() + Environment.NewLine);

        return produto = new Produto(nome, valor);



    }

    void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("nome nao pode ser nulo");

        if (nome.Length > 20)
        {
            nome = nome.Substring(0, 20); // subtrai os caracteres restantes no nome se ele passar de 20

            this.Nome = nome;
        }
        else
        {
            nome = nome.PadRight(20);
            this.Nome = nome;
        }

    }

    void SetValor(double valor)
    {

        if (valor <= 0 || valor > 999.99)
            throw new ArgumentException("valor deve estar entre 0,001 e 999,99");    
            this.ValorVenda = valor;
            
    }

    public string ToStringFile()
    {
        return $"{Nome}{ValorVenda.ToString("000.00", ptBR)}{CodigoBarras}{UltimaVenda}{DataCadastro}{Situacao}";
    }


}