using System.Globalization;
using System.Text.RegularExpressions;

public class ItemCompra
{
    // Identificador da compra à qual o item pertence.
    public int Id { get; private set; }

    // Deve corresponder à data da compra associada.
    public DateTime DataCompra { get; private set; }

    // Identificador da matéria-prima adquirida.
    public string MateriaPrima { get; private set; }

    public double Quantidade { get; private set; }

    // Preserva o valor unitário praticado no momento da compra.
    public double ValorUnitario { get; private set; }

    // Propriedade calculada com base na quantidade e no valor unitário.
    public double TotalItem
    {
        get
        {
            return Math.Round(Quantidade * ValorUnitario, 2);
        }
    }

    // Construtor responsável por validar e inicializar o item.
    public ItemCompra(int id, DateTime dataCompra,
                      string materiaPrimaId,
                      double quantidade,
                      double valorUnitario)
    {
        ValidarId(id);
        ValidarMateriaPrimaId(materiaPrimaId);
        ValidarQuantidade(quantidade);
        ValidarValorUnitario(valorUnitario);
        ValidarTotalItem(quantidade, valorUnitario);

        Id = id;
        DataCompra = dataCompra;
        MateriaPrima = materiaPrimaId;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }

    // Retorna o índice do item ou -1 caso não seja encontrado.
    public static int LocalizarItemCompra(
        List<ItemCompra> itensCompra,
        int compraId,
        string materiaPrimaId)
    {
        for (int i = 0; i < itensCompra.Count; i++)
        {
            if (itensCompra[i].Id == compraId &&
                itensCompra[i].MateriaPrima == materiaPrimaId)
            {
                return i;
            }
        }

        return -1;
    }

    // Retorna somente os itens pertencentes à compra informada.
    public static List<ItemCompra> ListarItensDaCompra(
        List<ItemCompra> itensCompra, int compraId)
    {
        List<ItemCompra> itensEncontrados = new List<ItemCompra>();

        foreach (ItemCompra item in itensCompra)
        {
            if (item.Id == compraId)
            {
                itensEncontrados.Add(item);
            }
        }

        return itensEncontrados;
    }

    // Valida as regras comerciais e retorna um novo item de compra.
    public static ItemCompra CadastrarItemCompra(
        List<ItemCompra> itensCompra,
        List<Compra> compras,
        List<MateriaPrima> materiasPrimas,
        int compraId,
        string materiaPrimaId,
        double quantidade,
        double valorUnitario)
    {
        int indiceCompra = Compra.LocalizarCompra(compras, compraId);

        if (indiceCompra == -1)
        {
            throw new Exception("Compra não encontrada");
        }

        Compra compra = compras[indiceCompra];

        //Uso do 'global::MateriaPrima para identificar explicitamente a classe, e a propriedade dessa classe
        int indiceMateriaPrima = global::MateriaPrima.LocalizarMateriaPrima(
            materiasPrimas, materiaPrimaId);

        if (indiceMateriaPrima == -1)
        {
            throw new Exception("Matéria-prima não encontrada");
        }

        MateriaPrima materiaPrima = materiasPrimas[indiceMateriaPrima];

        if (materiaPrima.Situacao != ESituacao.A)
        {
            throw new Exception(
                "Não é possível comprar matérias-primas inativas");
        }

        if (ListarItensDaCompra(itensCompra, compraId).Count >= 3)
        {
            throw new Exception(
                "A compra já atingiu o limite máximo de 3 itens");
        }

        ItemCompra item = new ItemCompra(
            compraId,
            compra.Data,
            materiaPrimaId,
            quantidade,
            valorUnitario);

        compra.AcrescentarValorTotal(item.TotalItem);

        return item;
    }

    // =====================================================
    // Métodos auxiliares de validação. XDDDD 
    // =====================================================

    // Método auxiliar que valida os limites do identificador.
    private static void ValidarId(int id)
    {
        if (id < 1 || id > 99999)
        {
            throw new ArgumentException("Id inválido.");
        }
    }

    // Método auxiliar que valida o formato MP0000.
    private static void ValidarMateriaPrimaId(string materiaPrimaId)
    {
        //Regex obtida via chatGPT
        //\z, garantindo o fim absoluto da string
        if (string.IsNullOrWhiteSpace(materiaPrimaId) ||
            !Regex.IsMatch(materiaPrimaId, @"^MP[0-9]{4}\z"))
        {
            throw new ArgumentException(
                "Identificador da matéria-prima inválido.");
        }
    }

    // Método auxiliar que valida os limites da quantidade.
    private static void ValidarQuantidade(double quantidade)
    {

        if (!(quantidade > 0))
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero");
        }

        else if (quantidade > 999.99)
        {
            throw new ArgumentException(
                "A quantidade ultrapassa o limite permitido de 999,99");
        }

        //verifica se a quantidade informada possui mais de duas casas decimais
        //Math.Round(quantidade, 2) Arredonda o valor para duas casas decimais
        else if (Math.Round(quantidade, 2) != quantidade)
        {
            throw new ArgumentException(
                "A quantidade deve possuir no máximo duas casas decimais.");
        }
    }

    // Método auxiliar que valida os limites do valor unitário.
    private static void ValidarValorUnitario(double valorUnitario)
    {
        if (!(valorUnitario > 0))
        {
            throw new ArgumentException(
                "O valor unitário deve ser maior que zero");
        }
        else if (valorUnitario > 999.99)
        {
            throw new ArgumentException(
                "O valor unitário ultrapassa o limite permitido de R$ 999,99");
        }
        else if (Math.Round(valorUnitario, 2) != valorUnitario)
        {
            throw new ArgumentException(
                "O valor unitário deve possuir no máximo duas casas decimais.");
        }
    }

    // Método auxiliar que valida o limite financeiro do item.
    private static void ValidarTotalItem(
        double quantidade, double valorUnitario)
    {
        if (Math.Round(quantidade * valorUnitario, 2) > 9999.99)
        {
            throw new ArgumentException(
                "O valor total do item ultrapassa o limite permitido de R$ 9.999,99");
        }
    }

    // Grava todos os itens de compra com campos de tamanho fixo.
    public static void GravarNoArquivo(
        List<ItemCompra> itensCompra, string caminhoArquivo)
    {
        using (StreamWriter sw = new StreamWriter(caminhoArquivo))
        {
            foreach (ItemCompra item in itensCompra)
            {
                string id = item.Id.ToString().PadLeft(5);

                string data = item.DataCompra.ToString(
                    "ddMMyyyy", CultureInfo.InvariantCulture);

                string materiaPrima = item.MateriaPrima.PadRight(6);

                string quantidade = item.Quantidade
                    .ToString("F2", CultureInfo.InvariantCulture)
                    .PadLeft(6);

                string valorUnitario = item.ValorUnitario
                    .ToString("F2", CultureInfo.InvariantCulture)
                    .PadLeft(6);

                string totalItem = item.TotalItem
                    .ToString("F2", CultureInfo.InvariantCulture)
                    .PadLeft(7);

                string registro = id + data + materiaPrima
                    + quantidade + valorUnitario + totalItem;

                sw.WriteLine(registro);
            }
        }
    }

    // Carrega os itens de compra armazenados no arquivo.
    public static List<ItemCompra> CarregarItensCompra(
        string caminhoArquivo)
    {
        List<ItemCompra> itensCompra = new List<ItemCompra>();

        if (!File.Exists(caminhoArquivo))
        {
            return itensCompra;
        }

        using (StreamReader sr = new StreamReader(caminhoArquivo))
        {
            string? linha;

            while ((linha = sr.ReadLine()) != null)
            {
                if (linha.Length != 38)
                {
                    throw new FormatException(
                        "Registro de item de compra inválido.");
                }

                int id = int.Parse(linha.Substring(0, 5).Trim());

                DateTime dataCompra = DateTime.ParseExact(
                    linha.Substring(5, 8),
                    "ddMMyyyy",
                    CultureInfo.InvariantCulture);

                string materiaPrima = linha.Substring(13, 6).Trim();

                double quantidade = double.Parse(
                    linha.Substring(19, 6).Trim(),
                    CultureInfo.InvariantCulture);

                double valorUnitario = double.Parse(
                    linha.Substring(25, 6).Trim(),
                    CultureInfo.InvariantCulture);

                double totalItem = double.Parse(
                    linha.Substring(31, 7).Trim(),
                    CultureInfo.InvariantCulture);

                ItemCompra item = new ItemCompra(
                    id,
                    dataCompra,
                    materiaPrima,
                    quantidade,
                    valorUnitario);

                // Verifica a consistencia do valor armazenado
                if (item.TotalItem != totalItem)
                {
                    throw new FormatException(
                        "Valor total do item inconsistente.");
                }

                itensCompra.Add(item);
            }
        }

        return itensCompra;
    }
}