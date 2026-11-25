public static class Utils { 

    //A produção requisita a compra
    //Função que verifica se tem itens disponiveis para produção
    //Ela recebe um a quantidade de matéria prima necessária e o id da matéria prima 
    public static bool QuantidadeDisponivelParaProducao(string arquivoProducao, int qntMateriaPrimaNecessaria, int idMateriaPrima, string arquivoCompra)
    {
        try
        {
            int quantidadeComprada = 0;
            StreamReader srC = new StreamReader(arquivoCompra); //Percorremos primeiro o arquivo de compras
            string linha;

            while ((linha = srC.ReadLine()) != null)
            {
                int.TryParse(linha.Substring(15, 6).Trim(), out int idMateriaDaLinha);
                //tenho certeza que, partindo do 15 caracter do arquivo, os proximos 6 caracteres representam o ID da Materia Prima

                if (idMateriaDaLinha == idMateriaPrima)
                {
                    int.TryParse(linha.Substring(21, 6).Trim(), out int quantidade);
                    //se o id da matéria prima for igual ao id recebido como parametro, pegamos a  propriedade Compra.quantidade da linha

                    quantidadeComprada += quantidade;
                }
            }
            srC.Close(); //fechando arquivo de compra

            if (quantidadeComprada == 0) //Se a quantiade de compras for igual a zero, significa que não existem itens para produção. retorna falso
            {
                return false;
            }

            int quantidadeGasta = 0;
            StreamReader srP = new StreamReader(arquivoProducao); //abrindo arquivo de ItemProducao

            while ((linha = srP.ReadLine()) != null)
            {
                int.TryParse(linha.Substring(15, 6).Trim(), out int idMateriaPrimaDaLinha);
                //partindo do character 15, os proximos 6 characteres representam o ID da Materia Prima
                if (idMateriaPrimaDaLinha == idMateriaPrima)
                {
                    int.TryParse(linha.Substring(21, 6).Trim(), out int quantidade);
                    //armazenamos a quantidade gasta em uma variavel auxiliar

                    quantidadeGasta += quantidade;
                }
            }

            srP.Close();

            if (qntMateriaPrimaNecessaria - (quantidadeComprada - quantidadeGasta) < 0)
            {
                return false;
            }
            return true;
        } catch(Exception e)
        {
            Console.WriteLine("ERRO INESPERADO");
            return false;
        }
        
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
}
