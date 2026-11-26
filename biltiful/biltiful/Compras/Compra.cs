using System.Globalization;

public class Compra : TransacaoComercial
{
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

    // Valida as regras comerciais e retorna uma nova compra.
    public static Compra CadastrarCompra(
        List<Compra> compras,
        string cnpjFornecedor,
        List<Fornecedor> fornecedores,
        List<Bloqueados> bloqueados)
    {
        Fornecedor fornecedor = BuscarFornecedor(fornecedores, cnpjFornecedor);

        ValidarSituacaoFornecedor(fornecedor);

        ValidarFornecedorBloqueado(bloqueados, cnpjFornecedor);

        DateTime dataCompra = DateTime.Now;

        ValidarDataAbertura(fornecedor, dataCompra);

        int id = GerarId(compras);

        return new Compra(id, dataCompra, cnpjFornecedor);
    }

    // =====================================================
    // Métodos auxiliares utilizados no cadastro de compras.XD
    // =====================================================

    // Método auxiliar responsável por localizar o fornecedor
    // pelo CNPJ ou lançar uma exceção caso não seja encontrado.
    private static Fornecedor BuscarFornecedor(
        List<Fornecedor> fornecedores,
        string cnpjFornecedor)
    {
        foreach (Fornecedor fornecedor in fornecedores)
        {
            if (fornecedor.CNPJ == cnpjFornecedor)
            {
                return fornecedor;
            }
        }

        throw new Exception("Fornecedor não encontrado");
    }

    // Método auxiliar responsável por verificar se o fornecedor
    // está ativo e apto a realizar operações comerciais.
    private static void ValidarSituacaoFornecedor(Fornecedor fornecedor)
    {
        if (fornecedor.ESituacao != 'A')
        {
            throw new Exception(
                "Não é possível realizar compras de fornecedores inativos");
        }
    }

    // Método auxiliar responsável por verificar se o CNPJ
    // do fornecedor consta na lista de bloqueados.
    private static void ValidarFornecedorBloqueado(
        List<Bloqueados> bloqueados,
        string cnpjFornecedor)
    {
        foreach (Bloqueados bloqueado in bloqueados)
        {
            if (bloqueado.CNPJ == cnpjFornecedor)
            {
                throw new Exception(
                    "Fornecedor está bloqueado para realizar operações comerciais");
            }
        }
    }

    // Método auxiliar responsável por validar o tempo mínimo
    // de seis meses de abertura da empresa.
    private static void ValidarDataAbertura(
        Fornecedor fornecedor, DateTime dataCompra)
    {
        if (fornecedor.DataAbertura.Date.AddMonths(6) > dataCompra.Date)
        {
            throw new Exception(
                "O fornecedor deve possuir pelo menos seis meses de abertura para realizar vendas à BILTIFUL");
        }
    }

    // Método auxiliar responsável por gerar o próximo ID
    // com base no maior identificador cadastrado.
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

    // Grava todas as compras no arquivo com campos de tamanho fixo.
    public static void GravarNoArquivo(
        List<Compra> compras, string caminhoArquivo)
    {
        using (StreamWriter sw = new StreamWriter(caminhoArquivo))
        {
            foreach (Compra compra in compras)
            {
                string id = compra.Id.ToString().PadLeft(5);

                string data = compra.Data.ToString("ddMMyyyy");

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
    public static List<Compra> CarregarCompras(string caminhoArquivo)
    {
        //A lista é preenchida com os objetos retornados do arquivo
        List<Compra> compras = new List<Compra>();

        //Caso não exista o arquico de compras, método retorna uma lista vazia 
        if (!File.Exists(caminhoArquivo))
        {
            return compras;
        }

        //O StreamReader realiza a leitura do arquivo
        using (StreamReader sr = new StreamReader(caminhoArquivo))
        {
            string? linha;

            while ((linha = sr.ReadLine()) != null)
            {
                //Lê uma linha do arquivo.
                //Atribui seu conteúdo à variável linha
                //Verifica se o resultado é diferente de null.
                if (linha.Length != 35)
                {
                    throw new FormatException("Registro de compra inválido.");
                }

                int id = int.Parse(linha.Substring(0, 5).Trim());

                //ParseExact() interpreta essa string utilizando exatamente o formato ddMMyyyy
                //5: índice a partir do qual começaremos a leitura.
                //8: quantidade de caracteres que serão extraídos.
                //CultureInfo.InvariantCulture) permite que a conversão de dados aconteça de maneira
                //independente das configurações regionais do computador
                DateTime dataCompra = DateTime.ParseExact(
                    linha.Substring(5, 8),
                    "ddMMyyyy",
                    CultureInfo.InvariantCulture);

                string fornecedor = linha.Substring(13, 14).Trim();

                double valorTotal = double.Parse(
                    linha.Substring(27, 8).Trim(),
                    CultureInfo.InvariantCulture);

                Compra compra = new Compra(
                    id, dataCompra, fornecedor, valorTotal);

                compras.Add(compra);
            }
        }

        return compras;
    }
}