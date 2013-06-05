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
using ImageValidation.Validator.ServiceReference2;
using ImageValidation.Core;
using ImageValidation.Collection;

namespace ImageValidation.Validator
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : PageFunction<String>
    {
        //ServiceReferenceOnline.ImageValidationServiceClient sRef = new ServiceReferenceOnline.ImageValidationServiceClient();
        ImageValidationServiceClient sRef = new ImageValidationServiceClient();
        string _username = string.Empty;
        string _password = string.Empty;
        Users _userDetails = null;

        public Login()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Set focus on username textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        

        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        /// <summary>
        /// Validate login user and roles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _username = txtUsername.Text.Trim();
                _password = txtPassword.Password;
                if (_username == "")
                {
                    lblMsg.Content = "Please enter Username";

                }
                else if (_password == "")
                {
                    lblMsg.Content = "Please enter Password";
                }
                else
                {
                    ValidateUser();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to connect to the Cloud Database", "Message:" +ex.Message);
            }
        }

        private void ValidateUser()
        {

            string _username = txtUsername.Text.Trim();
            string _password = txtPassword.Password;
            //string Password = txtPassword.Password.ToString();

            CollectionTool validator = new CollectionTool();

            _userDetails = sRef.ValidateUser(_username, _password);

            if (_userDetails.PermissionID != 0)
            {
                if (_userDetails.RoleName == "Validator")
                {

                    _userDetails.SessionID = Utilities.GetSessionId();
                    _userDetails.LoginDate = DateTime.Now;

                    bool result = sRef.SaveAccountHistory(_userDetails);
                    if (result == true)
                    {
                        //user details store in session
                        ApplicationState.SetValue("UserDetails", _userDetails);
                        NavigationService.Navigate(validator);
                    }
                    else
                    {
                        MessageBox.Show("Unable to connect to the Cloud Database", "Message");
                    }

                }
                else if (_userDetails.RoleName == "Admin")
                {
                    _userDetails.SessionID = Utilities.GetSessionId();
                    _userDetails.LoginDate = DateTime.Now;
                    bool result = sRef.SaveAccountHistory(_userDetails);
                    if (result == true)
                    {
                        //user details store in session
                        ApplicationState.SetValue("UserDetails", _userDetails);
                        NavigationService.Navigate(validator);
                    }
                    else
                    {
                    }
                }
                else if (_userDetails.RoleName == "PowerUser")
                {
                    _userDetails.SessionID = Utilities.GetSessionId();
                    _userDetails.LoginDate = DateTime.Now;
                    bool result = sRef.SaveAccountHistory(_userDetails);
                    if (result == true)
                    {
                        //user details store in session
                        ApplicationState.SetValue("UserDetails", _userDetails);
                        NavigationService.Navigate(validator);
                    }
                    else
                    {
                    }
                }
                else if (_userDetails.RoleName == "Client")
                {
                    _userDetails.SessionID = Utilities.GetSessionId();
                    _userDetails.LoginDate = DateTime.Now;
                    bool result = sRef.SaveAccountHistory(_userDetails);
                    if (result == true)
                    {
                        //user details store in session
                        ApplicationState.SetValue("UserDetails", _userDetails);
                        //ImageValidationClient client = new ImageValidationClient();
                        //NavigationService.Navigate(client);
                    }
                    else
                    {
                    }
                }
            }
            else
            {
                _userDetails.Username = txtUsername.Text.Trim();
                int UserId = sRef.CheckUsernameExists(_userDetails);
                if (UserId != 0)
                {
                    _userDetails.UserID = UserId;
                    bool History = sRef.SaveFailedLogon(_userDetails);
                    if (History == true)
                    {
                        MessageBox.Show("You have entered wrong Password", "Message");
                    }
                }
                else
                {
                    MessageBox.Show("You have entered wrong Username/Password", "Message");
                }
            }
        }
    }
}
