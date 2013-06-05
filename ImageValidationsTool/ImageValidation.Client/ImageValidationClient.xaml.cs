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
using ImageValidation.Client.ServiceReference2;
using ImageValidation.Collection;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using ImageValidation.Client.Controls;
using System.Net;

namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for ImageValidationClient.xaml
    /// </summary>
    /// 

    public partial class ImageValidationClient : PageFunction<String>
    {
        #region Declaration and Initialization section
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        ServiceReference2.ImageValidationServiceClient sRef = new ServiceReference2.ImageValidationServiceClient();

        ComputerInformation compInfo = new ComputerInformation();
        DriverInformation driverInfo = new DriverInformation();
        ApplicationInformation appsInfo = new ApplicationInformation();
        List<Applications> ObjAppLst = new List<Applications>();
        HotFixInformation hotfixInfo = new HotFixInformation();

        int ComputerID = 0;

        int CountApplication = 0;
        int CountMatchApplication = 0;
        int CountMatchVersionApplication = 0;
        int CountAppV = 0;
        int CountAppMissing = 0;


        int CountDriver = 0;
        int CountMatchDriver = 0;
        int CountMatchVersionDriver = 0;
        int CountDriverV = 0;
        int CountDrvrMissing = 0;

        int CountFileFolder = 0;
        int CountMatchFileFolder = 0;
        int CountFile = 0;

        int CountRegistry = 0;
        int CountMatchRegistry = 0;
        int CountReg = 0;

        int CountHotFix = 0;
        int CountMatchHotFix = 0;
        int CountHot = 0;

        ucCustomTreeCodeBehind treeControl;
        #endregion

        public ImageValidationClient()
        {
            InitializeComponent();
            LoadComputerInfo();

            treeControl = new ucCustomTreeCodeBehind();
            ShowTreeControl(treeControl);

            // Initilize log4net only when you want to start logging.
            log4net.Config.XmlConfigurator.Configure();
        }

        private void ShowTreeControl(Control control)
        {
            control.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            control.Width = double.NaN;
            control.Height = double.NaN;

            treeContainer.Children.Clear();
            treeContainer.Children.Add(control);
        }

        private void btnvalidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.WriteAllText(@"../../Report/ImageReport.csv", string.Empty);

                //lblprocess.Content = "Processing...";                
                BackgroundWorker worker = new BackgroundWorker();

                // Long running process !
                worker.DoWork += (o, ea) =>
                {
                    // Toggling the look&feel between ProgressCloud and Tree Summary data
                    Dispatcher.Invoke((Action)(() => treeControl.ClearTree()));
                    Dispatcher.Invoke((Action)(() => progressCloud.Visibility = System.Windows.Visibility.Visible));
                    Dispatcher.Invoke((Action)(() => treegbx.Visibility = System.Windows.Visibility.Hidden));

                    Computer ObjComp = compInfo.GetComputerInformation(); //Getting computer information from system

                    #region Get Computer ID By Model & Product

                    int ComputerIDByModel = sRef.SeleteComputerInfoByModel(ObjComp.Model); //Return ComputerID By Model
                    int ComputerIDByProduct = sRef.SeleteComputerInfoByProduct(ObjComp.Product); //Return ComputerID By Product

                    #endregion

                    if (ComputerIDByModel > 0)
                    {
                        BindData(ComputerIDByModel);
                    }
                    else if (ComputerIDByProduct > 0)
                    {
                        BindData(ComputerIDByProduct);
                    }
                    else
                    {
                        // Computer information not exist in azure database
                    }

                    //no direct interaction with the UI is allowed from this method
                    for (int i = 0; i < 100; i++)
                    {
                        System.Threading.Thread.Sleep(50);
                    }
                };

                worker.RunWorkerCompleted += (o, ea) =>
                {
                    //work has completed. you can now interact with the UI
                    _busyIndicator.IsBusy = false;
                };

                //set the IsBusy before you start the thread
                _busyIndicator.IsBusy = true;
                worker.RunWorkerAsync();
            }
            catch (Exception ex)
            { throw ex; }
        }

        private void BindData(int RecordID)
        {
            ComputerID = RecordID;

            ImageSource imgThumbUp = new BitmapImage(new Uri(@"/ImageValidation.Client;component/Images/Thumb-up.png", UriKind.Relative));

            ImageSource imgThumbDown = new BitmapImage(new Uri(@"/ImageValidation.Client;component/Images/Thumb-down.png", UriKind.Relative));

            #region Bind All DataGrid - Applications, Drivers, HotFixes, FileFolders, Registry etc after comparison & XML Serializer

            ComputerXml Mainobj = new ComputerXml();
            XmlDocument xmldoc = new XmlDocument();
            XmlSerializer ser = new XmlSerializer(Mainobj.GetType());

            #region Application
            //Comparing System`s Applications With Azure Database (Applications)
            List<Applications> allApplications = CompareApplications(ComputerID, xmldoc, ser);

            //Count if red,yellow,green exist 
            int CountAppRed = CompareApplications(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 0);
            int CountAppYellow = CompareApplications(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 1);
            CountAppV = CountAppRed;// CountAppYellow;
            CountAppMissing = CountAppYellow;// CountAppRed;

            #endregion
            #region Driver
            //Comparing System`s Drivers With Azure Database (Drivers)
            List<Driver> allDrivers = CompareDrivers(ComputerID, xmldoc, ser);

            //Count if red,yellow,green exist 
            int CountDvrRed = CompareDrivers(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 0);
            int CountDvrYellow = CompareDrivers(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 1);
            CountDrvrMissing = CountDvrRed;
            CountDriverV = CountDvrYellow;

            #endregion
            #region HotFixes
            //Comparing System`s HotFixes With Azure Database (Drivers)
            List<HotFix> allHotFixes = CompareHotFixes(ComputerID, xmldoc, ser);
            //CountHot = CountMatchHotFix;
            //Count if red,yellow,green exist 
            int CounthotRed = CompareHotFixes(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountHot = CounthotRed;

            #endregion
            #region File Folder
            //Comparing System`s FileFolders With Azure Database (FileFolders)
            List<FileFolder> allFileFolders = CompareFileFolder(ComputerID, xmldoc, ser);
            // CountFile = CountMatchFileFolder;
            //Count if red,yellow,green exist 
            int CountfileRed = CompareFileFolder(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountFile = CountfileRed;

            #endregion
            #region Registry
            //Comparing System`s Registry With Azure Database (FileFolders)
            List<RegistryGroupData> allRegisters = CompareRegistry(ComputerID, xmldoc, ser);
            CountReg = CountMatchRegistry;
            //Count if red,yellow,green exist 
            int CountregRed = CompareRegistry(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountReg = CountregRed;

            #endregion

            #endregion

            #region Summary section messages related to all if computer information exist after that check other info

            if (CountAppMissing > 0 || CountDrvrMissing > 0 || CountFile > 0 || CountHot > 0 || CountReg > 0)
            { // Overall Validation Failed
            }
            else
            { // Overall Validation Pass
            }

            /* APPLICATION */
            if (CountApplication > 0)
            {
                // lblapp.Content = " Application Validation: " + ((CountApp > 0) ? "Failed: " + CountApp + "Missing Software" + Mismatchappversion : "Passed");
                //imgthumbapp.Source = CountApp > 0 ? imgThumbDown : imgThumbUp;
                // Only show if Failed!
                if (CountAppMissing > 0)
                {
                    // TreeView update: Application
                    Dispatcher.Invoke((Action)(() => treeControl.LoadData(CountAppMissing, CountAppV, allApplications, this.log)));
                }
                else
                {
                    // Dispatcher.Invoke((Action)(() => imgthumbapp.Visibility = System.Windows.Visibility.Visible));
                }
            }
            else
            { // Overall Validation Failed
            }

            /* DRIVER */
            if (CountDriver > 0)
            {
                if (CountDrvrMissing > 0)
                {
                    // TreeView update: Driver
                    Dispatcher.Invoke((Action)(() => treeControl.LoadData(CountDrvrMissing, CountDriverV, allDrivers, this.log)));
                }
                else
                {
                    // Dispatcher.Invoke((Action)(() => imgthumbdriver.Visibility = System.Windows.Visibility.Visible));
                }
            }
            else
            { // No Driver Found
            }

            /* FILE & FOLDER */
            if (CountFileFolder > 0)
            {
                if (CountFile > 0)
                {
                    // Tree update: File and Folder
                    Dispatcher.Invoke((Action)(() => treeControl.LoadData(CountFile, allFileFolders, this.log)));
                }
                else
                {
                }
            }
            else
            {    // File Folder not found!!	
            }

            /* HOTFIX */
            if (CountHotFix > 0)
            {
                if (CountHot > 0)
                {
                    // Tree update: HotFix
                    Dispatcher.Invoke((Action)(() => treeControl.LoadData(CountHot, allHotFixes, this.log)));
                }
                else
                {
                }
            }
            else
            {    //  HotFix not Found!!
            }

            /* REGISTRY */
            if (CountRegistry > 0)
            {
                // Tree update: Registry
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(CountReg, allRegisters, this.log)));
            }
            else
            {   // Registry not Found!!
            }

            #endregion

            // Display the Tree Summary data
            Dispatcher.Invoke((Action)(() => ShowTreeControl(treeControl)));
            Dispatcher.Invoke((Action)(() => treegbx.Visibility = System.Windows.Visibility.Visible));
            Dispatcher.Invoke((Action)(() => progressCloud.Visibility = System.Windows.Visibility.Hidden));
        }

        private string ReadFile(string path)
        {
            try
            {
                StreamReader sr = new StreamReader(path);
                string data = sr.ReadToEnd();
                sr.Close();
                return data;
            }

            catch (Exception exception)
            {
                //handle error
                return exception.Message;
            }
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
            txtserial.Text = ObjComp.SerialNumber;
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
                Computer ObjComp = compInfo.GetComputerInformation();
                List<Applications> ObjAppLst = new List<Applications>();//= appsInfo.GetApplicationInformation();//Getting from system

                string xmlApp = sRef.SelectApplicationsByComputerID(ComputerID); //Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlApp);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                Applications[] arrApplications = myObj.Applications;

                string key32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                string key64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                if (ObjComp.OSArchitecture == "32-bit")
                {
                    ObjAppLst = appsInfo.GetApplicationInformation32(key32);
                }
                else if (ObjComp.OSArchitecture == "64-bit")
                {
                    ObjAppLst = appsInfo.GetApplicationInformation64(key64);
                }
                else
                { }

                List<Applications> applications = new List<Applications>(); //In this items added from azure database and its return with list of items
                foreach (var app in arrApplications)
                {
                    int status = 2;
                    int index = -1;
                    if (ObjAppLst.Any(y => y.DisplayName == app.DisplayName))
                    {
                         index = ObjAppLst.FindIndex(y => y.DisplayName == app.DisplayName);
                    }
                    else
                    {
                        status = 1;
                        CountMatchApplication++;
                    }

                    if (ObjAppLst.Any(y => y.Version == app.Version))
                    {
                        index = ObjAppLst.FindIndex(y => y.Version == app.Version);
                    }
                    else
                    {
                        status = 0;
                        CountMatchVersionApplication++;
                    }
                       
                    Console.WriteLine("---------------- PRINT SOFTWARE OBJ ------------ " +  index);
                    Console.WriteLine(" ApplicationUrl: "  + ((index >= 0) ? " BASE: " + ObjAppLst[index].ApplicationUrl : " CURR: " + app.ApplicationUrl));
                    Console.WriteLine(" Comments: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].Comments : " CURR: " + app.Comments));
                    Console.WriteLine(" Contact: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].Contact : " CURR: " + app.Contact));
                    Console.WriteLine(" DisplayName: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].DisplayName : " CURR: " + app.DisplayName));
                   Console.WriteLine(" DisplayVersion: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].DisplayVersion : " CURR: " + app.DisplayVersion));
                   /* Console.WriteLine(" EstimatedSize: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].EstimatedSize : " CURR: " + app.EstimatedSize));
                   Console.WriteLine(" HelpLink: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].HelpLink : " CURR: " + app.HelpLink));
                   Console.WriteLine(" HelpTelephone: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].HelpTelephone : " CURR: " + app.HelpTelephone));
                   Console.WriteLine(" InstallDate: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].InstallDate : " CURR: " + app.InstallDate));
                   Console.WriteLine(" InstallLocation: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].InstallLocation : " CURR: " + app.InstallLocation));
                   Console.WriteLine(" InstallSource: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].InstallSource : " CURR: " + app.InstallSource));
                   Console.WriteLine(" IsRequired: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].IsRequired : " CURR: " + app.IsRequired));
                   Console.WriteLine(" Language: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].Language : " CURR: " + app.Language));
                   Console.WriteLine(" ModifyPath: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].ModifyPath : " CURR: " + app.ModifyPath));
                   Console.WriteLine(" Publisher: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].Publisher : " CURR: " + app.Publisher));
                   Console.WriteLine(" ReadMe: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].ReadMe : " CURR: " + app.ReadMe));
                   Console.WriteLine(" UninstallString: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].UninstallString : " CURR: " + app.UninstallString));
                   Console.WriteLine(" UrlInfoAbout: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].UrlInfoAbout : " CURR: " + app.UrlInfoAbout));
                   Console.WriteLine(" URLUpdateInfo: " + ((index >= 0) ? " BASE: " + ObjAppLst[index].URLUpdateInfo : " CURR: " + app.URLUpdateInfo));*/
                    /*               ApplicationUrl:  BASE: 
               Comments:  BASE: 
               Contact:  BASE: 
               DisplayName:  BASE: MSXML 4.0 SP3 Parser
               DisplayVersion:  BASE: 4.30.2100.0
               EstimatedSize:  BASE: 1512
               HelpLink:  BASE: http://www.msdn.microsoft.com/xml
               HelpTelephone:  BASE: 
               InstallDate:  BASE: 20130227
               InstallLocation:  BASE: c:\Windows\SysWOW64\
               InstallSource:  BASE: c:\util\sonic\MSXMLMSI_40SP3\
               IsRequired:  BASE: 0
               Language:  BASE: 1033
               ModifyPath:  BASE: MsiExec.exe /I{196467F1-C11F-4F76-858B-5812ADC83B94}
               Publisher:  BASE: Microsoft Corporation
               ReadMe:  BASE: 
               UninstallString:  BASE: MsiExec.exe /I{196467F1-C11F-4F76-858B-5812ADC83B94}
               UrlInfoAbout:  BASE: 
               URLUpdateInfo:  BASE: 
               Version:  BASE: 69077044*/
             
                    applications.Add(new Applications()
                    {
                        DisplayName = app.DisplayName,
                        BaseAppVersion =  app.DisplayVersion,
                        Version = ((index >= 0) ? ObjAppLst[index].DisplayVersion : ""),
                        IsCompared = status,
                        HelpLink = ((index >= 0) ? ObjAppLst[index].HelpLink : ""),
                        InstallLocation = ((index >= 0) ? ObjAppLst[index].InstallLocation: ""),
                        InstallSource = ((index >= 0) ? ObjAppLst[index].InstallSource : ""),
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
                List<Driver> baseDriverLst = driverInfo.GetDriverInfo();//Getting from system
                string xmlDriver = sRef.SelectDriverByComputerID(ComputerID);//Getting from azure database by computerID in xml format

                xmldoc.LoadXml(xmlDriver);
                XmlNodeReader reader = new XmlNodeReader(xmldoc.DocumentElement);
                object obj = ser.Deserialize(reader);
                ComputerXml myObj = (ComputerXml)obj;
                Driver[] arrDrivers = myObj.Driver;

                List<Driver> driver = new List<Driver>();  //In this items added from azure database and its return with list of items
                foreach (var currDrivers in arrDrivers)
                {
                    int status = 0;
                    int index = -1;
                    if (baseDriverLst.Any(y => y.DeviceName == currDrivers.DeviceName))
                    {
                        index = baseDriverLst.FindIndex(y => y.DeviceName == currDrivers.DeviceName);
                        status = 1;
                    }
                    else
                    {
                        CountMatchDriver++;
                    }

                    if (baseDriverLst.Any(y => y.DriverVersion == currDrivers.DriverVersion))
                    {
                        index = baseDriverLst.FindIndex(y => y.DriverVersion == currDrivers.DriverVersion);
                        status = 2;
                    }
                    else
                    {
                        CountMatchVersionDriver++;
                    }
                    if (index >= 0)
                    { // Mismatched Drivers!!
                        /*    Console.WriteLine("---------------- MISMATCH OBJ ------------ ");
                               Console.WriteLine("MMID: " + index + " DeviceID: " + currDrivers.DeviceID);
                               Console.WriteLine("BASE DeviceName: " + currDrivers.DeviceName + " LOCAL IS: " + baseDriverLst[index].DeviceName);
                               Console.WriteLine("VERSION REQUIRED: " + baseDriverLst[index].DriverVersion + " CUrrent Version: " + currDrivers.DriverVersion);
                               Console.WriteLine("IDDriverProviderName: " + currDrivers.DriverProviderName + " CUrrent: " + baseDriverLst[index].DriverProviderName);
                               Console.WriteLine("friendlyName: " + currDrivers.friendlyName + " CUrrent: " + baseDriverLst[index].friendlyName);
                               Console.WriteLine("******httpUrl: " + currDrivers.httpUrl + " CUrrent: " + baseDriverLst[index].httpUrl + "******");
                         Console.WriteLine("HardWareID: " + currDrivers.HardWareID);
                         Console.WriteLine("InfName: " + currDrivers.InfName + " CUrrent: " + baseDriverLst[index].InfName);
                           Console.WriteLine("IsRequired: " + currDrivers.IsRequired);
                           Console.WriteLine("IsSigned: " + currDrivers.IsSigned);
                           Console.WriteLine("Manufacturer: " + currDrivers.Manufacturer);
                           Console.WriteLine("Name: " + currDrivers.Name);
                           Console.WriteLine("PDO: " + currDrivers.PDO); */
                    }
                    else
                    { // Missing Current Driver!!
                        // Console.WriteLine("---------------- MISSING OBJ ------------ ");

                    }

                    driver.Add(new Driver()
                    {
                        DeviceName = currDrivers.DeviceName,
                        DriverVersion = (index >= 0) ? baseDriverLst[index].DriverVersion : "", // else: missing current driver
                        BaseDriverVersion = currDrivers.DriverVersion,
                        IsCompared = status,
                        Description = currDrivers.Description,
                        InfName = (index >= 0) ? baseDriverLst[index].InfName : currDrivers.InfName,
                        Manufacturer = currDrivers.Manufacturer,
                        httpUrl = currDrivers.httpUrl,
                        HardWareID = currDrivers.HardWareID,
                        DriverProviderName = (index >= 0) ? baseDriverLst[index].DriverProviderName : currDrivers.DriverProviderName,
                        friendlyName = currDrivers.friendlyName
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
                foreach (var htfix in arrHotfix)
                {
                    bool status = false;

                    if (ObjHotFix.Any(y => y.HotFixIDs == htfix.HotFixIDs))
                    {
                        status = true;

                    }
                    else
                    {
                        CountMatchHotFix++;
                    }

                   Console.WriteLine("---------------- PRINT HOTFIX OBJ ------------ ");
                   Console.WriteLine("CSName: " + htfix.CSName);
                    Console.WriteLine("Description: " + htfix.Description);
                    Console.WriteLine("HotFixIDs: " + htfix.HotFixIDs);
                    Console.WriteLine("InstallDate: " + htfix.InstallDate);
                    Console.WriteLine("InstalledBy: " + htfix.InstalledBy);
                    Console.WriteLine("IsRequired: " + htfix.IsRequired);           

                    hotfix.Add(new HotFix()
                    {
                        HotFixIDs = htfix.HotFixIDs,
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
                    if (filefolder.Location.Contains("."))
                    {
                        if (File.Exists(filefolder.Location))
                        {
                            status = true;

                        }
                        else
                        {
                            CountMatchFileFolder++;
                        }
                    }
                    else
                    {
                        if (Directory.Exists(filefolder.Location))
                        {
                            status = true;

                        }
                        else
                        {
                            CountMatchFileFolder++;
                        }
                    }

                    /*Console.WriteLine("---------------- PRINT FILE FOLDER OBJ ------------ ");
                    Console.WriteLine(" FileFolderTypeID: " + filefolder.FileFolderTypeID);
                    Console.WriteLine("Location: " + filefolder.Location);
                    Console.WriteLine("NOTE: " + filefolder.Note);
                    Console.WriteLine("Type: " + (filefolder.FileFolderTypeID == 1 ? "File" : "Folder"));*/

                    filefolders.Add(new FileFolder()
                    {
                        Location = filefolder.Location,
                        Note = filefolder.Note.Replace("<p>", "").Replace("</p>",""),
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
        private List<RegistryGroupData> CompareRegistry(int ComputerID, XmlDocument xmldoc, XmlSerializer ser)
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

                List<RegistryGroupData> registry = new List<RegistryGroupData>();//In this items added from azure database and its return with list of items
                foreach (var registrygroup in arrRegistryGroup)
                {
                    bool Status = false;

                    // var RegData = arrRegistry.Where(p => !arrRegistry.Any(q => (p != q && p.ValueData == q.ValueData && p.Key == q.Key)));                  
                    var RegData = arrRegistry.GroupBy(x => new { x.ValueData, x.Key }).Select(y => y.First());

                    foreach (var item in RegData.Where(x => x.RegistryGroupID == registrygroup.RegistryGroupID))
                    {
                        string keystring = item.Key.Replace(@"HKEY_CURRENT_USER\", "");
                        keystring = keystring.Replace(@"HKEY_LOCAL_MACHINE\", "");
                        keystring = keystring.Replace(@"HKEY_CLASSES_ROOT\", "");
                        keystring = keystring.Replace(@"HKEY_USERS\", "");
                        keystring = keystring.Replace(@"HKEY_CURRENT_CONFIG\", "");
                        RegistryKey regKeyCurrentUser = Registry.CurrentUser.OpenSubKey(keystring);
                        RegistryKey regKeyClassesRoot = Registry.ClassesRoot.OpenSubKey(keystring);
                        RegistryKey regKeyLocalMachine = Registry.LocalMachine.OpenSubKey(keystring);
                        RegistryKey regKeyUsers = Registry.Users.OpenSubKey(keystring);
                        RegistryKey regKeyCurrentConfig = Registry.CurrentConfig.OpenSubKey(keystring);
                        if (regKeyCurrentUser != null)
                        {
                            //RegistryValueKind rvk = regKeyAppRoot.GetValueKind(item.Value);
                            string strValue = Convert.ToString(regKeyCurrentUser.GetValue(item.Value));
                            if (strValue == item.ValueData)
                            {
                                Status = true;
                            }
                        }
                        else if (regKeyClassesRoot != null)
                        {
                            //RegistryValueKind rvk = regKeyAppRoot.GetValueKind(item.Value);
                            string strValue = Convert.ToString(regKeyClassesRoot.GetValue(item.Value));
                            if (strValue == item.ValueData)
                            {
                                Status = true;
                            }
                        }
                        else if (regKeyUsers != null)
                        {
                            //RegistryValueKind rvk = regKeyAppRoot.GetValueKind(item.Value);
                            string strValue = Convert.ToString(regKeyUsers.GetValue(item.Value));
                            if (strValue == item.ValueData)
                            {
                                Status = true;
                            }
                        }
                        else if (regKeyCurrentConfig != null)
                        {
                            //RegistryValueKind rvk = regKeyAppRoot.GetValueKind(item.Value);
                            string strValue = Convert.ToString(regKeyCurrentConfig.GetValue(item.Value));
                            if (strValue == item.ValueData)
                            {
                                Status = true;
                            }
                        }
                        else if (regKeyLocalMachine != null)
                        {
                            //RegistryValueKind rvk = regKeyAppRoot.GetValueKind(item.Value);
                            string strValue = Convert.ToString(regKeyLocalMachine.GetValue(item.Value));
                            if (strValue == item.ValueData)
                            {
                                Status = true;
                            }
                        }

                        if (Status == false)
                        {
                            CountMatchRegistry++;
                        }

                        Console.WriteLine("---------------- PRINT REGISTER OBJ ------------ ");
                        Console.WriteLine(" DataType: " + item.DataType);
                        Console.WriteLine(" Key: " + item.Key);
                        Console.WriteLine(" RegistryGroupID: " + item.RegistryGroupID);
                        Console.WriteLine(" Value: " + item.Value);
                        Console.WriteLine(" ValueData: " + item.ValueData);
                        Console.WriteLine(" registrygroup.FileName: " + registrygroup.FileName);
                        Console.WriteLine(" registrygroup.Note: " + registrygroup.Note.Replace("<p>", "").Replace("</p>", ""));
                           
                        registry.Add(new RegistryGroupData()
                        {
                            RegKey = item.Key,// registrygroup.RegistryGroupID,
                            Note = registrygroup.Note.Replace("<p>", "").Replace("</p>", ""),
                            Type = item.DataType,
                            FileName = registrygroup.FileName,
                            Value = item.Value,
                            ValueData = item.ValueData,
                            IsCompared = Status
                        });
                    }

                    CountRegistry++;
                }
                return registry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion

        private void DetailReportBtn_Click(object sender, RoutedEventArgs e)
        {
            var rootAppender = ((log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository()).Root.Appenders.OfType<log4net.Appender.FileAppender>().FirstOrDefault();
            string filename = rootAppender != null ? rootAppender.File : string.Empty;

            OutputForm outputform = new OutputForm(filename);
            outputform.Visible = true;
        }

    }
}

