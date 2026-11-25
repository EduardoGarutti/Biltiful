
abstract class Banidos
{
    public List<string> LerArquivo(string arquivo)
    {
        var reader = new StreamReader(arquivo);
        List<string> info = new List<string>();
        do
        {
            info.Add(reader.ReadLine());

        } while (reader.Peek() != -1);
        reader.Close();
        return info;
    }

    public bool Localizar(string id, string arquivo)
    {
        List<string> info = this.LerArquivo(arquivo);
        if (info.Count == 0) {
            Console.WriteLine("Count = 0");
        }
        return  info.Exists(x => x.Trim() == id.Trim());

    }
    public void Cadastrar(string id, string arquivo)
    {
        try
        {
            if(!Localizar(id, arquivo))
            {
                var writer = new StreamWriter(arquivo, true);
                writer.WriteLine(id);
                writer.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Erro ao cadastrar banido");
        }
    }
}