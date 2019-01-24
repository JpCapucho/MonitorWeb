using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasIntDAO
    {
        /// <summary>
        /// Tras um titulo que esta na Intermediaria
        /// </summary>
        /// <param name="LEDGERJOURNALTRANS_OURNUMBER"></param>
        /// <returns><see cref="TituloInt"/></returns>
        public TituloInt GetTituloByNossoNumero(string LEDGERJOURNALTRANS_OURNUMBER)
        {
            var query = @"select SEQ, LEDGERJOURNALTRANS_OURNUMBER, LEDGERJOURNALTRANS_ORIGIN, STATUS, MSG, DATAAREAID
                        from DynamicsAX_PRD_INT.dbo.COLLECTION
                        where LEDGERJOURNALTRANS_OURNUMBER = @LEDGERJOURNALTRANS_OURNUMBER";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloInt>(query, new { @LEDGERJOURNALTRANS_OURNUMBER }).FirstOrDefault();
                return cobranca;
            }
        }

        /// <summary>
        /// Tras um titulo que esta na Intermediaria
        /// </summary>
        /// <param name="SEQ"></param>
        /// <returns><see cref="TituloInt"/></returns>
        public TituloInt GetTituloBySEQ(string SEQ)
        {
            var query = @"select SEQ, LEDGERJOURNALTRANS_OURNUMBER, LEDGERJOURNALTRANS_ORIGIN, STATUS, MSG, DATAAREAID
                        from DynamicsAX_PRD_INT.dbo.COLLECTION
                        where SEQ = @SEQ";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloInt>(query, new { @SEQ }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}