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

                Console.WriteLine("Cadastro realizado com sucesso");

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    Console.WriteLine("Reiniciando Processo de Cadastro...");
                    CadastrarProduto(produtos);
                }

                return;
            }

        }

        public static void ImprimirProduto(List<Produto> produtos)
        {

            Console.WriteLine("Deseja imprimir qual status de produto:");
           var escolha =  Utils.CriarMenu(new List<string> { 
                "[1] - Ativos",
                "[2] - Inativos",
                "[3] - Todos"
            });

            switch (escolha) {
                case '1':
                    Consultar.Consulta(Produto.ProdutoAtivos());
                    break;
                case '2':
                    Consultar.Consulta(Produto.ProdutosInativos());
                    break;
                case '3':
                    Consultar.Consulta(Produto.CarregarProdutos());
                    break;
            }

            Console.WriteLine("Precione ENTER para continuar");
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

                if (id != -1)
                {
                    Console.WriteLine(produtos[id]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    LocalizarProduto(produtos);
                }

                return;

            }

            Console.WriteLine("Precione ENTER para continuar");
            Console.ReadKey();
        }

        public static void ExcluirProduto(List<Produto> produtos)
        {
            try
            {
                Console.WriteLine("Informe codigo de barra do produto: ");
                string codBarra = Console.ReadLine();
                codBarra = Regex.Replace(codBarra, @"[^a-zA-Z0-9]", "");


                int id = Produto.LocalizarProduto(produtos, codBarra);

                produtos[id].AlterarSituacao(codBarra);

                Console.WriteLine("Exclusão realizada com sucesso, precione ENTER para continuar");
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    Console.WriteLine("Reiniciando Processo de Exclusão...");
                    ExcluirProduto(produtos);
                }

                return;
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

                Console.WriteLine("Edição realizada com sucesso, precione ENTER para continuar");
                Console.ReadKey();
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
