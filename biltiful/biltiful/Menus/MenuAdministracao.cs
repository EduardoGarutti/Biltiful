using biltiful.Controller;
using biltiful.Services;

public class MenuAdministracao 
{
    public static void Menu(SistemaService allLists)
    {
        while (true)
        {
            var escolha = Utils.CriarMenu(new List<string>(){
                "============CLIENTE============\n",
                "[1] - Gerenciar Cliente\n",
                "[2] - Gerenciar Fornecedor\n",
                "[3] - Gerenciar Produtos\n",
                "[4] - Gerenciar Matérias-Primas\n",
                "[5] - Consultar Inadimplentes\n",
                "[6] - Consultar Bloqueados\n",
                "[0] - Voltar\n",
                "================================\n"
                });

            switch (escolha)
            {
                case '1':
                    ClienteController.GerenciarCliente(allLists);
                    break;
                case '2':
                    FornecedorController.GerenciarFornecedor(allLists.Fornecedores);
                    break;
                case '3':
                    ProdutoController.GerenciarProdutos(allLists.Produtos);
                    break;
                case '4':
                    MateriaPrimaController.GerenciarMateriaPrima(allLists.MateriasPrimas);
                    break;
                case '5':
                    Consultar.Consulta(allLists.Inadimplentes);
                    break;
                case '6':
                    Consultar.Consulta(allLists.Bloqueados);
                    break;
                case '0':
                    return;
                default:
                    Console.WriteLine("Opção Invalida!");
                    break;
            }
        }
    }
}
