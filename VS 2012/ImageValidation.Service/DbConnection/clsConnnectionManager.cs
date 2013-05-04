using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Net.Mail;
using System.ComponentModel;
using System.Text;


public static class clsConnnectionManager
{
  
   public static SqlConnection SQlConnection()
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SQLAzureConn"].ToString());
        if (con.State == ConnectionState.Open)
        {
            con.Close();
        }
        else
        {
            con.Open();
        }
        return con;
    }

   public static SqlConnection SQlConnectionJobSenex()
   {
       SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["SQLAzureConn"].ToString());
       if (con.State == ConnectionState.Open)
       {
           con.Close();
       }
       else
       {
           con.Open();
       }
       return con;
   }
}
