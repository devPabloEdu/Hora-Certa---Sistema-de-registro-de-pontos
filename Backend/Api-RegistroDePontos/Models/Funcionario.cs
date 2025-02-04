using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistroDePontosApi.Models
{
    public class Funcionario
    {
        public string Nome { get; set; }
        //A propriedade Id funciona como a chave exclusiva em um banco de dados relacional.
        public int Id { get; set; }
    }
}