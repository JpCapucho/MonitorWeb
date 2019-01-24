using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasIntResDAO
    {
        public TituloIntRes GetTituloBySEQ(string SEQ)
        {
            var query = @"select SEQ, OURNUMBER, TITLENEWCOLLECTIONID, STATUS, MSG, DATAAREAID
                        from DynamicsAX_PRD_INT.dbo.COLLECTIONREISSUE
                        where SEQ = @SEQ";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloIntRes>(query, new { @SEQ }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}