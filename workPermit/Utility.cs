﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace workPermit
{
    internal class Utility
    {

        ////Get the connection string from App config file.  
        //internal static string GetConnectionString()
        //{
        //    //Util-2 Assume failure.  
        //    string returnValue = null;

        //    //Util-3 Look for the name in the connectionStrings section.  
        //    ConnectionStringSettings settings =
        //    ConfigurationManager.ConnectionStrings["workPermit.Properties.Settings.npdConnectionString"];

        //    //If found, return the connection string.  
        //    if (settings != null)
        //        returnValue = settings.ConnectionString;

        //    return returnValue;
        //}
    }

    static class Comparison
    {
        public static bool AreEqual(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
            {
                return string.IsNullOrEmpty(b);
            }
            else
            {
                return string.Equals(a, b);
            }
        }
    }
}