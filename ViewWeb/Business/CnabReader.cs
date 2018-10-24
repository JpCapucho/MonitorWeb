using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ViewWeb.Models;
using BoletoNet;

namespace ViewWeb.Business
{
    public class CnabReader
    {
        /// <summary>
        /// Ler o Arquivo de CNAB400 e retorna uma lista de titulos genericos e um bool
        /// </summary>
        /// <param name="arquivo"></param>
        /// <param name="CodigoBanco"></param>
        /// <returns><see cref="List{TituloGenerico}, bool"/></returns>
        public (List<TituloGenerico>, bool) LerArquivoCnab400(Stream arquivo, int CodigoBanco)
        {
            var Titulos = new List<TituloGenerico>();
            int BankCode;

            try
            {
                var banco = new Banco(CodigoBanco);
                var readerCnab = new ArquivoRetornoCNAB400();
                readerCnab.LerArquivoRetorno(banco, arquivo);
                if (CodigoBanco == 1)
                {
                    BankCode = Convert.ToInt32(readerCnab.HeaderRetorno.ComplementoRegistro2);
                }
                else if (CodigoBanco == 237)
                {
                    BankCode = Convert.ToInt32(readerCnab.HeaderRetorno.CodigoEmpresa);
                }
                else
                {
                    BankCode = readerCnab.HeaderRetorno.Conta;
                }

                foreach (var item in readerCnab.ListaDetalhe)
                {
                    var titulo = new TituloGenerico();
                    titulo.Codigo = item.NumeroDocumento;
                    titulo.NossoNumero = item.NossoNumeroComDV;
                    titulo.Juros = item.Juros;
                    titulo.Multa = item.ValorMulta;
                    titulo.ValorTitulo = item.ValorTitulo;
                    titulo.DataEmissao = item.DataOcorrencia;
                    titulo.DataVencto = item.DataVencimento;
                    titulo.Banco = string.Format(item.CodigoBanco.ToString() + "-" + item.CodigoInscricao);
                    titulo.Agencia = item.Agencia.ToString();
                    titulo.Conta = string.Format(item.Conta.ToString() + "-" + item.DACConta);
                    titulo.DataInclusao = DateTime.Now;
                    titulo.TipoOcorrencia = item.CodigoOcorrencia;
                    titulo.DescricaoOcorrencia = item.DescricaoOcorrencia;

                    Titulos.Add(titulo);
                }
            }
            catch (Exception)
            {

                throw;
            }

            return (Titulos, SgvOrRv(BankCode));
        }

        /// <summary>
        /// Verifica se a empresa das vendas é SGV ou RV
        /// </summary>
        /// <param name="CodigoBanco"></param>
        /// <returns></returns>
        public bool SgvOrRv(int CodigoBanco)
        {
            switch (CodigoBanco)
            {
                case 7804729:
                    return true;
                case 7798594:
                    return true;
                case 7798599:
                    return true;
                case 7798542:
                    return true;
                case 73000000:
                    return true;
                    //Itaú não tem o Código da Empresa, para validar está sendo usado a Conta
                case 31588:
                    return true;

                default:
                    return false;
            }
        }
    }
}