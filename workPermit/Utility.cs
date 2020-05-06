using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

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
            
            if (string.IsNullOrWhiteSpace(a))
            {
                if (string.IsNullOrWhiteSpace(b))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(b))
                {
                    return false;
                }
                else
                {
                    return string.Equals(a.Trim(), b.Trim());
                }
                
            }
        }
    }
}