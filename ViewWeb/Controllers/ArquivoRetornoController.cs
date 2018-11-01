using Cronn.Core.Tendencia.Business;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
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
            //lista para armazenar os titulos que do Cronn
            var boletos = new List<Cronn.Core.Tendencia.Model.Cobranca>();
            //lista para armazenar os titulos genericos
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
                        WindowsIdentity identity = WindowsIdentity.GetCurrent();
                        if (banco == 1)
                        {
                            Parallel.ForEach(lista.Item1, item =>
                            {
                                using (WindowsImpersonationContext impersonationContext = identity.Impersonate())
                                {
                                    var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero);
                                    boletos.Add(boleto);
                                }
                            });

                            //foreach (var item in lista.Item1)
                            //{
                            //    var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero);
                            //    boletos.Add(boleto);
                            //}
                        }
                        else
                        {
                            Parallel.ForEach(lista.Item1, item =>
                            {
                                using (WindowsImpersonationContext impersonationContext = identity.Impersonate())
                                {
                                    var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero.Substring(0, item.NossoNumero.Length - 1));
                                    boletos.Add(boleto);
                                }
                            });

                            //foreach (var item in lista.Item1)
                            //{
                            //    var boleto = new CobrancaBusiness().GetByNossoNumero(item.NossoNumero.Substring(0, item.NossoNumero.Length - 1));
                            //    boletos.Add(boleto);
                            //}
                        }
                    }
                }
            }

            //Pega a quantidade de boletos Pre e Pos pagos
            var PreOrPos = boletos
                .GroupBy(x => x.TipoCobranca)
                .Select(x => new Models.PreOrPos
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });

            //Pega a quantidade de ocorrencias por tipo
            var QtdaOcorrencias = boletosGenericos
                .GroupBy(x => x.TipoOcorrencia)
                .Select(x => new Models.TipoOcorrencia
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });

            TempData["PreOrPos"] = PreOrPos;
            TempData["QtdaOcorrencias"] = QtdaOcorrencias;
            TempData["Boletos"] = boletos.OrderBy(b => b.BoletoNossoNro);
            TempData["BoletosGenerico"] = boletosGenericos.OrderBy(b => b.NossoNumero);
            TempData["nomeArquivo"] = nomeArquivo;

            return RedirectToAction("Resumo");
        }

        /// <summary>
        /// Pegas as informaçoes da Action ArquivoCnab para enviar o resultado da leitura para a View Resumo
        /// </summary>
        /// <returns><see cref="Resumo"/></returns>
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