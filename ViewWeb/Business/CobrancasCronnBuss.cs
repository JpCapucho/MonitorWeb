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
        CobrancasCronnDAO db = new CobrancasCronnDAO();

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
    }
}