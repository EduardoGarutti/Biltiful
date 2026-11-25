public class ItemCompra
{
    // Identificador da compra a qual este item pertence.
    public int Id { get; private set; }

    // Data da compra. Deve corresponder a data da entidade Compra.
    public DateOnly DataCompra { get; private set; }

    // Identificador da matéria-prima adquirida.
    public string MateriaPrimaId { get; private set; }

    // Quantidade de matéria-prima adquirida.
    public double Quantidade { get; private set; }

    // Valor unitário da matéria-prima no momento da compra.
    //Se futuramente o preço da matéria-prima mudar, o valor da
    //compra antiga continuará sendo o mesmo
    public double ValorUnitario { get; private set; }

    //Responsável por combinar partes de um caminho,
    //utilizando os separadores apropriados do sistema operacional
    private static string CaminhoArquivo = Path.Combine(Utils.CaminhoDats, "Compra.dat");

    // Construtor responsável por validar os parâmetros recebidos
    // antes de atribuí-los às propriedades do objeto.
    public ItemCompra(int id, DateOnly dataCompra,
                      string materiaPrimaId,
                      double quantidade,
                      double valorUnitario)
    {
        //Verificacoes antes de instaciar um objeto

        // Verifica se o identificador está dentro dos limites permitidos.
        if (!ValidarId(id))
        {
            //ArgumentException é uma exceção do .NET lançada quando um método recebe um argumento inválido ou fora do esperado.
            throw new ArgumentException("Id inválido.");
        }

        // Verifica se a quantidade informada é válida.
        if (!ValidarQuantidade(quantidade))
        {
            throw new ArgumentException("Quantidade inválida.");
        }

        // Verifica se o valor unitário está dentro dos limites permitidos.
        if (!ValidarValorUnitario(valorUnitario))
        {
            throw new ArgumentException("Valor unitário inválido.");
        }

        // Verifica se o resultado da multiplicação da quantidade
        // pelo valor unitário respeita o limite total do item.
        if (!ValidarTotalItem(quantidade, valorUnitario))
        {
            throw new ArgumentException("Total do item excedeu o limite.");
        }

        // Após passar pelas validações, os parâmetros
        // são atribuídos às propriedades do objeto.
        Id = id;
        DataCompra = dataCompra;
        MateriaPrimaId = materiaPrimaId;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }

    // Propriedade calculada que representa o valor total do item.
    // Não possui set, pois seu valor depende de outras propriedades.
    public double TotalItem
    {
        get
        {
            // Multiplica a quantidade pelo valor unitário
            // e arredonda o resultado para duas casas decimais.
            return Math.Round(Quantidade * ValorUnitario, 2);
        }
    }

    // Valida se o ID está entre 1 e 99999.
    private bool ValidarId(int id)
    {
        return id >= 1 && id <= 99999;
    }

    // Valida se a quantidade é positiva e respeita o limite máximo.
    private bool ValidarQuantidade(double quantidade)
    {
        return quantidade > 0 && quantidade <= 999.99;
    }

    // Valida se o valor unitário é positivo e respeita o limite máximo.
    private bool ValidarValorUnitario(double valorUnitario)
    {
        return valorUnitario > 0 && valorUnitario <= 999.99;
    }

    // Valida se o valor total do item respeita o limite máximo.
    private bool ValidarTotalItem(double quantidade, double valorUnitario)
    {
        return Math.Round(quantidade * valorUnitario, 2) <= 9999.99;
    }
}