using System.Globalization;
using System.Text.RegularExpressions;

public class Cliente : EntidadeBase
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Clientes.dat"));

    public string CPF { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public char Sexo { get; set; }

    public Cliente(string cpf, string nome, DateTime dataNascimento, char sexo)
    {
        this.CPF = cpf;
        this.Nome = nome;
        this.DataNascimento = dataNascimento;
        this.Sexo = sexo;
    }
    public static List<Cliente> CarregarClientes()
    {
        Utils.ValidarCaminho(CaminhoArquivo);

        List<Cliente> listaClientes = new List<Cliente>();

        string[] clientes = File.ReadAllLines(CaminhoArquivo);

        foreach (var cliente in clientes)
        {
            string cpf = cliente.Substring(0, 14);
            string nome = cliente.Substring(14, 64);
            DateTime nascimento = DateTime.ParseExact(cliente.Substring(64, 74), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            char sexo = cliente[74];

            Cliente clienteCarregado = new Cliente(cpf, nome, nascimento, sexo);

            clienteCarregado.DataUltimaCompra = DateTime.ParseExact(cliente.Substring(75, 85), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            clienteCarregado.DataCadastro = DateTime.ParseExact(cliente.Substring(85, 95), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            clienteCarregado.Situacao = Enum.Parse<ESituacao>(cliente[95].ToString());

            listaClientes.Add(clienteCarregado);
        }

        return listaClientes;
    }

    public static Cliente CadastrarCliente(List<Cliente> lista, string cpf, string nome, DateTime nascimento, char sexo, bool Editando = false)
    {

        if (LocalizarCliente(lista, cpf) != -1 && !Editando) throw new Exception("Já existe um cliente com esse CPF");
        //if (LocalizarCliente(lista, cpf) == -1 && Editando) throw new Exception("Não existe um cliente com esse CPF");

        ValidarCPF(cpf);
        ValidarNome(nome);
        ValidarData(nascimento);
        ValidarSexo(sexo);

        return new Cliente(cpf, nome, nascimento, sexo);
    }

    public static int LocalizarCliente(List<Cliente> lista, string cpf)
    {
        Cliente clienteComCPFExistente = lista.Find(cliente => cliente.CPF == cpf);

        if (lista.Contains(clienteComCPFExistente)) return lista.IndexOf(clienteComCPFExistente);
        else return -1;
    }

    public static Cliente EditarCliente(List<Cliente> lista, string cpf, string novoNome, DateTime novaDataNascimento, char novoSexo)
    {
        return CadastrarCliente(lista, cpf, novoNome, novaDataNascimento, novoSexo, true);
    }

    public static void GravarNoArquivo(List<Cliente> clientes)
    {

        var writer = new StreamWriter(CaminhoArquivo, true);
        File.WriteAllText(CaminhoArquivo, "");

        foreach (var cliente in clientes)
        {
            string informacoesDoCliente = $"{cliente.CPF}" +
                                          $"{cliente.Nome.PadRight(50)}" +
                                          $"{cliente.DataNascimento.ToString("dd/MM/yyyy")}" +
                                          $"{cliente.Sexo}" +
                                          $"{cliente.DataUltimaCompra.ToString("dd/MM/yyyy")}" +
                                          $"{cliente.DataCadastro.ToString("dd/MM/yyyy")}" +
                                          $"{cliente.Situacao}";

            writer.WriteLine(informacoesDoCliente);
            
        }

        writer.Close();
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



}