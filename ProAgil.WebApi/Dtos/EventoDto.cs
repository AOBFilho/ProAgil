using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebApi.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage="O campo {0} é de preenchimento obrigatório")]
        [StringLength(500,MinimumLength=2,ErrorMessage="O campo {0} deve possuir no mínimo 2 caractérie e no máximo 500 caractéries")]
        public string Local { get; set; }
        public DateTime DataEvento { get; set; }
        
        [Required(ErrorMessage="O campo {0} é obrigatório")]
        public string Tema { get; set; }

        [Range(2,120000,ErrorMessage="O campo {0} de possuir no minimo 2 pessoas e no máximo 120000 pessoas")]
        public int QtdePessoas { get; set; }
        public string ImagemURL { get; set; }
        
        [Phone(ErrorMessage="O campo {0} está inválido")]
        public string Telefone { get; set; }
        
        [EmailAddress(ErrorMessage="O campo {0} está inválido")]
        public string Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}