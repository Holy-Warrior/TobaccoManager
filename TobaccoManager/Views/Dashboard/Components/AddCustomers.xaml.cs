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
using System.Windows.Shapes;

using TobaccoManager.Models;

namespace TobaccoManager.Views.Dashboard.Components
{
    /// <summary>
    /// Interaction logic for AddCustomers.xaml
    /// </summary>

    public partial class AddCustomers : Window
    {
        public Customer? NewCustomer { get; private set; }

        public AddCustomers()
        {
            InitializeComponent();
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            // Validation: Name required, at least one of phone or address
            var name = NameBox.Text.Trim();
            var phone = PhoneBox.Text.Trim();
            var address = AddressBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(phone) && string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("At least one of phone or address must be provided.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create new customer object
            NewCustomer = new Customer(name, string.IsNullOrWhiteSpace(phone) ? null : phone, string.IsNullOrWhiteSpace(address) ? null : address);

            // Signal that user submitted data
            this.DialogResult = true;
            this.Close();
        }

        private void Phone_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Optional: Add phone number formatting logic here
        }
    }
}
