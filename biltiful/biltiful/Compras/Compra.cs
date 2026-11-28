using System.Globalization;
using System.Text.RegularExpressions;

public class Compra : TransacaoComercial
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Compra.dat"));
    // Armazena o CNPJ do fornecedor responsável pela compra.
    public string Fornecedor { get; private set; }

    // Construtor utilizado para cadastrar uma nova compra.
    public Compra(int id, DateTime dataCompra, string fornecedor)
        : base(id, dataCompra)
    {
        Fornecedor = fornecedor;
    }

    // Construtor utilizado para recuperar uma compra do arquivo.
    public Compra(int id, DateTime dataCompra,
                  string fornecedor, double valorTotal)
        : base(id, dataCompra)
    {
        Fornecedor = fornecedor;
        ValorTotal = valorTotal;
    }

    // =====================================================
    // Métodos públicos definidos no Trello.
    // =====================================================

    // Valida as regras comerciais e retorna uma nova compra.
    public static Compra CadastrarCompra(
        List<Compra> compras,
        string cnpjFornecedor,
        List<Fornecedor> fornecedores,
        List<string> bloqueados)
    {

        // Normaliza o CNPJ antes de realizar as validações.
        cnpjFornecedor = Regex.Replace(cnpjFornecedor, @"\D", "");

        Fornecedor fornecedor = BuscarFornecedor(
            fornecedores, cnpjFornecedor);

        ValidarSituacaoFornecedor(fornecedor);

        ValidarFornecedorBloqueado(bloqueados, cnpjFornecedor);

        DateTime dataCompra = DateTime.Now;

        ValidarDataAbertura(fornecedor, dataCompra);

        int id = GerarId(compras);

        return new Compra(id, dataCompra, cnpjFornecedor);
    }

    // Retorna o índice da compra ou -1 caso não seja encontrada.
    public static int LocalizarCompra(List<Compra> compras, int id)
    {
        for (int i = 0; i < compras.Count; i++)
        {
            if (compras[i].Id == id)
            {
                return i;
            }
        }

        return -1;
    }

    // Exclui uma compra e todos os seus respectivos itens.
    public static void ExcluirCompra(
        List<Compra> compras,
        List<ItemCompra> itensCompra,
        int id)
    {
        int indice = LocalizarCompra(compras, id);

        if (indice == -1)
        {
            throw new Exception("Compra não encontrada");
        }

        // Percorre de trás para frente para não ignorar itens
        // durante a remoção.
        for (int i = itensCompra.Count - 1; i >= 0; i--)
        {
            if (itensCompra[i].Id == id)
            {
                itensCompra.RemoveAt(i);
            }
        }

        compras.RemoveAt(indice);
    }

    // Grava todas as compras no arquivo com campos de tamanho fixo.
    public static void GravarNoArquivo(
        List<Compra> compras)
    {
        // O using garante o descarte do StreamWriter
        // ao finalizar a operação.
        using (StreamWriter sw = new StreamWriter(CaminhoArquivo))
        {
            foreach (Compra compra in compras)
            {
                string id = compra.Id.ToString().PadLeft(5);

                string data = compra.Data.ToString(
                    "ddMMyyyy", CultureInfo.InvariantCulture);

                string fornecedor = compra.Fornecedor.PadRight(14);

                string valorTotal = compra.ValorTotal
                    .ToString("F2", CultureInfo.InvariantCulture)
                    .PadLeft(8);

                string registro = id + data + fornecedor + valorTotal;

                sw.WriteLine(registro);
            }
        }
    }

    // Carrega as compras gravadas no arquivo.
    public static List<Compra> CarregarCompras()
    {
        // A lista será preenchida com os objetos recuperados do arquivo.
        List<Compra> compras = new List<Compra>();

        // Caso o arquivo não exista, retorna uma lista vazia.
        if (!File.Exists(CaminhoArquivo))
        {
            return compras;
        }

        using (StreamReader sr = new StreamReader(CaminhoArquivo))
        {
            string? linha;

            // Realiza a leitura até encontrar o final do arquivo.
            while ((linha = sr.ReadLine()) != null)
            {
                // Cada registro deve possuir exatamente 35 caracteres.
                if (linha.Length != 35)
                {
                    throw new FormatException(
                        "Registro de compra inválido.");
                }

                int id = int.Parse(linha.Substring(0, 5).Trim());

                // Substring recebe o índice inicial e a quantidade
                // de caracteres que serão extraídos.
                // InvariantCulture evita diferenças de configuração regional.
                DateTime dataCompra = DateTime.ParseExact(
                    linha.Substring(5, 8),
                    "ddMMyyyy",
                    CultureInfo.InvariantCulture);

                string fornecedor = linha.Substring(13, 14).Trim();

                double valorTotal = double.Parse(
                    linha.Substring(27, 8).Trim(),
                    CultureInfo.InvariantCulture);

                // Reconstrói a compra preservando os dados originais.
                Compra compra = new Compra(
                    id, dataCompra, fornecedor, valorTotal);

                compras.Add(compra);
            }
        }

        return compras;
    }

    // =====================================================
    // Método auxiliar público utilizado pela classe ItemCompra  XXDD
    // =====================================================

    // Atualiza o valor total da compra apos o cadastro de um item.
    public void AcrescentarValorTotal(double valorItem)
    {
        if (!(valorItem >= 0 && valorItem <= 9999.99))
        {
            throw new ArgumentException("Valor do item inválido.");
        }

        double novoTotal = Math.Round(ValorTotal + valorItem, 2);

        if (novoTotal > 99999.99)
        {
            throw new Exception(
                "O valor total da compra passou o limite permitido!!");
        }

        ValorTotal = novoTotal;
    }

    // =====================================================
    // Métodos auxiliares privados utilizados no cadastro  XD
    // =====================================================

    // Localiza o fornecedor pelo CNPJ.
    // global::Fornecedor diferencia a classe Fornecedor
    // da propriedade Fornecedor existente nesta classe.
    private static Fornecedor BuscarFornecedor(
        List<Fornecedor> fornecedores,
        string cnpjFornecedor)
    {
        int indice = global::Fornecedor.LocalizarFornecedor(
            fornecedores, cnpjFornecedor);

        if (indice == -1)
        {
            throw new Exception("Fornecedor não encontrado");
        }

        return fornecedores[indice];
    }

    // Verifica se o fornecedor está ativo.
    private static void ValidarSituacaoFornecedor(
        Fornecedor fornecedor)
    {
        if (fornecedor.Situacao != ESituacao.A)
        {
            throw new Exception(
                "Não é possível realizar compras de fornecedores inativos");
        }
    }

    // Verifica se o CNPJ consta na lista de fornecedores bloqueados.
    private static void ValidarFornecedorBloqueado(
    List<string> bloqueados,
    string cnpjFornecedor)
    {
        if (Banidos.Localizar(bloqueados, cnpjFornecedor))
        {
            throw new Exception(
                "Fornecedor está bloqueado para realizar operações comerciais");
        }
    }

    // Valida o tempo mínimo de seis meses de abertura da empresa.
    private static void ValidarDataAbertura(
        Fornecedor fornecedor, DateTime dataCompra)
    {
        if (fornecedor.DataAbertura.Date.AddMonths(6) > dataCompra.Date)
        {
            throw new Exception(
                "O fornecedor deve possuir pelo menos seis meses de abertura para realizar vendas à BILTIFUL");
        }
    }

    // Gera o próximo ID com base no maior identificador cadastrado.
    private static int GerarId(List<Compra> compras)
    {
        int maiorId = 0;

        foreach (Compra compra in compras)
        {
            if (compra.Id > maiorId)
            {
                maiorId = compra.Id;
            }
        }

        if (maiorId >= 99999)
        {
            throw new Exception("Limite de identificadores atingido.");
        }

        return maiorId + 1;
    }

    public override string ToString()
    {
        return $"ID: {Id}\n" +
               $"Data da compra: {Data:dd/MM/yyyy}\n" +
               $"Fornecedor: {Fornecedor}\n" +
               $"Valor total: R$ {ValorTotal:F2}";
    }
}