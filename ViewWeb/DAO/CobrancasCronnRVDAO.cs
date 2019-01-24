using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasCronnRVDAO
    {
        public TituloCronnRV GetTituloRV(string NossoNumero)
        {
            var query = @"select SUBSTRING(t.NossoNumero, 1, 11) [NossoNumero], c.IdIntegracao [ClienteId]
                            from Cronn_PRD.RV.Titulo t
                            inner join Cronn_PRD.RV.Cliente c on (t.IdCliente = c.Id)
                            where t.NossoNumero <> 'ARRUMAR' and c.IdIntegracao is not null and SUBSTRING(t.NossoNumero, 1, 11) = @NossoNumero";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloCronnRV>(query, new { @NossoNumero }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}