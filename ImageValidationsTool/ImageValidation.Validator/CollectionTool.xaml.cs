﻿using System;
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
using System.Windows.Shapes;
using System.Windows.Navigation;
using ImageValidation.Validator.ServiceReference2;
using System.ComponentModel;
using ImageValidation.Collection;
using ImageValidation.Core;
using System.Diagnostics;
using System.Xml;
using System.IO;

namespace ImageValidation.Validator
{
    /// <summary>
    /// Interaction logic for CollectionTool.xaml
    /// </summary>
    public partial class CollectionTool : PageFunction<String>
    {

        ImageValidationServiceClient sRef = new ImageValidationServiceClient();
        private BackgroundWorker worker = new BackgroundWorker();
        int computerID;
        bool Result = false;

        //Create a Delegate that matches the Signature of the ProgressBar's SetValue method
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        string[] progressSteps = new string[] { "Collect Computer Information", "Collect Driver Information", "Collect Application Information", "Collect Microsoft Hotfix Information", "Save Final Output to XML File", "Upload information to the Cloud" };
        ComputerInformation compInfo = new ComputerInformation();
        DriverInformation driverInfo = new DriverInformation();
        ApplicationInformation appsInfo = new ApplicationInformation();
        List<Applications> ObjAppLst = new List<Applications>();
        HotFixInformation hotfixInfo = new HotFixInformation();
        string xmlData;
        public CollectionTool()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Set controls visibility and initilization
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectionLoginTool_Loaded(object sender, RoutedEventArgs e)
        {
            Users _usersDetails = ApplicationState.GetValue<Users>("UserDetails") as Users;
            this.WindowTitle = "Collection Tool";
            ShowHideProgress(false);
            ShowHideResultControls(false);

        }

        /// <summary>
        /// Collect all computer information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageCollector_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowHideProgress(true);
                ShowHideResultControls(false);
                SaveInformation();
                if (Result == true)
                {
                    ShowHideProgress(false);
                    ShowHideResultControls(true);
                    Result = false;
                    MessageBox.Show("Computer information upload to azure successfully", "Message");                   
                }
                else
                {
                    MessageBox.Show("Sorry, some network issues during upload to azure", "Message");

                }

                btnImageCollector.IsEnabled = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Network error: " + ex.Message);
                MessageBox.Show("Network error: " + ex.Message);
            }
        }


        /// <summary>
        /// Redirect to webportal link
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebPortal_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Open xml file from final output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        /// <summary>
        /// Collect and save all information to cloud db
        /// </summary>
        public void SaveInformation()
        {

            if (rdModelRecord.IsChecked == true && rdProductRecord.IsChecked == true)
            {
                SaveComputerInfoUpdateCheckedProductModelRecord(); // Save information if both checked
            }
            else if (rdProductRecord.IsChecked == true)
            {
                CollectSystemInformationIfProductChecked(); //Save information if product checked
            }
            else if (rdModelRecord.IsChecked == true)
            {
                CollectSystemInformationIfModelChecked(); //Save information if model checked
            }
            else
            {
                CollectSystemInformationInNotChecked(); //Save information if no checked               
            }
        }

        /// <summary>
        /// Collect and save information if not checked in primary force records
        /// </summary>
        public void CollectSystemInformationInNotChecked()
        {
            btnImageCollector.IsEnabled = false;
            //Configure the ProgressBar
            StatusBar1.Minimum = 0;
            StatusBar1.Maximum = 6;
            StatusBar1.Value = 0;
            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(StatusBar1.SetValue);

            do
            {
                value += 1;
                int stepNum = Convert.ToInt32(value);
                lblStepStart.Content = stepNum;
                int len = Convert.ToInt32(value);
                lblStepTitle.Content = progressSteps[len];

                // Collect Computer information
                Computer ObjComp = compInfo.GetComputerInformation();
                // Collect Driver information
                List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();

                // Collect Application information                
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

                // Collect Microsoft Hotfix information 
                List<HotFix> ObjHotfixLst = hotfixInfo.GetHotFixInfo();
                /* Create a xml file using these objects */
                ClientXML xml = new ClientXML();
                XmlDocument xmlDoc = xml.WriteXML(ObjComp, ObjDriverLst, ObjAppLst, ObjHotfixLst);
                xmlData = xmlDoc.OuterXml;

                if (value == 5)
                {
                    computerID = sRef.SaveComputerInformationFromXml(xmlData);
                    if (computerID > 0)
                    {
                        ShowHideProgress(false);
                        ShowHideResultControls(true);
                        Result = true;

                        lblComputerID.Content = computerID.ToString();

                        //Set NavigateUri of XML file
                        var xmlFilePath = System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                        lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                        lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                        break;
                    }
                }

                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

            }
            while (StatusBar1.Value != StatusBar1.Maximum);
            btnImageCollector.IsEnabled = true;
        }

        /// <summary>
        /// Collect and save information if Model checked in primary force records
        /// </summary>
        public void CollectSystemInformationIfModelChecked()
        {
            btnImageCollector.IsEnabled = false;
            //Configure the ProgressBar
            StatusBar1.Minimum = 0;
            StatusBar1.Maximum = 6;
            StatusBar1.Value = 0;
            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(StatusBar1.SetValue);

            do
            {
                value += 1;
                int stepNum = Convert.ToInt32(value);
                lblStepStart.Content = stepNum;
                int len = Convert.ToInt32(value);
                lblStepTitle.Content = progressSteps[len];

                Computer ObjComp = compInfo.GetComputerInformation();
                List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();
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
                List<HotFix> ObjHotfixLst = hotfixInfo.GetHotFixInfo();

                /* Create a xml file using these objects */
                ClientXML xml = new ClientXML();
                XmlDocument xmlDoc = xml.WriteXML(ObjComp, ObjDriverLst, ObjAppLst, ObjHotfixLst);
                string xmlData = xmlDoc.OuterXml;
                if (value == 5)
                {
                    bool CheckModel = sRef.CheckModelRecordExists(ObjComp.Model.ToString());
                    if (CheckModel == true)
                    {
                        computerID = sRef.SaveComputerInfoUpdateCheckedModelRecord(xmlData);
                        if (computerID > 0)
                        {
                            ShowHideProgress(false);
                            ShowHideResultControls(true);
                            Result = true;

                            lblComputerID.Content = computerID.ToString();

                            //Set NavigateUri of XML file
                            var xmlFilePath = System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                            lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                            lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                            break;
                        }
                    }
                    else
                    {
                        computerID = sRef.SaveComputerInfoUpdateCheckedModelRecordSet1(xmlData);
                        if (computerID > 0)
                        {
                            ShowHideProgress(false);
                            ShowHideResultControls(true);
                            Result = true;

                            lblComputerID.Content = computerID.ToString();

                            //Set NavigateUri of XML file
                            var xmlFilePath = System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                            lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                            lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                            break;
                        }
                    }
                }

                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

            }
            while (StatusBar1.Value != StatusBar1.Maximum);

            btnImageCollector.IsEnabled = true;

        }


        /// <summary>
        /// Collect and save information if Product checked in primary force records
        /// </summary>
        public void CollectSystemInformationIfProductChecked()
        {
            btnImageCollector.IsEnabled = false;
            //Configure the ProgressBar
            StatusBar1.Minimum = 0;
            StatusBar1.Maximum = 6;
            StatusBar1.Value = 0;
            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(StatusBar1.SetValue);

            do
            {
                value += 1;
                int stepNum = Convert.ToInt32(value);
                lblStepStart.Content = stepNum;
                int len = Convert.ToInt32(value);
                lblStepTitle.Content = progressSteps[len];


                Computer ObjComp = compInfo.GetComputerInformation();
                List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();
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
                List<HotFix> ObjHotfixLst = hotfixInfo.GetHotFixInfo();

                /* Create a xml file using these objects */
                ClientXML xml = new ClientXML();
                XmlDocument xmlDoc = xml.WriteXML(ObjComp, ObjDriverLst, ObjAppLst, ObjHotfixLst);
                string xmlData = xmlDoc.OuterXml;


                if (value == 5)
                {
                    bool CheckModel = sRef.CheckModelRecordExists(ObjComp.Model.ToString());
                    if (CheckModel == true)
                    {
                        computerID = sRef.SaveComputerInfoUpdateCheckedProductRecord(xmlData);
                        if (computerID > 0)
                        {
                            ShowHideProgress(false);
                            ShowHideResultControls(true);
                            Result = true;
                            lblComputerID.Content = computerID.ToString();

                            //Set NavigateUri of XML file
                            var xmlFilePath = System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                            lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                            lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                            break;

                        }
                    }
                    else
                    {
                        computerID = sRef.SaveComputerInfoUpdateCheckedProductRecordSet1(xmlData);
                        if (computerID > 0)
                        {
                            ShowHideProgress(false);
                            ShowHideResultControls(true);
                            Result = true;
                            lblComputerID.Content = computerID.ToString();

                            //Set NavigateUri of XML file
                            var xmlFilePath = System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                            lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                            lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                            break;

                        }
                    }
                }

                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

            }
            while (StatusBar1.Value != StatusBar1.Maximum);

            btnImageCollector.IsEnabled = true;

        }


        /// <summary>
        /// Collect and save information if both checked in primary force records
        /// </summary>
        public void SaveComputerInfoUpdateCheckedProductModelRecord()
        {
            btnImageCollector.IsEnabled = false;
            //Configure the ProgressBar
            StatusBar1.Minimum = 0;
            StatusBar1.Maximum = 6;
            StatusBar1.Value = 0;
            //Stores the value of the ProgressBar
            double value = 0;

            //Create a new instance of our ProgressBar Delegate that points
            //  to the ProgressBar's SetValue method.
            UpdateProgressBarDelegate updatePbDelegate = new UpdateProgressBarDelegate(StatusBar1.SetValue);

            do
            {
                value += 1;
                int stepNum = Convert.ToInt32(value);
                lblStepStart.Content = stepNum;
                int len = Convert.ToInt32(value);
                lblStepTitle.Content = progressSteps[len];

                Computer ObjComp = compInfo.GetComputerInformation();
                List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();
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
                List<HotFix> ObjHotfixLst = hotfixInfo.GetHotFixInfo();

                /* Create a xml file using these objects */
                ClientXML xml = new ClientXML();
                XmlDocument xmlDoc = xml.WriteXML(ObjComp, ObjDriverLst, ObjAppLst, ObjHotfixLst);
                string xmlData = xmlDoc.OuterXml;

                if (value == 5)
                {
                    computerID = sRef.SaveComputerInfoUpdateCheckedProductModelRecord(xmlData);
                    if (computerID > 0)
                    {
                        ShowHideProgress(false);
                        ShowHideResultControls(true);
                        Result = true;

                        lblComputerID.Content = computerID.ToString();

                        //Set NavigateUri of XML file
                        var xmlFilePath =System.IO.Path.Combine(@"C:\Windows\Temp", ObjComp.Model + ".xml");
                        lnkXMLOut.NavigateUri = new Uri(xmlFilePath);//(GetXmlUrl(computerID));
                        lnkXMLOut.Inlines.Add(xmlFilePath);//(GetXmlUrl(computerID));
                        break;
                    }

                }

                Dispatcher.Invoke(updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

            }
            while (StatusBar1.Value != StatusBar1.Maximum);

            btnImageCollector.IsEnabled = true;

        }

        /// <summary>
        /// Show hide controls
        /// </summary>
        /// <param name="showControls"></param>

        private void ShowHideProgress(bool showControls)
        {
            if (showControls)
            {
                //Visible prograssbar and other controls
                StatusBar1.Visibility = System.Windows.Visibility.Visible;
                lblStep.Visibility = System.Windows.Visibility.Visible;
                lblStepStart.Visibility = System.Windows.Visibility.Visible;
                lblStepOf.Visibility = System.Windows.Visibility.Visible;
                lblStepEnd.Visibility = System.Windows.Visibility.Visible;
                lblStepTitle.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                StatusBar1.Visibility = System.Windows.Visibility.Hidden;
                lblStep.Visibility = System.Windows.Visibility.Hidden;
                lblStepStart.Visibility = System.Windows.Visibility.Hidden;
                lblStepOf.Visibility = System.Windows.Visibility.Hidden;
                lblStepEnd.Visibility = System.Windows.Visibility.Hidden;
                lblStepTitle.Visibility = System.Windows.Visibility.Hidden;

            }

        }

        /// <summary>
        /// Show hide controls
        /// </summary>
        /// <param name="showControls"></param>
        private void ShowHideResultControls(bool showControls)
        {
            if (showControls)
            {
                //gbImageResult.Visibility = System.Windows.Visibility.Visible;
                //lblCloudRecord.Visibility = System.Windows.Visibility.Visible;
                lblID.Visibility = System.Windows.Visibility.Visible;
                lblXMLOutput.Visibility = System.Windows.Visibility.Visible;
               // grdResult.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                gbImageResult.Visibility = System.Windows.Visibility.Hidden;
                //lblCloudRecord.Visibility = System.Windows.Visibility.Hidden;
                lblID.Visibility = System.Windows.Visibility.Hidden;
                lblXMLOutput.Visibility = System.Windows.Visibility.Hidden;
                grdResult.Visibility = System.Windows.Visibility.Hidden;

            }

        }

        private void hprlnkWebUrl_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void hprlnkWebUrl1_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {

        }

        public string GetXmlUrl(int ComputerID)
        {
            string XmlPath = "https://imagevalidationblob.blob.core.windows.net/computerinfo/xml-info-" + ComputerID.ToString() + ".xml";
            return XmlPath;
        }
    }
}
