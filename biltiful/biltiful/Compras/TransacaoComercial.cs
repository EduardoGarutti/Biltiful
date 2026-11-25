public abstract class TransacaoComercial
{
    public int Id { get; protected set; }
    public DateOnly Data { get; protected set; }
    public double ValorTotal { get; protected set; }

    protected TransacaoComercial(int id, DateOnly data)
    {
        if (!ValidarId(id))
        {
            throw new ArgumentException("Id inválido.");
        }

        Id = id;
        Data = data;
    }

    private bool ValidarId(int id)
    {
        return id >= 1 && id <= 99999;
    }
}