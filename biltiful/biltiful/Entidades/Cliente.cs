using System.Globalization;
using System.Text.RegularExpressions;

public class Cliente : EntidadeBase
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "Clientes.dat"));

    public string CPF { get; set; }
    public string Nome { get; set; }
    public DateTime DataNascimento { get; set; }
    public char Sexo { get; set; }

    public Cliente(string cpf, string nome, string dataNascimento, char sexo)
    {
        this.CPF = cpf;
        this.Nome = nome;
        this.DataNascimento = DateTime.ParseExact(dataNascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture); 
        this.Sexo = sexo;
    }

    public static List<Cliente> CarregarClientes()
    {
        Utils.ValidarCaminho(CaminhoArquivo);

        List<Cliente> listaClientes = new List<Cliente>();

        //Pega todos os clientes do arquivo guardando cada linha/cliente como um item num vetor
        string[] clientes = File.ReadAllLines(CaminhoArquivo); 

        //Para cada linha/cliente pego, desmembra a linha pegando os dados pelos indices que estão posicionados, os atribui em variaveis também convertendo se necessário, cria um objeto novo com essas informações e então adiciona na lista que será retornada
        foreach (var cliente in clientes)
        {
            string cpf = cliente.Substring(0, 11);
            string nome = cliente.Substring(11, 61).TrimEnd();
            string nascimento = cliente.Substring(61, 71);
            char sexo = cliente[71];

            Cliente clienteCarregado = new Cliente(cpf, nome, nascimento, sexo);

            //Esses dados são inseridos após a instanciação pois não são atribuidos pelo construtor mas sim gerados automaticamente pela classe base, e são informações variaveis que são alteradas por outros métodos 
            clienteCarregado.DataUltimaCompra = DateTime.ParseExact(cliente.Substring(72, 82), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            clienteCarregado.DataCadastro = DateTime.ParseExact(cliente.Substring(82, 92), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            clienteCarregado.Situacao = Enum.Parse<ESituacao>(cliente[92].ToString());

            listaClientes.Add(clienteCarregado);
        }

        return listaClientes;
    }

    public static Cliente CadastrarCliente(List<Cliente> lista, string cpf, string nome, string nascimento, char sexo, bool Editando = false)
    {
        //Remoção de possíveis espaçoes e caracteres especiais para garantir padronizãção de tamanho e formato
        cpf = cpf.TrimEnd().TrimStart();
        nome = nome.TrimEnd().TrimStart();
        nascimento = nascimento.TrimEnd().TrimStart();

        //Se já existir um cliente com esse CPF e a ação não é de edição mas criação, retorna uma exceção
        if (LocalizarCliente(lista, cpf) != -1 && !Editando) throw new Exception("Já existe um cliente com esse CPF");
        //Se não existir um cliente com esse CPF e a ação for de edição, retorna uma exceção
        if (LocalizarCliente(lista, cpf) == -1 && Editando) throw new Exception("Não existe um cliente com esse CPF");
        
        ValidarCPF(cpf);
        ValidarNome(nome);
        ValidarData(nascimento);
        ValidarSexo(sexo);

        //Remove todos os caracteres especiais para então instanciar um objeto 
        cpf = Regex.Replace(cpf, @"\.||\-", "");


        return new Cliente(cpf, nome, nascimento, sexo);
    }

    public static int LocalizarCliente(List<Cliente> lista, string cpf)
    {
        //Verifica se já há um cliente com o CPF informado
        Cliente? clienteComCPFExistente = lista.Find(cliente => cliente.CPF == cpf);

        //Se exisitr um cliente, retorna o índice dele na lista, se não, retorna -1
        if (clienteComCPFExistente != null) return lista.IndexOf(clienteComCPFExistente);
        else return -1;
    }

    public static Cliente EditarCliente(List<Cliente> lista, string cpf, string novoNome, string novaDataNascimento, char novoSexo)
    {
        //Autoexplicativo
        return CadastrarCliente(lista, cpf, novoNome, novaDataNascimento, novoSexo, true);
    }

    public static void GravarNoArquivo(List<Cliente> clientes)
    {
        //Cria um fluxo de escrita habilitado para adicionar ao invés de substituir
        var writer = new StreamWriter(CaminhoArquivo, true);
        //Limpa o arquivo
        File.WriteAllText(CaminhoArquivo, "");

        //Para cada objeto cliente na lista, pega suas propriedades e coloca formatado numa string que vai representar sua linha no arquivo e então adiciona no arquivo
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

    public static bool EhMenorDeIdade(Cliente cliente)
    {
        //Pega a idade em anos sem considerar meses ou dias
        DateTime hoje = DateTime.Today;
        int idade = hoje.Year - cliente.DataNascimento.Year;

        //Se o mês e dia do nascimento, sem considerar o ano, for maior que o mês e dia de hoje, subtrai 1 ano pois esse ano o cliente ainda não fez aniversário(peguei do Lucas)
        if (cliente.DataNascimento.Date > hoje.AddYears(-idade).Date) idade--;

        if (idade < 18) return true;
        else return false;
    }

    public static bool ValidarSexo(char sexo)
    {
        string sexoFormatado = sexo.ToString().ToLower();

        if (sexoFormatado != "m" || sexoFormatado != "f") throw new Exception("Sexo inválido!");

        return true;
    }

    public static bool ValidarCPF(string cpf)
    {
        //Verifica se o CPF segue a formatação de: 3 números, um ponto opcional, 3 números, um ponto opcional, 3 números, um traço opcioal e 2 números
        bool EhValido = Regex.IsMatch(cpf, @"^(\d{3}\.?){2}\d{3}\-?\d{2}$");
        if (!EhValido) throw new Exception("O CPF não está no formato correto");
        
        //Pega os 9 dígitos antes do traço e verifica se eles são todos iguais
        string digitosIniciais = cpf.Replace(".", "")[0..9];
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

        string digitosFinais = cpf.Replace("-", "")[9..];
        int soma;
        int indiceDigitosIniciais;
        int indiceNumerosFinais = 0;

        //Pesquisa as fórmulas e métodos pra validar CPF amigão 
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

            //Verifica se os 2 últimos dígitos correspondem aos outros 9 de acordo com a fórmula
            if (digitosFinais[indiceNumerosFinais] != cpf[9 + indiceNumerosFinais]) throw new Exception("O CPF não é válido! Os dígitos verificadores não correspondem aos 9 primeiros");

            //Adiciona o primeiro dígito(e também o segundo, mas não importa) pós traço já validado aos dígitos que serão usados para validação 
            digitosIniciais += digitosFinais[indiceNumerosFinais];

            indiceNumerosFinais++;
        }

        return true;
    }

    public override string ToString()
    {
        return $"CPF: {this.CPF}\nNome: {this.Nome}\nData de nascimento: {this.DataNascimento}\nSexo: {this.Sexo}\nData do cadastro: {this.DataCadastro}\nData da última compra: {this.DataUltimaCompra}\nSituação: {this.Situacao}";
    }

}