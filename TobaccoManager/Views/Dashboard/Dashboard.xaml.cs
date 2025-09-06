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

namespace TobaccoManager.Views.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Calendar_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Auth.Auth();
        }
    }
}
