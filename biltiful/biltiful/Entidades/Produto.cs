using System.ComponentModel.DataAnnotations;
using System.Globalization;
public class Produto
{

    public string CodigoBarras { get; private set; }
    public string Nome { get; private set; }
    public double ValorVenda { get; private set; }
    public DateOnly UltimaVenda { get; private set; }
    public DateOnly DataCadastro { get; private set; }
    public ESituacao Situacao { get; private set; }
    //public int QtdDisponivel { get; set; } 

    private static readonly CultureInfo ptBR = new CultureInfo("pt-BR"); //pedi pro amigão para poder formatar com 000,00

    public static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dats", "produto.dat"); //sobe 3 niveis de pasta a partir do do bin do projeto

    public Produto()
    {

    }

    public Produto(string nome, double valor, string codBarras)
    {
        CodigoBarras = codBarras;
        SetNome(nome);
        SetValor(valor);
        DataCadastro = DateOnly.FromDateTime(DateTime.Now);
        UltimaVenda = DataCadastro;
        Situacao = ESituacao.Ativo;
        
    }

    public static List<Produto> CarregarProdutos() //retorna todos os produtos, ativos e inativos
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
            p.Situacao = Enum.Parse<ESituacao>(linha.Substring(59, 1));

            lista.Add(p);

        }

        return lista;

    }

    public static List<Produto> ProdutoAtivos() // lista todos os produtos Ativos
    {

        var listaAtivos = new List<Produto>();
        var lista = CarregarProdutos();
        foreach (var item in lista)
        {
            if (item.Situacao == ESituacao.Ativo)
            {
                listaAtivos.Add(item);
            }
        }

        return listaAtivos;


    }

    public static List<Produto> ProdutosInativos() //lista todos os produtos Inativos
    {
        var listaInativos = new List<Produto>();
        var lista = CarregarProdutos();
        foreach (var item in lista)
        {
            if(item.Situacao == ESituacao.Inativo)
            {
                listaInativos.Add(item);
            }
        }

        return listaInativos;
    }

    public static Produto CadastrarProduto(List<Produto> produtos, string codigoBarras, string nome, double valorVenda)
    {

        foreach(var item in produtos)
        {
            if (codigoBarras == item.CodigoBarras)
                throw new ArgumentException("Ja existe um codigo de barras igual a este");
        }


        var produto = new Produto(nome, valorVenda, codigoBarras);

        File.AppendAllText(filePath, produto.ToStringFile() + Environment.NewLine);

        return produto;



    }

    public static int LocalizarProduto(List<Produto> produtos, string codigoBarras)
    {
        foreach(var item in produtos) // percorre a lista e retorna o indice do produto caso encontre um com esse codigo de barras
        {
            if(codigoBarras == item.CodigoBarras)
            {
                return produtos.IndexOf(item);
            }
        }

        return -1; // caso não encontre retorna -1
    }

    public static Produto EditarProduto(string codigoBarras, string novoNome, double novoValorVenda)
    {
        var lista = CarregarProdutos();

        foreach(var item in lista)
        {

            if (codigoBarras == item.CodigoBarras)
            {
                item.Nome = novoNome;
                item.ValorVenda = novoValorVenda;

                return item;
            }

        }

        throw new ArgumentException("Produto não encontrado");

    }

    public void AlterarSituacao(string codigoBarras)
    {
        var lista = CarregarProdutos(); 

        foreach (var item in lista)
        {
            if(item.CodigoBarras == codigoBarras)
            {

                if(Situacao == ESituacao.Ativo)
                {
                    Situacao = ESituacao.Inativo;
                }
                else
                {
                    Situacao = ESituacao.Ativo;
                }
            }
        }


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

        if (valor <= 0 || valor >= 1000)
            throw new ArgumentException("valor deve estar entre 0,001 e 999,99");   
        
            this.ValorVenda = valor;
            
    }

    public string ToStringFile()
    {
        return $"{Nome}{ValorVenda.ToString("000.00", ptBR)}{CodigoBarras}{UltimaVenda}{DataCadastro}{Situacao}";
    }


}