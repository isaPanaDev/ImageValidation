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
using ImageValidation.Client.ServiceReference1;
using ImageValidation.Core;
using ImageValidation.Collection;
using System.Xml;
using System.IO;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBar1 : PageFunction<String>
    {
        BackgroundWorker bgStarter = new BackgroundWorker();
        ImageValidationServiceClient sRef = new ImageValidationServiceClient();
        public ProgressBar()
        {
            InitializeComponent();
          
        }

        ComputerInformation compInfo = new ComputerInformation();
        DriverInformation driverInfo = new DriverInformation();
        ApplicationInformation appsInfo = new ApplicationInformation();
        List<Applications> ObjAppLst = new List<Applications>();
        HotFixInformation hotfixInfo = new HotFixInformation();
        //private BackgroundWorker worker = new BackgroundWorker();
       

       // bgStarter
        //   


        private void bgStarter_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
            BackgroundWorker bgStarter = (BackgroundWorker)sender;

            SaveInformation();
          ////////////////////////////////////
                 bgStarter.ReportProgress(2, "");
         
            }
            catch (Exception ex)
            {
                MessageBox.Show("bgStarter_DoWork: " + ex.Message);
            }
        }

        private void bgStarter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                if (e.ProgressPercentage == 0)
                {
                    //Status status = (Status)e.UserState;
                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ProgressChanged: " + ex.Message);
            }
        }


         private void bgStarter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //do nothing as no updates are required
        }

         private void ImageCollector_Click(object sender, System.Windows.RoutedEventArgs e)
         {
             bgStarter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgStarter_RunWorkerCompleted);
             bgStarter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgStarter_RunWorkerCompleted);
             bgStarter.DoWork += new DoWorkEventHandler(bgStarter_DoWork);
             bgStarter.ProgressChanged += new ProgressChangedEventHandler(bgStarter_ProgressChanged);
             bgStarter.WorkerReportsProgress = true;
             bgStarter.RunWorkerAsync(3);
         }


         public void SaveInformation()
         {

             try
             {

                 
                     CollectSystemInformationInNotChecked();

                 

             }
             catch (Exception ex)
             {
                 //Console.WriteLine(ex.GetBaseException());
             }

         }



         public void CollectSystemInformationInNotChecked()
         {
             // Collect Computer information
             Computer ObjComp = compInfo.GetComputerInformation();

             // Collect Driver information
             List<Driver> ObjDriverLst = driverInfo.GetDriverInfo();

             // Collect Application information
             //List<Applications> ObjAppLst = appsInfo.GetApplicationInformation();
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
             string xmlData = xmlDoc.OuterXml;


             ////FOR TESTIING ONLY            
             //XmlDocument doc = new XmlDocument();
             //doc.Load("C:\\Windows\\Model-uniqueid.xml");
             //string xmlData = doc.InnerXml;

             bool result = sRef.SaveComputerInformationFromXml(xmlData);

             //bool result = sRef.SaveComputerInformation(ObjHotfixLst.ToArray()); 

         }

         public void CollectSystemInformationIfModelChecked()
         {

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


             bool CheckModel = sRef.CheckModelRecordExists(ObjComp.Model.ToString());
             if (CheckModel == true)
             {
                 bool _result = sRef.SaveComputerInfoUpdateCheckedModelRecord(xmlData);
                 if (_result == true)
                     MessageBox.Show("Computer information upload to azure successfully");
                 else
                     MessageBox.Show("Sorry, some network issues during upload to azure");
             }
             else
             {
                 bool _result = sRef.SaveComputerInfoUpdateCheckedModelRecordSet1(xmlData);
                 if (_result == true)
                     MessageBox.Show("Computer information upload to azure successfully");
                 else
                     MessageBox.Show("Sorry, some network issues during upload to azure");
             }

         }

         public void CollectSystemInformationIfProductChecked()
         {
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


             bool CheckModel = sRef.CheckModelRecordExists(ObjComp.Model.ToString());
             if (CheckModel == true)
             {
                 bool _result = sRef.SaveComputerInfoUpdateCheckedProductRecord(xmlData);
                 if (_result == true)
                     MessageBox.Show("Computer information upload to azure successfully");
                 else
                     MessageBox.Show("Sorry, some network issues during upload to azure");
             }
             else
             {
                 bool _result = sRef.SaveComputerInfoUpdateCheckedProductRecordSet1(xmlData);
                 if (_result == true)
                     MessageBox.Show("Computer information upload to azure successfully");
                 else
                     MessageBox.Show("Sorry, some network issues during upload to azure");
             }

         }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (!worker.IsBusy)
        //    {
        //        this.Cursor = Cursors.Wait;
        //        progressBar.IsIndeterminate = true;
        //        button.Content = "Cancel";
        //        worker.RunWorkerAsync();
        //    }
        //    else
        //    {
        //        worker.CancelAsync();
        //    }
        //}

        //private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    this.Cursor = Cursors.Arrow;

        //    if (e.Error != null)
        //    {
        //        MessageBox.Show(e.Error.Message);
        //    }

        //    button.Content = "Start";
        //    progressBar.IsIndeterminate = false;
        //}

        //private void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    for (int i = 1; i <= 100; i++)
        //    {
        //        lbl.Content = i.ToString();
        //        if (worker.CancellationPending)
        //            break;

        //        Thread.Sleep(50);
        //    }
        //}
    }
}
