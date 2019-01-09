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

        public ActionResult GridBoletos()
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
            var cobrancas = new List<Models.TituloGenerico>();
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
                    var titulos = lista.Item1.Select(x => x.NossoNumero).ToList();
                    var distinctTitulos = titulos.Distinct().ToList();

                    if (lista.Item2)
                    {
                        WindowsIdentity identity = WindowsIdentity.GetCurrent();
                        if (banco == 1)
                        {
                            Parallel.ForEach(distinctTitulos, item =>
                            {
                                using (WindowsImpersonationContext impersonationContext = identity.Impersonate())
                                {
                                    var boleto = new CobrancaBusiness().GetByNossoNumero(item);
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
                            Parallel.ForEach(distinctTitulos, item =>
                            {
                                using (WindowsImpersonationContext impersonationContext = identity.Impersonate())
                                {
                                    var boleto = new CobrancaBusiness().GetByNossoNumero(item.Substring(0, item.Length - 1));
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


                    foreach (var item in boletosGenericos)
                    {
                        var cobranca = new Models.TituloGenerico();
                        cobranca = item;

                        if (banco == 1)
                        {
                            var cliente = boletos
                                .Where(x => x != null && x.BoletoNossoNro == item.NossoNumero)
                                //.Select(b => b.Cliente.Id)
                                .Select(x => new { x.Cliente.Id, x.TipoCobranca})
                                .ToList();

                            foreach (var res in cliente)
                            {
                                cobranca.ClienteId = res.Id;
                                cobranca.TipoCobranca = string.IsNullOrEmpty(res.TipoCobranca.ToString()) ? "PosPago"
                                    : res.TipoCobranca == 0 ? "PosPago" : "PrePago";
                            }

                        }
                        else
                        {
                            var cliente = boletos
                                .Where(x => x != null &&  x.BoletoNossoNro == item.NossoNumero.Substring(0, item.NossoNumero.Length - 1))
                                //.Select(b => b.Cliente.Id)
                                .Select(x => new { x.Cliente.Id, x.TipoCobranca })
                                .ToList();

                            foreach (var res in cliente)
                            {
                                cobranca.ClienteId = res.Id;
                                cobranca.TipoCobranca = string.IsNullOrEmpty(res.TipoCobranca.ToString()) ? "PosPago"
                                    : res.TipoCobranca == 0 ? "PosPago" : "PrePago";
                            }
                        }

                        cobrancas.Add(cobranca);
                    }
                }
            }





            //Pega a quantidade de boletos Pre e Pos pagos
            var PreOrPos = boletos
                .Where(x => x != null)
                .GroupBy(x => x.TipoCobranca)
                .Select(x => new Models.PreOrPos
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });


            //Pega a quantidade de ocorrencias por tipo
            var QtdaOcorrencias = boletosGenericos
                .Where(x => x.TipoOcorrencia != null)
                .GroupBy(x => x.TipoOcorrencia)
                .Select(x => new Models.TipoOcorrencia
                {
                    Chave = x.Key.ToString(),
                    Quantidade = x.Count()
                });

            TempData["PreOrPos"] = PreOrPos;
            TempData["QtdaOcorrencias"] = QtdaOcorrencias;
            TempData["Boletos"] = boletos.Where(b => b != null).OrderBy(b => b.BoletoNossoNro);
            TempData["BoletosGenerico"] = boletosGenericos.OrderBy(b => b.NossoNumero);
            TempData["nomeArquivo"] = nomeArquivo;
            TempData["Cobrancas"] = cobrancas.Where(x => x != null).OrderBy(x => x.NossoNumero);

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

                var cobrancas = TempData["Cobrancas"];
                ViewBag.Cobrancas = cobrancas;
            }
            return View();
        }
    }
}