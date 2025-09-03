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
    /// Interaction logic for AccountRecover.xaml
    /// </summary>
    public partial class AccountRecover : Page
    {
        private Frame _authFrame;

        public AccountRecover(Frame authFrame)
        {
            InitializeComponent();
            _authFrame = authFrame;
        }

        private void Recover_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            _authFrame.Navigate(new Login(_authFrame));
        }
    }
}
