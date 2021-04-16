using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fintech
{
    public class connString
    {
        public string connstring(string connLocation)
        {

            string config = "saveaseSqlNG";

            return ConfigurationManager.ConnectionStrings[config].ConnectionString;

        }
    }
}