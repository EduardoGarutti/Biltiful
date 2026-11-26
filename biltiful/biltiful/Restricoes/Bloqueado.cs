public class Bloqueados : Banidos
{
    public string Arquivo { get; private set; } = @"C:/Aula/bloqueados.dat";

   public List<string> CarregarArquivo(string arquivo)
    {
        try
        {
            return base.CarregarArquivo(arquivo);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    public bool Cadastrar(string cnpf)
    {
        try
        {
            return true;
        }
        catch
        {
            Console.WriteLine("Erro ao cadastrar CNPJ bloqueado");
            return false;
        }
    }

    public bool Localizar(List<string> lista, string cnpf)
    {
        try
        {

            return base.Localizar(lista, cnpf);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }

    public bool Remover(List<string> lista, string cnpj)
    {
        try
        {
            return base.Remover(lista, cnpj, )
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}