
namespace biltiful

{
    public class VendasMenu
    {


        public static void Cadastrar(List<Venda> vendas, List<Cliente> clientes, List<string> inadimplentes)
        {
            List<ItemVenda> itemVendas = ItemVenda.CarregarItensVenda("");

            int quantidadeDeProdutos = 0;
            bool usuarioQuerContinuar = true;
            try
            {
                Console.WriteLine("Digite o CPF do cliente");
                string cpf = Console.ReadLine();

                if (Cliente.LocalizarCliente(clientes, cpf) == -1)
                {
                    throw new Exception("Cliente não encontrando");
                }
                if (Banidos.Localizar(inadimplentes, cpf))
                {
                    throw new Exception("Cliente esta banido");
                }

                List<Produto> produtos = Produto.ProdutoAtivos();
                foreach (Produto prod in produtos)
                {
                    Console.WriteLine(prod);
                }

                decimal valorDaVenda = 0;
                List<Produto> produtosDaCompra = new List<Produto>();

                do
                {
                    Console.WriteLine("Digite o codigo de barra");
                    string codigoDeBarra = Console.ReadLine();

                    int indiceProd = Produto.LocalizarProduto(produtos, codigoDeBarra);

                    if (indiceProd == null && indiceProd == -1)
                    {
                        throw new Exception("Produto não encontrado");
                    }

                    Console.WriteLine("Quantidade de venda");
                    int quantVenda = int.Parse(Console.ReadLine());

                    if(quantVenda <= 0)
                    {
                        throw new Exception("Compra não pode ser menor que 0");

                    }

                    Produto produtoAtualDaCompra = produtos[indiceProd];
                    valorDaVenda += produtoAtualDaCompra.ValorVenda;

                    if (quantidadeDeProdutos < 2) {
                        Console.WriteLine("Deseja adicionar mais produtos?");
                    }

                    produtosDaCompra.Add(produtoAtualDaCompra);

                } while (quantidadeDeProdutos < 3 || usuarioQuerContinuar);

                Venda vendaAtual = Venda.CadastrarVenda(clientes, cpf, valorDaVenda);

                foreach (Produto item in produtosDaCompra) {

                    ItemVenda.CadastrarItemVenda(itemVendas, vendas, produtos,vendaAtual.Id, item.CodigoBarras, 1);
                        
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                if (Utils.TentarNovamente())
                {
                    Cadastrar(vendas, clientes, inadimplentes);
                }
            }
        }

        public static void Localizar(List<Venda> vendas)
        {
            try
            {
                Console.WriteLine("Digite o ID da venda que deseja localizar");
                int idvenda = int.Parse(Console.ReadLine());
                int indiceVenda = Venda.LocalizarVenda(vendas, idvenda);
                Console.WriteLine(vendas[indiceVenda]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                if (Utils.TentarNovamente())
                {
                    Localizar(vendas);
                }
            }
        }

        public static void Salvar(List<Venda> vendas, string arquivo)
        {
            try
            {
                Console.WriteLine("Iniciando processo de registro");
                Venda.GravarNoArquivo(vendas, arquivo);
            }
            catch
            {

            }
        }
    }
}
