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

namespace ImageValidation.Client.Controls
{
    /// <summary>
    /// Interaction logic for ucCustomTreeCodeBehind.xaml
    /// </summary>
    public partial class ucCustomTreeCodeBehind : UserControl
    {
        private Label currentLabel;
        #region Constructor

        public ucCustomTreeCodeBehind()
        {
            InitializeComponent();

            //LoadTree();
        }
        
        #endregion

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           // LoadTreeTest();
        }
        
        #endregion

        #region Private Methods

        private TreeViewItem GetTreeViewTitle(string text, string imagePath)
        {
            TreeViewItem item = new TreeViewItem();
            item.IsExpanded = true;
            item.FontSize = 14;
           
            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            // create Image
            Image image = new Image();
            try
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + imagePath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+ " - path: " + imagePath);
            }

            // Label
            currentLabel = new Label();
            currentLabel.Content = text;

            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(currentLabel);

            // assign stack to header
            item.Header = stack;
            return item;
        }

        private TreeViewItem GetTreeViewDetail(string text, string detailString, Color boxColor)
        {
            TreeViewItem item = new TreeViewItem();
            item.IsExpanded = true;
            item.FontSize = 12;

            // create stack panel
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            // create Image
            Border border = new Border();
            border.Width = 12;
            border.Height = 12;
            border.Background = new SolidColorBrush(boxColor);

            // Label
            Label lbl = new Label();
            lbl.Content = text;
            Label lbl_detail = new Label();
            lbl_detail.Content = detailString;
            lbl_detail.Foreground = System.Windows.Media.Brushes.DarkBlue;            

            stack.Children.Add(border);
            stack.Children.Add(lbl);
            stack.Children.Add(lbl_detail);

            //item.HeaderTemplate.ad  

            item.Header = stack;
            return item;
        }
        
        #endregion

        #region Loading Tree
       
        internal void ClearTree()
        {
            tvMain.Items.Clear();
        }

        internal void LoadData(int iMissing, int iMismatch, List<Applications> allApplications, log4net.ILog log)
        {
            TreeViewItem treeItem = null;
            string image = "Add-icon.png";
            string mismatch = (iMismatch > 0) ? ", " + iMismatch + " Version Mismatch" : "";
            string header = " Software : " + iMissing + " Missing " + mismatch;
                   
            treeItem = GetTreeViewTitle(header, image);

            foreach (Applications apps in allApplications)
            {
                string errorTag = "0"; // success
                try
                {
                    Color itemsColor;
                    if (apps.IsCompared == 1)
                    {
                        itemsColor = Colors.Red; errorTag = "2";
                    }
                    else if (apps.IsCompared == 2)
                    {
                        itemsColor = Colors.Yellow; errorTag = "1";
                    }
                    else
                        itemsColor = Colors.Green;

                    // Log the report
                    log.Error("a" + "," + errorTag + "," + apps.DisplayName + "," + apps.Version + "," 
                             + apps.HelpLink + "," + apps.InstallLocation + "," + apps.InstallSource);

                    if (itemsColor == Colors.Green) continue;
                    
                    // Build the detailed string
                    string detailString = (apps.HelpLink == "") ? "" : "(HelpLink: " + apps.HelpLink +")";
                    
                    // Only display red and yellow items in the TreeView
                    treeItem.Items.Add(GetTreeViewDetail(apps.DisplayName, detailString, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(int iMissing, int iMismatch, List<Driver> allDrivers, log4net.ILog log)
        {
            TreeViewItem treeItem = null;
            string image = "Add-icon.png";
            string mismatch = (iMismatch > 0) ? ", " + iMismatch + " Version Mismatch" : "";
            string header = " Drivers : " + iMissing + " Missing " + mismatch;
           
            treeItem = GetTreeViewTitle(header, image);

            foreach (Driver driver in allDrivers)
            {
                string errorTag = "0"; // success
                try
                {
                    Color itemsColor;
                    if (driver.IsCompared == 0)
                    {
                       itemsColor = Colors.Red; errorTag = "2";
                    }
                    else if (driver.IsCompared == 1)
                    {
                         itemsColor = Colors.Yellow; errorTag = "1";
                    }
                    else
                        itemsColor = Colors.Green;

                    // Log the report
                    log.Error("d" + "," + errorTag + "," + driver.DeviceName + "," + driver.BaseDriverVersion + "," +
                             driver.DriverVersion + "," + driver.DriverProviderName + ", " + driver.InfName);

                    if (itemsColor == Colors.Green) continue;

                    // Build the detailed string
                    string detailString = "(Base: " + driver.BaseDriverVersion + ((driver.DriverVersion == "") ? ")" : " Current: " + driver.DriverVersion + ")");
                        
                    // Only display red and yellow items in the TreeView
                    treeItem.Items.Add(GetTreeViewDetail(driver.DeviceName, detailString, itemsColor));
               
                   /*   Console.WriteLine("---------------- DRIVER ------------ ");
                      Console.WriteLine("ID: " + driver.DeviceID);
                      Console.WriteLine("DeviceName: " + driver.DeviceName);
                      Console.WriteLine("VERSION: " + driver.DriverVersion);
                      Console.WriteLine("*** LOCAL VERSION: *****" + driver.BaseDriverVersion);
                      Console.WriteLine("IDDriverProviderName: " + driver.DriverProviderName);
                      Console.WriteLine("friendlyName: " + driver.friendlyName);
                      Console.WriteLine("httpUrl: " + driver.httpUrl);
                      Console.WriteLine("HardWareID: " + driver.HardWareID);
                      Console.WriteLine("InfName: " + driver.InfName);
                      Console.WriteLine("IsRequired: " + driver.IsRequired);
                      Console.WriteLine("IsSigned: " + driver.IsSigned);
                      Console.WriteLine("Manufacturer: " + driver.Manufacturer);
                      Console.WriteLine("Name: " + driver.Name);
                      Console.WriteLine("PDO: " + driver.PDO);*/
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(int cMissing, List<FileFolder> allFileFolders, log4net.ILog log)
        {
            TreeViewItem treeItem = null;
            string image = "Add-icon.png";
            string header = " File and Folders : " + cMissing + " Missing ";
                    
            treeItem = GetTreeViewTitle(header, image);

            foreach (FileFolder filefolder in allFileFolders)
            {
                string errorTag = "0"; // success
                try
                {
                    Color itemsColor;
                    if (filefolder.IsCompared)
                    {
                        itemsColor = Colors.Green; errorTag = "0";
                    }
                    else
                    {
                        itemsColor = Colors.Red; errorTag = "2";
                    }

                    // Log the report
                    log.Error("f" + "," + errorTag + "," + filefolder.Location + "," + filefolder.Note+ "," + filefolder.Type);
                    
                    if (itemsColor == Colors.Green) continue;

                    // Build the detailed string
                    string detailString = (filefolder.Note == "") ? "" : "(Note: " + filefolder.Note + ")";

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(filefolder.Location, detailString, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(int cMissing, List<HotFix> allHotFixes, log4net.ILog log)
        {
            TreeViewItem treeItem = null;
            string image = "Add-icon.png";
            string header = " Microsoft HotFix : " + cMissing + " Missing ";
                   
            treeItem = GetTreeViewTitle(header, image);

            foreach (HotFix hotfix in allHotFixes)
            {
                string errorTag = "0"; // success
                try
                {
                    Color itemsColor;
                    if (hotfix.IsCompared)
                    {
                        itemsColor = Colors.Green; errorTag = "0";
                    }
                    else 
                    {
                        itemsColor = Colors.Red; errorTag = "2";
                    }

                    // Log the report
                    log.Error("h" + "," + errorTag + "," + hotfix.HotFixIDs + ":" + errorTag);

                    if (itemsColor == Colors.Green) continue;
                    
                    // Build the detailed string
                    string detailString = "";

                    // Displays red or green items 
                    treeItem.Items.Add(GetTreeViewDetail(hotfix.HotFixIDs, detailString, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(int cMissing, List<RegistryGroupData> allRegisters, log4net.ILog log)
        {
            TreeViewItem treeItem = null;
            string image = "Add-icon.png";
            string header = " Registry : " + cMissing + " Missing ";
                  
            treeItem = GetTreeViewTitle(header, image);

            foreach (RegistryGroupData registry in allRegisters)
            {
               string errorTag = "0"; // success
               try
               {
                    Color itemsColor;
                    if (registry.IsCompared)
                    {
                        itemsColor = Colors.Green; errorTag = "0";
                    }
                    else
                    {
                        itemsColor = Colors.Red; errorTag = "2";
                    }

                    // Log the report
                    log.Error("r" + "," + errorTag + "," + registry.RegKey + "," + registry.FileName + "," + registry.Note + "," + registry.Type);

                    if (itemsColor == Colors.Green) continue;

                    // Build the detailed string
                    string detailString = "(File: " + registry.FileName + ((registry.Note == "") ? ")" : " Note: " + registry.Note + ")");
                   
                    // Displays red or green items 
                    treeItem.Items.Add(GetTreeViewDetail(registry.RegKey, detailString, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }
        #endregion

        /* Testing Methods */
        internal void LoadTree()
        {
            string image = "Thumb-down.png";
            TreeViewItem treeItem = null;

            // North America 
            treeItem = GetTreeViewDetail("North America", "", Colors.Green);

            treeItem.Items.Add(GetTreeViewTitle("USA", image));
            // treeItem.Items.Add(GetTreeViewTitle("Canada", "canada.png"));
            treeItem.Items.Add(GetTreeViewTitle("Mexico", image));

            tvMain.Items.Add(treeItem);

            // South America 
            treeItem = GetTreeViewDetail("South America", "", Colors.LightGreen);

            treeItem.Items.Add(GetTreeViewTitle("Argentina", image));
            treeItem.Items.Add(GetTreeViewTitle("Brazil", image));
            treeItem.Items.Add(GetTreeViewTitle("Uruguay", image));

            tvMain.Items.Add(treeItem);

            // Europe
            treeItem = GetTreeViewDetail("Europe", "", Colors.Brown);

            treeItem.Items.Add(GetTreeViewTitle("UK", image));
            treeItem.Items.Add(GetTreeViewTitle("Denmark", image));
            treeItem.Items.Add(GetTreeViewTitle("France", image));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeViewDetail("Asia", "", Colors.Red);

            treeItem.Items.Add(GetTreeViewTitle("Pakistan", image));
            treeItem.Items.Add(GetTreeViewTitle("Japan", image));
            treeItem.Items.Add(GetTreeViewTitle("China", image));

            tvMain.Items.Add(treeItem);

            // Asia
            treeItem = GetTreeViewDetail("Africa", "", Colors.Yellow);

            treeItem.Items.Add(GetTreeViewTitle("Somalia", image));
            treeItem.Items.Add(GetTreeViewTitle("Uganda", image));
            treeItem.Items.Add(GetTreeViewTitle("Egypt", image));

            tvMain.Items.Add(treeItem);


            // Australia
            treeItem = GetTreeViewDetail("Australia", "", Colors.Purple);

            treeItem.Items.Add(GetTreeViewTitle("Australia", image));
            tvMain.Items.Add(treeItem);


            // Antarctica
            treeItem = GetTreeViewDetail("Antarctica", "", Colors.Blue);

            tvMain.Items.Add(treeItem);

        }
        internal void LoadDataTest(object title)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            treeItem.Items.Add(GetTreeViewDetail("USA", "", Colors.Red));
            treeItem.Items.Add(GetTreeViewDetail("Canada", "", Colors.Yellow));
            treeItem.Items.Add(GetTreeViewDetail("Mexico", "", Colors.Yellow));

            tvMain.Items.Add(treeItem);
        }
    }
}
