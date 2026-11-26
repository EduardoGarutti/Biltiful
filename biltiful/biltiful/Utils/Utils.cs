using System.Globalization;

public static class Utils
{

    //A produção requisita a compra
    //Função que verifica se tem itens disponiveis para produção
    //Ela recebe um a quantidade de matéria prima necessária e o id da matéria prima 
    public static bool QuantidadeDisponivelParaProducao(List<Object> listaItemProducao, double qntMateriaPrimaNecessaria, string idMateriaPrima, List<Object> listaItemCompra)
    {
        double quantidadeComprada = 0;

        foreach (var compra in listaItemCompra)
        {
            if (compra.MateriaPrima.Equals(idMateriaPrima))
            {
                quantidadeComprada += compra.Quantidade;
            }
        }

        if (quantidadeComprada <= 0)
        {
            return false;
        }

        double quantidadeGasta = 0;

        foreach (var producao in listaItemProducao)
        {
            if (producao.MateriaPrima.Equals(idMateriaPrima))
            {
                quantidadeGasta += producao.QuantidadeMateriaPrima;
            }
        }

        if ((quantidadeComprada - quantidadeGasta) - qntMateriaPrimaNecessaria < 0)
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
