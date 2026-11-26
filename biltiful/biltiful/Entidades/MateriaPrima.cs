public class MateriaPrima
{

    public string Id { get; set; }
    public string Nome { get; set; }
    public DateOnly UltimaCompra { get; set; }
    public DateOnly DataCadastro { get; set; }
    public char Situacao { get; set; }


    public MateriaPrima()
    {

    }
    public MateriaPrima(string nome, string id)
    {

        Id = id;
        SetNome(nome);
        DataCadastro = DateOnly.FromDateTime(DateTime.Now);
        UltimaCompra = DataCadastro;
        Situacao = (char)ESituacao.Ativo;

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
            m.Situacao = char.Parse(linha.Substring(46, 1));

            lista.Add(m);

        }

        return lista;

    }


    public static MateriaPrima CadastrarMateriaPrima(List<MateriaPrima> materias, string nome, string id)
    {

        foreach (var materia in materias)
        {

            if (materia.Id == id)
            {
                throw new ArgumentException("Matéria ja cadastrada");
            }

        }
            var materiaPrima = new MateriaPrima(nome, id);

            File.AppendAllText(filePath, materiaPrima.ToStringFile() + Environment.NewLine);

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
            if(materia.Situacao == (char)ESituacao.Ativo)
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
            if (materia.Situacao == (char)ESituacao.Inativo)
            {
                ativas.Add(materia);
            }
        }

        return ativas;
    }


    void SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("nome nao pode ser nulo");

        if (nome.Length > 20)
        {
            nome = nome.Substring(0, 20); // subtrai os caracteres restantes no nome se ele passar de 20

            this.Nome = nome;
        }
        else
        {
            nome = nome.PadRight(20);
            this.Nome = nome;
        }

    }

    public string ToStringFile()
    {
        return $"{Nome}{Id}{DataCadastro}{UltimaCompra}{Situacao}";
    }

}