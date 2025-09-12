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
using TobaccoManager.Contexts;
using TobaccoManager.Models;

namespace TobaccoManager.Views.Auth
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Signup : Page
    {
        private Frame _authFrame;

        public Signup(Frame authFrame)
        {
            InitializeComponent();
            _authFrame = authFrame;
        }

        private void Signup_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = ConfirmPasswordBox.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("All feilds are required.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Handling incorrect email format
            if (!System.Text.RegularExpressions.Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Please enter a valid email address.", "Invalid Email", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password.Length <= 5)
            {
                MessageBox.Show("Password must be more than 5 characters.", "Password Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();

            bool usernameExists = db.Users.Any(u => u.Name == username);

            if (usernameExists)
            {
                MessageBox.Show("Username is already taken.", "Signup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            bool emailExists = db.Users.Any(u => u.Email == email);

            if (emailExists)
            {
                MessageBox.Show("email is already exists.", "Signup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // All checks passed, moving to SecurityQuestion page

            _authFrame.Navigate(new SecurityQuestion(_authFrame, username, email, password));
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new Login(_authFrame));
        }

        private void UsernameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                EmailBox.Focus();
        }

        private void EmailBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                PasswordBox.Focus();
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConfirmPasswordBox.Focus();
        }

        private void ConfirmPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Signup_Click(sender, e); // Trigger signup
        }

    }
}
