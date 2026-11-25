using System.Text.RegularExpressions;

public class Cliente : EntidadeBase
{
    public static string CaminhoArquivo = Environment.GetEnvironmentVariable("Dats/Clientes.dat");

    public string CPF { get; set; }
    public char Sexo { get; set; }

    public Cliente(string cpf, char sexo, string nome, DateOnly dataNascimento, DateOnly dataUltimaCompra, char eSituacao) : base( nome, dataNascimento, dataUltimaCompra, eSituacao)
    {
        this.CPF = cpf;
        this.Sexo = sexo;

        VerificarExistenciaDoArquivo(CaminhoArquivo);
    }

    public static bool ValidarCPF(string cpf)
    {
        bool EhValido = Regex.IsMatch(cpf, @"^(\d{3}\.){2}\d{3}\-\d{2}$");
        if (!EhValido) throw new Exception("O CPF não está no formato correto");
        string digitosIniciais = cpf[0..11].Replace(".", "");
        for (int n = 1; n <= cpf.Length; n++)
        {
            if (digitosIniciais[n - 1] == digitosIniciais[n])
            {
                EhValido = false;
            }
            else
            {
                EhValido = true;
                break;
            }
        }
        if (!EhValido) throw new Exception("O número do CPF não é válido, todos os primeiros 9 números são iguais");

        string digitosFinais = cpf[12..];
        int soma = 0;
        int indiceNumerosIniciais = 0;

        for(int p = 10; p >= 2; p--)
        {
            soma += (int)digitosIniciais[indiceNumerosIniciais] * p;
            indiceNumerosIniciais++;
        }
        int resto1 = soma % 11;
        if (resto1 < 2) digitosFinais = "0" + digitosFinais[1];




        return true;
    }


}