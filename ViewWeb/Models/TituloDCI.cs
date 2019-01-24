using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewWeb.Models
{
    public class TituloDCI
    {
        public int Id { get; set; }

        public string NossoNumero { get; set; }

        public decimal? Valor { get; set; }

        public int? ClienteId { get; set; }

        public string ContaCliente { get; set; }

        public string Canal { get; set; }

        public string Empresa { get; set; }

        public DateTime? Emissao { get; set; }

        public DateTime? Vencimento { get; set; }

        public int Processado { get; set; }

        public DateTime? DataProcessamento { get; set; }

        public string CobrancaOrigemID { get; set; }
    }
}