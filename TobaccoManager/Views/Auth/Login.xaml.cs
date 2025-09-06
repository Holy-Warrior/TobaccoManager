using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TobaccoManager.Views.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private Frame _authFrame;

        public Login(Frame authFrame)
        {
            InitializeComponent();
            _authFrame = authFrame;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {   
            Application.Current.MainWindow.Content = new TobaccoManager.Views.Dashboard.Dashboard();
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new Signup(_authFrame));
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new AccountRecover(_authFrame));
        }
    }
}
