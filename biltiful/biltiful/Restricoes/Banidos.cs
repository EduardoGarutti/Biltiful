public class Banidos
{
    static public List<string> CarregarArquivo(string arquivo)
    {
        try
        {
            var reader = new StreamReader(arquivo);
            List<string> info = new List<string>();
            while (reader.Peek() != -1)
            {
                info.Add(reader.ReadLine());
            }
            ;
            reader.Close();
            return info;
        }
        catch (Exception ex)
        {
            throw new("Não foi possivel ler o arquivo: " + ex.Message);
        }
    }

    static public void GravarNoArquivo(List<string> list, string arquivo)
    {
        try
        {
            var writer = new StreamWriter(arquivo);
            foreach (var item in list)
            {
                writer.WriteLine(item);
            }

            writer.Close();

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    static public bool Localizar(List<string> lista, string id)
    {
        try
        {
            return lista.Exists(x => x.Trim() == id.Trim());
        }
        catch
        {
            throw new Exception("Não foi possivel executar a funcao de localizacao");
        }

    }
    static public void Cadastrar(List<string> lista, string id, string arquivo)
    {
        try
        {
            if (Localizar(lista, id))
            {
                throw new Exception("Já existe esse registro");

            }

            lista.Add(id);
            GravarNoArquivo(lista, arquivo);
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possivel cadastrar banido");
        }
    }

    static public void Remover(List<string> lista, string id)
    {

        try
        {
            if (!lista.Remove(id.Trim()))
            {
                throw new Exception("Banido não encontrado");
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao remover banido");
        }

    }
}