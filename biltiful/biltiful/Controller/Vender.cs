namespace biltiful.Controller
{
    public class Vender
    {

        public static void RealizarVenda(List<Venda> vendas, List<Cliente> clientes, List<string> inadimplentes)
        {
            try
            {
                Console.WriteLine("Digite o CPF do cliente");
                string cpf = Console.ReadLine();
                Venda.CadastrarVenda(vendas, clientes, inadimplentes, cpf);

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
                string idvenda = Console.ReadLine();
                int indiceVenda = vendas.LocalizarVenda(idvenda);
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

        public static void Salvar(List<Venda> vendas)
        {
            try
            {
                Console.WriteLine("Iniciando processo de registro");
                Venda.GravarNoArquivo(vendas);
            }
            catch
            {

            }
        }

    }
}
