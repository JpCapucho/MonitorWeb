﻿using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using ViewWeb.Models;

namespace ViewWeb.DAO
{
    public class CobrancasCronnDAO
    {
        /// <summary>
        /// Tras os ultimos mil boletos
        /// </summary>
        /// <returns><see cref="List{TituloResumo}}"/></returns>
        public List<TituloResumo> GetCobrancaCronn()
        {
            var query = @"select top 1000 BoletoNossoNro, ValorTotal
                        from Cronn_PRD.Sgv.Cobranca
                        order by Id desc";

            using (var conn = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobrancas = conn.Query<TituloResumo>(query).ToList();
                return cobrancas;
            }
        }

        /// <summary>
        /// Retorna uma unica cobrança
        /// </summary>
        /// <param name="OurNumber"></param>
        /// <returns><see cref="TituloResumo"/></returns>
        public TituloResumo GetTitulo(string BoletoNossoNro)
        {
            var query = @"select BoletoNossoNro, ValorTotal
                        from Cronn_PRD.Sgv.Cobranca
                        where BoletoNossoNro = @BoletoNossoNro";

            using (var db = new SqlConnection(ConexaoDAO.GetConexao()))
            {
                var cobranca = db.Query<TituloResumo>(query, new { @BoletoNossoNro }).FirstOrDefault();
                return cobranca;
            }
        }
    }
}