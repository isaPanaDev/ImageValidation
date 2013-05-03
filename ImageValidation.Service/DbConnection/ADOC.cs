using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net;
using System.Collections;
using System.Net.Mail;
using System.Text.RegularExpressions;


public class ADOC
{
    //private var
    private SqlConnection con;
    private SqlCommand cmd;
    private static ADOC oADO;
    private DataTable dt;
    private DataSet ds;
    public static ADOC GetObject
    {
        get
        {

            if (oADO == null)
                oADO = new ADOC();
            return oADO;
        }
    }
    //C'tor
    private ADOC()
    {
        con = new SqlConnection();
        // Connection String   ->
        string constr;
        //constr=new SqlConnection(ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);
        constr = ConfigurationSettings.AppSettings["SQLAzureConn"].ToString();
        con.ConnectionString = constr;
        cmd = new SqlCommand();
        cmd.Connection = con;
    }
    //Open a connection
  public  bool OpenCon()
    {
        try
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            return true;
        }
        catch
        {
            return false;

        }
       
    }
    //Close the open connection
  public bool CloseCon()
    {
        try
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            this.con.Close();
        }
    }
  
    //Public Functions
    //------BOOLIAN RETURN TYPE---------------------------------------------------------------------------------------
    public bool ExecuteDML(string sql) //Used for Insert,Delete and Modify records
    {
        if (!OpenCon()) return false;
        //sql = convertQuote(sql);
        cmd.CommandText = sql;
        try
        {
            int nor = cmd.ExecuteNonQuery();
            if (nor <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        catch
        {
           return false;
        }
        finally
        {
            this.con.Close();
        }
    }
    //--------STRING RETURN TYPE--------------------------------------------------------------------------------------
    public string GetSingleResult(string sql) //Get single record
    {
        if (!OpenCon()) return "false";
        cmd.CommandText = sql;
        try
        {
            object data = cmd.ExecuteScalar();
            return data.ToString();
        }
        catch
        {
            return "0";//"Not Found"
        }
        finally
        {
            this.con.Close();
        }
    }
    //============================  Add some methods by AMAN =========================

    
   
    //*****************************************************************
  /// <summary>
  /// 
  /// </summary>
  /// <param name="To"></param>
  /// <param name="subject"></param>
  /// <param name="MailBody"></param>
  /// <returns></returns>
  /// 

    public static bool SendEmail(string To, string subject, string MailBody)
    {
        bool isMailSent = false;
        MailMessage message = new MailMessage("info@romiotech.com", To);
        message.Body = MailBody;
        message.Subject = subject;
        message.IsBodyHtml = true;
        SmtpClient emailClient = new SmtpClient();
        emailClient.Host = "mail.romiotech.com";
        emailClient.Credentials = new NetworkCredential("info@romiotech.com", "Hello32");
        emailClient.UseDefaultCredentials = true;
        emailClient.EnableSsl = false;
        emailClient.Send(message);
        isMailSent = false;
        return isMailSent;
    }
    public static bool IsValidEmailAddress(string InputEmailAddress)
    {
        return Regex.IsMatch(InputEmailAddress, @"[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z_+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9}$");
    }
    public static DataTable RemoveDuplicate(DataTable dTable, string colName)
    {
        Hashtable hTable = new Hashtable();
        ArrayList duplicateList = new ArrayList();

        foreach (DataRow drow in dTable.Rows)
        {
            if (hTable.Contains(drow[colName]))
            {
                duplicateList.Add(drow);
            }
            else
            {
                hTable.Add(drow[colName], string.Empty);
            }
        }

        foreach (DataRow dRow in duplicateList)
            dTable.Rows.Remove(dRow);

        return dTable;
    }
    public static bool IsConnectedToInternet()
    {
        bool isConnected = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        if (isConnected == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
   

   

    public object GetSingleResultObj(string sql) //Get single record
    {
        if (!OpenCon()) return "false";
        cmd.CommandText = sql;
        try
        {
            object data = cmd.ExecuteScalar();
            return data;
        }
        catch
        {
            return "0";//"Not Found"
        }
        finally
        {
            this.con.Close();
        }
    }
    //-------SQLDATAREADER RETUNN TYPE--------------------------------------------------------------------------------
    public SqlDataReader GetDataByReader(string qry)//Get multipal records simple
    {
        if (!OpenCon()) return null;
        cmd.CommandText = qry;
        cmd.CommandType = CommandType.Text;
        SqlDataReader dr = null;
        try
        {
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return dr;
        }
        catch
        {
            return null;
        }
        finally
        {
            //cmd.Parameters.Clear();
            //dr.Close();
        }
    }

    public SqlDataReader GetDataByReader(string qry, SqlParameter[] arrparam)//Get multipal records 
    {
        if (!OpenCon()) return null;
        cmd.CommandText = qry;
        cmd.CommandType = CommandType.Text;
        foreach (SqlParameter param in arrparam)
            cmd.Parameters.Add(param);
        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return dr;

        }
        catch
        {

            return null;
        }
       
    }

    public SqlDataReader GetDataByReader(string qry, CommandType cmdtype, SqlParameter[] arrparam)//Get multipal records
    {
        if (!OpenCon()) return null;
        cmd.CommandText = qry;
        cmd.CommandType = cmdtype;
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter param in arrparam)
            cmd.Parameters.Add(param);
        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return dr;
        }
        catch
        {
            return null;
        }

    }

    //made by ...........
    public DataTable GetTable(string query)
    {
        if (!OpenCon()) return null;
        dt = new DataTable();
        try
        {
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            return dt;
        }
        catch
        {
            return null;
        }
        finally
        {
           this.con.Close();
        }
    }

    public DataSet GetDatset(string qry, string tbl)
    {
        if (!OpenCon()) return null;
        ds = new DataSet();        
        try
        {
            SqlDataAdapter da = new SqlDataAdapter(qry, con);
            da.Fill(ds, tbl);
            return ds;           
        }
        catch
        {
            return null;
        }
        finally
        {
            this.con.Close();
        }
    }

    public string GetConnectionState()
    {
        return con.State.ToString();
    }
    //public object GetDatset(object p)
    //{
    //    throw new Exception("The method or operation is not implemented.");
    //}





}