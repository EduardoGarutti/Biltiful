using System.Text.RegularExpressions;

public class Cliente : EntidadeBase
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Clientes.dat"));

    public string CPF { get; set; }
    public char Sexo { get; set; }

    public Cliente(string cpf, char sexo, string nome, string dataNascimento) : base( nome, dataNascimento)
    {
        this.CPF = cpf;
        this.Sexo = sexo;

        VerificarExistenciaDoArquivo(CaminhoArquivo);
    }
    public static bool ValidarSexo(char sexo)
    {
        string sexoFormatado = sexo.ToString().ToLower();
        if (sexoFormatado != "m" || sexoFormatado != "f") throw new Exception("Sexo inválido!");

        return true;
    }


    public static bool ValidarCPF(string cpf)
    {
        bool EhValido = Regex.IsMatch(cpf, @"^(\d{3}\.){2}\d{3}\-\d{2}$");
        if (!EhValido) throw new Exception("O CPF não está no formato correto");

        string digitosIniciais = cpf[0..11].Replace(".", "");
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

        if (!EhValido) throw new Exception("O CPF não é válido! Todos os primeiros 9 dígitos são iguais");

        string digitosFinais = cpf[12..];
        int soma;
        int indiceDigitosIniciais;
        int indiceNumerosFinais = 0;

        foreach (var digito in digitosFinais)
        {
            soma = 0;
            indiceDigitosIniciais = 0;

            for (int p = 10 + indiceNumerosFinais; p >= 2; p--)
            {
                soma += int.Parse(digitosIniciais[indiceDigitosIniciais].ToString()) * p;
                indiceDigitosIniciais++;
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

            if (digitosFinais[indiceNumerosFinais] != cpf[12 + indiceNumerosFinais]) throw new Exception("O CPF não é válido! Os dígitos verificadores não correspondem aos 9 primeiros");

            digitosIniciais += digitosFinais[indiceNumerosFinais];

            indiceNumerosFinais++;
        }

        return true;
    }

    public static Cliente CadastrarCliente(List<Cliente> lista, string cpf, string nome, string nascimento, char sexo)
    {
        return new Cliente(cpf, sexo, nome, nascimento);
    }

    public static int LocalizarCPF(List<Cliente> lista, string cpf)
    {
        Cliente clienteComCPFRepetido = lista.Find(cliente => cliente.CPF == cpf);

        if (lista.Contains(clienteComCPFRepetido)) return lista.IndexOf(clienteComCPFRepetido);
        else return -1;
    }

    public static List<Cliente> CarregarClientes()
    {
        List<Cliente> listaClientes = new List<Cliente>();

        string[] clientes = File.ReadAllLines(CaminhoArquivo);

        foreach (var cliente in clientes)
        {
            string cpf = cliente.Substring(0, 14);
            string nome = cliente.Substring(14, 64);
            string nascimento = cliente.Substring(64, 74);
            char sexo = cliente[74];
            Cliente clienteCarregado = new Cliente(cpf, sexo, nome, nascimento);
            clienteCarregado.DataUltimaCompra = DateOnly.ParseExact(cliente.Substring(75, 85), "dd/MM/yyyy");
            clienteCarregado.DataCadastro = DateOnly.ParseExact(cliente.Substring(85, 95), "dd/MM/yyyy");
            clienteCarregado.ESituacao = cliente[95];

            listaClientes.Add(clienteCarregado);
        }

        return listaClientes;
    }

    public void EditarCliente(List<Cliente> lista, string cpf, string novoNome, string novaDataNascimento, char novoSexo)
    {
        int indiceDoClienteASerEditado = LocalizarCPF(lista, cpf);

        lista[indiceDoClienteASerEditado].Nome = novoNome;
        lista[indiceDoClienteASerEditado].DataNascimento = DateOnly.ParseExact(novaDataNascimento, "dd/MM/yyyy");
        lista[indiceDoClienteASerEditado].Sexo = novoSexo;

    }

    public static void GravarNoArquivo(List<Cliente> clientes)
    {
        

        foreach (var cliente in clientes)
        {
            string informacoesDoCliente = $"{cliente.CPF}" +
                                          $"{cliente.Nome.PadRight(50)}" +
                                          $"{cliente.DataNascimento.ToString("dd/MM/yyyy")} " +
                                          $"{cliente.Sexo}" +
                                          $"{cliente.DataUltimaCompra.ToString("dd/MM/yyyy")}" +
                                          $"{cliente.DataCadastro.ToString("dd/MM/yyyy")}" +
                                          $"{cliente.ESituacao}";
        }
    }


}