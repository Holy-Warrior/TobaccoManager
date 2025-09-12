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
    /// Interaction logic for SecurityQuestion.xaml
    /// </summary>
    public partial class SecurityQuestion : Page
    {
        private Frame _authFrame;
        private string _username, _email, _password;

        public SecurityQuestion(Frame authFrame, string username, string email, string password)
        {
            InitializeComponent();
            _authFrame = authFrame;
            _username = username;
            _email = email;
            _password = password;

            UsernameTextBlock.Text = _username;
        }

        private void CompleteSignupButton_Click(object sender, RoutedEventArgs e)
        {
            string securityQuestion = (SecurityQuestionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string securityAnswer = SecurityAnswerBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(securityQuestion) || string.IsNullOrWhiteSpace(securityAnswer))
            {
                MessageBox.Show("Please select a security question and provide an answer.", "Missing Security Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();

            var newUser = new User(
                name: _username,
                email: _email,
                password: _password,
                securityQuestion: securityQuestion,
                securityAnswer: securityAnswer
            );

            db.Users.Add(newUser);
            db.SaveChanges();

            MessageBox.Show("Account created successfully! You can now log in.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            _authFrame.Navigate(new Login(_authFrame));
        }

        private void SecurityAnswerBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CompleteSignupButton_Click(sender, e);
        }

    }
}
