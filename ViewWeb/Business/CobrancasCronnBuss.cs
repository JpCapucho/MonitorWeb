using Cronn.Core.Tendencia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewWeb.DAO;
using ViewWeb.Models;

namespace ViewWeb.Business
{
    public class CobrancasCronnBuss
    {
        private CobrancasCronnDAO db = new CobrancasCronnDAO();

        /// <summary>
        /// Pega os titulos que estão no Cronn
        /// </summary>
        /// <returns><see cref="List{TituloResumo}<"/></returns>
        public List<TituloResumo> GetTitulos()
        {
            var titulos = new List<TituloResumo>();

            var titulosDAO = db.GetCobrancaCronn();

            foreach (var item in titulosDAO.ToList())
            {
                var titulo = new TituloResumo();
                titulo = item;

                titulos.Add(titulo);
            }
            return titulos;
        }

        /// <summary>
        /// Pega os titulos que estão no Cronn
        /// </summary>
        /// <param name="BoletoNossoNro"></param>
        /// <returns><see cref="List{Cobranca}"/></returns>
        public List<Cobranca> GetCobrancas(List<string> BoletoNossoNro)
        {
            var index = 0;
            //var lastIndex = BoletoNossoNro.Count() > 2000 ? (BoletoNossoNro.Count() - 2000) : BoletoNossoNro.Count();
            var cont = (BoletoNossoNro.Count() / 2000) + 1;
            var contador = BoletoNossoNro.Count() / cont;
            var cobrancas = new List<Cobranca>();

            for (int i = 0; i <= cont; i++)
            {
                contador = i == cont ? (BoletoNossoNro.Count() - index) : contador;
                var result = BoletoNossoNro.GetRange(index, contador);

                var res = db.GetCobrancas(result);
                foreach (var item in res)
                {
                    cobrancas.Add(item);
                }

                index += result.Count();
            }

            if (cobrancas.Count() != BoletoNossoNro.Count())
            {
                var compare = BoletoNossoNro.Except(cobrancas.Select(x => x.BoletoNossoNro)).ToList();

                foreach (var item in compare)
                {
                    var cob = new Cronn.Core.Tendencia.Business.CobrancaBusiness().GetByNossoNumero(item);
                    if (cob != null)
                    {
                        cobrancas.Add(cob);
                    }
                }

                return cobrancas;
            }
            else
            {
                return cobrancas;
            }
        }
    }
}