using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ViewWeb.DAO
{
    public class ConexaoDAO
    {
        public static string GetConexao()
        {
            return ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
        }
    }
}