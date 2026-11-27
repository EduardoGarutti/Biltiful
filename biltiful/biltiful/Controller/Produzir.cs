using System.Text.RegularExpressions;

public class Produzir()
{
    public static void RealizarProducao(List<Producao> producoes, List<MateriaPrima> materiasPrimas)
    {
        List<ItemProducao> itemProducoes = null;

        try
        {
            string codigoDeBarras;
            string comecoValido;
            do
            {
                Console.WriteLine("Informe Codigo de Barras do produto(Começando obrigatóriamente com 789): ");
                codigoDeBarras = Console.ReadLine();

                codigoDeBarras = Regex.Replace(codigoDeBarras, @"[^a-zA-Z0-9]", "");
                comecoValido = codigoDeBarras.Substring(0, 3);
            } while (codigoDeBarras.Length != 13 || comecoValido.Equals("789"));

            if (Produto.LocalizarProduto(Produto.ProdutoAtivos(), codigoDeBarras) == -1)
            {
               throw new Exception ("Produto não encontrado");
            }

            Console.WriteLine("Informe a quantidade que deseja produzir:"); 

            bool quantidadeValida = int.TryParse(Console.ReadLine(), out var quantidade);

            if (!quantidadeValida)
            {
                Console.WriteLine("Informe uma Quantidade Válida");
            }
            if (quantidade <= 0 || quantidade > 99999)
            {
                throw new Exception("Quantidade deve ser maior que zero e menor que 99999");
            }

            Producao producao = Producao.CadastrarProducao(producoes, codigoDeBarras, quantidade);
            Console.WriteLine("Informe quantas Materias primas o produto utiliza?");

            bool quantidadeMPValida = int.TryParse(Console.ReadLine(), out var quantidadeMP);
            
            if (!quantidadeMPValida)//imprime e passa
            {
                throw new Exception("Informe uma Quantidade Válida");
            }

            if(quantidadeMP <= 0 || quantidadeMP >= 99999)
            {
                throw new Exception("A Quantidade deve ser maior que 0 e no máximo 99.999!");
            }

            for (int i = 1; i <= quantidadeMP; i++)
            {
                Console.WriteLine($"Informe o Codigos da {i}ª Materia Prima utilizada por esse produto:");
                string idMateriaPrima = Console.ReadLine();
                if (MateriaPrima.LocalizarMateria(MateriaPrima.ListarMateriasPrimasAtivas(materiasPrimas), idMateriaPrima) == -1)//so ve se existe
                {
                    throw new Exception("Informe um código de Matéria Prima Válido!");
                }
                Console.WriteLine($"Informe a Quantidade da {i}ª Materia Prima utilizada por esse produto:");
                 int quantidadeMateria = int.Parse(Console.ReadLine());
                itemProducoes.Add(ItemProducao.CadastrarItemProducao(producao, idMateriaPrima, quantidadeMateria));

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao Realizar Produção: {ex.Message}");
        }

    }
}
