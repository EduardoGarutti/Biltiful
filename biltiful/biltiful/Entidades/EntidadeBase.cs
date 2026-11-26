using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

public abstract class EntidadeBase
{

    public string Nome{ get; set; }
    public DateOnly DataNascimento { get; set; }
    public DateOnly DataUltimaCompra { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly DataCadastro { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public char ESituacao { get; set; } = 'A';

    public EntidadeBase(string nome, string dataNascimento)
    {
        this.Nome = nome;
        this.DataNascimento = DateOnly.ParseExact(dataNascimento, "dd/MM/yyyy");
    }

    public void VerificarExistenciaDoArquivo(string arquivo)
    {
        if (!File.Exists(arquivo))
        {
            File.Create(arquivo);
        }
    }

    public static bool ValidarNome(string nome)
    {
        if (nome.Length > 50) throw new Exception("O nome possui mais que 50 caracteres");
        return true;
       
    }

    public static bool ValidarData(string data)
    {
        bool EhValido = Regex.IsMatch(data, @"^\d{2}\/\d{2}\/\d{4}$");
        if (!EhValido) throw new Exception("Data ou formato inválido");
        return true;

    }

    public void AlterarSituacao()
    {
        this.ESituacao = 'I';
    }



}