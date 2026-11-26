public abstract class TransacaoComercial
{
    public int Id { get; protected set; }
    public DateTime Data { get; protected set; }
    public double ValorTotal { get; protected set; }

    protected TransacaoComercial(int id, DateTime data)
    {
        Id = id;
        Data = data;
        ValorTotal = 0;
    }
}