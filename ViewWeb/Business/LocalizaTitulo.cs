using Cronn.Core.Tendencia.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ViewWeb.DAO;
using ViewWeb.Models;

namespace ViewWeb.Business
{
    public class LocalizaTitulo
    {
        /// <summary>
        /// Pega o nosso numero e verifica onde se encontra
        /// </summary>
        /// <param name="OurNumber"></param>
        /// <returns><see cref="ResumTittle"/></returns>
        public ResumTittle SearchTitulo(string OurNumber)
        {
            var returnTittle = new ResumTittle();
            if (OurNumber != null)
            {
                var boleto = new Cronn.Core.Tendencia.Model.Cobranca();
                var dciDAO = new CobrancasDciDAO();
                var dci = new TituloDCI();
                var intTitulo = new CobrancasIntDAO();
                var bankDAO = new CobrancasAxBankCollectionDAO();
                var inter = new TituloInt();
                var bank = new TituloAxBankCollection();
                var rv = new TituloCronnRV();
                var rvDAO = new CobrancasCronnRVDAO();
                var res = new TituloIntRes();
                var resDAO = new CobrancasIntResDAO();


                //Verifica o tamanho da string
                if (OurNumber.Length == 12 || OurNumber.Length == 9)
                {
                    var cobranca = new CobrancaBusiness().GetByNossoNumero(OurNumber.Substring(0, OurNumber.Length - 1));
                    if (cobranca == null)
                    {
                        rv = rvDAO.GetTituloRV(OurNumber.Substring(0, OurNumber.Length - 1));
                        dci = dciDAO.GetTitulo(OurNumber.Substring(0, OurNumber.Length - 1));
                    }
                    else
                    {
                        boleto = cobranca;
                        dci = dciDAO.GetTitulo(OurNumber.Substring(0, OurNumber.Length - 1));
                    }
                }
                else
                {
                    var cobranca = new CobrancaBusiness().GetByNossoNumero(OurNumber);
                    if (cobranca == null)
                    {
                        rv = rvDAO.GetTituloRV(OurNumber);
                        dci = dciDAO.GetTitulo(OurNumber);
                    }
                    else
                    {
                        boleto = cobranca;
                        dci = dciDAO.GetTitulo(OurNumber);
                    }
                }

                if (dci != null)
                {
                    if (dci.Id.ToString() != null)
                    {
                        inter = intTitulo.GetTituloBySEQ(dci.Id.ToString());
                        if (inter != null)
                        {
                            bank = bankDAO.GetTituloByNossoNumero(inter.LEDGERJOURNALTRANS_OURNUMBER);
                        }
                        else
                        {
                            res = resDAO.GetTituloBySEQ(dci.Id.ToString());
                            if (res != null)
                            {
                                bank = bankDAO.GetTituloByNossoNumero(res.OURNUMBER);
                            }
                        }

                    }
                }

                returnTittle.Sgv = boleto == null ? false : string.IsNullOrEmpty(boleto.BoletoNossoNro) ? false : true;
                returnTittle.PreOrPos = string.IsNullOrEmpty(boleto.TipoCobranca.ToString()) ? "PosPago" : boleto.TipoCobranca == 0 ? "PosPago" : "PrePago";
                returnTittle.Rv = rv == null ? false : string.IsNullOrEmpty(rv.NossoNumero) ? false : true;
                returnTittle.Dci = dci == null ? false : string.IsNullOrEmpty(dci.NossoNumero) ? false : true;
                returnTittle.Int = inter == null ? false : string.IsNullOrEmpty(inter.LEDGERJOURNALTRANS_OURNUMBER) ? false : true;
                returnTittle.IntRes = res == null ? false : string.IsNullOrEmpty(res.OURNUMBER) ? false : true;
                returnTittle.Ax = bank == null ? false : string.IsNullOrEmpty(bank.OURNUMBER) ? false : true;
            }

            return returnTittle;
        }
    }
}