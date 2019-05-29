using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using ViewWeb.Business;

namespace ViewWeb.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BoletosDciJson()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            using (WindowsImpersonationContext impersonationContext = identity.Impersonate())
            {
                CobrancasDCIBuss db = new CobrancasDCIBuss();
                var result = db.GetTitulos();

                int rv = result.Where(x => x.Canal == "RV").Count();
                int tendencia = result.Where(x => x.Canal == "TENDENCIA").Count();
                decimal valorTotal = Convert.ToDecimal(result.Sum(x => x.Valor));
                decimal valorRv = Convert.ToDecimal(result.Where(x => x.Canal == "RV").Sum(x => x.Valor));
                decimal valorTendencia = Convert.ToDecimal(result.Where(x => x.Canal == "TENDENCIA").Sum(x => x.Valor));

                var obj = new Tittle();
                obj.RV = rv;
                obj.TENDENCIA = tendencia;
                obj.ValorTotal = valorTotal.ToString("C", CultureInfo.GetCultureInfo("pt-BR"));
                obj.ValorRV = valorRv;
                obj.ValorTENDENCIA = valorTendencia;

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

        private class Tittle
        {
            public int RV { get; set; }

            public int TENDENCIA { get; set; }

            public string ValorTotal { get; set; }

            public decimal ValorRV { get; set; }

            public decimal ValorTENDENCIA { get; set; }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}