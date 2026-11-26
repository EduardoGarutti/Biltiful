class Inadimplentes
{

    public string Arquivo { get; private set; } = @"C:/Aula/risco.dat";
    public bool Cadastrar(string cpf)
    {
        try
        {
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Localizar(string cnpf)
    {
        try
        { }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Remover(string cpf)
    {

        try
        {
            return true;
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex.Message);
            return false;

        }

    }

}