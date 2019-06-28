namespace ProAgil.WebApi.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public decimal Preco { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public int Quantidade { get; set; }
    }
}