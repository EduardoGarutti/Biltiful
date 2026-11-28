using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace biltiful.Controller
{
    public static class ProdutoController
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

        public static void ImprimirProduto(List<Produto> produtos)
        {

            Consultar.Consulta(produtos);
            Console.WriteLine("Precione enter para continuar");
            Console.ReadKey();

        }

        public static void LocalizarProduto(List<Produto> produtos)
        {

            try
            {
                Console.WriteLine("Informe codigo de barra do produto: ");
                string codBarra = Console.ReadLine();
                codBarra = Regex.Replace(codBarra, @"[^a-zA-Z0-9]", "");


                int id = Produto.LocalizarProduto(produtos, codBarra);

                if (id == -1)
                    throw new Exception("Produto não encontrado!");

                Console.WriteLine(produtos[id]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    LocalizarProduto(produtos);
                }

            }
        }

        public static void ExcluirProduto(List<Produto> produtos)
        {
            try
            {
                Console.WriteLine("Informe codigo de barra do produto: ");
                string codBarra = Console.ReadLine();
                codBarra = Regex.Replace(codBarra, @"[^a-zA-Z0-9]", "");


                int id = Produto.LocalizarProduto(produtos, codBarra);

                if (id == -1)
                    throw new Exception("Produto não encontrado!");

                produtos[id].AlterarSituacao(codBarra);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    Console.WriteLine("Reiniciando Processo de Exclusão...");
                    ExcluirProduto(produtos);
                }
            }
        }

        public static void EditarProduto(List<Produto> produtos)
        {
            try
            {

                Console.WriteLine("Informe codigo de barra do produto: ");
                string codBarra = Console.ReadLine();
                codBarra = Regex.Replace(codBarra, @"[^a-zA-Z0-9]", "");

                Console.WriteLine("Informe nome do Produto: ");
                string nomeProduto = Console.ReadLine();

                Console.WriteLine("Informe Valor da venda:");
                decimal valorVenda = decimal.Parse(Console.ReadLine());

                while (valorVenda is > (decimal)999.99 or <= 0)
                {
                    Console.WriteLine("Valor de venda Invalido! Informe novamente: ");
                    valorVenda = decimal.Parse(Console.ReadLine());
                }

                Produto.EditarProduto(codBarra, nomeProduto, valorVenda);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    CadastrarProduto(produtos);
                }
            }
        }
    }
}
