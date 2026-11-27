public class Comprar()
{
    public static void RealizarCompra(List<Fornecedor> fornecedores, List<Compra> compras, List<string> bloqueados, List<MateriaPrima> materiaPrima)
    {
        try
        {
            Consultar.Imprimir(materiaPrima);
            Console.WriteLine("-----------------------------");

            Console.WriteLine("Informe CPNJ do forncedor");
            string cnpj = Console.ReadLine();

            //aqui tenho que perguntar o id da materia que vai ser comprada
            Console.WriteLine("Informe o id da matéria-prima que deseja comprar");
            string idMateria = Console.ReadLine();
            MateriaPrima.LocalizarMateria(materiaPrima, idMateria); //esta localizando a matéria prima
            

            

            compras.Add(Compra.CadastrarCompra(compras, cnpj, fornecedores, bloqueados));
        } catch(Exception e)
        {
            Console.WriteLine("Erro: " + e.Message);
            RealizarCompra(fornecedores, compras, bloqueados, materiaPrima);
        }
        
            Console.WriteLine("Compra realizada com Sucesso!");
    }
}

