public class Comprar()
{
    public static void RealizarCompra<T>(List<T> fornecedores, List<T> compras, List<T> bloqueados, List<T> materiaPrima)
    {
        try
        {
            Consultar.Imprimir(materiaPrima);
            Console.WriteLine("-----------------------------");

            Console.WriteLine("Informe CPNJ do forncedor");
            string cnpj = Console.ReadLine();

            compras.Add(Compra.CadastrarCompra(compras, cnpj, fornecedores, bloqueados));
            Console.WriteLine("Compra realizada com Sucesso!");
        } catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            RealizarCompra(fornecedores, compras, bloqueados);
        }
        
    }
}

