using biltiful.Producao;
using System.Globalization;

public static class Utils
{

    //A produção requisita a compra
    //Função que verifica se tem itens disponiveis para produção
    //Ela recebe um a quantidade de matéria prima necessária e o id da matéria prima, Uma lista de ItemProducao e outra Lista de ItemCompra
    public static bool QuantidadeDisponivelParaProducao(List<ItemProducao> listaItemProducao, double qntMateriaPrimaNecessaria, string idMateriaPrima, List<ItemCompra> listaItemCompra)
    {
        double quantidadeItem = 0;

        foreach (var compra in listaItemCompra)
        {
            if (compra.MateriaPrima.Equals(idMateriaPrima)) //Percorre a lista de Item Compra.
                                                            //Caso haja algum item comprado com o mesmo id Requisitado, acrescenta a quantidade em uma variavel de controle
            {
                quantidadeItem += compra.Quantidade;
            }
        }

        if (quantidadeItem <= 0) //Se a quantidade comprada for menor ou igual a 0, não temos estoque
        {
            return false;
        }


        foreach (var producao in listaItemProducao) 
        {
            if (producao.MateriaPrima.Equals(idMateriaPrima))//percorre a lista de ItemProducao. 
                                                             //Se o ID da Materia Prima for igual ao ItemProducao.MateriaPrimaID, siginifica que aquele item foi gasto na producao
                                                             //Pegamos a quantidade gasta na produção e subtraimos da quantidade comprada, obtendo assim a quantidade de Materia Prima Atual
            {
                quantidadeItem -= producao.QuantidadeMateriaPrima;
            }
        }

        if (quantidadeItem - qntMateriaPrimaNecessaria < 0) // Se a quantidade de Item menos da quantidade requisita for menor ou igual a zero,
                                                            // significa que não temos itens o suficiente
        {
            return false;
        }
        return true;
    }

    //Função que valida e cria o diretório Dats
    public static bool ValidarDats()
    {
        //Concatena caminho + nome do diretório
        string caminho = AppContext.BaseDirectory + "Dats";

        try
        {
            //Se o diretório não existe, cria um novo diretório Dats
            if (!Directory.Exists(caminho))
            {
                Directory.CreateDirectory(caminho);
            }
            return true;
        }
        catch (Exception e)
        {
            //Se ocorrer alum erro no processo, retorna um false
            Console.WriteLine(e.Message);
            return false;
        }
    }

    //Função que cria menu by: pestana
    public static char CriarMenu(List<string> options)
    {
        foreach (string item in options)
            Console.Write(item);
        try
        {
            Console.Write("Informe a opção desejada: ");
            return char.TryParse(Console.ReadLine(), out char option) ? option : '\0';
        }
        catch
        {
            return '\0';
        }
    }

    //Função que valida se aquele caminho existe
    public static bool ValidarCaminho(string caminhoDoArquivo)
    {
        if (ValidarDats())
        {
            if (!File.Exists(caminhoDoArquivo))

            {
                File.Create(caminhoDoArquivo).Close();
                return true;
            }
            else
            {
                return true;
            }   
        }
        return false;

    }
}
