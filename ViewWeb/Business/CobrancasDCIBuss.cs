using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewWeb.DAO;
using ViewWeb.Models;

namespace ViewWeb.Business
{
    public class CobrancasDCIBuss
    {
        CobrancasDciDAO db = new CobrancasDciDAO();

        public List<TituloDCI> GetTitulos()
        {
            var titulos = new List<TituloDCI>();

            var titulosDAO = db.GetTitulosDci();

            foreach (var item in titulosDAO.ToList())
            {
                var titulo = new TituloDCI();
                titulo = item;

                titulos.Add(titulo);
            }
            return titulos;
        }
    }
}