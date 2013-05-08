using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Data.SqlClient;
using System.Data;


using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using ImageValidation.Service.Data;
using ImageValidation.Core;
using System.Configuration;
using System.Xml;
using System.IO;

using System.Xml.Serialization;

using System.Reflection;
using Microsoft.WindowsAzure.StorageClient;
//using ImageValidation.WCFService.Data;

namespace ImageValidation.Service
{
    public class ImageValidationService : IImageValidationService
    {
        /// <summary>
        /// For storing xml file to Azure Blob Storage
        /// </summary>
        BlobStorageService _blobStorageService = new BlobStorageService();

        bool result = false;
        int RowID;
        private string strConn = "";
        SqlConnection sqlConn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SQLAzureConn"].ConnectionString);
        SqlCommand cmd;

        //Validator Collector Tool
        #region Validator Collector Tool

        public Users ValidateUser(string Username, string Password)
        {
            Users userDetails = new Users();
            userDetails = clsDataAccessLayer.clsServiceMethods.ValidateUser(Username, Password);
            return userDetails;

        }

        /// <summary>
        /// Save account history in db
        /// </summary>
        /// <param name="users">Users details</param>
        /// <returns>bool</returns>
        public bool SaveAccountHistory(Users users)
        {
            result = clsDataAccessLayer.clsServiceMethods.SaveValidateLoginAccountDetails(users);
            return result;
        }

        /// <summary>
        /// Check username exists in db
        /// </summary>
        /// <param name="ObjUsers">Users</param>
        /// <returns>int</returns>
        public int CheckUsernameExists(Users ObjUsers)
        {
            int UserID = clsDataAccessLayer.clsServiceMethods.CheckUsernameExists(ObjUsers);
            return UserID;
        }

        /// <summary>
        /// Save login history
        /// </summary>
        /// <param name="ObjUsers">Users</param>
        /// <returns>bool</returns>
        public bool SaveFailedLogon(Users ObjUsers)
        {
            result = clsDataAccessLayer.clsServiceMethods.SaveFailedLogonDetails(ObjUsers);
            return result;
        }

        /// <summary>
        /// Save all Computer Information in db
        /// </summary>
        /// <param name="ObjComp">Computer</param>
        /// <param name="ObjDriverLst">Hotfix list</param>
        /// <returns>bool</returns>
        public bool SaveComputerInformation(Computer ObjComp, List<Applications> ObjAppsLst, List<HotFix> ObjHotfixLst)
        {

            ListtoDataTableConverter lstToTableHotfix = new ListtoDataTableConverter();
            DataTable dtHotfix = lstToTableHotfix.ToDataTable(ObjHotfixLst);

            ListtoDataTableConverter lstToTableApps = new ListtoDataTableConverter();
            DataTable dtApps = lstToTableApps.ToDataTable(ObjAppsLst);


            try
            {
                sqlConn.Open();
                SqlCommand cmdProc = new SqlCommand("Usp_SaveComputerInformation", sqlConn);
                cmdProc.CommandType = CommandType.StoredProcedure;
                //cmdProc.Parameters.AddWithValue("@Type", "InsertDetails");
                //cmdProc.Parameters.AddWithValue("@ComputerTable", ObjComp);
                cmdProc.Parameters.AddWithValue("@ApplicationTable", dtApps);
                cmdProc.Parameters.AddWithValue("@HotFixTable", dtHotfix);
                int results = cmdProc.ExecuteNonQuery();
                //strMsg = "Saved successfully.";
            }
            catch (Exception)
            {

            }
            finally
            { }

            //clsDataAccessLayer.clsServiceMethods.SaveComputerInformation(ObjDriverLst);
            return result;
        }


        /// <summary>
        /// Save all Computer Information in db
        /// </summary>
        /// <param name="ObjComp">Computer</param>
        /// <param name="ObjDriverLst">Hotfix list</param>
        /// <returns>bool</returns>
        public int SaveComputerInformationFromXml(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInformationFromXml(objComputer, dtDriver, dtApplication, dtHotfix);

                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);

            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;
        }

        public DataTable GetDataTableFromObjects(object[] objects)
        {
            if (objects != null && objects.Length > 0)
            {
                Type t = objects[0].GetType();
                DataTable dt = new DataTable(t.Name);
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    dt.Columns.Add(new DataColumn(pi.Name));
                }

                foreach (var o in objects)
                {
                    DataRow dr = dt.NewRow();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        dr[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);
                    }

                    dt.Rows.Add(dr);
                }
                return dt;
            }
            return null;
        }


        public DataTable ConvertArrayToTable(Array myList)
        {
            DataTable dt = new DataTable();
            if (myList.Length > 0)
            {
                PropertyInfo[] propInfos = myList.GetValue(0).GetType().GetProperties();

                foreach (PropertyInfo propInfo in propInfos)
                {
                    dt.Columns.Add(propInfo.Name, propInfo.PropertyType);
                }

                foreach (object tempObject in myList)
                {
                    DataRow dr = dt.NewRow();

                    for (int i = 0; i < propInfos.Length; i++)
                    {
                        dr[i] = propInfos[i].GetValue(tempObject, null);
                    }

                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// Check model number exists in db
        /// </summary>
        /// <param name="ModelNo"></param>
        /// <returns>bool</returns>
        public bool CheckModelRecordExists(string ModelNo)
        {
            int count = clsDataAccessLayer.clsServiceMethods.CheckModelRecordExists(ModelNo);

            if (count > 0)
                result = true;
            else
                result = false;

            return result;
        }


        /// <summary>
        /// Save computer information and update existing model set to 0
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="modelNo"></param>
        /// <returns>bool</returns>
        public int SaveComputerInfoUpdateCheckedModelRecord(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInfoUpdateCheckedModelRecord(objComputer, dtDriver, dtApplication, dtHotfix);


                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);

            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;
        }


        /// <summary>
        /// Save computer information and update existing model set to 1
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="modelNo"></param>
        /// <returns>bool</returns>
        public int SaveComputerInfoUpdateCheckedModelRecordSet1(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInfoUpdateCheckedModelRecordSet1(objComputer, dtDriver, dtApplication, dtHotfix);

                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);

            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;

        }

        /// <summary>
        /// Check product number exists in db
        /// </summary>
        /// <param name="ModelNo"></param>
        /// <returns></returns>
        public bool CheckProductRecordExists(string Model)
        {
            int count = clsDataAccessLayer.clsServiceMethods.CheckProductRecordExists(Model);

            if (count > 0)
                result = true;
            else
                result = false;

            return result;
        }

        public int SaveComputerInfoUpdateCheckedProductRecord(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInfoUpdateCheckedProductRecord(objComputer, dtDriver, dtApplication, dtHotfix);

                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);

            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;
        }

        /// <summary>
        /// Save computer information and update existing model set to 1
        /// </summary>
        /// <param name="xmlData"></param>
        /// <param name="modelNo"></param>
        /// <returns>bool</returns>
        public int SaveComputerInfoUpdateCheckedProductRecordSet1(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInfoUpdateCheckedProductRecordSet1(objComputer, dtDriver, dtApplication, dtHotfix);

                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);

            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;
        }

        /// <summary>
        /// Save computer information and update existing model and product set to 1
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public int SaveComputerInfoUpdateCheckedProductModelRecord(string xmlData)
        {
            try
            {
                ComputerXml obj = new ComputerXml();
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(xmlData);
                XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
                XmlSerializer ser = new XmlSerializer(obj.GetType());
                object obj1 = ser.Deserialize(reader);

                // Then you just need to cast obj into whatever type it is, e.g.:
                ComputerXml myObj = (ComputerXml)obj1;
                Computer objComputer = myObj.Computer;
                Driver[] arrDrivers = myObj.Driver;
                Applications[] arrApplications = myObj.Applications;
                HotFix[] arrHotfix = myObj.HotFix;

                //Convert list to datatable                
                DataTable dtDriver = ConvertArrayToTable(arrDrivers);
                DataTable dtApplication = ConvertArrayToTable(arrApplications);
                DataTable dtHotfix = ConvertArrayToTable(arrHotfix);

                RowID = clsDataAccessLayer.clsServiceMethods.SaveComputerInfoUpdateCheckedProductModelRecord(objComputer, dtDriver, dtApplication, dtHotfix);

                //Blob Storage for xml out put file 
                BlobStorage(xmlData);

                //Insert Data in Product Table (Inserting Product)
                clsDataAccessLayer.clsServiceMethods.InsertProduct(objComputer.Product);
            }
            catch (Exception)
            { }
            finally
            { }

            return RowID;
        }

        #endregion

        //ImageValidationClient Tool..
        #region Client Collection tool

        /// <summary>
        /// Select Computer Info By Model & return in true/false
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SeleteComputerInfoByModel(string model)
        {
            int result = clsDataAccessLayer.clsServiceMethods.GetComputerInfoByModel(model);
            return result;
        }

        /// <summary>
        /// Select Computer Info By Product & return in true/false
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int SeleteComputerInfoByProduct(string product)
        {
            int result = clsDataAccessLayer.clsServiceMethods.GetComputerInfoByProduct(product);
            return result;
        }

        //Selete Applications from DB by ComputerID
        public string SelectApplicationsByComputerID(int computerId)
        {
            string resulttbl = clsDataAccessLayer.clsServiceMethods.GetApplicationsByComputerID(computerId);
            return resulttbl;
        }

        //Selete Drivers from DB by ComputerID
        public string SelectDriverByComputerID(int computerId)
        {
            string resulttbl = clsDataAccessLayer.clsServiceMethods.GetDriverByComputerID(computerId);
            return resulttbl;
        }

        //Selete HotFixes from DB by ComputerID
        public string SelectHotFixByComputerID(int computerId)
        {
            string resulttbl = clsDataAccessLayer.clsServiceMethods.GetHotFixByComputerID(computerId);
            return resulttbl;
        }

        //Selete FileFolders from DB by ComputerID
        public string SelectFileFolderByComputerID(int computerId)
        {
            string resulttbl = clsDataAccessLayer.clsServiceMethods.GetFileFolderByComputerID(computerId);
            return resulttbl;
        }

        //Selete Registry from DB by ComputerID
        public string SelectRegistryByComputerID(int computerId)
        {
            string resulttbl = clsDataAccessLayer.clsServiceMethods.GetRegistryByComputerID(computerId);
            return resulttbl;
        }
        #endregion

        //Create Blob container & blob file
        #region Store XML file to Azure Storege Blob
        private void BlobStorage(string xmlData)
        {
            //Store XML file to Azure Server Storage Blob         

            // Retrieve a reference to a container 
            CloudBlobContainer blobContainer =
                _blobStorageService.GetCloudBlobContainer();

            CloudBlob blob =
                blobContainer.GetBlobReference("xml-info-" + RowID + ".xml");

            System.IO.MemoryStream mStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlData));

            // Create or overwrite the "myblob" blob with contents from a local file
            blob.UploadFromStream(mStream);


        }
        #endregion
    }
}
