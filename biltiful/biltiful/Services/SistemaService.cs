
using biltiful.Models;

namespace biltiful.Services;

public class SistemaService
{
    public List<Cliente> Clientes { get; private set; }
    public List<Fornecedor> Fornecedores { get; private set; }
    public List<Produto> Produtos { get; private set; }
    public List<MateriaPrima> MateriasPrimas { get; private set; }
    public List<Producao> Producoes { get; private set; }
    public List<Compra> Compras { get; private set; }
    public List<Venda> Vendas { get; private set; }
    public List<string> Inadimplentes { get; private set; }
    public List<string> Bloqueados { get; private set; }

    public SistemaService()
    { 
        CarregarDados();
    }

    private void CarregarDados()
    {

        string pathInadimplentes = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Riscos.dat"));
        string pathBloqueados = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Bloqueados.dat"));
        string pathVendas = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Venda.dat"));

        List<Cliente> clientes = Cliente.CarregarClientes();
        List<Fornecedor> fornecedores = Fornecedor.CarregarFornecedores();
        List<Producao> producoes = Producao.CarregarProducoes();
        List<Compra> compras = Compra.CarregarCompras();
        List<Produto> produtos = Produto.CarregarProdutos();
        List<MateriaPrima> materiasPrimas = MateriaPrima.CarregarMateriaPrima();
        List<string> inadimplentes = Banidos.CarregarArquivo(pathInadimplentes);
        List<string> bloqueados = Banidos.CarregarArquivo(pathBloqueados);
        List<Venda> vendas = Venda.CarregarVendas(pathVendas);
    }

    public void SalvarDados()
    {
        string pathInadimplentes = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Riscos.dat"));
        string pathBloqueados = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Bloqueados.dat"));
        string pathVendas = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Venda.dat"));

        Cliente.GravarNoArquivo(Clientes);
        Fornecedor.GravarNoArquivo(Fornecedores);
        Produto.GravarNoArquivo(Produtos);
        MateriaPrima.GravarNoArquivo(MateriasPrimas);
        Producao.SalvarArquivoProducao(Producoes);
        Compra.GravarNoArquivo(Compras);

        Banidos.GravarNoArquivo(Inadimplentes, pathInadimplentes);
        Banidos.GravarNoArquivo(Bloqueados, pathBloqueados);
        Venda.GravarNoArquivo(Vendas, pathVendas);
    }
}
