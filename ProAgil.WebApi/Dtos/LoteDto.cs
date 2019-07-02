using System;

namespace ProAgil.WebApi.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public decimal Preco { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public int Quantidade { get; set; }
    }
}