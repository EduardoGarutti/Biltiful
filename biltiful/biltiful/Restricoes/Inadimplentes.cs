class Inadimplentes : Banidos
{

    public string Arquivo { get; private set; } = @"C:/Aula/risco.dat";
    public bool Cadastrar(string cpf)
    {
        try
        {
            base.Cadastrar(cpf, this.Arquivo );
            return true;
        }
        catch
        {
            Console.WriteLine("Erro ao cadastrar CPF bloqueado");
            return false;
        }
    }

    public bool Localizar(string cnpf)
    {
        return base.Localizar(cnpf, this.Arquivo);

    }
}