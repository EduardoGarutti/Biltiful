class Bloqueados : Banidos
{
    public string Arquivo { get; private set; } = @"C:/Aula/bloqueados.dat";
    public bool Cadastrar(string cnpf)
    {
        try
        {
            base.Cadastrar(cnpf, this.Arquivo);
            return true;
        }
        catch
        {
            Console.WriteLine("Erro ao cadastrar CNPJ bloqueado");
            return false;
        }
    }

    public bool Localizar(string cnpf) {

        try
        {
            return base.Localizar(cnpf, this.Arquivo);
        }
        catch(Exception ex) 
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }

    public bool Remover(string cnpf)
    {
        try
        {
            base.Remover(cnpf, this.Arquivo);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}