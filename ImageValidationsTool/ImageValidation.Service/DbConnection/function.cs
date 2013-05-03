using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Net.Mail;

/// <summary>
/// Summary description for function
/// </summary>
public class function
{
	public function()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="branch"></param>
    /// <returns></returns>
    public string GetQuery()
    {
        string query =  " select a.org,(case when b.move_in='109' then 'Normal' when b.move_in='110' then 'Special HouseHold' when"+
                        " b.move_in= '111' then 'Business Class'  else '-'  end)'movein',b.created_date, b.Pk_date, e.branch,e.username,"+
                        " a.name,e.user_type_code,a.phone,convert(varchar,b.mv_date,105) as mv_date,a.email,b.created_by,a.addr,b.ref_id," +
                        " b.origin_city,b.Destination_city,b.paid_amount,b.quotation_amount,b.discount from tbl_step1_Visitor_info as a " +
                        " inner join  tbl_step1_General_info as b on a.id=b.pid "+
                        " inner join tbl_User_Registration e on e.username=b.created_by";                        
       
        return query;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="QuotationNo"></param>
    /// <param name="gvobj"></param>
    /// <param name="sqlQuery"></param>
    /// <returns></returns>
   public DataTable GetSearchbyQuotationNo(string QuotationNo, GridView gvobj, string sqlQuery )
    {
        string message = string.Empty;
        string strBookingAmt = string.Empty;
        DataTable dtobj = null;
        string sql = string.Empty;
        string[] str = QuotationNo.Split('/');
        if (str.Length == 1)
         {
            sql = "select ref_id from tbl_step1_General_info where pid='" + str[0].Trim().ToString() +"'";
            QuotationNo = ADOC.GetObject.GetSingleResult(sql).ToString();
            if (QuotationNo != "" && QuotationNo != "0")
            {
                dtobj = new DataTable();
                sqlQuery += " and b.ref_id='" + QuotationNo + "'";
                dtobj = ADOC.GetObject.GetTable(sqlQuery);
                if (dtobj.Rows.Count > 0)
                {              
                    gvobj.DataSource = dtobj;
                    gvobj.DataBind();
                }
                else
                {
                  
                    gvobj.DataSource = null;
                    gvobj.DataBind();
                }
            }
        }
        else
        {
            dtobj = new DataTable();
            sqlQuery += " and b.ref_id='" + QuotationNo + "'";
            dtobj = ADOC.GetObject.GetTable(sqlQuery);
            if (dtobj.Rows.Count > 0)
            {
                message = "";
                gvobj.DataSource = dtobj;
                gvobj.DataBind();
              
            }
            else
            {
              
                gvobj.DataSource = null;
                gvobj.DataBind();
                message = "Found " + dtobj.Rows.Count.ToString() + " Quotation"; 
            }
        }
        return dtobj;
    }

    public  void FillDropDown(DropDownList ddl, string qStr, string text, string value)
    {
        try
        {
            ddl.DataSource = ADOC.GetObject.GetTable(qStr);
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
            ddl.Items.Insert(0, "--Select--");
            ddl.Items[0].Value = "0";
        }
        catch
        {
            //return "";
        }    
   
    }
    //public void FillListBox(ListBox lst, string qStr, string text, string value)
    //{
    //    try
    //    {
    //        lst.DataSource = ADOC.GetObject.GetTable(qStr);
    //        lst.DataTextField = text;
    //        lst.DataValueField = value;
    //        lst.DataBind();
    //        lst.Items[0].Value = "0";
    //    }
    //    catch
    //    {
    //        //return "";
    //    }

    //}
    public void FillDropDown_State(DropDownList ddl, string qStr, string text, string value)
    {
        try
        {
            ddl.DataSource = ADOC.GetObject.GetTable(qStr);
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
            ddl.Items.Insert(0, "--State--");
            ddl.Items[0].Value = "0";
        }
        catch
        {
            //return "";
        }
    }
    public void FillDropDown_State2(DropDownList ddl, string qStr, string text, string value)
    {
        try
        {
            ddl.DataSource = ADOC.GetObject.GetTable(qStr);
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
            ddl.Items.Insert(0, "--All--");
            ddl.Items[0].Value = "0";
        }
        catch
        {
            //return "";
        }
    }
 
    public void FillDropDown_Type(DropDownList ddl, string qStr, string text, string value)
    {
        try
        {
            ddl.DataSource = ADOC.GetObject.GetTable(qStr);
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
            ddl.Items.Insert(0, "--Select Type--");
            ddl.Items[0].Value = "0";
        }
        catch
        {
            //return "";
        }


    }
    public void FillCheckBox(CheckBoxList chk, string str, string txt, string value)
    {
        try
        {
            chk.DataSource = ADOD.GetObject.GetDataByDataTable(str);
            chk.DataTextField = txt;
            chk.DataValueField = value;
            chk.DataBind();

        }
        catch
        {
        }
    }
    public void FillRadio(RadioButtonList rdo, string sql, string txt, string value)
    {
        try
        {
            rdo.DataSource = ADOD.GetObject.GetDataByDataTable(sql);
            rdo.DataTextField = txt;
            rdo.DataValueField = value;
            rdo.DataBind();
            rdo.SelectedIndex = 0;
        }
        catch
        {
        }
    }
    public void FillListBox(ListBox lst, string sql, string txt, string value)
    {
        try
        {
            lst.DataSource = ADOD.GetObject.GetDataByDataTable(sql);
            lst.DataTextField = txt;
            lst.DataValueField = value;
            lst.DataBind();
        }
        catch
        {
        }
    }
   
    /// <summary>
    /// Method to Send E-Mail
    /// </summary>
    /// <param name="ToEmail">Address To Send E-Mail</param>
    /// <param name="subject">Subject of E-Mail</param>
    /// <param name="body">Body of E-Mail</param>
    /// <param name="attachFile">Path of Attachment File</param>
    public void SendMail(string ToEmail, string subject, string body, string attachFile)
    {
        try
        {

            string FromEmail = WebConfigurationManager.AppSettings["FromEmail"].ToString();
            MailMessage msg = new MailMessage(FromEmail, ToEmail);
            msg.Subject = subject;
            msg.Body = body;
            if (attachFile != "")
                msg.Attachments.Add(new Attachment(attachFile));
            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Send(msg);
        }
        catch
        {

        }
    }

    
    public string calDist(double lat1,double lat2,double lon1,double lon2)
    {
        double R = 6371; // km
        double dLat = (lat2 - lat1);
        double dLon = (lon2 - lon1);//.toRad();
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double d = R * c;
        return d.ToString("N2");
    }

    public void FillDropDownAllSearch(DropDownList ddl, string qStr, string text, string value)
    {
        try
        {

            ddl.DataSource = ADOC.GetObject.GetTable(qStr);
            ddl.DataTextField = text;
            ddl.DataValueField = value;
            ddl.DataBind();
            ddl.Items.Insert(0, "--All--");
            ddl.Items[0].Value = "0";
        }
        catch
        {
            //return "";
        }

    }
}
