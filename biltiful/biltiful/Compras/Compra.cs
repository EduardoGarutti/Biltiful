public class Compra : TransacaoComercial
{
    // Armazena o CNPJ do fornecedor associado à compra.
    public string Fornecedor { get; private set; }

    // Armazena os itens pertencentes à compra.
    // Lista geral de todas as compras cadastradas.
    private static List<ItemCompra> Itens { get; set; }

    //Construtor utilizado para uma nova compra, que ainda não possui itens e,
    //consequentemente, possui valor total igual a zero
    public Compra(int id, DateTime dataCompra, string fornecedor)
        : base(id, dataCompra)
    {
        Fornecedor = fornecedor;

        // Inicializa uma lista vazia para receber os itens da compra.
        Itens = new List<ItemCompra>();
    }

    // Construtor utilizado para recuperar uma compra
    // anteriormente gravada no arquivo Compra.dat.
    public Compra(int id, DateTime dataCompra,
                  string fornecedor, double valorTotal)
        : base(id, dataCompra)
    {
        Fornecedor = fornecedor;
        ValorTotal = valorTotal;
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

    // Retorna o índice da compra pelo ID ou -1 caso não seja encontrada.
    public static int LocalizarCompra(List<Compra> compras, int id)
    {
        for (int i = 0; i < compras.Count; i++) {

            if (compras[i].Id == id) { 
                return i;
            }
            
        }
        return -1;
    }

    public void ExcluirCompra(List<Compra> compras, List<ItemCompra> itensCompra, int id)
    {
        int indice = LocalizarCompra(compras, id);

        if (indice == -1)
        {
            throw new Exception("Compra não encontrada");
        }

        // Percorre a lista de trás para frente para evitar
        // problemas com o deslocamento dos índices após a remoção
        for (int i = itensCompra.Count -1; i>= 0; i--) {

            if (itensCompra[i].Id == id)
            {
                itensCompra.RemoveAt(indice);
            }
        }

        compras.RemoveAt(indice);
    }

    

    // Gera o próximo identificador com base no maior ID cadastrado
    // Caso seja 0 garante que o primeiro Id é o primeiro
    private static int GerarId(List<Compra> compras)
    {
        int maiorId = 0;

        foreach (Compra compra in compras)
        {

            if (compra.Id == compra.Id)
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
}