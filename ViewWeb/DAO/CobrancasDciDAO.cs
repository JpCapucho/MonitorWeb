using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasDciDAO
    {
        /// <summary>
        /// Tras titulos dos ultimos 5 dias
        /// </summary>
        /// <returns><see cref="List{TituloDCI}<"/></returns>
        public List<TituloDCI> GetTitulosDci()
        {
            var query = @"select NossoNumero, Valor, ClienteID, ContaCliente, Canal, Empresa, Emissao, Vencimento, Processado, DataProcessamento, CobrancaOrigemID
                    from DCI_Web.dbo.DciBoletos
                    where convert(date, Emissao, 103) between DATEADD(DAY, -5, convert(date, GETDATE(), 103)) and convert(date, GETDATE(), 103)
                    order by ID desc";      

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobrancas = db.Query<TituloDCI>(query).ToList();
                return cobrancas;
            }
        }

        /// <summary>
        /// Tras um titulo
        /// </summary>
        /// <param name="OurNumber"></param>
        /// <returns><see cref="TituloDCI"/></returns>
        public TituloDCI GetTitulo(string NossoNumero)
        {
            var query = @"select Id, NossoNumero, Valor, ClienteID, ContaCliente, Canal, Empresa, Emissao, Vencimento, Processado, DataProcessamento, CobrancaOrigemID
                    from DCI_Web.dbo.DciBoletos
                    where NossoNumero = @NossoNumero";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloDCI>(query, new { @NossoNumero }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}