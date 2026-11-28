using System.Globalization;
using System.Text.RegularExpressions;

public class Fornecedor : EntidadeBase
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "Fornecedor.dat"));

    public string CNPJ { get; set; }
    public string RazaoSocial { get; set; }
    public DateTime DataAbertura { get; set; }


    public Fornecedor(string cnpj, string razaoSocial, string dataAbertura)
    {
        this.CNPJ = cnpj;
        this.RazaoSocial = razaoSocial;
        this.DataAbertura = DateTime.ParseExact(dataAbertura, "dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    public static List<Fornecedor> CarregarFornecedores()
    {
        Utils.ValidarCaminho(CaminhoArquivo);

        List<Fornecedor> listaFornecedores = new List<Fornecedor>();

        //Pega todos os fornecedores do arquivo guardando cada linha/fornecedor como um item num vetor
        string[] fornecedores = File.ReadAllLines(CaminhoArquivo);

        //Para cada linha/fornecedor pego, desmembra a linha pegando os dados pelos indices que estão posicionados, os atribui em variaveis também convertendo se necessário, cria um objeto novo com essas informações e então adiciona na lista que será retornada
        foreach (var fornecedor in fornecedores)
        {
            string cnpj = fornecedor.Substring(0, 14);
            string razaoSocial = fornecedor.Substring(14, 64).TrimEnd();
            string abertura = fornecedor.Substring(64, 74);

            Fornecedor fornecedorCarregado = new Fornecedor(cnpj, razaoSocial, abertura);

            //Esses dados sãao inseridos após a instanciação pois não são atribuidos pelo construtor mas sim gerados automaticamente pela classe base, e são informações variaveis que são alteradas por outros métodos 
            fornecedorCarregado.DataUltimaCompra = DateTime.ParseExact(fornecedor.Substring(74, 84), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            fornecedorCarregado.DataCadastro = DateTime.ParseExact(fornecedor.Substring(84, 94), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            fornecedorCarregado.Situacao = Enum.Parse<ESituacao>(fornecedor[94].ToString());

            listaFornecedores.Add(fornecedorCarregado);
        }

        return listaFornecedores;
    }

    public static Fornecedor CadastrarFornecedor(List<Fornecedor> lista, string cnpj, string razaoSocial, string abertura, bool Editando = false)
    {
        //Remoção de possíveis espaçoes e caracteres especiais para garantir padronizãção de tamanho e formato
        cnpj = Regex.Replace(cnpj, @"\.||\/||\-", "").TrimEnd().TrimStart();
        razaoSocial = razaoSocial.TrimEnd().TrimStart();
        abertura = abertura.TrimEnd().TrimStart();

        //Se já existir um fornecedor com esse CNPJ e a ação não é de edição mas criação, retorna uma exceção
        if (LocalizarFornecedor(lista, cnpj) != -1 && !Editando) throw new Exception("Já existe um fornecedor com esse CNPJ");
        //Se não existir um fornecedor com esse CNPJ e a ação for de edição, retorna uma exceção
        if (LocalizarFornecedor(lista, cnpj) == -1 && Editando) throw new Exception("Não existe um fornecedor com esse CNPJ");

        ValidarCNPJ(cnpj);
        ValidarNome(razaoSocial);
        ValidarData(abertura);

        //Remove todos os caracteres especiais para então instanciar um objeto 
        cnpj = Regex.Replace(cnpj, @"\.||\/||\-", "");

        return new Fornecedor(cnpj, razaoSocial, abertura);
    }

    public static int LocalizarFornecedor(List<Fornecedor> lista, string cnpj)
    {
        //Verifica se já há um fornecedor com o CNPJ informado
        Fornecedor? fornecedorComCNPJExistente = lista.Find(fornecedor => fornecedor.CNPJ == cnpj);

        //Se exisitr um fornecedor, retorna o índice dele na lista, se não, retorna -1
        if (fornecedorComCNPJExistente != null) return lista.IndexOf(fornecedorComCNPJExistente);
        else return -1;
    }

    public static Fornecedor EditarFornecedor(List<Fornecedor> lista, string cnpj, string novaRazaoSocial, string novaDataAbertura)
    {
        //Autoexplicativo
        return CadastrarFornecedor(lista, cnpj, novaRazaoSocial, novaDataAbertura, true);
    }

    public static List<Fornecedor> ListarFornecedoresAtivos(List<Fornecedor> lista)
    {
        List<Fornecedor> fornecedoresAtivos = new List<Fornecedor>();

        //Para cada item da lista, verifica se ele está com a situação como ativa e se sim o adiciona na lista que será retornado
        foreach (var fornecedor in lista)
        {
            if (fornecedor.Situacao == ESituacao.A) fornecedoresAtivos.Add(fornecedor);
        }

        return fornecedoresAtivos;
    }

    public static void GravarNoArquivo(List<Fornecedor> fornecedores)
    {
        //Cria um fluxo de escrita habilitado para adicionar ao invés de substituir
        var writer = new StreamWriter(CaminhoArquivo, true);
        //Limpa o arquivo
        File.WriteAllText(CaminhoArquivo, "");

        //Para cada objeto fornecedor na lista, pega suas propriedades e coloca formatado numa string que vai representar sua linha no arquivo e então adiciona no arquivo
        foreach (var fornecedor in fornecedores)
        {
            string informacoesDoFornecedor = $"{fornecedor.CNPJ}" +
                                             $"{fornecedor.RazaoSocial.PadRight(50)}" +
                                             $"{fornecedor.DataAbertura.ToString("dd/MM/yyyy")}" +
                                             $"{fornecedor.DataUltimaCompra.ToString("dd/MM/yyyy")}" +
                                             $"{fornecedor.DataCadastro.ToString("dd/MM/yyyy")}" +
                                             $"{fornecedor.Situacao}";

            writer.WriteLine(informacoesDoFornecedor);

        }

        writer.Close();
    }

    public static bool AbertaRecentemente(Fornecedor fornecedor)
    {
        //Pega a idade da empresa em meses sem considerar dias(peguei do chat)
        DateTime hoje = DateTime.Today;
        int idadeDaEmpresaEmMeses = (hoje.Year - fornecedor.DataAbertura.Year) * 12 + hoje.Month - fornecedor.DataAbertura.Month;

        //Se o dia de abertura, sem considerar o ano, for maior que o dia de hoje, subtrai 1 mês
        if (hoje.Day < fornecedor.DataAbertura.Day) idadeDaEmpresaEmMeses--;

        if (idadeDaEmpresaEmMeses < 6) return true;
        else return false;
    }

    public static bool ValidarCNPJ(string cnpj)
    {
        //Verifica se o CNPJ segue a formatação de: 2 números, um ponto opcional, 3 números, um ponto opcional, 3 números, uma barra opcioal, 4 números, um traço opcional e 2 números
        bool EhValido = Regex.IsMatch(cnpj, @"^\d{2}\.?\d{3}\.?\d{3}\/?\d{4}\-?\d{2}$");
        if (!EhValido) throw new Exception("O CNPJ não está no formato correto");

        //Pega os 12 dígitos antes do traço e verifica se eles são todos iguais
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

        //Pesquisa as fórmulas e métodos pra validar CNPJ amigão 
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

            //Verifica se os 2 últimos dígitos correspondem aos outros 12 de acordo com a fórmula
            if (digitosFinais[indiceNumerosFinais] != cnpj[16 + indiceNumerosFinais]) throw new Exception("O CPF não é válido! Os dígitos verificadores não correspondem aos 9 primeiros");

            //Adiciona o primeiro dígito(e também o segundo, mas não importa) pós traço já validado aos dígitos que serão usados para validação 
            digitosIniciais += digitosFinais[indiceNumerosFinais];

            indiceNumerosFinais++;
        }

        return true;
    }

    public override string ToString()
    {
        return $"CNPJ: {this.CNPJ}\nRazão social: {this.RazaoSocial}\nData de abertura: {this.DataAbertura}\nData do cadastro: {this.DataCadastro}\nData da última compra: {this.DataUltimaCompra}\nSituação: {this.Situacao}";
    }
}
