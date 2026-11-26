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
        //Regex obtida atraves do chatGPT 
        if (string.IsNullOrWhiteSpace(materiaPrimaId) ||
            !Regex.IsMatch(materiaPrimaId, @"^MP[0-9]{4}$"))
        {
            throw new ArgumentException(
                "Identificador da matéria-prima inválido.");
        }
    }

    // Método auxiliar que valida os limites da quantidade.
    private static void ValidarQuantidade(double quantidade)
    {

        if (quantidade <= 0)
        {
            throw new ArgumentException(
                "A quantidade deve ser maior que zero");
        }

        if (quantidade > 999.99)
        {
            throw new ArgumentException(
                "A quantidade ultrapassa o limite permitido de 999,99");
        }
    }

    // Método auxiliar que valida os limites do valor unitário.
    private static void ValidarValorUnitario(double valorUnitario)
    {
        if (double.IsNaN(valorUnitario) || valorUnitario <= 0)
        {
            throw new ArgumentException(
                "O valor unitário deve ser maior que zero");
        }

        if (valorUnitario > 999.99)
        {
            throw new ArgumentException(
                "O valor unitário ultrapassa o limite permitido de R$ 999,99");
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
}