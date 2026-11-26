using System.Text.RegularExpressions;

public class Fornecedor : EntidadeBase
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Fornecedor.dat"));

    public string CNPJ { get; set; }

    public Fornecedor(string cnpj, string nome, string dataNascimento) : base(nome, dataNascimento)
    {
        this.CNPJ = cnpj;

        VerificarExistenciaDoArquivo(CaminhoArquivo);
        Cadastrar();
    }

    public static bool ValidarCNPJ(string cnpj)
    {
        bool EhValido = Regex.IsMatch(cnpj, @"^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$");
        if (!EhValido) throw new Exception("O CNPJ não está no formato correto");

        string digitosIniciais = Regex.Replace(cnpj[0..15], @"\.||\/", "");

        for (int n = 1; n < digitosIniciais.Length; n++)
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

        if (!EhValido) throw new Exception("O CNPJ não é válido! Todos os primeiros 12 dígitos são iguais");

        string digitosFinais = cnpj[16..];
        int soma;
        int indiceDigitosIniciais;
        int indiceNumerosFinais = 0;

        foreach (var digito in digitosFinais)
        {
            soma = 0;
            indiceDigitosIniciais = 0;
            bool segundaIteracao = false;

            for (int p = 5 + indiceNumerosFinais; p >= 2; p--)
            {
                soma += int.Parse(digitosIniciais[indiceDigitosIniciais].ToString()) * p;
                indiceDigitosIniciais++;

                if (!segundaIteracao && p == 2)
                {
                    p = 10;
                    segundaIteracao = true;
                }
            }

            int resto = soma % 11;

            switch (indiceNumerosFinais)
            {
                case 0:
                    if (resto < 2) digitosFinais = "0" + digitosFinais[1];
                    else digitosFinais = $"{(11 - resto)}" + digitosFinais[1];
                    break;

                case 1:
                    if (resto < 2) digitosFinais = digitosFinais[0] + "0";
                    else digitosFinais = digitosFinais[0] + $"{(11 - resto)}";
                    break;
            }

            if (digitosFinais[indiceNumerosFinais] != cnpj[16 + indiceNumerosFinais]) throw new Exception("O CPF não é válido! Os dígitos verificadores não correspondem aos 9 primeiros");

            digitosIniciais += digitosFinais[indiceNumerosFinais];

            indiceNumerosFinais++;
        }

        return true;
    }

    public void Cadastrar()
    {
        string fornecedor = $"{this.CNPJ}" +
                            $"{this.Nome.PadRight(50)}" +
                            $"{this.DataNascimento.ToString("dd/MM/yyyy")}" +
                            $"{this.DataUltimaCompra.ToString("dd/MM/yyyy")}" +
                            $"{this.DataCadastro.ToString("dd/MM/yyyy")}" +
                            $"{this.ESituacao}";

        var writer = new StreamWriter(CaminhoArquivo, true);

        writer.WriteLine(fornecedor);
        writer.Close();

    }
    
    public string LocalizarFornecedor(string cnpjProcurado)
    {
        string[] fornecedores = File.ReadAllLines(CaminhoArquivo);

        foreach (var fornecedor in fornecedores)
        {
            string cnpj = fornecedor.Substring(0, 18);

            if(cnpj == cnpjProcurado)
            {
                return fornecedor;
            }

        }

        return "Não foi encontrado nenhum fornecedor com esse CNPJ";
    }
}
