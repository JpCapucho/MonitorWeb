using Cronn.Core.Tendencia.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ViewWeb.Controllers
{
    public class ArquivoRetornoController : Controller
    {
        // GET: ArquivoRetorno
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ArquivoCnab(FormCollection formCollection)
        {
            var boletos = new List<Cronn.Core.Tendencia.Model.Cobranca>();
            var boletosGenericos = new List<Models.TituloGenerico>();
            var nomeArquivo = "";

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["UploadedFile"];
                nomeArquivo = file.FileName;

                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    int banco = Convert.ToInt32(Request["banco"]);
                    var lerArquivo = new Business.CnabReader();
                    var lista = lerArquivo.LerArquivoCnab400(file.InputStream, banco);
                    boletosGenericos = lista.Item1;

                    if (lista.Item2)
                    {
                        if (banco == 1)
                        {
                            foreach (var item in lista.Item1)
                            {
                                var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero);
                                boletos.Add(boleto);
                            }
                        }
                        else
                        {
                            foreach (var item in lista.Item1)
                            {
                                var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero.Substring(0, item.NossoNumero.Length - 1));
                                boletos.Add(boleto);
                            }
                        }
                    }
                }
            }

            var PreOrPos = boletos
                .GroupBy(x => x.TipoCobranca)
                .Select(x => new Models.PreOrPos
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });

            var QtdaOcorrencias = boletosGenericos
                .GroupBy(x => x.TipoOcorrencia)
                .Select(x => new Models.TipoOcorrencia
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });

            TempData["PreOrPos"] = PreOrPos;
            TempData["QtdaOcorrencias"] = QtdaOcorrencias;
            TempData["Boletos"] = boletos;
            TempData["BoletosGenerico"] = boletosGenericos;
            TempData["nomeArquivo"] = nomeArquivo;

            return RedirectToAction("Resumo");
        }

        public ActionResult Resumo()
        {
            if (TempData["Boletos"] != null && TempData["BoletosGenerico"] != null)
            {
                var boletos = TempData["Boletos"];
                ViewBag.Boletos = boletos;

                var boletosGenerico = TempData["BoletosGenerico"];
                ViewBag.BoletosGenerico = boletosGenerico;

                var PreOrPos = TempData["PreOrPos"];
                ViewBag.PreOrPos = PreOrPos;

                var QtdaOcorrencias = TempData["QtdaOcorrencias"];
                ViewBag.QtdaOcorrencias = QtdaOcorrencias;

                var nomeArquivo = TempData["nomeArquivo"];
                ViewBag.nomeArquivo = nomeArquivo;
            }
            return View();
        }
    }
}