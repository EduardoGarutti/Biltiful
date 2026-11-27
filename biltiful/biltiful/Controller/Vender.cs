{
    {

        {
            List<ItemVenda> itemVendas = ItemVenda.CarregarItensVenda("");

            int quantidadeDeProdutos = 0;
            bool usuarioQuerContinuar = true;
            try
            {
                Console.WriteLine("Digite o CPF do cliente");
                string cpf = Console.ReadLine();

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

        public static void Localizar(List<Venda> vendas)
        {
            try
            {
                Console.WriteLine("Digite o ID da venda que deseja localizar");
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

        {
            try
            {
                Console.WriteLine("Iniciando processo de registro");
            }
            catch
            {

            }
        }

    }
}
