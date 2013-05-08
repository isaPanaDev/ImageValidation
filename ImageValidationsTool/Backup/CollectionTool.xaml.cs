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
using ImageValidation.Collection;
using System.Xml;
using System.IO;
using System.Data;
using System.Collections;
using System.Configuration;
using System.Web;
using System.Xml.Linq;
using System.Windows.Media.Animation;
//using System.Threading.Tasks;
using System.ComponentModel;


namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for CollectionTool.xaml
    /// </summary>
    public partial class CollectionTool : PageFunction<String>
    {
        public CollectionTool()
        {
            InitializeComponent();
        }


        ComputerInformation compInfo = new ComputerInformation();
        HotFixInformation hotfixInfo = new HotFixInformation();
        DriverInformation driverInfo = new DriverInformation();


        //Create a Delegate that matches the Signature of the ProgressBar's SetValue method
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private void CollectionLoginTool_Loaded(object sender, RoutedEventArgs e)
        {

            Computer comp = compInfo.GetComputerInformation();
            //driverInfo.GetComputerInformation();
            ClientXML xml = new ClientXML();
            xml.WriteXML(comp);




            RegistryInformation regInfo = new RegistryInformation();
            string key32 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            string key64 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            ////regInfo.Read(key);
            if (comp.OSArchitecture == "32-bit")
            {
                regInfo.RegistryInfo32(key32);
            }
            else if (comp.OSArchitecture == "64-bit")
            {
                regInfo.RegistryInfo64(key64);
            }
            else
            { }



            Hotfix hotfix = hotfixInfo.GetHotFixInfo();
            //Driver drive = driverInfo.GetDriverInfo();
            

        }


        private void ImageCollector_Click(object sender, RoutedEventArgs e)
        {

            ////var worker = new BackgroundWorker { WorkerReportsProgress = true };
            ////worker.ProgressChanged += (s, e) => ProgressBar1.Value = e.ProgressPercentage;
            ////worker.DoWork += (s, e) =>
            ////{
            ////    for (int i = 0; i < 4; i++)
            ////    {
            ////        Thread.Sleep(1000);
            ////        var percentage = (i + 1) * 25;
            ////        worker.ReportProgress(percentage);
            ////    }
            ////};

            ////worker.RunWorkerCompleted += (s, e) =>
            ////{
            ////    System.Windows.MessageBox.Show("Done!");
            ////    worker.Dispose();
            ////    worker = null;
            ////};

            ////worker.RunWorkerAsync();


            //btnImageCollector.IsEnabled = false;
            //int SecondsToComplete = 30; //Program specific value

            //System.Windows.Controls.ProgressBar progress = new System.Windows.Controls.ProgressBar();
            //progress.IsIndeterminate = false;
            //progress.Orientation = System.Windows.Controls.Orientation.Horizontal;
            //progress.Width = 419;
            //progress.Height = 20;

            //Duration duration = new Duration(TimeSpan.FromSeconds((SecondsToComplete * 1.35)));
            //DoubleAnimation doubleAnimatiion = new DoubleAnimation(200, duration);
            //StatusBar1.Items.Add(progress);

            //try
            //{
            //    TaskScheduler uiThread = TaskScheduler.FromCurrentSynchronizationContext();

            //    Action MainThreadDoWork = new Action(() =>
            //    {
            //        //add thread safe code here.
            //        //Confirm thread will not use GUI thread
            //        System.Threading.Thread.Sleep(20000);
            //    });

            //    Action ExecuteProgressBar = new Action(() =>
            //    {
            //        progress.BeginAnimation(System.Windows.Controls.ProgressBar.ValueProperty, doubleAnimatiion);
            //    });

            //    Action FinalThreadDoWOrk = new Action(() =>
            //    {
            //        btnImageCollector.IsEnabled = true;
            //        StatusBar1.Items.Remove(progress);
            //    });

            //    Task MainThreadDoWorkTask = Task.Factory.StartNew(() => MainThreadDoWork());

            //    Task ExecuteProgressBarTask = new Task(ExecuteProgressBar);
            //    ExecuteProgressBarTask.RunSynchronously();

            //    MainThreadDoWorkTask.ContinueWith(t => FinalThreadDoWOrk(), uiThread);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show("Error", ex.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            //}

        }
    }
}
