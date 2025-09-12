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
    /// Interaction logic for AccountRecover.xaml
    /// </summary>
    public partial class AccountRecover : Page
    {
        private Frame _authFrame;
        private string _username;

        public AccountRecover(Frame authFrame, string username = "")
        {
            InitializeComponent();
            _authFrame = authFrame;
            _username = username;

            if (!string.IsNullOrWhiteSpace(_username))
            {
                using var db = new AppDbContext();
                var user = db.Users.FirstOrDefault(u => u.Name == _username);

                if (user != null)
                {
                    UsernameTextBox.Text = _username;
                    StoredQuestion.Text = user.SecurityQuestion;
                    AnswerBox.IsEnabled = true;
                    RecoverButton.IsEnabled = true;
                }
            }
        }

        private void VerifyUsername_Click(object sender, RoutedEventArgs e)
        {
            IsUsernameCorrect();
        }

        private bool IsUsernameCorrect()
        {
            _username = UsernameTextBox.Text.Trim();
            
            if (!string.IsNullOrWhiteSpace(_username))
            {
                using var db = new AppDbContext();
                var user = db.Users.FirstOrDefault(u => u.Name == _username);

                if (user != null)
                {
                    StoredQuestion.Text = user.SecurityQuestion;
                    AnswerBox.IsEnabled = true;
                    RecoverButton.IsEnabled = true;
                    return true;
                }
                else
                {
                    StoredQuestion.Text = string.Empty;
                    MessageBox.Show("Username is not found.", "Invalid Info", MessageBoxButton.OK, MessageBoxImage.Error);
                    AnswerBox.IsEnabled = false;
                    RecoverButton.IsEnabled = false;
                    return false;
                }
            }

            MessageBox.Show("Username field is required.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
            AnswerBox.IsEnabled = false;
            RecoverButton.IsEnabled = false;
            return false;
        }

        private void Recover_Click(object sender, RoutedEventArgs e)
        {
            if (!IsUsernameCorrect())
            {
                return;
            }

            string answer = AnswerBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(_username) || string.IsNullOrWhiteSpace(answer))
            {
                MessageBox.Show("Please fill in all fields.", "Missing Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(u => u.Name == _username && u.SecurityAnswer == answer);

            if (user != null)
            {
                MessageBox.Show($"Your password is: {user.Password}", "Password Recovered", MessageBoxButton.OK, MessageBoxImage.Information);
                _authFrame.Navigate(new Login(_authFrame));
            }
            else
            {
                MessageBox.Show("Incorrect answer to the security question.", "Recovery Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new Login(_authFrame));
        }
    }
}
