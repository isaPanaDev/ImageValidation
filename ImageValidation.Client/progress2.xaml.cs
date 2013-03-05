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
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;


namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for progress2.xaml
    /// </summary>
    public partial class progress2 : PageFunction<String>
    {
        private BackgroundWorker worker = new BackgroundWorker();
        public progress2()
        {
            InitializeComponent();
            

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.ProgressChanged += worker_ProgressChanged;
            //worker.RunWorkerAsync();

            worker.RunWorkerAsync();
            this.Cursor = Cursors.Wait;
            button.IsEnabled = false;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
            if (e.Error != null)
                MessageBox.Show(e.Error.Message);
            button.IsEnabled = true;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                //lbl.Content = i.ToString();
                Thread.Sleep(100);
                worker.ReportProgress(i);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }
    }
}
