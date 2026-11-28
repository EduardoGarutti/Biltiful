using System.Globalization;
using System.Text.RegularExpressions;

public class Cadastrar()
{

    
    
    public static void CadastrarProduto(List<Produto> produtos)
    {
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

            Console.WriteLine("Informe nome do Produto: ");
            string nomeProduto = Console.ReadLine();

            Console.WriteLine("Informe Valor da venda:");
            decimal valorVenda = decimal.Parse(Console.ReadLine());

            while (valorVenda is > (decimal)999.99 or <= 0)
            {
                Console.WriteLine("Valor de venda Invalido! Informe novamente: ");
                valorVenda = decimal.Parse(Console.ReadLine());
            }

            produtos.Add(Produto.CadastrarProduto(produtos, codigoDeBarras, nomeProduto, valorVenda));
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarProduto(produtos);
            }
        }

    }
    public static void CadastrarMateriaPrima(List<MateriaPrima> materiasPrimas)
    {
        try
        {
            Console.WriteLine("Informe o nome da matéria prima: ");
            string nomeMateriaPrima = Console.ReadLine();

            materiasPrimas.Add(MateriaPrima.CadastrarMateriaPrima(materiasPrimas, nomeMateriaPrima));
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            if (Utils.TentarNovamente())
            {
                Console.WriteLine("Reiniciando Processo de Cadastro...");
                CadastrarMateriaPrima(materiasPrimas);
            }
        }
    }
}


