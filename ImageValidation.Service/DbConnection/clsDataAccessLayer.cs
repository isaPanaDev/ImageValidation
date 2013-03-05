using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using ImageValidation.Core;
using System.Reflection;
using System.Xml;
using System.IO;
using ImageValidation.Service.Data;


public static class clsDataAccessLayer
{
    static DataTable objdt;
    static SqlCommand cmd = null;
    static SqlConnection con = null;
    static SqlDataAdapter adp = null;
    public static string sql = string.Empty;
    static string stringResult = string.Empty;
    static bool queryresult = false;
    static function objfun = new function();
    static DataSet ds = null;
    static int rowCount = 0;
    static int result = 0;



    public enum SQlCMDType
    {
        Insert,
        Update,
        Delete,
        Select,
    }

    public class clsServiceMethods
    {
        //Validator Collector Tool
        #region Validator Collector Tool
        public static Users ValidateUser(string Username, string Password)
        {
            string myPassword = string.Empty;
            myPassword = Utility.HashPassword(Password);
            Users userDetails = new Users();
            objdt = new DataTable();
            sql = "select UP.PermissionID,P.RoleName,U.UserID,U.Username,U.FirstName,U.LastName,U.Email from [User]U inner join [UserPermission]UP ON  U.UserID=UP.UserID INNER JOIN [Permission]P ON P.PermissionID=UP.PermissionID AND " +
            "U.UserID=UP.UserID WHERE Username='" + Username.ToString() + "' AND Password='" + myPassword.ToString() + "'";
            objdt = ADOC.GetObject.GetTable(sql);
            if (objdt.Rows.Count > 0)
            {
                userDetails.UserID = Convert.ToInt32(objdt.Rows[0]["UserID"].ToString());
                userDetails.PermissionID = Convert.ToInt32(objdt.Rows[0]["PermissionID"].ToString());
                userDetails.RoleName = objdt.Rows[0]["RoleName"].ToString();
                userDetails.Username = objdt.Rows[0]["Username"].ToString();
                userDetails.FirstName = objdt.Rows[0]["FirstName"].ToString();
                userDetails.LastName = objdt.Rows[0]["FirstName"].ToString();
                userDetails.Email = userDetails.Username = objdt.Rows[0]["Username"].ToString();
            }
            else
            {
                //userDetails.UserID = 0;
                //userDetails.PermissionID = 0;
                userDetails.Username = null;
                userDetails.FirstName = null;
                userDetails.LastName = null;
                userDetails.Email = null;
            }

            return userDetails;
        }

        public static bool SaveValidateLoginAccountDetails(Users ObjUser)
        {
            try
            {
                sql = " insert into AccountHistory(UserID,LoginDate,SessionID) values('" + ObjUser.UserID + "','" + ObjUser.LoginDate + "','" + ObjUser.SessionID + "');";
                if (ADOC.GetObject.ExecuteDML(sql))
                {
                    queryresult = true;
                }
            }
            catch (Exception)
            {
                queryresult = false;
            }
            return queryresult;
        }

        public static int CheckUsernameExists(Users ObjUser)
        {
            int UserID = 0;
            objdt = new DataTable();
            sql = "select * from [dbo].[User] where Username='" + ObjUser.Username + "'";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt.Rows.Count > 0)
            {
                UserID = Convert.ToInt32(objdt.Rows[0]["UserID"].ToString());

            }
            return UserID;
        }

        public static bool SaveFailedLogonDetails(Users ObjUser)
        {
            try
            {
                sql = "insert into FailedLogon(UserID,DateTime) Values('" + ObjUser.UserID + "',GETDATE())";
                if (ADOC.GetObject.ExecuteDML(sql))
                {
                    queryresult = true;
                }
            }
            catch (Exception)
            {
                queryresult = false;
            }
            return queryresult;
        }
                
        public static bool SaveComputerInformation(List<HotFix> ObjDriverLst)
        {
            try
            {
                ListtoDataTableConverter lstToTable = new ListtoDataTableConverter();
                DataTable dtDrivers = lstToTable.ToDataTable(ObjDriverLst);


                sql = "Usp_SaveComputerInformation @HotFixTable='" + dtDrivers + "'";
                //sql = "";
                if (ADOC.GetObject.ExecuteDML(sql) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                queryresult = false;
            }
            return queryresult;
        }

        public static int SaveComputerInformationFromXml(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {

            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);
            SqlCommand cmd;

            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInformation", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
            cmdProc.Parameters.AddWithValue("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@ID"].Value.ToString());

            //if (results == 1)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }

        public static int CheckModelRecordExists(string Modelno)
        {
            int UserID = 0;
            objdt = new DataTable();
            sql = "select * from Computer where IsPrimaryModel='1' and Model='" + Modelno + "'";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt.Rows.Count > 0)
            {
                UserID = Convert.ToInt32(objdt.Rows[0]["UserID"].ToString());

            }
            return UserID;
        }

        public static int SaveComputerInfoUpdateCheckedModelRecord(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {
            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);
            SqlCommand cmd;

            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInfoUpdateCheckedModelRecord", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
            cmdProc.Parameters.AddWithValue("@IDs", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@IDs"].Value.ToString());

            //if (results > 0)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }

        public static int SaveComputerInfoUpdateCheckedModelRecordSet1(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {

            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);
            SqlCommand cmd;

            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInfoUpdateCheckedModelRecordSet1", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
            cmdProc.Parameters.AddWithValue("@IDs", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@IDs"].Value.ToString());

            //if (results > 0)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }

        public static int CheckProductRecordExists(string Modelno)
        {
            int UserID = 0;
            objdt = new DataTable();
            sql = "select * from Computer where IsPrimaryProduct='1' and Model='" + Modelno + "'";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt.Rows.Count > 0)
            {
                UserID = Convert.ToInt32(objdt.Rows[0]["UserID"].ToString());

            }
            return UserID;
        }

        public static int SaveComputerInfoUpdateCheckedProductRecord(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {

            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);

            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInfoUpdateCheckedProductRecord", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);

            cmdProc.Parameters.AddWithValue("@IDs", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@IDs"].Value.ToString());

            //if (results > 0)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }

        public static int SaveComputerInfoUpdateCheckedProductRecordSet1(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {

            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);


            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInfoUpdateCheckedProductRecordSet1", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
            cmdProc.Parameters.AddWithValue("@IDs", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@IDs"].Value.ToString());

            //if (results > 0)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }
        
        public static int SaveComputerInfoUpdateCheckedProductModelRecord(Computer objComputer, DataTable dtDriver, DataTable dtApplication, DataTable dtHotfix)
        {

            SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);

            sqlConn.Open();
            SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInfoUpdateCheckedProductModelRecord", sqlConn);
            cmdProc.CommandType = CommandType.StoredProcedure;
            //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
            //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
            cmdProc.Parameters.AddWithValue("@IDs", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmdProc.Parameters.AddWithValue("@UserID", objComputer.UserID);
            cmdProc.Parameters.AddWithValue("@Model", objComputer.Model);
            cmdProc.Parameters.AddWithValue("@Product", objComputer.Product);
            cmdProc.Parameters.AddWithValue("@BuildNumber", objComputer.BuildNumber);
            cmdProc.Parameters.AddWithValue("@Caption", objComputer.Caption);
            cmdProc.Parameters.AddWithValue("@CSDVersion", objComputer.CDSVersion);
            cmdProc.Parameters.AddWithValue("@InstallDate", objComputer.InstallDate);
            cmdProc.Parameters.AddWithValue("@MUILanguages", objComputer.MUILanguages);
            cmdProc.Parameters.AddWithValue("@OSArchitecture", objComputer.OSArchitecture);
            cmdProc.Parameters.AddWithValue("@OSLanguage", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@OSProductSuite", objComputer.OSProductSuite);
            cmdProc.Parameters.AddWithValue("@OSType", objComputer.OSType);
            cmdProc.Parameters.AddWithValue("@ServicePackMajorVersion", objComputer.ServicePackMajorVersion);
            cmdProc.Parameters.AddWithValue("@ServicePackMinorVersion", objComputer.ServicePackMinorVersion);
            cmdProc.Parameters.AddWithValue("@SystemDirectory", objComputer.SystemDirectory);
            cmdProc.Parameters.AddWithValue("@Version", objComputer.Version);
            cmdProc.Parameters.AddWithValue("@WindowsDirectory", objComputer.WindowsDirectory);
            cmdProc.Parameters.AddWithValue("@Manufacturer", objComputer.Manufacturer);
            cmdProc.Parameters.AddWithValue("@Manufacturer2", objComputer.Manufacturer2);
            cmdProc.Parameters.AddWithValue("@ComputerRecordAddDate", objComputer.ComputerRecordAddDate);
            cmdProc.Parameters.AddWithValue("@IsPrimaryProduct", objComputer.IsPrimaryProduct);
            cmdProc.Parameters.AddWithValue("@IsPrimaryModel", objComputer.IsPrimaryModel);


            cmdProc.Parameters.AddWithValue("@DriverTable", dtDriver);
            cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApplication);
            cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
            int results = cmdProc.ExecuteNonQuery();
            int id = Convert.ToInt32(cmdProc.Parameters["@IDs"].Value.ToString());

            //if (results > 0)
            //    queryresult = true;
            //else
            //    queryresult = false;

            return id;

        }

        #endregion

        //Validator client Tool
        #region ImageValidationClient Tool..  Comparing computer information/Driver/Applications/etc. to azure database

        public static int GetComputerInfoByModel(string Model)
        {
            int computerID = 0;
            objdt = new DataTable();
            sql = "Select * From Computer Where Model='" + Model + "' AND IsPrimaryModel = 1";
            objdt = ADOC.GetObject.GetTable(sql);
            if (objdt != null)
            {
                if (objdt.Rows.Count > 0)
                {
                    computerID = Convert.ToInt32(objdt.Rows[0]["ComputerID"].ToString());
                }
            }
            return computerID;
        }

        public static int GetComputerInfoByProduct(string Product)
        {
            int computerID = 0;
            objdt = new DataTable();
            sql = "Select * From Computer Where Product='" + Product + "' AND IsPrimaryProduct = 1";
            objdt = ADOC.GetObject.GetTable(sql);
            if (objdt != null)
            {
                if (objdt.Rows.Count > 0)
                {
                    computerID = Convert.ToInt32(objdt.Rows[0]["ComputerID"].ToString());
                }
            }
            return computerID;
        }


        /// <summary>
        /// Get Drivers By ComputerID
        /// </summary>
        /// <param name="computerId"></param>
        /// <returns></returns>
        public static string GetDriverByComputerID(int computerId)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");

            objdt = new DataTable();

            sql = "SELECT * FROM Driver WHERE ComputerID =" + computerId + "";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt != null)
            {
                xmlTextWriter.WriteStartElement("Driver");
                foreach (DataRow row in objdt.Rows) // Loop over the rows.
                {
                    xmlTextWriter.WriteStartElement("Driver");
                    xmlTextWriter.WriteElementString("DeviceName", row["DeviceName"].ToString());
                    xmlTextWriter.WriteElementString("DriverVersion", row["DriverVersion"].ToString());
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndElement();
                XmlDocument docSave = new XmlDocument();
                docSave.LoadXml(stringWriter.ToString());
                string xmlData = stringWriter.ToString();
                return xmlData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Applications By ComputerID
        /// </summary>
        /// <param name="computerId"></param>
        /// <returns></returns>
        public static string GetApplicationsByComputerID(int computerId)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");

            objdt = new DataTable();

            sql = "SELECT * FROM Application WHERE ComputerID =" + computerId + "";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt != null)
            {
                xmlTextWriter.WriteStartElement("Applications");
                foreach (DataRow row in objdt.Rows) // Loop over the rows.
                {
                    xmlTextWriter.WriteStartElement("Applications");
                    xmlTextWriter.WriteElementString("DisplayName", row["DisplayName"].ToString());
                    xmlTextWriter.WriteElementString("Version", row["Version"].ToString());
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndElement();
                XmlDocument docSave = new XmlDocument();
                docSave.LoadXml(stringWriter.ToString());
                string xmlData = stringWriter.ToString();
                return xmlData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get HotFixes By ComputerID
        /// </summary>
        /// <param name="computerId"></param>
        /// <returns></returns>
        public static string GetHotFixByComputerID(int computerId)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");

            objdt = new DataTable();

            sql = "SELECT * FROM HotFix WHERE ComputerID =" + computerId + "";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt != null)
            {
                xmlTextWriter.WriteStartElement("HotFix");
                foreach (DataRow row in objdt.Rows) // Loop over the rows.
                {
                    xmlTextWriter.WriteStartElement("HotFix");
                    xmlTextWriter.WriteElementString("HotFixIDs", row["HotFixIDs"].ToString());
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndElement();
                XmlDocument docSave = new XmlDocument();
                docSave.LoadXml(stringWriter.ToString());
                string xmlData = stringWriter.ToString();
                return xmlData;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get FileFolder By ComputerID
        /// </summary>
        /// <param name="computerId"></param>
        /// <returns></returns>
        public static string GetFileFolderByComputerID(int computerId)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");

            objdt = new DataTable();

            sql = "SELECT * FROM FileFolder WHERE ComputerID =" + computerId + "";
            objdt = ADOC.GetObject.GetTable(sql);

            if (objdt != null)
            {
                xmlTextWriter.WriteStartElement("FileFolder");
                foreach (DataRow row in objdt.Rows) // Loop over the rows.
                {
                    xmlTextWriter.WriteStartElement("FileFolder");
                    xmlTextWriter.WriteElementString("Location", row["Location"].ToString());
                    xmlTextWriter.WriteElementString("FileFolderTypeID", row["FileFolderTypeID"].ToString());
                    xmlTextWriter.WriteElementString("Note", row["Note"].ToString());
                    xmlTextWriter.WriteEndElement();
                }
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndElement();
                XmlDocument docSave = new XmlDocument();
                docSave.LoadXml(stringWriter.ToString());
                string xmlData = stringWriter.ToString();
                return xmlData;
            }
            else
            {
                return null;
            }
        }

        public static string GetRegistryByComputerID(int computerId)
        {
            StringWriter stringWriter = new StringWriter();
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("ComputerInformation");


            DataTable objRGingdt = new DataTable();

            sql = "SELECT * FROM RegistryGrouping WHERE ComputerID=" + computerId + "";
            objRGingdt = ADOC.GetObject.GetTable(sql);

            if (objRGingdt != null)
            {
                xmlTextWriter.WriteStartElement("RegistryGroup");

                var distinctRows = (from DataRow dRow in objRGingdt.Rows
                                    select dRow["RegistryGroupID"]).Distinct();
                foreach (var data in distinctRows)
                {
                    int RegistryGroupID = (int)data;

                    DataTable objRegistryGroupdt = new DataTable();
                    sql = "SELECT * FROM RegistryGroup WHERE RegistryGroupID=" + RegistryGroupID + "";
                    objRegistryGroupdt = ADOC.GetObject.GetTable(sql);

                    if (objRegistryGroupdt != null)
                    {
                        foreach (DataRow RegistryGroupdtRow in objRegistryGroupdt.Rows)
                        {
                            xmlTextWriter.WriteStartElement("RegistryGroup");
                            xmlTextWriter.WriteElementString("RegistryGroupID", RegistryGroupdtRow["RegistryGroupID"].ToString());
                            xmlTextWriter.WriteElementString("FileName", RegistryGroupdtRow["FileName"].ToString());
                            xmlTextWriter.WriteElementString("Note", RegistryGroupdtRow["Note"].ToString());
                            xmlTextWriter.WriteEndElement();
                        }
                    }

                }
                xmlTextWriter.WriteEndElement();


                xmlTextWriter.WriteStartElement("Registrys");
                foreach (DataRow RGingdtRow in objRGingdt.Rows)
                {
                    int RegistryID = (int)RGingdtRow["RegistryID"];
                    int RegistryGroupID = (int)RGingdtRow["RegistryGroupID"];

                    DataTable objRegistrydt = new DataTable();

                    sql = "SELECT * FROM Registry WHERE RegistryID=" + RegistryID + "";
                    objRegistrydt = ADOC.GetObject.GetTable(sql);

                    if (objRegistrydt != null)
                    {
                        foreach (DataRow RegistrydtRow in objRegistrydt.Rows)
                        {
                            xmlTextWriter.WriteStartElement("Registrys");
                            xmlTextWriter.WriteElementString("Key", RegistrydtRow["Key"].ToString());
                            xmlTextWriter.WriteElementString("Value", RegistrydtRow["Value"].ToString());
                            xmlTextWriter.WriteElementString("ValueData", RegistrydtRow["ValueData"].ToString());
                            xmlTextWriter.WriteElementString("DataType", RegistrydtRow["DataType"].ToString());
                            xmlTextWriter.WriteElementString("RegistryGroupID", RegistryGroupID.ToString());
                            xmlTextWriter.WriteEndElement();
                        }
                    }

                }
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteEndElement();
                XmlDocument docSave = new XmlDocument();
                docSave.LoadXml(stringWriter.ToString());
                string xmlData = stringWriter.ToString();
                return xmlData;

            }
            else
            {
                return null;
            }
        }
        #endregion



    }
}
