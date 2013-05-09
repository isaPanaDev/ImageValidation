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

        #region Constructor

        public ucCustomTreeCodeBehind()
        {
            InitializeComponent();
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
            image.Source = new BitmapImage(new Uri("pack://application:,,/Images/" + imagePath));

            // Label
            Label lbl = new Label();
            lbl.Content = text;


            // Add into stack
            stack.Children.Add(image);
            stack.Children.Add(lbl);

            // assign stack to header
            item.Header = stack;
            return item;
        }

        private TreeViewItem GetTreeViewDetail(string text, Color boxColor)
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

            stack.Children.Add(border);
            stack.Children.Add(lbl);

            //item.HeaderTemplate.ad  

            item.Header = stack;
            return item;
        }
        
        #endregion


        internal void LoadData(object title)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            treeItem.Items.Add(GetTreeViewDetail("USA", Colors.Red));
            treeItem.Items.Add(GetTreeViewDetail("Canada", Colors.Yellow));
            treeItem.Items.Add(GetTreeViewDetail("Mexico", Colors.Yellow));

            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(object title, List<Applications> allApplicationItems)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            foreach (Applications apps in allApplicationItems)
            {
                try
                {
                    Color itemsColor;
                    if (apps.IsCompared == 0)
                        itemsColor = Colors.Red;
                    else if (apps.IsCompared == 1)
                        itemsColor = Colors.Yellow;
                    else
                        itemsColor = Colors.Green;

                    if (itemsColor == Colors.Green) continue;

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(apps.DisplayName, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(object title, List<Driver> allDrivers)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            foreach (Driver driver in allDrivers)
            {
                try
                {
                    Color itemsColor;
                    if (driver.IsCompared == 1)
                        itemsColor = Colors.Red;
                    else if (driver.IsCompared == 0)
                        itemsColor = Colors.Yellow;
                    else
                        itemsColor = Colors.Green;

                    if (itemsColor == Colors.Green) continue;

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(driver.DeviceName, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(object title, List<FileFolder> allFileFolders)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            foreach (FileFolder filefolder in allFileFolders)
            {
                try
                {
                    Color itemsColor;
                    if (filefolder.IsCompared == true)
                        itemsColor = Colors.Red;
                    else if (filefolder.IsCompared == false)
                        itemsColor = Colors.Yellow;
                    else
                        itemsColor = Colors.Green;

                    if (itemsColor == Colors.Green) continue;

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(filefolder.Location, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(object title, List<HotFix> allHotFixes)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            foreach (HotFix hotfix in allHotFixes)
            {
                try
                {
                    Color itemsColor;
                    if (hotfix.IsCompared == true)
                        itemsColor = Colors.Yellow;
                    else if (hotfix.IsCompared == false)
                        itemsColor = Colors.Red;
                    else
                        itemsColor = Colors.Green;

                    if (itemsColor == Colors.Green) continue;

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(hotfix.HotFixIDs, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }

        internal void LoadData(object title, List<RegistryGroupData> allRegisters)
        {
            TreeViewItem treeItem = null;
            string image = "Thumb-down.png";

            // North America 
            treeItem = GetTreeViewTitle(title.ToString(), image);

            foreach (RegistryGroupData registry in allRegisters)
            {
                try
                {
                    Color itemsColor;
                    if (registry.IsCompared == true)
                        itemsColor = Colors.Red;
                    else if (registry.IsCompared == false)
                        itemsColor = Colors.Yellow;
                    else
                        itemsColor = Colors.Green;

                    if (itemsColor == Colors.Green) continue;

                    // Only display red and yellow items
                    treeItem.Items.Add(GetTreeViewDetail(registry.RegKey, itemsColor));
                }
                catch (Exception ex)
                { throw ex; }
            }
            tvMain.Items.Add(treeItem);
        }
    }
}
