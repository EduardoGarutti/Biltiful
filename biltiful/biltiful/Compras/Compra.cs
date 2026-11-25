public class Compra : TransacaoComercial
{
    // Armazena o CNPJ do fornecedor associado à compra.
    public string FornecedorId { get; private set; }

    // Armazena os itens pertencentes à compra.
    // O private set impede a substituição da lista por outras classes,
    // mas não impede a inclusão ou remoção direta de elementos.
    public List<ItemCompra> Itens { get; private set; }

    // Construtor responsável por inicializar o objeto Compra.
    // Encaminha o ID e a data para o construtor da classe pai
    // TransacaoComercial através do base().
    public Compra(int id, DateOnly dataCompra, string fornecedorId)
        : base(id, dataCompra)
    {
        FornecedorId = fornecedorId;

        // Inicializa uma lista vazia para receber os itens da compra.
        Itens = new List<ItemCompra>();
    }

    // Responsável por adicionar um item à compra.
    // Retorna true se o item for adicionado com sucesso
    // ou false caso alguma validação não seja atendida.
    public bool AdicionarItem(ItemCompra item)
    {
        // Verifica se a compra ainda pode receber novos itens.
        // Caso tenha atingido o limite, interrompe o método.
        if (!ValidarQuantidadeItens())
        {
            return false;
        }

        // Verifica se o item pertence à compra atual,
        // comparando seu identificador e sua data.
        if (!ValidarItem(item))
        {
            return false;
        }

        // Adiciona o item à lista após passar pelas validações.
        Itens.Add(item);

        // Atualiza o valor total da compra considerando
        // todos os itens, inclusive o que acabou de ser adicionado.
        ValorTotal = CalcularValorTotal();

        // Informa que a inclusão foi realizada com sucesso.
        return true;
    }

    // Responsável por calcular o valor total da compra,
    // somando o valor total de cada item presente na lista.
    public double CalcularValorTotal()
    {
        // Variável acumuladora que inicia com zero.
        double total = 0;

        // Percorre todos os objetos ItemCompra presentes na lista.
        foreach (ItemCompra item in Itens)
        {
            // Soma o valor total do item ao valor acumulado.
            // O TotalItem é calculado pela própria classe ItemCompra.
            total += item.TotalItem;
        }

        // Retorna a soma dos valores de todos os itens.
        return total;
    }

    // Verifica se a compra ainda possui espaço para receber um item.
    // A documentação estabelece o limite máximo de três itens.
    private bool ValidarQuantidadeItens()
    {
        // Retorna true quando existem menos de três itens.
        // Retorna false quando o limite já foi atingido.
        return Itens.Count < 3;
    }

    // Verifica se o item pertence à compra atual.
    // Evita associar itens de compras diferentes.
    private bool ValidarItem(ItemCompra item)
    {
        // O ID do item deve ser igual ao ID da compra.
        // A data do item também deve ser igual à data da compra.
        // Ambas as condições precisam ser verdadeiras.
        return item.Id == Id && item.DataCompra == Data;
    }

    // Verifica se a compra atende às condições internas
    // necessárias para sua finalização.
    // Este método apenas valida, não realiza a gravação no arquivo.
    public bool PodeFinalizar()
    {
        // A compra deve possuir no mínimo um item.
        // A compra deve possuir no máximo três itens.
        // O valor total não pode ultrapassar R$ 99.999,99.
        return Itens.Count >= 1
        && Itens.Count <= 3
        && ValorTotal <= 99999.99;
    }
}