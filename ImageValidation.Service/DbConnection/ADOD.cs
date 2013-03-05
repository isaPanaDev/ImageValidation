using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

public class ADOD
{
    SqlConnection con;
    SqlCommand cmd;
    SqlDataAdapter da;
    DataTable dt;
    private static ADOD oADO;
    public static string grp = "server=216.205.99.29;database=sigma_infotech_ClientMT;integrated security=true;user id=sigma123;password=sipl;";
  
    public static ADOD GetObject
    {
        get
        {
            if (oADO == null)
                oADO = new ADOD();
            return oADO;

        }
    }
	private ADOD()
	{
        con = new SqlConnection();
        string constr = grp;
        //ConfigurationSettings.AppSettings["keysql"];
        con.ConnectionString = constr;
        
        cmd = new SqlCommand();
        cmd.Connection = con;
        da = new SqlDataAdapter(cmd);
        SqlCommandBuilder cb = new SqlCommandBuilder(da);
	}
    public DataTable GetDataByDataTable(string sql)
    {
        cmd.CommandText = sql;
        dt = new DataTable();
        try
        {
            da.Fill(dt);
            return (dt);
        }
        catch
        {
            return null;
        }        
    }
    public int SearchById(ref DataTable dt, object ToysId)
    {
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i][0].ToString().Trim() == ToysId.ToString())
            {
                return i;
            }
        }
        return -1;
    }
    public bool DeleteRecFromTable(ref DataTable dt, object ToysId)
    {
        try
        {
            int rno = SearchById(ref dt, ToysId);
            if (rno < 0) return false;
            dt.Rows[rno].Delete();
            //dt.AcceptChanges();
            UpdateOnDb(ref dt);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public bool AddRecFromTable(ref DataTable dt, object ugrid, object ugrname, object ugrpassword, object ugrlabel)
    {
        try
        {
            dt = GetDataByDataTable("select * from userinfo");
            DataRow row = dt.NewRow();
            row[0] = ugrid;
            row[1] = ugrname ;
            row[2] = ugrpassword ;
            row[3] = ugrlabel;
            dt.Rows.Add(row);
            UpdateOnDb(ref dt);
            return true;
        }
        catch
        {
            return false;
        }  
    }
    public bool UpdateOnDb(ref DataTable dt)
    {
        try
        {
            da.Update(dt);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
