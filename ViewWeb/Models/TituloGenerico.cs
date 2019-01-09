using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewWeb.Models
{
    public class TituloGenerico
    {
        public long Id { get; set; }

        public string Codigo { get; set; }

        public string NossoNumero { get; set; }

        public decimal? Juros { get; set; }

        public decimal? Multa { get; set; }

        public decimal? ValorTitulo { get; set; }

        public DateTime? DataEmissao { get; set; }

        public DateTime? DataVencto { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }

        public DateTime? DataInclusao { get; set; }

        public int? TipoOcorrencia { get; set; }

        public string TipoCobranca { get; set; }

        public long? ClienteId { get; set; }

        public string DescricaoOcorrencia { get; set; }
    }
}