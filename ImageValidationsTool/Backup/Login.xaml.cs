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

namespace ImageValidation.Client
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : PageFunction<String>
    {
        public Login()
        {
            InitializeComponent();
        }

        Users _userDetails=null; 
       
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        public Users CheckLogin(string username, string password)
        {
            Users user = new Users();
            user.RoleID = 2;            
            return user;
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            string Username = txtUsername.Text.Trim();
            string Password = txtPassword.Password.ToString();

            _userDetails = CheckLogin(Username, Password);

            if (_userDetails != null)
            {
                if (_userDetails.RoleID == 2)
                {
                    
                    CollectionTool col=new CollectionTool();
                    
 
                }
                else if (_userDetails.RoleID == 3)
                {
 
                }
            }
            else
            {
                MessageBox.Show("You have entered wrong Username/Password");
            }
            

        }
    }
}
