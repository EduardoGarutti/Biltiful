public class Comprar()
{
    public static void RealizarCompra<T>(List<Fornecedor> fornecedores, List<Compra> compras, List<Bloqueados> bloqueados, List<MateriaPrima> materiaPrima)
    {
        // Declara a referência fora do try para permitir seu acesso no catch.
        // Inicialmente, não existe uma compra instanciada.
        Compra? compra = null;

        try
        {
            // Solicita o CNPJ do fornecedor.
            Console.Write("Informe o CNPJ do fornecedor: ");
            string cnpj = Console.ReadLine()!;

            // Delega à classe Compra as validações de negócio:
            // existência do fornecedor, situação, bloqueio e tempo de abertura.
            // Após as validações, gera o ID e retorna uma nova Compra.
            compra = Compra.CadastrarCompra(
                compras,
                cnpj,
                fornecedores,
                bloqueados
            );

            // Solicita a quantidade de itens que farão parte da compra.
            Console.Write("Quantos itens deseja comprar (1 a 3)? ");
            int quantidadeItens = int.Parse(Console.ReadLine()!);

            // Valida a quantidade de itens conforme o enunciado.
            if (quantidadeItens < 1 || quantidadeItens > 3)
            {
                throw new ArgumentException(
                    "A compra deve possuir entre 1 e 3 itens.");
            }

            // Adiciona a compra à lista de compras.
            // Isso permite que CadastrarItemCompra localize a compra
            // pelo seu ID e atualize posteriormente seu valor total.
            compras.Add(compra);

            // Executa o cadastro individual de cada item da compra.
            for (int i = 0; i < quantidadeItens; i++)
            {
                Console.WriteLine($"\nItem {i + 1}");

                // Solicita o identificador da matéria-prima.
                Console.Write("Código da matéria-prima: ");
                string materiaPrimaId = Console.ReadLine()!;

                // Solicita a quantidade de matéria-prima comprada.
                Console.Write("Quantidade: ");
                double quantidade = double.Parse(Console.ReadLine()!);

                // Solicita o valor unitário da matéria-prima.
                Console.Write("Valor unitário: ");
                double valorUnitario = double.Parse(Console.ReadLine()!);

                // Delega à classe ItemCompra a responsabilidade
                // de validar e instanciar um novo item.
                //
                // O ID da compra estabelece o relacionamento entre
                // o objeto Compra e o objeto ItemCompra.
                //
                // O método também atualiza o valor total da compra.
                ItemCompra item = ItemCompra.CadastrarItemCompra(
                    itensCompra,
                    compras,
                    materiasPrimas,
                    compra.Id,
                    materiaPrimaId,
                    quantidade,
                    valorUnitario
                );

                // Adiciona o item validado à lista de itens das compras.
                itensCompra.Add(item);
            }

            // Informa que todos os itens foram cadastrados com sucesso.
            Console.WriteLine("\nCompra realizada com sucesso!");

            // Exibe os dados da compra utilizando seu ToString().
            Console.WriteLine(compra);

            compras.Add(Compra.CadastrarCompra(compras, cnpj, fornecedores, bloqueados));
        } catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            RealizarCompra<T>(fornecedores, compras, bloqueados, materiaPrima);
        }
        
            Console.WriteLine("Compra realizada com Sucesso!");
    }
}

