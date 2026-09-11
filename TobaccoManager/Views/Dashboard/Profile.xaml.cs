using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using TobaccoManager.Contexts;
using TobaccoManager.Services;

namespace TobaccoManager.Views.Dashboard
{
    /// <summary>
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Profile()
        {
            InitializeComponent();
            LoadCurrentUser();
        }

        private void LoadCurrentUser()
        {
            var user = Session.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("No user is currently signed in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SaveDetailsButton.IsEnabled = false;
                ChangePasswordButton.IsEnabled = false;
                return;
            }

            NameBox.Text = user.Name;
            EmailBox.Text = user.Email;
        }

        private void SaveDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Session.CurrentUser;
            if (currentUser == null)
                return;

            var name = NameBox.Text.Trim();
            var email = EmailBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var dbUser = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);
                if (dbUser == null)
                {
                    MessageBox.Show("User not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dbUser.Name = name;
                dbUser.Email = email;
                db.SaveChanges();

                // Refresh the session so the sidebar and other pages see the update.
                Session.CurrentUser = dbUser;

                MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update profile: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = Session.CurrentUser;
            if (currentUser == null)
                return;

            var currentPassword = CurrentPasswordBox.Password;
            var newPassword = NewPasswordBox.Password;
            var confirmPassword = ConfirmPasswordBox.Password;

            if (currentPassword != currentUser.Password)
            {
                MessageBox.Show("Current password is incorrect.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("New password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirmation do not match.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using var db = new AppDbContext();
                var dbUser = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);
                if (dbUser == null)
                {
                    MessageBox.Show("User not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                dbUser.Password = newPassword;
                db.SaveChanges();

                Session.CurrentUser = dbUser;

                CurrentPasswordBox.Clear();
                NewPasswordBox.Clear();
                ConfirmPasswordBox.Clear();

                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to change password: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                return address.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
