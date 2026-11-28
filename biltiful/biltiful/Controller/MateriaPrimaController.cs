using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace biltiful.Controller
{
    public class MateriaPrimaController
    {

        public static void CadastrarMateriaPrima(List<MateriaPrima> materiaPrimas)
        {
            try
            {
                Console.WriteLine("Informe nome do MateriaPrima: ");
                string nomeMateriaPrima = Console.ReadLine();

                MateriaPrima.CadastrarMateriaPrima(materiaPrimas, nomeMateriaPrima);

                Console.WriteLine("Cadastro realizado com sucesso, precione ENTER para continuar");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    Console.WriteLine("Reiniciando Processo de Cadastro...");
                    CadastrarMateriaPrima(materiaPrimas);
                }

                return;
            }
        }

        public static void ImprimirMateriaPrima(List<MateriaPrima> materiaPrimas)
        {

            Console.WriteLine("Deseja imprimir qual status de materia prima:");
            var escolha = Utils.CriarMenu(new List<string> {
                "[1] - Ativos",
                "[2] - Inativos",
                "[3] - Todos"
            });

            switch (escolha)
            {
                case '1':
                    Consultar.Consulta(MateriaPrima.ListarMateriasPrimasAtivas(materiaPrimas));
                    break;
                case '2':
                    Consultar.Consulta(MateriaPrima.ListarMateriasPrimasInativas(materiaPrimas));
                    break;
                case '3':
                    Consultar.Consulta(MateriaPrima.CarregarMateriaPrima());
                    break;
            }

            Console.WriteLine("Precione ENTER para continuar");
            Console.ReadKey();
        }

        public static void LocalizarMateriaPrima(List<MateriaPrima> MateriaPrimas)
        {

            try
            {
                Console.WriteLine("Informe ID da materia prima: ");
                string idMP = Console.ReadLine();

                int id = MateriaPrima.LocalizarMateria(MateriaPrimas, idMP);

                if (id != -1)
                {
                    Console.WriteLine(MateriaPrimas[id]);
                } else
                {
                    throw new Exception("Materia prima não encontrada");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    LocalizarMateriaPrima(MateriaPrimas);
                }

                return;

            }

            Console.WriteLine("Precione ENTER para continuar");
            Console.ReadKey();
        }

        public static void ExcluirMateriaPrima(List<MateriaPrima> MateriaPrimas)
        {
            try
            {
                Console.WriteLine("Informe id da materia prima: ");
                string idInformado = Console.ReadLine();

                int id = MateriaPrima.LocalizarMateria(MateriaPrimas, idInformado);
                if (id != -1)
                {
                    MateriaPrimas[id].AlterarSituacao(id + "");
                }
                else
                {
                    throw new Exception("Materia prima não encontrada");
                }

                Console.WriteLine("Exclusão realizada com sucesso, precione ENTER para continuar");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
                if (Utils.TentarNovamente())
                {
                    ExcluirMateriaPrima(MateriaPrimas);
                }
            }
        }

    }
}
