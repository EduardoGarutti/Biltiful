class Inadimplentes : Banidos
{

    public string Arquivo { get; private set; } = @"C:/Aula/risco.dat";
    public bool Cadastrar(string cpf)
    {
        try
        {
            base.Cadastrar(cpf, this.Arquivo);
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
        {
            return base.Localizar(cnpf, this.Arquivo);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool Remover(string cpf) {

        try
        {
            base.Remover(cpf, this.Arquivo);
            return true;
        }
        catch (Exception ex) {

            Console.WriteLine(ex.Message);
            return false;

        }
    
    }

}