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
using TobaccoManager.Models;
using TobaccoManager.Contexts;

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
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Name == username && u.Password == password);

            if (user != null)
            {
                Application.Current.MainWindow.Content = new TobaccoManager.Views.Dashboard.Dash();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new Signup(_authFrame));
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            _authFrame.Navigate(new AccountRecover(_authFrame, username));
        }

        private void UsernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PasswordBox.Focus();
            }
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Login_Click(sender, e);
            }
        }
    }
}
