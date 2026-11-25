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

        return base.Localizar(cnpf, this.Arquivo);

    }

}