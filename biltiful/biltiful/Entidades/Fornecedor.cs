public class Fornecedor : EntidadeBase
{
    public static string CaminhoArquivo = Environment.GetEnvironmentVariable("Dats/Fornecedor.dat");

    public string CNPJ { get; set; }

    public Fornecedor(string cnpj, string nome, DateOnly dataNascimento, DateOnly dataUltimaCompra, char eSituacao) : base(nome, dataNascimento, dataUltimaCompra, eSituacao)
    {
        this.CNPJ = cnpj;

        VerificarExistenciaDoArquivo(CaminhoArquivo);
    }

    
}