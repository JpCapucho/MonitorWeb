using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ViewWeb.Business
{
    public class ExportToCSV
    {
        public void ExportCSV(string path, List<Cronn.Core.Tendencia.Model.Cobranca> cobrancas)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var item in cobrancas)
                {
                    writer.WriteLine(string.Format("NossoNumero - Valor do Título - Cliente SGV"
                        ,item.BoletoNossoNro.ToString(), item.ValorTotal.ToString(), item.Cliente.ToString()));
                }
            }
        }
    }
}