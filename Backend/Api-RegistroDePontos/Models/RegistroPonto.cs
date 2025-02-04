using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistroDePontosApi.Models
{
    public class RegistroPonto
    {
        public int Id { get; set; }
        public int FuncionarioId { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? PontoDeEntrada {get; set;}

        public DateTime? PontoDeAlmoço {get; set;}

        public DateTime? PontoDeVoltaAlmoço {get; set;}

        public DateTime? PontoDeSaída {get; set;}
    }
}