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
    public partial class Dash : Page
    {
        public Dash()
        {
            InitializeComponent();
            DashMain.Content = new Dashboard();
        }
        
        private void HighlightNavButton(object sender)
        {
            // Reset all buttons to default
            DashboardButton.Background = Brushes.White;
            ProductsButton.Background = Brushes.White;
            CalendarButton.Background = Brushes.White;
            ProfileButton.Background = Brushes.White;

            // Highlight the clicked one
            if (sender is Button clickedButton)
            {
                clickedButton.Background = new BrushConverter().ConvertFrom("#ffedb8") as SolidColorBrush;
            }
        }


        private void Dashboard_Click(object sender, RoutedEventArgs e)
        {
            HighlightNavButton(sender);
            DashMain.Content = new Dashboard();
        }

        private void Products_Click(object sender, RoutedEventArgs e)
        {
            HighlightNavButton(sender);
            DashMain.Content = new Products();
        }

        private void Calendar_Click(object sender, RoutedEventArgs e)
        {
            HighlightNavButton(sender);
            DashMain.Content = new Calendar();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            HighlightNavButton(sender);
            DashMain.Content = new Profile();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new Auth.Auth();
        }
    }
}
