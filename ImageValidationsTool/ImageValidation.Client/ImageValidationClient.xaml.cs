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
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using ImageValidation.Client.Controls;

namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for ImageValidationClient.xaml
    /// </summary>
    /// 
    
    public partial class ImageValidationClient : PageFunction<String>
    {
       
        #region Declaration and Initialization section

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
        int CountApp = 0;


        int CountDriver = 0;
        int CountMatchDriver = 0;
        int CountMatchVersionDriver = 0;
        int CountDriverV = 0;
        int CountDrvr = 0;

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
           // LoadComputerInfo();

            treeControl = new ucCustomTreeCodeBehind();
            ShowTreeControl(treeControl, "Custom Tree");
        }
        
        private void ShowTreeControl(Control control, string title)
        {
            control.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            control.Width = double.NaN;
            control.Height = double.NaN;

            gvContainer.Children.Clear();
            gvContainer.Children.Add(control);
        }

        private void btnvaidate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //lblprocess.Content = "Processing...";                
                BackgroundWorker worker = new BackgroundWorker();
               
                // Long running process !
                worker.DoWork += (o, ea) =>
                {
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
                        #region Summary messages displays if computer information not exist in azure database

                        lbloverall.Content = "- Overall Validation: Overall failed (Result not found).";
                        lblapp.Content = "- Application Validation: Not Found Any Record.";
                        lbldriver.Content = "- Driver Validation: Not Found Any Record.";
                        lblfilefolder.Content = "- File and Folder Validation: Not Found Any Record.";
                        lblhotfixes.Content = "- Microsoft HotFix Validation: Not Found Any Record.";
                        lblregistry.Content = "- Registry Validation: Not Found Any Record.";

                        #endregion
                        lblprocess.Visibility = System.Windows.Visibility.Hidden;
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
            //dataGrid_app.ItemsSource = CompareApplications(ComputerID, xmldoc, ser);
            List<Applications> allApplications =  CompareApplications(ComputerID, xmldoc, ser);
            Dispatcher.Invoke((Action)(() => dataGrid_app.ItemsSource = allApplications));

            //Count if red,yellow,green exist 
            int CountAppRed = CompareApplications(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 0);
            int CountAppYellow = CompareApplications(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 1);

            CountAppV = CountAppYellow;
            CountApp = CountAppRed;

            if (CountAppRed > 0)
            {
                //elpRedapp.Visibility = System.Windows.Visibility.Visible;
                Dispatcher.Invoke((Action)(() => elpRedapp.Visibility = System.Windows.Visibility.Visible));
            }
            else if (CountAppYellow > 0)
            {
                //elpYellowapp.Visibility = System.Windows.Visibility.Visible;
                Dispatcher.Invoke((Action)(() => elpYellowapp.Visibility = System.Windows.Visibility.Visible));
            }
            else
            {
                //elpGreenapp.Visibility = System.Windows.Visibility.Visible;
                Dispatcher.Invoke((Action)(() => elpGreenapp.Visibility = System.Windows.Visibility.Visible));
            }
            #endregion
            #region Driver
            //Comparing System`s Drivers With Azure Database (Drivers)
            //dataGrid_driver.ItemsSource = CompareDrivers(ComputerID, xmldoc, ser);
            List<Driver> allDrivers = CompareDrivers(ComputerID, xmldoc, ser);
            Dispatcher.Invoke((Action)(() => dataGrid_driver.ItemsSource = allDrivers));

            //Count if red,yellow,green exist 
            int CountDvrRed = CompareDrivers(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 0);
            int CountDvrYellow = CompareDrivers(ComputerID, xmldoc, ser).Count(x => x.IsCompared == 1);
            CountDrvr = CountDvrRed;
            CountDriverV = CountDvrYellow;
            if (CountDvrRed > 0)
            {
                //elpRedDrv.Visibility = System.Windows.Visibility.Visible;
                Dispatcher.Invoke((Action)(() => elpRedDrv.Visibility = System.Windows.Visibility.Visible));
            }
            else if (CountDvrYellow > 0)
            {
                //elpYellowDrv.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpYellowDrv.Visibility = System.Windows.Visibility.Visible));
            }
            else
            {
                //elpGreenDrv.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() =>elpGreenDrv.Visibility = System.Windows.Visibility.Visible));
            }
            #endregion
            #region HotFixes
            //Comparing System`s HotFixes With Azure Database (Drivers)
            //dataGrid_hotfix.ItemsSource = CompareHotFixes(ComputerID, xmldoc, ser);
            List<HotFix> allHotFixes = CompareHotFixes(ComputerID, xmldoc, ser);
            Dispatcher.Invoke((Action)(() => dataGrid_hotfix.ItemsSource = allHotFixes));
            //CountHot = CountMatchHotFix;
            //Count if red,yellow,green exist 
            int CounthotRed = CompareHotFixes(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountHot = CounthotRed;
            if (CounthotRed > 0)
            {
                //elpRedhot.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpRedhot.Visibility = System.Windows.Visibility.Visible));
            }
            else
            {
                //elpGreenhot.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpGreenhot.Visibility = System.Windows.Visibility.Visible));
            }
            #endregion
            #region File Folder
            //Comparing System`s FileFolders With Azure Database (FileFolders)
            //dataGrid_filefolder.ItemsSource = CompareFileFolder(ComputerID, xmldoc, ser);
            List<FileFolder> allFileFolders = CompareFileFolder(ComputerID, xmldoc, ser);
            Dispatcher.Invoke((Action)(() => dataGrid_filefolder.ItemsSource = allFileFolders));
            // CountFile = CountMatchFileFolder;
            //Count if red,yellow,green exist 
            int CountfileRed = CompareFileFolder(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountFile = CountfileRed;
            if (CountfileRed > 0)
            {
                //elpRedfile.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpRedfile.Visibility = System.Windows.Visibility.Visible));
            }
            else
            {
                //elpGreenfile.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpGreenfile.Visibility = System.Windows.Visibility.Visible));
            }
            #endregion
            #region Registry
            //Comparing System`s Registry With Azure Database (FileFolders)
            //dataGrid_registr.ItemsSource = CompareRegistry(ComputerID, xmldoc, ser);
            List<RegistryGroupData> allRegisters = CompareRegistry(ComputerID, xmldoc, ser);
            Dispatcher.Invoke((Action)(() => dataGrid_registr.ItemsSource = allRegisters));
           // CountReg = CountMatchRegistry;
            //Count if red,yellow,green exist 
            int CountregRed = CompareRegistry(ComputerID, xmldoc, ser).Count(x => x.IsCompared == false);
            CountReg = CountregRed;
            if (CountregRed > 0)
            {
                //elpRedreg.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpRedreg.Visibility = System.Windows.Visibility.Visible));
            }
            else
            {
                //elpGreenreg.Visibility = System.Windows.Visibility.Visible;
                 Dispatcher.Invoke((Action)(() => elpGreenreg.Visibility = System.Windows.Visibility.Visible));
            }
            #endregion
            //_grdMainlContent.Visibility = System.Windows.Visibility.Visible; //Display DataGrids Container

            #endregion

            #region Summary section messages related to all if computer information exist after that check other info

            if (CountApp > 0 || CountDrvr > 0 || CountFile > 0 || CountHot > 0 || CountReg > 0)
            {
                //lbloverall.Content = " Overall Validation: Failed";
                Dispatcher.Invoke((Action)(() => lbloverall.Content = " Overall Validation: Failed"));
                //imgthumboverall.Source = imgThumbDown; 
				Dispatcher.Invoke((Action)(() => imgthumboveralld.Visibility = System.Windows.Visibility.Visible));           
            }
            else
            {
                //lbloverall.Content = " Overall Validation: Passed";
                Dispatcher.Invoke((Action)(() => lbloverall.Content = " Overall Validation: Passed"));
                //imgthumboverall.Source = imgThumbUp;
				Dispatcher.Invoke((Action)(() => imgthumboverall.Visibility = System.Windows.Visibility.Visible));          
            }

            string Mismatchappversion = ((CountAppV > 0) ? ", " + CountAppV + " Version Mismatch" : "");
            string Mismatchdriverversion = ((CountDriverV > 0) ? ", " + CountDriverV + " Version Mismatch" : "");
            if (CountApplication > 0)
            {
               // lblapp.Content = " Application Validation: " + ((CountApp > 0) ? "Failed: " + CountApp + "Missing Software" + Mismatchappversion : "Passed");
                //imgthumbapp.Source = CountApp > 0 ? imgThumbDown : imgThumbUp;
				 Dispatcher.Invoke((Action)(() => lblapp.Content = " Application Validation: " + ((CountApp > 0) ? "Failed: " + CountApp + "Missing Software" + Mismatchappversion : "Passed")));
            
				if (CountApp > 0)
               	{
                   Dispatcher.Invoke((Action)(() => imgthumbappd.Visibility = System.Windows.Visibility.Visible));
               	}
               	else
               	{
                   Dispatcher.Invoke((Action)(() => imgthumbapp.Visibility = System.Windows.Visibility.Visible));
               	}

                // Tree update: Application
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(lblapp.Content, allApplications)));
			}
            else
            {
                /*lbloverall.Content = " Overall Validation: Overall failed (Result not found).";
                dataGrid_app.Visibility = System.Windows.Visibility.Hidden;
                lblappval.Content = "Application Validation Record Not Found!!";
                imgthumbapp.Source = imgThumbUp;
                elpRedapp.Visibility = System.Windows.Visibility.Hidden;
                elpYellowapp.Visibility = System.Windows.Visibility.Hidden;
                elpGreenapp.Visibility = System.Windows.Visibility.Hidden;*/
				 Dispatcher.Invoke((Action)(() => lbloverall.Content = " Overall Validation: Overall failed (Result not found)."));
                Dispatcher.Invoke((Action)(() => dataGrid_app.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => lblappval.Content = "Application Validation Record Not Found!!"));
               // imgthumbapp.Source = imgThumbUp;
               Dispatcher.Invoke((Action)(() => imgthumbapp.Visibility = System.Windows.Visibility.Visible));
               Dispatcher.Invoke((Action)(() => elpRedapp.Visibility = System.Windows.Visibility.Hidden));
               Dispatcher.Invoke((Action)(() => elpYellowapp.Visibility = System.Windows.Visibility.Hidden));
               Dispatcher.Invoke((Action)(() => elpGreenapp.Visibility = System.Windows.Visibility.Hidden));
            }

            if (CountDriver > 0)
            {
                //lbldriver.Content = " Driver Validation: " + ((CountDrvr > 0) ? "Failed: " + CountDrvr + " Missing Software" + Mismatchdriverversion : "Passed");
                //imgthumbdriver.Source = CountDrvr > 0 ? imgThumbDown : imgThumbUp;
				Dispatcher.Invoke((Action)(() => lbldriver.Content = " Driver Validation: " + ((CountDrvr > 0) ? "Failed: " + CountDrvr + " Missing Software" + Mismatchdriverversion : "Passed")));
                if (CountDrvr > 0)
                {
                    Dispatcher.Invoke((Action)(() => imgthumbdriverd.Visibility = System.Windows.Visibility.Visible));
                }
                else
                {
                    Dispatcher.Invoke((Action)(() => imgthumbdriver.Visibility = System.Windows.Visibility.Visible));
                }

                // Tree update: Driver
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(lbldriver.Content, allDrivers)));
            }
            else
            {
                /*lbldriver.Content = " Driver Validation: Not Found Any Record.";
                dataGrid_driver.Visibility = System.Windows.Visibility.Hidden;
                lblDrvr.Content = "Driver Validation Record Not Found!!";
                imgthumbdriver.Source = imgThumbUp;
                elpRedDrv.Visibility = System.Windows.Visibility.Hidden;
                elpYellowDrv.Visibility = System.Windows.Visibility.Hidden;
                elpYellowDrv.Visibility = System.Windows.Visibility.Hidden;*/
				 Dispatcher.Invoke((Action)(() => lbldriver.Content = " Driver Validation: Not Found Any Record."));
                //Dispatcher.Invoke((Action)(() => dataGrid_driver.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => lblDrvr.Content = "Driver Validation Record Not Found!!"));
               // imgthumbdriver.Source = imgThumbUp;
               Dispatcher.Invoke((Action)(() => imgthumbdriver.Visibility = System.Windows.Visibility.Visible));
               Dispatcher.Invoke((Action)(() => elpRedDrv.Visibility = System.Windows.Visibility.Hidden));
               Dispatcher.Invoke((Action)(() => elpYellowDrv.Visibility = System.Windows.Visibility.Hidden));
               Dispatcher.Invoke((Action)(() => elpYellowDrv.Visibility = System.Windows.Visibility.Hidden));
            }

            if (CountFileFolder > 0)
            {
                //lblfilefolder.Content = " File and Folder Validation: " + ((CountFile > 0) ? "Failed: " + CountFile + " Missing Software" : "Passed");
                //imgthumbfilefolder.Source = CountFile > 0 ? imgThumbDown : imgThumbUp;
				 Dispatcher.Invoke((Action)(() => lblfilefolder.Content = " File and Folder Validation: " + ((CountFile > 0) ? "Failed: " + CountFile + " Missing Software" : "Passed")));
                if (CountFile > 0)
                {
                    Dispatcher.Invoke((Action)(() => imgthumbfilefolderd.Visibility = System.Windows.Visibility.Visible));
                }
                else
                {
                    Dispatcher.Invoke((Action)(() => imgthumbfilefolder.Visibility = System.Windows.Visibility.Visible));
                }

                // Tree update: File and Folder
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(lblfilefolder.Content, allFileFolders)));
            }
            else
            {    // File Folder not found!!	
                // Hide Grid
                Dispatcher.Invoke((Action)(() => lblfilefolder.Content = " File and Folder Validation: Not Found Any Record."));
               // Dispatcher.Invoke((Action)(() => dataGrid_filefolder.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => lblfilefolderval.Content = "File Folder validation Record Not Found!!"));
                // Show/Hide Thumbnails
                Dispatcher.Invoke((Action)(() => imgthumbfilefolder.Visibility = System.Windows.Visibility.Visible));
                Dispatcher.Invoke((Action)(() => elpRedfile.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => elpGreenfile.Visibility = System.Windows.Visibility.Hidden));
            }

            if (CountHotFix > 0)
            {
               // lblhotfixes.Content = " Microsoft HotFix Validation: " + ((CountHot > 0) ? "Failed: " + CountHot + " Missing Software" : "Passed");
                //imgthumbhotfixes.Source = CountHot > 0 ? imgThumbDown : imgThumbUp;
				Dispatcher.Invoke((Action)(() => lblhotfixes.Content = " Microsoft HotFix Validation: " + ((CountHot > 0) ? "Failed: " + CountHot + " Missing Software" : "Passed")));
                if (CountHot > 0)
                {
                    Dispatcher.Invoke((Action)(() => imgthumbhotfixesd.Visibility = System.Windows.Visibility.Visible));
                }
                else
                {
                    Dispatcher.Invoke((Action)(() => imgthumbhotfixes.Visibility = System.Windows.Visibility.Visible));
                }

                // Tree update: HotFix
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(lblhotfixes.Content, allHotFixes)));
            }
            else
            {    //  HotFix not Found!!
                // Hide Grid
				Dispatcher.Invoke((Action)(() => lblhotfixes.Content = " Microsoft HotFix Validation: Not Found Any Record."));
                Dispatcher.Invoke((Action)(() => lblhotfixval.Content = "Microsoft HotFix Validation Record Not Found!!"));
               // Dispatcher.Invoke((Action)(() => dataGrid_hotfix.Visibility = System.Windows.Visibility.Hidden));
                // Show/Hide Thumbnails
                Dispatcher.Invoke((Action)(() => imgthumbhotfixes.Visibility = System.Windows.Visibility.Visible));
                Dispatcher.Invoke((Action)(() => elpRedhot.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => elpGreenhot.Visibility = System.Windows.Visibility.Hidden));
            }

            if (CountRegistry > 0)
            {
                //lblregistry.Content = " Registry Validation: " + ((CountReg > 0) ? "Failed: " + CountReg + " Missing Software" : "Passed");
                //imgthumbregistry.Source = CountReg > 0 ? imgThumbDown : imgThumbUp;
				Dispatcher.Invoke((Action)(() => lblregistry.Content = " Registry Validation: " + ((CountReg > 0) ? "Failed: " + CountReg + " Missing Software" : "Passed")));
                if (CountReg > 0)
                {
                    Dispatcher.Invoke((Action)(() => imgthumbregistryd.Visibility = System.Windows.Visibility.Visible));
                }
                else
                {
                    Dispatcher.Invoke((Action)(() => imgthumbregistry.Visibility = System.Windows.Visibility.Visible));
                }

                // Tree update: Registry
                Dispatcher.Invoke((Action)(() => treeControl.LoadData(lblregistry.Content, allRegisters)));
            }
            else
            {   // Registry not Found!!
                // Hide Grid
				Dispatcher.Invoke((Action)(() => lblregistry.Content = " Registry Validation: Not Found Any Record."));
                Dispatcher.Invoke((Action)(() => lblrgstryval.Content = "Registry Validation Record Not Found!!"));
               // Dispatcher.Invoke((Action)(() => dataGrid_registr.Visibility = System.Windows.Visibility.Hidden));
                // Show/Hide Thumbnails
                Dispatcher.Invoke((Action)(() => imgthumbregistry.Visibility = System.Windows.Visibility.Visible));
                Dispatcher.Invoke((Action)(() => elpRedreg.Visibility = System.Windows.Visibility.Hidden));
                Dispatcher.Invoke((Action)(() => elpGreenreg.Visibility = System.Windows.Visibility.Hidden));
            }

            #endregion

            //lblprocess.Visibility = System.Windows.Visibility.Hidden;
            Dispatcher.Invoke((Action)(() => lblprocess.Visibility = System.Windows.Visibility.Hidden));
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

                    if (ObjAppLst.Any(y => y.DisplayName == app.DisplayName))
                    {

                    }
                    else
                    {
                        status = 1;
                        CountMatchApplication++;
                    }

                    if (ObjAppLst.Any(y => y.Version == app.Version))
                    {

                    }
                    else
                    {
                        status = 0;
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




                        //string[] reg_Key = keystring.Split('{');// Substring(keystring.IndexOf('{') + 1);
                        //string RegistryKey = reg_Key[0];

                        //using (Microsoft.Win32.RegistryKey key = Registry.LocalMachine.OpenSubKey(RegistryKey))
                        //{
                        //    if (key != null)
                        //    {                               
                        //        foreach (string subKeyName in key.GetSubKeyNames())
                        //        {
                        //            if (subKeyName != null)
                        //            {
                        //                using (RegistryKey
                        //                    tempKey = key.OpenSubKey(subKeyName))
                        //                {
                        //                    foreach (string valueName in tempKey.GetValueNames())
                        //                    {
                        //                        if (valueName == item.Value && tempKey.GetValue(valueName).ToString() == item.ValueData && tempKey.GetValueKind(valueName).ToString() == item.DataType)
                        //                        {
                        //                            Status = true;
                        //                        }
                        //                    }
                        //                }                                        
                        //            }
                        //        }
                        //    }

                        //}

                        if (Status == false)
                        {
                            CountMatchRegistry++;
                        }


                        registry.Add(new RegistryGroupData()
                        {
                            RegKey = item.Key,// registrygroup.RegistryGroupID,
                            Note = registrygroup.Note,
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

    }
}

