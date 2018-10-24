using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ViewWeb.Business
{
    public class VerificaCodigoBanco
    {
        public int CodigoBanco(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var header = reader.ReadLine();
                var CodigoBanco = Convert.ToInt32(header.Substring(76, 3));
                return CodigoBanco;
            }
        }
    }
}