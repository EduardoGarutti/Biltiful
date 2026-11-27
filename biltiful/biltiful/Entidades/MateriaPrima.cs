public class MateriaPrima
{

    public string Id { get; set; }
    public string Nome { get; set; }
    public DateOnly UltimaCompra { get; set; }
    public DateOnly DataCadastro { get; set; }
    public ESituacao Situacao { get; set; }

    
    public MateriaPrima()
    {

    }
    public MateriaPrima(string nome)
    {
        do
        {
            Id = "MP" + Random.Shared.Next(1000, 9999);
        } while (LocalizarMateria(CarregarMateriaPrima(), Id) > 0);
        
        SetNome(nome);
        DataCadastro = DateOnly.FromDateTime(DateTime.Now);
        UltimaCompra = DataCadastro;
        Situacao = ESituacao.A;

    }


    public static readonly string filePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "dats", "materiaPrima.dat");


    public static List<MateriaPrima> CarregarMateriaPrima() //retorna todos as materias primas, ativas e inativas
    {

        List<MateriaPrima> lista = new List<MateriaPrima>();
        if (!File.Exists(filePath))
            return lista;

        foreach (string linha in File.ReadAllLines(filePath))
        {

            MateriaPrima m = new MateriaPrima();

            m.Nome = linha.Substring(0, 20);
            m.Id = linha.Substring(20, 6);
            m.DataCadastro = DateOnly.Parse(linha.Substring(26, 10));
            m.UltimaCompra = DateOnly.Parse(linha.Substring(36, 10));
            m.Situacao = Enum.Parse<ESituacao>(linha.Substring(46, 1));

            lista.Add(m);

        }

        return lista;

    }


    public static MateriaPrima CadastrarMateriaPrima(List<MateriaPrima> materias, string nome)
    {

        var materiaPrima = new MateriaPrima();

        materiaPrima = new MateriaPrima(nome);
        
            //File.AppendAllText(filePath, materiaPrima.ToStringFile() + Environment.NewLine);
            //estava lendo o arquivo e salvando o novo elemento no final dele, mas alterei
            //para o padrão do projeto de salvar em uma lista 


            return materiaPrima;

    }



    public static int LocalizarMateria(List<MateriaPrima> materias, string id)
    {
        foreach (var materia in materias)
        {
            if(materia.Id == id)
            {
                return materias.IndexOf(materia);
            }
        }

        return -1;
    }


    public static List<MateriaPrima> ListarMateriasPrimasAtivas(List<MateriaPrima> materias)
    {
        var ativas = new List<MateriaPrima>();

        foreach(var materia in materias)
        {
            if(materia.Situacao == ESituacao.A)
            {
                ativas.Add(materia);
            }
        }

        return ativas;
    }


    public static List<MateriaPrima> ListarMateriasPrimasInativas(List<MateriaPrima> materias)
    {
        var ativas = new List<MateriaPrima>();

        foreach (var materia in materias)
        {
            if (materia.Situacao == ESituacao.I)
            {
                ativas.Add(materia);
            }
        }

        return ativas;
    }


    public void AlterarSituacao(string id)
    {
        var lista = CarregarMateriaPrima();

        foreach (var item in lista)
        {
            if (item.Id == id)
            {

                if (Situacao == ESituacao.A)
                {
                    Situacao = ESituacao.I;
                }
                else
                {
                    Situacao = ESituacao.A;
                }
            }
        }


    }

    void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("nome nao pode ser nulo");

        if (nome.Length > 20)
        {
            throw new Exception("nome não pode ultrapassar 20 caracteres");

        }

        Nome = nome;

    }

    public static void GravarNoArquivo(List<MateriaPrima> materias)
    {

        var writer = new StreamWriter(filePath, true);
        File.WriteAllText(filePath, "");
        foreach(var materia in materias)
        {
            string informacoes = materia.ToFile();
            writer.WriteLine(informacoes);
        }

        writer.Close();

    }

    public string ToFile()
    {
        return $"{Nome.PadRight(20)}{Id}{DataCadastro}{UltimaCompra}{Situacao}";
    }




    public override string ToString()
    {
        return $"""
            Nome Matéria-Prima: {Nome}
            Id: {Id}
            Data de Cadastro: {DataCadastro}
            Data da última Compra: {UltimaCompra}
            Situação: {Situacao}
            """;
    }

}