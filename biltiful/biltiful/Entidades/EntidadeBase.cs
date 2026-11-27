using Microsoft.VisualBasic;
using System.Globalization;
using System.Text.RegularExpressions;

public abstract class EntidadeBase
{

    public DateTime DataUltimaCompra { get; set; } = DateTime.Today;
    public DateTime DataCadastro { get; set; } = DateTime.Today;
    public ESituacao Situacao { get; set; } = ESituacao.A;

    public static bool ValidarNome(string nome)
    {
        if (nome.Length > 50) throw new Exception("O nome possui mais que 50 caracteres");
        if (nome.Length == 0) throw new Exception("O nome está vazio");

        return true;
       
    }

    public static bool ValidarData(DateTime data)
    {
        string dataConvertida = data.ToString("dd/MM/yyyy");
        bool EhValido = Regex.IsMatch(dataConvertida, @"^\d{2}\/\d{2}\/\d{4}$");
        if (!EhValido) throw new Exception("Data ou formato inválido");
        return true;

    }

    public void AlterarSituacao()
    {
        if(Situacao == ESituacao.A)
        {
            Situacao = ESituacao.I;
        }
        else
        {
            Situacao = ESituacao.A;
        }
    }



}