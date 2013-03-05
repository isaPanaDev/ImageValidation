using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageValidation.Core;
using System.Data;
using ImageValidation.Client.ServiceReference1;
using ImageValidation.Collection;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for ImageValidationClient.xaml
    /// </summary>
    public partial class ImageValidationClient : PageFunction<String>
    {
        #region Declaration and Initialization section

        ServiceReference1.ImageValidationServiceClient sRef = new ServiceReference1.ImageValidationServiceClient();

        ComputerInformation compInfo = new ComputerInformation();
        DriverInformation driverInfo = new DriverInformation();
        ApplicationInformation appsInfo = new ApplicationInformation();
        List<Applications> ObjAppLst = new List<Applications>();
        HotFixInformation hotfixInfo = new HotFixInformation();

        int ComputerID = 0;

        int CountApplication = 0;
        int CountMatchApplication = 0;
        int CountMatchVersionApplication = 0;

        int CountDriver = 0;
        int CountMatchDriver = 0;
        int CountMatchVersionDriver = 0;

        int CountFileFolder = 0;
        int CountMatchFileFolder = 0;

        int CountRegistry = 0;
        int CountMatchRegistry = 0;

        int CountHotFix = 0;
        int CountMatchHotFix = 0;

        #endregion
        public ImageValidationClient()
        {
            InitializeComponent();
            LoadComputerInfo();
        }

        private void btnvaidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Computer ObjComp = compInfo.GetComputerInformation(); //Getting computer information from system

                #region Get Computer ID By Model & Product

                int ComputerIDByModel = sRef.SeleteComputerInfoByModel(ObjComp.Model); //Return ComputerID By Model
                int ComputerIDByProduct = sRef.SeleteComputerInfoByProduct(ObjComp.Product); //Return ComputerID By Product

                #endregion

                if (ComputerIDByModel > 0 && ComputerIDByProduct > 0)
                {
                    ComputerID = ComputerIDByModel;

                    ImageSource imgThumbUp = new BitmapImage(new Uri(@"/ImageValidation.Client;component/Images/Thumb-up.png", UriKind.Relative));

                    ImageSource imgThumbDown = new BitmapImage(new Uri(@"/ImageValidation.Client;component/Images/Thumb-down.png", UriKind.Relative));


                    #region Bind All DataGrid - Applications, Drivers, HotFixes, FileFolders, Registry etc after comparison & XML Serializer

                    ComputerXml Mainobj = new ComputerXml();
                    XmlDocument xmldoc = new XmlDocument();
                    XmlSerializer ser = new XmlSerializer(Mainobj.GetType());

                    //Comparing System`s Applications With Azure Database (Applications)
                    dataGrid_app.ItemsSource = CompareApplications(ComputerID, xmldoc, ser);

                    //Comparing System`s Drivers With Azure Database (Drivers)
                    dataGrid_driver.ItemsSource = CompareDrivers(ComputerID, xmldoc, ser);

                    //Comparing System`s HotFixes With Azure Database (Drivers)
                    dataGrid_hotfix.ItemsSource = CompareHotFixes(ComputerID, xmldoc, ser);

                    //Comparing System`s FileFolders With Azure Database (FileFolders)
                    dataGrid_filefolder.ItemsSource = CompareFileFolder(ComputerID, xmldoc, ser);

                    //Comparing System`s Registry With Azure Database (FileFolders)
                    dataGrid_registr.ItemsSource = CompareRegistry(ComputerID, xmldoc, ser);

                    //_grdMainlContent.Visibility = System.Windows.Visibility.Visible; //Display DataGrids Container

                    #endregion

                    #region Summary section messages related to all if computer information exist after that check other info

                    if (CountMatchApplication > 0 || CountMatchDriver > 0 || CountMatchFileFolder > 0 || CountMatchHotFix > 0 || CountMatchRegistry > 0)
                    {
                        lbloverall.Content = " Overall Validation: Failed";
                        imgthumboverall.Source = imgThumbDown;
                    }
                    else
                    {
                        lbloverall.Content = " Overall Validation: Passed";
                        imgthumboverall.Source = imgThumbUp;
                    }



                    string Mismatchappversion = ((CountMatchVersionApplication > 0) ? ", " + CountMatchVersionApplication + " Version Mismatch" : "");
                    string Mismatchdriverversion = ((CountMatchVersionDriver > 0) ? ", " + CountMatchVersionDriver + " Version Mismatch" : "");
                    if (CountApplication > 0)
                    {
                        lblapp.Content = " Application Validation: " + ((CountMatchApplication > 0) ? "Failed: " + CountMatchApplication + "Missing Software" + Mismatchappversion : "Passed");
                        imgthumbapp.Source = CountMatchApplication > 0 ? imgThumbDown : imgThumbUp;
                    }
                    else
                    {
                        lbloverall.Content = " Overall Validation: Overall failed (Result not found).";
                        dataGrid_app.ItemsSource = null; //.Visibility = System.Windows.Visibility.Hidden;
                    }

                    if (CountDriver > 0)
                    {
                        lbldriver.Content = " Driver Validation: " + ((CountMatchDriver > 0) ? "Failed: " + CountMatchDriver + " Missing Software" + Mismatchdriverversion : "Passed");
                        imgthumbdriver.Source = CountMatchDriver > 0 ? imgThumbDown : imgThumbUp;
                    }
                    else
                    {
                        lbldriver.Content = " Driver Validation: Not Found Any Record.";
                        dataGrid_driver.ItemsSource = null; //.Visibility = System.Windows.Visibility.Hidden;
                    }

                    if (CountFileFolder > 0)
                    {
                        lblfilefolder.Content = " File and Folder Validation: " + ((CountMatchFileFolder > 0) ? "Failed: " + CountMatchFileFolder + " Missing Software" : "Passed");
                        imgthumbfilefolder.Source = CountMatchFileFolder > 0 ? imgThumbDown : imgThumbUp;
                    }
                    else
                    {
                        lblfilefolder.Content = " File and Folder Validation: Not Found Any Record.";
                        dataGrid_filefolder.ItemsSource = null; //.Visibility = System.Windows.Visibility.Hidden;
                    }

                    if (CountHotFix > 0)
                    {
                        lblhotfixes.Content = " Microsoft HotFix Validation: " + ((CountMatchHotFix > 0) ? "Failed: " + CountMatchHotFix + " Missing Software" : "Passed");
                        imgthumbhotfixes.Source = CountMatchHotFix > 0 ? imgThumbDown : imgThumbUp;
                    }
                    else
                    {
                        lblhotfixes.Content = " Microsoft HotFix Validation: Not Found Any Record.";
                        dataGrid_hotfix.ItemsSource = null; //Visibility = System.Windows.Visibility.Hidden;
                    }

                    if (CountRegistry > 0)
                    {
                        lblregistry.Content = " Registry Validation: " + ((CountMatchRegistry > 0) ? "Failed: " + CountMatchRegistry + " Missing Software" : "Passed");
                        imgthumbregistry.Source = CountMatchRegistry > 0 ? imgThumbDown : imgThumbUp;
                    }
                    else
                    {
                        lblregistry.Content = " Registry Validation: Not Found Any Record.";
                        dataGrid_registr.ItemsSource = null; //.Visibility = System.Windows.Visibility.Hidden;
                    }

                    #endregion

                }
                else
                {
                    #region Summary messages displays if computer information not exist in azure database

                    lbloverall.Content = "- Overall Validation: Overall failed (Result not found).";
                    lblapp.Content = "- Application Validation: Not Found Any Record.";
                    lbldriver.Content = "- Driver Validation: Not Found Any Record.";
                    lblfilefolder.Content = "- File and Folder Validation: Not Found Any Record.";
                    lblhotfixes.Content = "- Microsoft HotFix Validation: Not Found Any Record.";
                    lblregistry.Content = "- Registry Validation: Not Found Any Record.";

                    #endregion

                }
            }
            catch (Exception ex)
            { throw ex; }
        }

        #region Load all Data also comparison with azure database to local system (Computer Info, Applications, Drivers, HotFixes, FileFolders, Registry etc.)

        /// <summary>
        /// Load Computer information from local system.
        /// </summary>
        public void LoadComputerInfo()
        {
            Computer ObjComp = compInfo.GetComputerInformation();
            txtarch.Text = ObjComp.OSArchitecture;
            txtmodel.Text = ObjComp.Model;
            txtos.Text = ObjComp.Caption;
            txtproduct.Text = ObjComp.Product;
            txtserial.Text = ObjComp.BuildNumber;
        }

        /// <summary>
        /// This function return an application list and azure data also comparison with system data.
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        private List<Applications> CompareApplications(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
        {
            try
            {
                List<Applications> ObjAppLst = appsInfo.GetApplicationInformation();//Getting from system

                string xmlApp = sRef.SelectApplicationsByComputerID(ComputerID); //Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlApp);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                Applications[] arrApplications = myObj.Applications;

                List<Applications> applications = new List<Applications>(); //In this items added from azure database and its return with list of items

                foreach (var app in arrApplications)
                {
                    int status = 0;

                    if (ObjAppLst.Any(y => y.DisplayName == app.DisplayName))
                    {
                        status = 1;
                    }
                    else
                    {
                        CountMatchApplication++;
                    }

                    if (ObjAppLst.Any(y => y.Version == app.Version))
                    {
                        status = 2;
                    }
                    else
                    {
                        CountMatchVersionApplication++;
                    }


                    applications.Add(new Applications()
                    {
                        DisplayName = app.DisplayName,
                        Version = app.Version,
                        IsCompared = status
                    });
                    CountApplication++;
                }

                return applications;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// This function return an Drivers list and azure data also comparison with system data.
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        private List<Driver> CompareDrivers(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
        {
            try
            {
                List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();//Getting from system

                string xmlDriver = sRef.SelectDriverByComputerID(ComputerID);//Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlDriver);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                Driver[] arrDrivers = myObj.Driver;

                List<Driver> driver = new List<Driver>();  //In this items added from azure database and its return with list of items
                foreach (var app in arrDrivers)
                {
                    int status = 0;

                    if (ObjDriverLst.Any(y => y.DeviceName == app.DeviceName))
                    {
                        status = 1;

                    }
                    else
                    {
                        CountMatchDriver++;
                    }

                    if (ObjDriverLst.Any(y => y.DriverVersion == app.DriverVersion))
                    {

                        status = 2;
                    }
                    else
                    {
                        CountMatchVersionDriver++;
                    }

                    driver.Add(new Driver()
                    {
                        DeviceName = app.DeviceName,
                        DriverVersion = app.DriverVersion,
                        IsCompared = status
                    });
                    CountDriver++;
                }

                return driver;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// This function return an HotFixs list and azure data also comparison with system data.
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        private List<HotFix> CompareHotFixes(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
        {
            try
            {
                List<HotFix> ObjHotFix = hotfixInfo.GetHotFixInfo(); //Getting from system

                string xmlDriver = sRef.SelectHotFixByComputerID(ComputerID); //Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlDriver);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                HotFix[] arrHotfix = myObj.HotFix;

                List<HotFix> hotfix = new List<HotFix>(); //In this items added from azure database and its return with list of items
                foreach (var app in arrHotfix)
                {
                    bool status = false;

                    if (ObjHotFix.Any(y => y.HotFixIDs == app.HotFixIDs))
                    {
                        status = true;

                    }
                    else
                    {
                        CountMatchHotFix++;
                    }

                    hotfix.Add(new HotFix()
                    {
                        HotFixIDs = app.HotFixIDs,
                        IsCompared = status
                    });

                    CountHotFix++;
                }

                return hotfix;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// This function return an FileFolder list and azure data also comparison with system data.
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        private List<FileFolder> CompareFileFolder(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
        {
            try
            {
                string xmlDriver = sRef.SelectFileFolderByComputerID(ComputerID); //Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlDriver);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                FileFolder[] arrFilefolder = myObj.FileFolder;

                List<FileFolder> filefolders = new List<FileFolder>(); //In this items added from azure database and its return with list of items
                foreach (var filefolder in arrFilefolder)
                {
                    bool status = false;

                    if (File.Exists(filefolder.Location))
                    {
                        status = true;

                    }
                    else
                    {
                        CountMatchFileFolder++;
                    }

                    filefolders.Add(new FileFolder()
                    {
                        Location = filefolder.Location,
                        Note = filefolder.Note,
                        Type = filefolder.FileFolderTypeID == 1 ? "File" : "Folder",
                        IsCompared = status
                    });
                    CountFileFolder++;
                }

                return filefolders;
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// This function return an RegistryGroup list and azure data also comparison with system data.
        /// </summary>
        /// <param name="ComputerID"></param>
        /// <param name="xmldoc"></param>
        /// <param name="ser"></param>
        /// <returns></returns>
        private List<RegistryGroup> CompareRegistry(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
        {
            try
            {
                RegistryInformation ObjeRegInfo = new RegistryInformation();
                Computer ObjComp = compInfo.GetComputerInformation();//Getting from system

                string xmlRegistry = sRef.SelectRegistryByComputerID(ComputerID);//Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlRegistry);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;

                Registrys[] arrRegistry = myObj.Registrys;
                RegistryGroup[] arrRegistryGroup = myObj.RegistryGroup;

                List<RegistryGroup> registry = new List<RegistryGroup>();//In this items added from azure database and its return with list of items
                foreach (var registrygroup in arrRegistryGroup)
                {
                    bool Status = false;
                    foreach (var item in arrRegistry.Where(x => x.RegistryGroupID == registrygroup.RegistryGroupID))
                    {
                        string keystring = item.Key;
                        string[] reg_Key = keystring.Split('{');// Substring(keystring.IndexOf('{') + 1);
                        string RegistryKey = reg_Key[0];

                        using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKey))
                        {
                            if (key != null)
                            {
                                foreach (string subKeyName in key.GetSubKeyNames())
                                {
                                    if (subKeyName != null)
                                    {
                                        using (RegistryKey
                                            tempKey = key.OpenSubKey(subKeyName))
                                        {
                                            foreach (string valueName in tempKey.GetValueNames())
                                            {

                                                if (valueName == item.Value && tempKey.GetValue(valueName).ToString() == item.ValueData && tempKey.GetValueKind(valueName).ToString() == item.DataType)
                                                {
                                                    Status = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }

                    if (Status == false)
                    {
                        CountMatchRegistry++;
                    }
                    registry.Add(new RegistryGroup()
                    {
                        FileName = registrygroup.RegistryGroupID,
                        Note = registrygroup.Note,
                        IsCompared = Status
                    });
                    CountRegistry++;
                }
                return registry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

