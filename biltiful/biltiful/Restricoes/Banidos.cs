
public abstract class Banidos
{
    public List<string> CarregarArquivo(string arquivo)
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

    public void GravarNoArquivo(List<string> list, string arquivo)
    {
        try
        {
            var writer = new StreamWriter(arquivo, true);
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

    public  bool Localizar(List<string> lista, string id)
    {
        try
        {
            if (lista.Count != 0)
            {
                return lista.Exists(x => x.Trim() == id.Trim());
            }

            return false;
        }
        catch
        {
            throw new Exception("Não foi possivel executar a funcao de localizacao");
        }

    }
    public void Cadastrar(List<string> lista, string id, string arquivo)
    {
        try
        {
            if(!Localizar(lista, id))
            {
                var writer = new StreamWriter(arquivo, true);
                writer.WriteLine(id);
                writer.Close();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            throw new Exception("Não foi possivel cadastrar banido: /n Possivel Origem:" + ex.Message);
        }
    }

    public void Remover(List<string> lista, string id, string arquivo)
    {

        try
        {
            if (!lista.Remove(id))
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