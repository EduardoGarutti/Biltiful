using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

public abstract class EntidadeBase
{

    public string Nome{ get;
        set
        {
            bool EhValido = value.Length <= 50;
            if (!EhValido) throw new Exception("O nome possui mais que 50 caracteres");
            else field = value;

        }
    }
    public DateOnly DataNascimento { get; set; }
    public DateOnly DataUltimaCompra { get; set; }
    public DateOnly DataCadastro { get; set; } = new DateOnly();
    public char ESituacao { get; set; }

    public EntidadeBase(string nome, DateOnly dataNascimento, DateOnly dataUltimaCompra, char eSituacao)
    {
        this.Nome = nome;
        this.DataNascimento = dataNascimento;
        this.DataUltimaCompra = dataUltimaCompra;
        this.ESituacao = eSituacao;
    }

    public void VerificarExistenciaDoArquivo(string arquivo)
    {
        if (!File.Exists(arquivo))
        {
            File.Create(arquivo);
        }
    }

    

}