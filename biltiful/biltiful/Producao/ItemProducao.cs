public class ItemProducao
{
    public static string CaminhoArquivo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Dats", "ItemProduto.dat"));
    public static int Contador = 1;
    public int Id { get; private set; } //99.999 5 dig
    public DateOnly DataProducao { get; private set; } //dd/mm/yyyy
    public string MateriaPrima { get; private set; }
    public int QuantidadeMateriaPrima { get; private set; }// 999,99


}

