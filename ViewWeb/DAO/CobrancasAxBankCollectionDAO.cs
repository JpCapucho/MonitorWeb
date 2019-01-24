using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasAxBankCollectionDAO
    {
        /// <summary>
        /// Tras um titulo do Ax BankCollectionTrans
        /// </summary>
        /// <param name="OURNUMBER"></param>
        /// <returns><see cref="TituloAxBankCollection"/></returns>
        public TituloAxBankCollection GetTituloByNossoNumero(string OURNUMBER)
        {
            var query = @"select OURNUMBER, ACCOUNTNUM, DATAAREAID
                            from DynamicsAX_PRD.dbo.BANKCOLLECTIONTRANS
                            where JOURNALTYPE = 7 and OURNUMBER = @OURNUMBER";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloAxBankCollection>(query, new { @OURNUMBER }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}